﻿DROP FUNCTION IF EXISTS core.get_item_cost_price(item_id_ integer, party_id_ integer, unit_id_ integer);
CREATE FUNCTION core.get_item_cost_price(item_id_ integer, party_id_ integer, unit_id_ integer)
RETURNS money
AS
$$
	DECLARE _price money;
	DECLARE _unit_id integer;
	DECLARE _factor decimal;
	DECLARE _tax_rate decimal;
	DECLARE _includes_tax boolean;
	DECLARE _tax money;
BEGIN

	--Fist pick the catalog price which matches all these fields:
	--Item, Customer Type, Price Type, and Unit.
	--This is the most effective price.
	SELECT 
		item_cost_prices.price, 
		item_cost_prices.unit_id,
		item_cost_prices.includes_tax
	INTO 
		_price, 
		_unit_id,
		_includes_tax		
	FROM core.item_cost_prices
	WHERE item_cost_prices.item_id=$1
	AND item_cost_prices.party_id=$2
	AND item_cost_prices.unit_id = $3;

	IF(_unit_id IS NULL) THEN
		--We do not have a cost price of this item for the unit supplied.
		--Let's see if this item has a price for other units.
		SELECT 
			item_cost_prices.price, 
			item_cost_prices.unit_id,
			item_cost_prices.includes_tax
		INTO 
			_price, 
			_unit_id,
			_includes_tax
		FROM core.item_cost_prices
		WHERE item_cost_prices.item_id=$1
		AND item_cost_prices.party_id=$2;
	END IF;

	
	IF(_price IS NULL) THEN
		--This item does not have cost price defined in the catalog.
		--Therefore, getting the default cost price from the item definition.
		SELECT 
			cost_price, 
			unit_id,
			cost_price_includes_tax
		INTO 
			_price, 
			_unit_id,
			_includes_tax
		FROM core.items
		WHERE core.items.item_id = $1;
	END IF;

	IF(_includes_tax) THEN
		_tax_rate := core.get_item_tax_rate($1);
		_price := _price / ((100 + _tax_rate)/ 100);
	END IF;

	--Get the unitary conversion factor if the requested unit does not match with the price defition.
	_factor := core.convert_unit($3, _unit_id);

	RETURN _price * _factor;
END
$$
LANGUAGE plpgsql;

DROP FUNCTION IF EXISTS core.get_item_selling_price(item_id_ integer, party_type_id_ integer, price_type_id_ integer, unit_id_ integer);
CREATE FUNCTION core.get_item_selling_price(item_id_ integer, party_type_id_ integer, price_type_id_ integer, unit_id_ integer)
RETURNS money
AS
$$
	DECLARE _price money;
	DECLARE _unit_id integer;
	DECLARE _factor decimal;
	DECLARE _tax_rate decimal;
	DECLARE _includes_tax boolean;
	DECLARE _tax money;
BEGIN

	--Fist pick the catalog price which matches all these fields:
	--Item, Customer Type, Price Type, and Unit.
	--This is the most effective price.
	SELECT 
		item_selling_prices.price, 
		item_selling_prices.unit_id,
		item_selling_prices.includes_tax
	INTO 
		_price, 
		_unit_id,
		_includes_tax		
	FROM core.item_selling_prices
	WHERE item_selling_prices.item_id=$1
	AND item_selling_prices.party_type_id=$2
	AND item_selling_prices.price_type_id =$3
	AND item_selling_prices.unit_id = $4;

	IF(_unit_id IS NULL) THEN
		--We do not have a selling price of this item for the unit supplied.
		--Let's see if this item has a price for other units.
		SELECT 
			item_selling_prices.price, 
			item_selling_prices.unit_id,
			item_selling_prices.includes_tax
		INTO 
			_price, 
			_unit_id,
			_includes_tax
		FROM core.item_selling_prices
		WHERE item_selling_prices.item_id=$1
		AND item_selling_prices.party_type_id=$2
		AND item_selling_prices.price_type_id =$3;
	END IF;

	
	IF(_price IS NULL) THEN
		--This item does not have selling price defined in the catalog.
		--Therefore, getting the default selling price from the item definition.
		SELECT 
			selling_price, 
			unit_id,
			selling_price_includes_tax
		INTO 
			_price, 
			_unit_id,
			_includes_tax
		FROM core.items
		WHERE core.items.item_id = $1;
	END IF;

	IF(_includes_tax) THEN
		_tax_rate := core.get_item_tax_rate($1);
		_price := _price / ((100 + _tax_rate)/ 100);
	END IF;

	--Get the unitary conversion factor if the requested unit does not match with the price defition.
	_factor := core.convert_unit($4, _unit_id);

	RETURN _price * _factor;
END
$$
LANGUAGE plpgsql;

DROP FUNCTION IF EXISTS transactions.verification_trigger() CASCADE;
CREATE FUNCTION transactions.verification_trigger()
RETURNS TRIGGER
AS
$$
	DECLARE _transaction_master_id bigint;
	DECLARE _transaction_posted_by integer;
	DECLARE _old_verifier integer;
	DECLARE _old_status integer;
	DECLARE _old_reason national character varying(128);
	DECLARE _verifier integer;
	DECLARE _status integer;
	DECLARE _reason national character varying(128);
	DECLARE _can_verify boolean;
	DECLARE _is_sys boolean;
	DECLARE _rejected smallint=-3;
	DECLARE _closed smallint=-2;
	DECLARE _withdrawn smallint=-1;
	DECLARE _unapproved smallint = 0;
	DECLARE _auto_approved smallint = 1;
	DECLARE _approved smallint=2;
	DECLARE _book text;
	DECLARE _can_verify_sales boolean;
	DECLARE _sales_verification_limit money;
	DECLARE _can_verify_purchase boolean;
	DECLARE _purchase_verification_limit money;
	DECLARE _can_verify_gl boolean;
	DECLARE _gl_verification_limit money;
	DECLARE _can_verify_self boolean;
	DECLARE _self_verification_limit money;
	DECLARE _posted_amount money;
BEGIN
	IF TG_OP='DELETE' THEN
		RAISE EXCEPTION 'Deleting a transaction is not allowed. Mark the transaction as rejected instead.';
	END IF;

	IF TG_OP='UPDATE' THEN
		RAISE NOTICE 'Columns except the following will be ignored for this update: verified_by_user_id, verification_status_id, verification_reason.';

		IF(OLD.transaction_master_id IS DISTINCT FROM NEW.transaction_master_id) THEN
			RAISE EXCEPTION 'Cannot update the column "transaction_master_id".';
		END IF;

		IF(OLD.transaction_counter IS DISTINCT FROM NEW.transaction_counter) THEN
			RAISE EXCEPTION 'Cannot update the column "transaction_counter".';
		END IF;

		IF(OLD.transaction_code IS DISTINCT FROM NEW.transaction_code) THEN
			RAISE EXCEPTION 'Cannot update the column "transaction_code".';
		END IF;

		IF(OLD.book IS DISTINCT FROM NEW.book) THEN
			RAISE EXCEPTION 'Cannot update the column "book".';
		END IF;

		IF(OLD.value_date IS DISTINCT FROM NEW.value_date) THEN
			RAISE EXCEPTION 'Cannot update the column "value_date".';
		END IF;

		IF(OLD.transaction_ts IS DISTINCT FROM NEW.transaction_ts) THEN
			RAISE EXCEPTION 'Cannot update the column "transaction_ts".';
		END IF;

		IF(OLD.login_id IS DISTINCT FROM NEW.login_id) THEN
			RAISE EXCEPTION 'Cannot update the column "login_id".';
		END IF;

		IF(OLD.user_id IS DISTINCT FROM NEW.user_id) THEN
			RAISE EXCEPTION 'Cannot update the column "user_id".';
		END IF;

		IF(OLD.sys_user_id IS DISTINCT FROM NEW.sys_user_id) THEN
			RAISE EXCEPTION 'Cannot update the column "sys_user_id".';
		END IF;

		IF(OLD.office_id IS DISTINCT FROM NEW.office_id) THEN
			RAISE EXCEPTION 'Cannot update the column "office_id".';
		END IF;

		IF(OLD.cost_center_id IS DISTINCT FROM NEW.cost_center_id) THEN
			RAISE EXCEPTION 'Cannot update the column "cost_center_id".';
		END IF;

		_transaction_master_id := OLD.transaction_master_id;
		_book := OLD.book;
		_old_verifier := OLD.verified_by_user_id;
		_old_status := OLD.verification_status_id;
		_old_reason := OLD.verification_reason;
		_transaction_posted_by := OLD.user_id;		
		_verifier := NEW.verified_by_user_id;
		_status := NEW.verification_status_id;
		_reason := NEW.verification_reason;
		_is_sys := office.is_sys(_verifier);


		SELECT
			sum(amount)
		INTO
			_posted_amount
		FROM
			transactions.transaction_details
		WHERE transactions.transaction_details.transaction_master_id = _transaction_master_id
		AND transactions.transaction_details.tran_type='Cr';


		SELECT
			true,
			can_verify_sales_transactions,
			sales_verification_limit,
			can_verify_purchase_transactions,
			purchase_verification_limit,
			can_verify_gl_transactions,
			gl_verification_limit,
			can_self_verify,
			self_verification_limit
		INTO
			_can_verify,
			_can_verify_sales,
			_sales_verification_limit,
			_can_verify_purchase,
			_purchase_verification_limit,
			_can_verify_gl,
			_gl_verification_limit,
			_can_verify_self,
			_self_verification_limit
		FROM
		policy.voucher_verification_policy
		WHERE user_id=_verifier
		AND is_active=true
		AND now() >= effective_from
		AND now() <= ends_on;

		IF(_verifier IS NULL) THEN
			RAISE EXCEPTION 'Access is denied.';
		END IF;		
		
		IF(_status != _withdrawn AND _can_verify = false) THEN
			RAISE EXCEPTION 'Access is denied. You don''t have the right to verify the transaction.';
		END IF;

		IF(_status = _withdrawn AND _can_verify = false) THEN
			IF(_transaction_posted_by != _verifier) THEN
				RAISE EXCEPTION 'Access is denied. You don''t have the right to withdraw the transaction.';
			END IF;
		END IF;

		IF(_status = _auto_approved AND _is_sys = false) THEN
			RAISE EXCEPTION 'Access is denied.';
		END IF;


		IF(_can_verify = false) THEN
			RAISE EXCEPTION 'Access is denied.';
		END IF;


		--Is trying verify self transaction.
		IF(NEW.verified_by_user_id = NEW.user_id) THEN
			IF(_can_verify_self = false) THEN
				RAISE EXCEPTION 'Please ask someone else to verify the transaction you posted.';
			END IF;
			IF(_can_verify_self = true) THEN
				IF(_posted_amount > _self_verification_limit AND _self_verification_limit > 0::money) THEN
					RAISE EXCEPTION 'Self verfication limit exceeded. The transaction was not verified.';
				END IF;
			END IF;
		END IF;

		IF(lower(_book) LIKE '%sales%') THEN
			IF(_can_verify_sales = false) THEN
				RAISE EXCEPTION 'Access is denied.';
			END IF;
			IF(_can_verify_sales = true) THEN
				IF(_posted_amount > _sales_verification_limit AND _sales_verification_limit > 0::money) THEN
					RAISE EXCEPTION 'Sales verfication limit exceeded. The transaction was not verified.';
				END IF;
			END IF;			
		END IF;


		IF(lower(_book) LIKE '%purchase%') THEN
			IF(_can_verify_purchase = false) THEN
				RAISE EXCEPTION 'Access is denied.';
			END IF;
			IF(_can_verify_purchase = true) THEN
				IF(_posted_amount > _purchase_verification_limit AND _purchase_verification_limit > 0::money) THEN
					RAISE EXCEPTION 'Purchase verfication limit exceeded. The transaction was not verified.';
				END IF;
			END IF;			
		END IF;


		IF(lower(_book) LIKE 'journal%') THEN
			IF(_can_verify_gl = false) THEN
				RAISE EXCEPTION 'Access is denied.';
			END IF;
			IF(_can_verify_gl = true) THEN
				IF(_posted_amount > _gl_verification_limit AND _gl_verification_limit > 0::money) THEN
					RAISE EXCEPTION 'GL verfication limit exceeded. The transaction was not verified.';
				END IF;
			END IF;			
		END IF;

		NEW.last_verified_on := now();

	END IF;	
	RETURN NEW;
END
$$
LANGUAGE plpgsql;


CREATE TRIGGER verification_update_trigger
AFTER UPDATE
ON transactions.transaction_master
FOR EACH ROW 
EXECUTE PROCEDURE transactions.verification_trigger();

CREATE TRIGGER verification_delete_trigger
BEFORE DELETE
ON transactions.transaction_master
FOR EACH ROW 
EXECUTE PROCEDURE transactions.verification_trigger();


DROP FUNCTION IF EXISTS transactions.auto_verify(bigint) CASCADE;

CREATE FUNCTION transactions.auto_verify(bigint)
RETURNS VOID
AS
$$
	DECLARE _transaction_master_id bigint;
	DECLARE _transaction_posted_by integer;
	DECLARE _verifier integer;
	DECLARE _status integer;
	DECLARE _reason national character varying(128);
	DECLARE _rejected smallint=-3;
	DECLARE _closed smallint=-2;
	DECLARE _withdrawn smallint=-1;
	DECLARE _unapproved smallint = 0;
	DECLARE _auto_approved smallint = 1;
	DECLARE _approved smallint=2;
	DECLARE _book text;
	DECLARE _auto_verify_sales boolean;
	DECLARE _sales_verification_limit money;
	DECLARE _auto_verify_purchase boolean;
	DECLARE _purchase_verification_limit money;
	DECLARE _auto_verify_gl boolean;
	DECLARE _gl_verification_limit money;
	DECLARE _posted_amount money;
	DECLARE _auto_verification boolean=true;
	DECLARE _has_policy boolean=false;
BEGIN
	_transaction_master_id := $1;

	SELECT
		transactions.transaction_master.book,
		transactions.transaction_master.user_id
	INTO
		_book,
		_transaction_posted_by 	
	FROM
	transactions.transaction_master
	WHERE transactions.transaction_master.transaction_master_id=_transaction_master_id;
	

	_verifier := office.get_sys_user_id();
	_status := 2;
	_reason := 'Automatically verified by workflow.';

	SELECT
		sum(amount)
	INTO
		_posted_amount
	FROM
		transactions.transaction_details
	WHERE transactions.transaction_details.transaction_master_id = _transaction_master_id
	AND transactions.transaction_details.tran_type='Cr';


	SELECT
		true,
		verify_sales_transactions,
		sales_verification_limit,
		verify_purchase_transactions,
		purchase_verification_limit,
		verify_gl_transactions,
		gl_verification_limit
	INTO
		_has_policy,
		_auto_verify_sales,
		_sales_verification_limit,
		_auto_verify_purchase,
		_purchase_verification_limit,
		_auto_verify_gl,
		_gl_verification_limit
	FROM
	policy.auto_verification_policy
	WHERE user_id=_transaction_posted_by
	AND is_active=true
	AND now() >= effective_from
	AND now() <= ends_on;



	IF(lower(_book) LIKE 'sales%') THEN
		IF(_auto_verify_sales = false) THEN
			_auto_verification := false;
		END IF;
		IF(_auto_verify_sales = true) THEN
			IF(_posted_amount > _sales_verification_limit AND _sales_verification_limit > 0::money) THEN
				_auto_verification := false;
			END IF;
		END IF;			
	END IF;


	IF(lower(_book) LIKE 'purchase%') THEN
		IF(_auto_verify_purchase = false) THEN
			_auto_verification := false;
		END IF;
		IF(_auto_verify_purchase = true) THEN
			IF(_posted_amount > _purchase_verification_limit AND _purchase_verification_limit > 0::money) THEN
				_auto_verification := false;
			END IF;
		END IF;			
	END IF;


	IF(lower(_book) LIKE 'journal%') THEN
		IF(_auto_verify_gl = false) THEN
			_auto_verification := false;
		END IF;
		IF(_auto_verify_gl = true) THEN
			IF(_posted_amount > _gl_verification_limit AND _gl_verification_limit > 0::money) THEN
				_auto_verification := false;
			END IF;
		END IF;			
	END IF;

	IF(_has_policy=true) THEN
		IF(_auto_verification = true) THEN
			UPDATE transactions.transaction_master
			SET 
				last_verified_on = now(),
				verified_by_user_id=_verifier,
				verification_status_id=_status,
				verification_reason=_reason
			WHERE
				transactions.transaction_master.transaction_master_id=_transaction_master_id;
		END IF;
	ELSE
		RAISE NOTICE 'No auto verification policy found for this user.';
	END IF;
RETURN;
END
$$
LANGUAGE plpgsql;

DROP VIEW IF EXISTS transactions.verified_transactions_view;
DROP VIEW IF EXISTS transactions.transaction_view;
CREATE VIEW transactions.transaction_view
AS
SELECT
	transactions.transaction_master.transaction_master_id,
	transactions.transaction_master.transaction_counter,
	transactions.transaction_master.transaction_code,
	transactions.transaction_master.book,
	transactions.transaction_master.value_date,
	transactions.transaction_master.transaction_ts,
	transactions.transaction_master.login_id,
	transactions.transaction_master.user_id,
	transactions.transaction_master.sys_user_id,
	transactions.transaction_master.office_id,
	transactions.transaction_master.cost_center_id,
	transactions.transaction_master.reference_number,
	transactions.transaction_master.statement_reference AS master_statement_reference,
	transactions.transaction_master.last_verified_on,
	transactions.transaction_master.verified_by_user_id,
	transactions.transaction_master.verification_status_id,
	transactions.transaction_master.verification_reason,
	transactions.transaction_details.transaction_detail_id,
	transactions.transaction_details.tran_type,
	transactions.transaction_details.account_id,
	transactions.transaction_details.statement_reference,
	transactions.transaction_details.cash_repository_id,
	transactions.transaction_details.amount
FROM
transactions.transaction_master
INNER JOIN transactions.transaction_details
ON transactions.transaction_master.transaction_master_id = transactions.transaction_details.transaction_master_id;

CREATE VIEW transactions.verified_transactions_view
AS
SELECT * FROM transactions.transaction_view
WHERE verification_status_id > 0;


DROP FUNCTION IF EXISTS transactions.get_cash_repository_balance(integer);
CREATE FUNCTION transactions.get_cash_repository_balance(integer)
RETURNS money
AS
$$
	DECLARE _debit money;
	DECLARE _credit money;
BEGIN
	SELECT COALESCE(SUM(amount), 0::money) INTO _debit
	FROM transactions.verified_transactions_view
	WHERE cash_repository_id=$1
	AND tran_type='Dr';

	SELECT COALESCE(SUM(amount), 0::money) INTO _credit
	FROM transactions.verified_transactions_view
	WHERE cash_repository_id=$1
	AND tran_type='Cr';

	RETURN _debit - _credit;
END
$$
LANGUAGE plpgsql;

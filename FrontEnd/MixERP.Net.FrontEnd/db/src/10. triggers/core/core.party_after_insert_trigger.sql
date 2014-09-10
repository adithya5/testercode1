CREATE FUNCTION core.party_after_insert_trigger()
RETURNS TRIGGER
AS
$$
	DECLARE _parent_account_id bigint= core.get_account_id_by_parameter('Party.Parent.Account');
	DECLARE _party_code text;
	DECLARE _account_id bigint;
BEGIN
	_party_code := core.get_party_code(NEW.first_name, NEW.middle_name, NEW.last_name);

	IF(COALESCE(NEW.party_name, '') = '') THEN
		NEW.party_name := REPLACE(TRIM(COALESCE(NEW.last_name, '') || ', ' || NEW.first_name || ' ' || COALESCE(NEW.middle_name, '')), ' ', '');
	END IF;

	UPDATE core.parties
	SET 
		party_code=_party_code
	WHERE core.parties.party_id=NEW.party_id;


	--Create a new account
	IF(NEW.account_id IS NULL) THEN

		INSERT INTO core.accounts(account_master_id, account_code, currency_code, account_name, parent_account_id)
		SELECT core.get_account_master_id_by_account_id(_parent_account_id), _party_code, NEW.currency_code, _party_code || ' (' || NEW.party_name || ')', _parent_account_id
		RETURNING account_id INTO _account_id;
	
		UPDATE core.parties
		SET 
			account_id=_account_id 
		WHERE core.parties.party_id=NEW.party_id;
	END IF;


	RETURN NEW;
END
$$
LANGUAGE plpgsql;

CREATE TRIGGER party_after_insert_trigger
AFTER INSERT
ON core.parties
FOR EACH ROW EXECUTE PROCEDURE core.party_after_insert_trigger();
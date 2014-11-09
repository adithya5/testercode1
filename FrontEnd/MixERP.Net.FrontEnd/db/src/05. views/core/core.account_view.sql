CREATE VIEW core.account_view
AS
SELECT
    core.accounts.account_id,
    core.accounts.account_code,
    core.accounts.account_name,
    core.accounts.description,
    core.accounts.sys_type,
    core.accounts.parent_account_id,
    parent_accounts.account_code AS parent_account_code,
    parent_accounts.account_name AS parent_account_name,
    core.account_masters.account_master_id,
    core.account_masters.account_master_code,
    core.account_masters.account_master_name,
    core.has_child_accounts(core.accounts.account_id) AS has_child,
    core.accounts.confidential
FROM
    core.account_masters
    INNER JOIN core.accounts 
    ON core.account_masters.account_master_id = core.accounts.account_master_id
    LEFT OUTER JOIN core.accounts AS parent_accounts 
    ON core.accounts.parent_account_id = parent_accounts.account_id;


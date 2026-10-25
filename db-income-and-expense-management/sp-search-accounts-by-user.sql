SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<César Ricardo Castellón Ramírez>
-- Create date: <14/05/2026>
-- Description:	<Buscar cuentas por el id del usuario>
-- =============================================
CREATE OR ALTER PROCEDURE sp_search_accounts_by_user
	-- Add the parameters for the stored procedure here
	@user_id INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		a.account_id AS AccountId,
		a.account_name AS AccountName,
		a.account_balance AS AccountBalance,
		at.account_type_name AS AccountTypeName
	FROM
		accounts a
	INNER JOIN
		account_types at ON at.account_type_id = a.account_type_id
	WHERE
		at.user_id = @user_id
	ORDER BY
		at.order_by;
END
GO

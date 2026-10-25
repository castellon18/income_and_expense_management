SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<César Ricardo Castellón Ramírez>
-- Create date: <15/05/2026>
-- Description:	<Obtener cuenta por id>
-- =============================================
CREATE PROCEDURE dbo.sp_get_account_by_id
	-- Add the parameters for the stored procedure here
	@account_id INT,
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
		a.account_type_id AS AccountTypeId
	FROM
		accounts a
	INNER JOIN
		account_types at ON at.account_type_id = a.account_type_id
	WHERE
		at.user_id = @user_id AND a.account_id = @account_id;
END
GO

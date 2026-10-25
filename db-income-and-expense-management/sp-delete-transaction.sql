USE db_income_and_expense_management;
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<César Ricardo Castellón Ramírez>
-- Create date: <20/05/2026>
-- Description:	<Borrar transaccíon>
-- =============================================
CREATE PROCEDURE dbo.sp_delete_transaction
	-- Add the parameters for the stored procedure here
	@transaction_id INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @amount DECIMAL(18,4);
	DECLARE @account_id INT;
	DECLARE @operation_type_id INT;

    -- Insert statements for procedure here
	SELECT
		@amount=t.amount,
		@account_id=t.account_id,
		@operation_type_id=c.operation_type_id
	FROM
		dbo.transactions t
	INNER JOIN
		dbo.categories c ON c.category_id=t.category_id
	WHERE
		t.transaction_id=@transaction_id;

	IF(@operation_type_id=2)
		SET @amount*=-1;

	UPDATE accounts SET
		account_balance-=@amount
	WHERE
		account_id=@account_id;

	DELETE FROM dbo.transactions WHERE transaction_id=@transaction_id;
END
GO

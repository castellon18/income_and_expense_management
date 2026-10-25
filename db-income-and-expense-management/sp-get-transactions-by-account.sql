SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<César Ricardo Castellón Ramírez>
-- Create date: <21/05/2026>
-- Description:	<Obtener transacciones por cuenta>
-- =============================================
CREATE OR ALTER PROCEDURE dbo.sp_get_transactions_by_account
	-- Add the parameters for the stored procedure here
	@user_id INT,
	@account_id INT,
	@start_date DATE,
	@end_date DATE
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		t.transaction_id AS TransactionId,
		t.transaction_date AS TransactionDate,
		c.category_name AS CategoryName,
		a.account_name AS AccountName,
		c.operation_type_id AS OperationTypeId,
		t.amount AS Amount
	FROM
		dbo.transactions t
	INNER JOIN
		dbo.categories c ON c.category_id=t.category_id
	INNER JOIN
		dbo.accounts a ON a.account_id=t.account_id
	WHERE
		t.account_id=@account_id AND
		t.user_id=@user_id AND
		transaction_date BETWEEN @start_date AND @end_date;
END
GO

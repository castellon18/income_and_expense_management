SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<César Ricardo Castellón Ramírez>
-- Create date: <22/05/2026>
-- Description:	<Obtener transacciones por usuario>
-- =============================================
CREATE OR ALTER PROCEDURE dbo.sp_get_transactions_by_user
	-- Add the parameters for the stored procedure here
	@user_id INT,
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
		t.amount AS Amount,
		t.transaction_note AS TransactionNote
	FROM
		dbo.transactions t
	INNER JOIN
		dbo.categories c ON c.category_id=t.category_id
	INNER JOIN
		dbo.accounts a ON a.account_id=t.account_id
	WHERE
		t.user_id=@user_id AND
		transaction_date BETWEEN @start_date AND @end_date
	ORDER BY
		t.transaction_date DESC;
END
GO

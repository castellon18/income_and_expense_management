SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<César Ricardo Castellón Ramírez>
-- Create date: <26/05/2026>
-- Description:	<Obtener transacciones por mes>
-- =============================================
CREATE OR ALTER PROCEDURE dbo.sp_get_transactions_by_month
	-- Add the parameters for the stored procedure here
	@user_id INT,
	@year INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		MONTH(t.transaction_date) AS Months,
		SUM(t.amount) AS Amount,
		c.operation_type_id AS OperationTypeId
	FROM
		dbo.transactions t
	INNER JOIN
		dbo.categories c ON c.category_id=t.category_id
	WHERE
		t.user_id=@user_id AND YEAR(transaction_date)=@year
	GROUP BY
		MONTH(t.transaction_date),
		c.operation_type_id;
END
GO

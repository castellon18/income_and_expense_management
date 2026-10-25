SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<César Ricardo Castellón Ramírez>
-- Create date: <25/05/2026>
-- Description:	<Obtener transaciones por semana>
-- =============================================
CREATE OR ALTER PROCEDURE dbo.sp_get_transactions_by_week
	-- Add the parameters for the stored procedure here
	@start_date DATE,
	@end_date DATE,
	@user_id INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		DATEDIFF(DAY,@start_date,t.transaction_date)/7+1 AS Weeks,
		SUM(t.amount) AS Amount,
		C.operation_type_id AS OperationTypeId
	FROM
		dbo.transactions t
	INNER JOIN
		dbo.categories c ON c.category_id=t.category_id
	WHERE
		t.user_id=@user_id AND
		t.transaction_date BETWEEN @start_date AND @end_date
	GROUP BY
		DATEDIFF(DAY,@start_date,t.transaction_date)/7+1,
		C.operation_type_id;
END
GO

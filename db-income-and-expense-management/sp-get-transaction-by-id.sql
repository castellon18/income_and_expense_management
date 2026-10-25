SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<César Ricardo>
-- Create date: <Create Date>
-- Description:	<Description>
-- =============================================
CREATE OR ALTER PROCEDURE dbo.sp_get_transaction_by_id
	-- Add the parameters for the stored procedure here
	@transaction_id INT,
	@user_id INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		t.transaction_id AS TransactionId,
		t.user_id AS UserId,
		t.transaction_date AS TransactionDate,
		t.amount AS Amount,
		t.account_id AS AccountId,
		t.category_id AS CategoryId,
		t.transaction_note AS TransactionNote,
		c.operation_type_id AS OperationTypeId
	FROM
		dbo.transactions t
	INNER JOIN
		dbo.categories c ON c.category_id=t.category_id
	WHERE
		t.transaction_id=@transaction_id AND t.user_id=@user_id;
END
GO

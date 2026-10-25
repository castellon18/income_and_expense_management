SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<César Ricardo Castellón Ramírez>
-- Create date: <19/05/2026>
-- Description:	<Insert transacción>
-- =============================================
CREATE OR ALTER PROCEDURE dbo.sp_insert_transaction
	-- Add the parameters for the stored procedure here
	@user_id INT,
	@transaction_date DATETIME,
	@amount DECIMAL(18,4),
	@account_id INT,
	@category_id INT,
	@transaction_note NVARCHAR(1000)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO dbo.transactions(
		user_id,
		transaction_date,
		amount,
		account_id,
		category_id,
		transaction_note
	) VALUES(
		@user_id,
		@transaction_date,
		ABS(@amount),
		@account_id,
		@category_id,
		@transaction_note
	);

	UPDATE accounts SET
		account_balance += @amount
	WHERE
		account_id=@account_id;

	SELECT SCOPE_IDENTITY();
END
GO

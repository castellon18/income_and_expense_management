SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<César Ricardo Castellón Ramírez>
-- Create date: <20/05/2026>
-- Description:	<Actualizar transacción>
-- =============================================
CREATE OR ALTER PROCEDURE dbo.sp_update_transaction
	-- Add the parameters for the stored procedure here
	@transaction_id INT,
	@transaction_date DATETIME,
	@amount DECIMAL(18,4),
	@previous_amount DECIMAL(18,4),
	@account_id INT,
	@previous_account_id INT,
	@category_id INT,
	@transaction_note NVARCHAR(1000)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
	--reverse previous transaction
	UPDATE accounts SET
		account_balance += @previous_amount
	WHERE
		account_id=@previous_account_id;

	--perform a new transaction
	UPDATE accounts SET
		account_balance += @amount
	WHERE
		account_id=@account_id;

	UPDATE transactions SET
		transaction_date=@transaction_date,
		amount=ABS(@amount),
		account_id=@account_id,
		category_id=@category_id,
		transaction_note=@transaction_note
	WHERE
		transaction_id=@transaction_id;
END
GO

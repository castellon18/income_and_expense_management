SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<César Ricardo Castellón Ramírez>
-- Create date: <15/05/2026>
-- Description:	<Actualizar cuenta>
-- =============================================
CREATE PROCEDURE dbo.sp_update_account
	-- Add the parameters for the stored procedure here
	@account_id INT,
	@account_name NVARCHAR(50),
	@account_type_id INT,
	@account_balance DECIMAL(18,4),
	@account_description NVARCHAR(1000)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE accounts SET
		account_name=@account_name,
		account_type_id=@account_type_id,
		account_balance=@account_balance,
		account_description=@account_description
	WHERE
		account_id=@account_id;
END
GO

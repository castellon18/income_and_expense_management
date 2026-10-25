SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<César Ricardo Castellón Ramírez>
-- Create date: <14/05/2026>
-- Description:	<Insertar una nueva cuenta>
-- =============================================
CREATE PROCEDURE sp_insert_account
	-- Add the parameters for the stored procedure here
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
	INSERT INTO accounts(
		account_name,
		account_type_id,
		account_balance,
		account_description
	) VALUES(
		@account_name,
		@account_type_id,
		@account_balance,
		@account_description
	);

	SELECT CAST(SCOPE_IDENTITY() AS INT)
END
GO

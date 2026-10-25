SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<César Ricardo Castellón Ramírez>
-- Create date: <14/05/2026>
-- Description:	<Insertar un tipo de cuenta>
-- =============================================
CREATE PROCEDURE sp_insert_account_type
	-- Add the parameters for the stored procedure here
	@account_type_name NVARCHAR(50),
	@user_id INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @order_by INT;
    
	-- Insert statements for procedure here
	SELECT @order_by = COALESCE(MAX(order_by),0)+1 FROM account_types WHERE user_id=@user_id;

	INSERT INTO account_types(
		account_type_name,
		user_id,
		order_by
	) VALUES(
		@account_type_name,
		@user_id,
		@order_by
	);

	SELECT SCOPE_IDENTITY();
END
GO

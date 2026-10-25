USE [db_income_and_expense_management]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<César Ricardo Castellón Ramírez>
-- Create date: <15/05/2026>
-- Description:	<Actualizar cuenta>
-- =============================================
CREATE PROCEDURE [dbo].[sp_update_account_type] 
	-- Add the parameters for the stored procedure here
	@account_type_id INT,
	@account_type_name NVARCHAR(50), 
	@order_by INT,
	@user_id INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE account_types SET
		account_type_name = @account_type_name,
		order_by = @order_by
	WHERE
		account_type_id = @account_type_id AND user_id = @user_id;
END
GO

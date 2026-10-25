SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<César Ricardo Castellón Ramírez>
-- Create date: <19/05/2026>
-- Description:	<Eliminar categoría>
-- =============================================
CREATE OR ALTER PROCEDURE dbo.sp_delete_category
	-- Add the parameters for the stored procedure here
	@category_id INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE FROM categories WHERE category_id=@category_id;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<César Ricardo Castellón Ramírez>
-- Create date: <18/05/2026>
-- Description:	<Actualizar categoría>
-- =============================================
CREATE PROCEDURE dbo.sp_update_category
	-- Add the parameters for the stored procedure here
	@category_id INT,
	@category_name NVARCHAR(50),
	@operation_type_id INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE dbo.categories SET
		category_name=@category_name,
		operation_type_id=@operation_type_id
	WHERE
		category_id=@category_id;
END
GO

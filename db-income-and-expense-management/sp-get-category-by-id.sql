SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<César Ricardo Castellón Ramírez>
-- Create date: <18/05/2026>
-- Description:	<Obtener categoria por id>
-- =============================================
CREATE OR ALTER PROCEDURE dbo.sp_get_category_by_id
	-- Add the parameters for the stored procedure here
	@category_id INT,
	@user_id INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		category_id AS CategoryId,
		category_name AS CategoryName,
		operation_type_id AS OperationTypeId
	FROM
		dbo.categories
	WHERE
		category_id=@category_id AND user_id=@user_id;
END
GO

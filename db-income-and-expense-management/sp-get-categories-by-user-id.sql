SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<César Ricardo Castellón Ramírez>
-- Create date: <18/05/2026>
-- Description:	<Obtener las categorías por usuario>
-- =============================================
CREATE OR ALTER PROCEDURE dbo.sp_get_categories_by_user_id
	-- Add the parameters for the stored procedure here
	@user_id INT,
	@records_to_skip INT,
	@records_per_page INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		c.category_id AS CategoryId,
		c.category_name AS CategoryName,
		ot.description_operation AS OperationTypeName
	FROM
		dbo.categories c
	INNER JOIN
		dbo.operation_types ot ON ot.operation_type_id=c.operation_type_id
	WHERE
		c.user_id=@user_id
	ORDER BY
		c.category_id
	OFFSET @records_to_skip ROWS
	FETCH NEXT @records_per_page ROWS ONLY;
END
GO

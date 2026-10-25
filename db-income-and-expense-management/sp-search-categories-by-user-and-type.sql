SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<César Ricardo Castellón Ramírez>
-- Create date: <20/05/2026>
-- Description:	<Obtener categorías por usuario y tipo de operación>
-- =============================================
CREATE OR ALTER PROCEDURE dbo.sp_search_categories_by_user_and_type
	-- Add the parameters for the stored procedure here
	@user_id INT, 
	@operation_type_id INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		category_id AS CategoryId,
		category_name AS CategoryName,
		user_id AS UserId,
		operation_type_id AS OperationTypeId
	FROM
		dbo.categories
	WHERE
		user_id=@user_id AND operation_type_id=@operation_type_id;
END
GO

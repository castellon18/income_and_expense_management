SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<César Ricardo Castellón Ramírez>
-- Create date: <18/05/2026>
-- Description:	<Insert nueva categoría>
-- =============================================
CREATE OR ALTER PROCEDURE dbo.sp_insert_category
	-- Add the parameters for the stored procedure here
	@category_name NVARCHAR(50),
	@Operation_type_id INT,
	@user_id INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO dbo.categories(
		category_name,
		operation_type_id,
		user_id
	) VALUES(
		@category_name,
		@Operation_type_id,
		@user_id
	);
	SELECT SCOPE_IDENTITY();
END
GO

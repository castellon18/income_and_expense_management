SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<César Ricardo Castellón Ramírez>
-- Create date: <27/05/2026>
-- Description:	<Insertar nuevo usuario>
-- =============================================
CREATE OR ALTER PROCEDURE dbo.sp_insert_user
	-- Add the parameters for the stored procedure here
	@user_account_name NVARCHAR(50),
	@normalized_user_account_name NVARCHAR(50),
	@email NVARCHAR(256),
	@normalized_email NVARCHAR(256),
	@password_hash NVARCHAR(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO users(
		user_account_name,
		normalized_user_account_name,
		email,
		normalized_email,
		password_hash
	) VALUES (
		@user_account_name,
		@normalized_user_account_name,
		@email,
		@normalized_email,
		@password_hash
	);

	SELECT SCOPE_IDENTITY();
END
GO

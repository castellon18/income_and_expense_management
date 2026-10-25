SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<César Ricardo Castellón Ramírez>
-- Create date: <05/06/2026>
-- Description:	<actualizar contraseña del usuario>
-- =============================================
CREATE OR ALTER PROCEDURE dbo.sp_update_password_user
	-- Add the parameters for the stored procedure here
	@user_id INT,
	@password_hash NVARCHAR(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE users SET
		password_hash=@password_hash
	WHERE
		user_id=@user_id;
END
GO

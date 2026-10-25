SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<César Ricardo Castellón Ramírez>
-- Create date: <27/05/2026>
-- Description:	<Insertar nuevo usuario>
-- =============================================
CREATE OR ALTER PROCEDURE dbo.sp_get_user_by_email
	-- Add the parameters for the stored procedure here
	@normalized_email NVARCHAR(256)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		user_id AS UserId,
		user_account_name AS UserAccountName,
		normalized_user_account_name AS NormalizedUserAccountName,
		email AS Email,
		normalized_email AS NormalizedEmail,
		password_hash AS PasswordHash
	FROM
		users
	WHERE
		normalized_email=@normalized_email;
END
GO

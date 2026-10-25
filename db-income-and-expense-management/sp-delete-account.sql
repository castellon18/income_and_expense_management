SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<César Ricardo Castellón Ramírez>
-- Create date: <18/05/2026>
-- Description:	<Delete cuenta>
-- =============================================
CREATE PROCEDURE sp_delete_account
	-- Add the parameters for the stored procedure here
	@account_id INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE FROM dbo.accounts WHERE account_id=@account_id;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<César Ricardo Castellón Ramírez>
-- Create date: <04/06/2026>
-- Description:	<Crear datos para usario nuevo>
-- =============================================
CREATE OR ALTER PROCEDURE dbo.sp_create_data_for_new_user
	-- Add the parameters for the stored procedure here
	@user_id INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO dbo.account_types(
		account_type_name,
		user_id,
		order_by
	) VALUES(
		'Cuenta Debito BAC C$',
		@user_id,
		1
	), (
		'Cuenta Ahorro BAC U$',
		@user_id,
		2
	), (
		'Tarjeta Crédito BAC',
		@user_id,
		3
	), (
		'Prestamo BAC U$',
		@user_id,
		4
	);

	INSERT INTO dbo.accounts(
		account_name,
		account_type_id,
		account_balance,
		account_description
	)
	SELECT
		account_type_name,
		account_type_id,
		0,
		'Nueva Cuenta'
	FROM
		account_types
	WHERE
		user_id=@user_id;

	INSERT INTO dbo.categories(
		category_name,
		operation_type_id,
		user_id
	) VALUES(
		'Salario',
		1,
		@user_id
	), (
		'Mercado',
		2,
		@user_id
	), (
		'Servicio Basico',
		2,
		@user_id
	), (
		'Comida Rapida',
		2,
		@user_id
	), (
		'Pago Deudores',
		1,
		@user_id
	);
END
GO

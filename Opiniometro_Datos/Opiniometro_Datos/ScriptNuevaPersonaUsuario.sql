/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

-- Agregar Persona y Usuario
IF OBJECT_ID('SP_AgregarPersonaUsuario') IS NOT NULL
	DROP PROCEDURE SP_AgregarPersonaUsuario
GO
CREATE PROCEDURE SP_AgregarPersonaUsuario
	@Correo			NVARCHAR(50),
	@Contrasenna	NVARCHAR(50),
	@Cedula			CHAR(9),
	@Nombre			NVARCHAR(50),
	@Apellido1		NVARCHAR(50),
	@Apellido2		NVARCHAR(50),
	@Direccion		NVARCHAR(256)
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Id UNIQUEIDENTIFIER=NEWID()

	INSERT INTO Persona
	VALUES (@Cedula, @Nombre, @Apellido1, @Apellido2)--, @Direccion) En la tabla no está "Direccion" y no me permitió agregarlo

	INSERT INTO Usuario
	VALUES (@Correo, HASHBYTES('SHA2_512', @Contrasenna+CAST(@Id AS NVARCHAR(36))), 1, @Cedula, @Id)
END
GO

EXEC SP_AgregarPersonaUsuario @Correo='barryallen@correo.com', @Contrasenna='12345678', @Cedula='123456789', @Nombre='Barry', @Apellido1='Allen', @Apellido2='Garcia', @Direccion='Central City';

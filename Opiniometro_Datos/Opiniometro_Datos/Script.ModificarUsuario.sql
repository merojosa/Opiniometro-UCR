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

-- Modificar Persona
IF OBJECT_ID('SP_ModificarPersona') IS NOT NULL
	DROP PROCEDURE SP_ModificarPersona
GO
CREATE PROCEDURE SP_ModificarPersona
	@CedulaBusqueda		VARCHAR(9),
	@Cedula				CHAR(9),
	@Nombre				NVARCHAR(50),
	@Apellido1			NVARCHAR(50),
	@Apellido2			NVARCHAR(50),
	@Direccion			NVARCHAR(256)
AS
BEGIN
	UPDATE Persona
	SET Cedula = @Cedula, Nombre = @Nombre, Apellido1 = @Apellido1, Apellido2 = @Apellido2, Direccion = @Direccion
	WHERE Cedula = @CedulaBusqueda;
END
GO

EXEC SP_ModificarPersona @CedulaBusqueda = '987654321', @Cedula='987654321', @Nombre='Barry2', @Apellido1='Allen2', @Apellido2='Garcia2', @Direccion='Central City2';
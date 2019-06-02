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

-- Eliminar Persona
IF OBJECT_ID('SP_DesactivarUsuario') IS NOT NULL
	DROP PROCEDURE SP_DesactivarUsuario
GO
CREATE PROCEDURE SP_DesactivarUsuario
	@CedulaBusqueda		VARCHAR(9)
AS
BEGIN
	Update Usuario
	SET Activo = 0
	WHERE Cedula = @CedulaBusqueda;
END
GO

EXEC SP_DesactivarUsuario @CedulaBusqueda = '987654321';
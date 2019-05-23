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
IF OBJECT_ID('SP_EliminarPersona') IS NOT NULL
	DROP PROCEDURE SP_EliminarPersona
GO
CREATE PROCEDURE SP_EliminarPersona
	@CedulaBusqueda		VARCHAR(9)
AS
BEGIN
	DELETE
	FROM Persona
	WHERE Cedula = @CedulaBusqueda;
END
GO

EXEC SP_EliminarPersona @CedulaBusqueda = '987654321';
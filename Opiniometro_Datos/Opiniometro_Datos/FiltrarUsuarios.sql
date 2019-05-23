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

-- Ver los usuarios agregados recientemente
IF OBJECT_ID('SP_FiltrarUsuarios') IS NOT NULL
	DROP PROCEDURE SP_FiltrarUsuarios
GO
CREATE PROCEDURE SP_FiltrarUsuarios
@Ced VARCHAR(10)
AS
BEGIN
	SELECT Cedula, Nombre, Apellido1, Apellido2
	FROM Persona
	WHERE Cedula LIKE @Ced+'%';
END
GO

EXEC SP_FiltrarUsuarios @Ced = '12';
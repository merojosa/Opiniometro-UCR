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

CREATE PROCEDURE [dbo].[Obtener_Secciones_Por_Formulario]
	@ForCod char(6)
AS
BEGIN
	SET NOCOUNT ON
	SELECT C.TituloSeccion
	FROM Conformado_Item_Sec_Form C
	WHERE C.CodigoFormulario = @ForCod
END


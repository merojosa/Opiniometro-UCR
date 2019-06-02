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

GO
CREATE PROCEDURE [dbo].[Obtener_Secciones_Por_Formulario]
	@ForCod CHAR(6)
AS
BEGIN
	SET NOCOUNT ON
	SELECT DISTINCT C.TituloSeccion
	FROM Conformado_Item_Sec_Form C
	WHERE C.CodigoFormulario = @ForCod
END
GO

GO
CREATE PROCEDURE [dbo].[Obtener_Items_Por_Seccion]
	@ForCod CHAR(6),
	@TitSec NVARCHAR(120)
AS
BEGIN
	SET NOCOUNT ON
	SELECT DISTINCT C.ItemId
	FROM Conformado_Item_Sec_Form C
	WHERE C.TituloSeccion = @TitSec AND C.CodigoFormulario = @ForCod
END
GO
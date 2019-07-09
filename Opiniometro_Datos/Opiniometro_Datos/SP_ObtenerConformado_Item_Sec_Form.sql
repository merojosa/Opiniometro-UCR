CREATE PROCEDURE [dbo].[SP_ObtenerConformado_Item_Sec_Form]

AS

	SELECT c.CodigoFormulario, c.ItemId, c.NombreFormulario, c.Orden_Item, c.Orden_Seccion, c.TituloSeccion
	from Conformado_Item_Sec_Form c

RETURN 0

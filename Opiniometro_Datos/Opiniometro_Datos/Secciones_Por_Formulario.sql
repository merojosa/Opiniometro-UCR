CREATE PROCEDURE [dbo].[Secciones_Por_Formulario]
	@ForCod char(6)
AS
BEGIN
	SET NOCOUNT ON
	SELECT C.TituloSeccion
	FROM Conformado_Item_Sec_Form C
	WHERE C.CodigoFormulario = @ForCod
END


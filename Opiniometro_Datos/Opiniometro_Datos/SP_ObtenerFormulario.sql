CREATE PROCEDURE [dbo].[SP_ObtenerFormulario]
	@cod char(6)
AS
BEGIN
	SET NOCOUNT ON
	SELECT Nombre
	FROM Formulario
	WHERE Formulario.CodigoFormulario = @cod
END

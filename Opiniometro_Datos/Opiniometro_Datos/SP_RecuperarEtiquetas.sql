CREATE PROCEDURE [dbo].[SP_RecuperarEtiquetas]
	@tipoPregunta INT,
	@idPregunta INT
AS
BEGIN
	IF @tipoPregunta = 2 --seleccion unica
		BEGIN
			SELECT	OpcionRespuesta
			FROM	Opciones_De_Respuestas_Seleccion_Unica 
			WHERE	ItemId = @idPregunta
		END
	ELSE IF @tipoPregunta = 3 --like dislike(si/no)
		BEGIN
			SELECT	OpcionRespuesta
			FROM	Opciones_De_Respuestas_Seleccion_Unica
			WHERE	ItemId = @idPregunta
		END
	ELSE IF @tipoPregunta = 4 --seleccion multiple
		BEGIN
			SELECT	OpcionRespuesta
			FROM	Opciones_De_Respuestas_Seleccion_Multiple
			WHERE	ItemID = @idPregunta
		END
	ELSE IF @tipoPregunta = 5 --Escalar
		BEGIN
			SELECT	Escalar.Inicio,Escalar.Fin
			FROM	Escalar 
			WHERE	ItemId = @idPregunta
		END
	ELSE IF @tipoPregunta = 6 --Escalar Estrella
		BEGIN
			SELECT	Escalar.Inicio,Escalar.Fin
			FROM	Escalar 
			WHERE	ItemId = @idPregunta
		END
END

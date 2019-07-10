GO
CREATE PROCEDURE SP_Insertar_Opciones_De_Respuestas_Seleccion_Unica
	@itemid NVARCHAR(10),
	@orden smallint,
	@opcionrespuesta NVARCHAR(150)
AS
	INSERT INTO Opciones_De_Respuestas_Seleccion_Unica( ItemId, Orden, OpcionRespuesta )
	VALUES	( @itemid, @orden, @opcionrespuesta );

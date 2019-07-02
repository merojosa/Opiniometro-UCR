/* Descomentar tras el merge con master

CREATE TYPE [dbo].[TuplasResponde] AS TABLE
	(
		idItem NVARCHAR (10),
		tituloSeccion NVARCHAR (120),
		fechaRespuesta DATE,
		codFormulario CHAR (6),
		cedEstudiante CHAR (9),
		cedProfesor CHAR (9),
		annoGrupo SMALLINT,
		semestreGrupo TINYINT,
		numGrupo TINYINT,
		siglaGrupo CHAR (6),
		observacion NVARCHAR(500),
		respuesta NVARCHAR(500),
		respuestaProfesor NVARCHAR(500)
	)

GO
CREATE PROCEDURE [dbo].[AlmacenarVariasRespuestas]
	@tuplasPorInsertar [dbo].[TuplasResponde] READONLY
AS
	INSERT INTO [dbo].[Responde] 
	(
		ItemId,
		TituloSeccion,
		FechaRespuesta,
		CodigoFormularioResp,
		CedulaPersona,
		CedulaProfesor,
		AnnoGrupoResp,
		SemestreGrupoResp,
		NumeroGrupoResp,
		SiglaGrupoResp,
		Observacion,
		Respuesta,
		RespuestaProfesor
	)
	SELECT 
		idItem,
		tituloSeccion,
		fechaRespuesta,
		codFormulario,
		cedEstudiante,
		cedProfesor,
		annoGrupo,
		semestreGrupo,
		numGrupo,
		siglaGrupo,
		observacion,
		respuesta,
		respuestaProfesor
	FROM @tuplasPorInsertar;
RETURN 0
GO
*/
CREATE PROCEDURE [dbo].[SP_ObtenerObservacion]
	@codFormulario CHAR(6),
	@codSeccion NVARCHAR(120),
	@cedProfesor CHAR(9),
	@annoGrupo SMALLINT,
	@semestreGrupo TINYINT,
	@numGrupo TINYINT,
	@siglaCurso CHAR(6),
	@itemId NVARCHAR(10)
AS
BEGIN
	SET NOCOUNT ON
	SELECT Observacion
	FROM Responde
	WHERE CodigoFormularioResp= @codFormulario AND Responde.TituloSeccion = @codSeccion AND CedulaProfesor= @cedProfesor AND AnnoGrupoResp= @annoGrupo AND 
			SemestreGrupoResp= @semestreGrupo AND NumeroGrupoResp= @numGrupo AND SiglaGrupoResp= @siglaCurso AND Responde.ItemId = @itemId
END

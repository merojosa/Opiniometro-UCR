CREATE VIEW [dbo].[RespondePorGruposView]
	AS 
	SELECT i.TipoPregunta, COUNT(r.Respuesta) as cntRespuestas
	FROM [Responde] as r JOIN [Item] as i on r.ItemId = i.ItemId
	GROUP BY r.CedulaProfesor, r.CodigoFormularioResp, r.SiglaGrupoResp, r.AnnoGrupoResp, r.SemestreGrupoResp, r.NumeroGrupoResp, i.TipoPregunta

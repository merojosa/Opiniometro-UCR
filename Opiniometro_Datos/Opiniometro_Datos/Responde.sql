﻿CREATE TABLE [dbo].[Responde]
(
	[ItemId] NVARCHAR(10) NOT NULL,
	[TituloSeccion] NVARCHAR(120) NOT NULL, 
    [FechaRespuesta] DATE NOT NULL, 
    [CodigoFormularioResp] CHAR(6) NOT NULL, 
    [CedulaPersona] CHAR(9) NOT NULL, 
    [CedulaProfesor] CHAR(9) NOT NULL, 
    [AnnoGrupoResp] SMALLINT NOT NULL , 
    [SemestreGrupoResp] TINYINT NOT NULL , 
    [NumeroGrupoResp] TINYINT NOT NULL , 
    [SiglaGrupoResp] CHAR(6) NOT NULL, 
    [Observacion] NVARCHAR(500) NULL, 
    [Respuesta] NVARCHAR(500) NOT NULL, 
    [RespuestaProfesor] NVARCHAR(500) NULL ,
	CONSTRAINT [PK_Responde] PRIMARY KEY([ItemId], [TituloSeccion], [FechaRespuesta], [CodigoFormularioResp], [CedulaPersona], [CedulaProfesor], [AnnoGrupoResp], [SemestreGrupoResp], [NumeroGrupoResp], [SiglaGrupoResp], [Respuesta]),
	CONSTRAINT [FK_Res_Ite] FOREIGN KEY ([ItemId])
	REFERENCES [Item] ([ItemId]), --ON DELETE SET DEFAULT ON UPDATE CASCADE,
	CONSTRAINT [FK_Res_Sec] FOREIGN KEY ([TituloSeccion])
	REFERENCES [Seccion] ([Titulo]), --ON DELETE CASCADE ON UPDATE CASCADE
	CONSTRAINT [FK_Res_ForRes] FOREIGN KEY ([FechaRespuesta], [CodigoFormularioResp], [CedulaPersona], [CedulaProfesor], [AnnoGrupoResp], [SemestreGrupoResp], [NumeroGrupoResp], [SiglaGrupoResp])
	REFERENCES [Formulario_Respuesta] ([Fecha], [CodigoFormulario], [CedulaPersona], [CedulaProfesor], [AnnoGrupo], [SemestreGrupo], [NumeroGrupo], [SiglaGrupo]) ON DELETE NO ACTION ON UPDATE CASCADE
)

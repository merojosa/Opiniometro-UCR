﻿CREATE TABLE [dbo].[Item]
(
	[ItemId] NVARCHAR(10) NOT NULL PRIMARY KEY,
	[TextoPregunta] NVARCHAR(120)	NOT NULL,
	[TieneObservacion] BIT,
	[TipoPregunta] TINYINT NOT NULL,
	[NombreCategoria] NVARCHAR(20) NOT NULL,
	[EtiquetaObservacion] NVARCHAR(25) NULL, 
    CONSTRAINT FK_Ite_Cat FOREIGN KEY([NombreCategoria]) REFERENCES [Categoria] ([NombreCategoria])  
)

CREATE TABLE [dbo].[Opciones_De_Respuestas_Seleccion_Multiple]
(
	ItemId NVARCHAR(10) NOT NULL,
	Orden SMALLINT NOT NULL,
	OpcionRespuesta NVARCHAR(150) NOT NULL
	CONSTRAINT PK_Opciones_De_Respuestas_Seleccion_Multiple PRIMARY KEY(ItemId, Orden, OpcionRespuesta)
	CONSTRAINT FK_OpcResMul_Ite FOREIGN KEY(ItemId) REFERENCES Seleccion_Multiple(ItemId)
		ON UPDATE CASCADE
)

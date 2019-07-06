CREATE TABLE [dbo].[Opciones_De_Respuestas_Seleccion_Unica]
(
	ItemId NVARCHAR(10) NOT NULL,
	Orden SMALLINT NOT NULL,
	OpcionRespuesta NVARCHAR(150) NOT NULL
	CONSTRAINT PK_Opciones_De_Respuestas_Seleccion_Unica PRIMARY KEY(ItemId, Orden, OpcionRespuesta)
	CONSTRAINT FK_OpcResUni_Ite FOREIGN KEY(ItemId) REFERENCES Seleccion_Unica(ItemId)
		ON UPDATE CASCADE
)

CREATE TABLE [dbo].[Opciones_De_Respuestas_Seleccion_Unica]
(
	ItemId NVARCHAR(10) NOT NULL,
	Orden TINYINT NOT NULL,
	OpcionRespuesta TINYINT NOT NULL
	CONSTRAINT PK_Opciones_De_Respuestas_Seleccion_Unica PRIMARY KEY(ItemId, Orden, OpcionRespuesta)
	CONSTRAINT FK_OpcResMul_Ite FOREIGN KEY(ItemId) REFERENCES Seleccion_Unica(ItemId)
		ON UPDATE CASCADE
)

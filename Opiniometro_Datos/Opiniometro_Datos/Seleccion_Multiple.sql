CREATE TABLE [dbo].[Seleccion_Multiple]
(
	[ItemId] NVARCHAR(10) NOT NULL,
	CONSTRAINT FK_Sel_Mul_Ite FOREIGN KEY (ItemId) REFERENCES Item(ItemId), 
	CONSTRAINT PK_Seleccion_Multiple PRIMARY KEY(ItemId)
)

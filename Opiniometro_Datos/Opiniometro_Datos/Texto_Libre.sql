﻿CREATE TABLE [dbo].[Texto_Libre]
(
	[ItemId] INT NOT NULL PRIMARY KEY,
	CONSTRAINT FK_Tex_Ite FOREIGN KEY (ItemId) REFERENCES Item(ItemID)
)

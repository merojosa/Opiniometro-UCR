CREATE TABLE [dbo].[Conformado_Item_Sec_Form]
(
	[ItemId] INT NOT NULL,
	[CodigoFormulario] CHAR(6) NOT NULL,
	TituloSeccion NVARCHAR(120) NOT NULL, 
	NombreFormulario VARCHAR(30),
	CONSTRAINT PK_Conformado_Item_Sec_Form PRIMARY KEY (ItemId,CodigoFormulario,TituloSeccion),
	CONSTRAINT FK_Con_Ite FOREIGN KEY(ItemId) REFERENCES Item(ItemId),
	CONSTRAINT FK_Con_For FOREIGN KEY(CodigoFormulario) REFERENCES Formulario(CodigoFormulario),
	CONSTRAINT FK_Con_Sec FOREIGN KEY(TituloSeccion) REFERENCES Seccion(Titulo)
	--Orden_Item 
	--Orden_Seccion
)
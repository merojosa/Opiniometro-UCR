CREATE TABLE [dbo].[Conformado_For_Sec]
(
	[CodigoFormulario] CHAR(6) NOT NULL,
	TituloSeccion NVARCHAR(120) NOT NULL, 
	Orden_Seccion INT,
	CONSTRAINT PK_Conformado_Sec_Form PRIMARY KEY (CodigoFormulario,TituloSeccion),
	CONSTRAINT FK_Conf_For FOREIGN KEY(CodigoFormulario) REFERENCES Formulario(CodigoFormulario)
		ON UPDATE CASCADE,
	CONSTRAINT FK_Conf_Sec FOREIGN KEY(TituloSeccion) REFERENCES Seccion(Titulo)
		ON UPDATE CASCADE,
)

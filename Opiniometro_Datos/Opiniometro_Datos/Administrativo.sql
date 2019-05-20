CREATE TABLE [dbo].[Administrativo]
(
	[CedulaAdministrativo]	CHAR(9)		NOT NULL,
	[Tipo]					VARCHAR(30)	NOT NULL,
	CONSTRAINT PK_Administrativo
		PRIMARY KEY (CedulaAdministrativo),
	CONSTRAINT FK_Adm_Per
		FOREIGN KEY (CedulaAdministrativo) REFERENCES Persona(Cedula)
			ON DELETE NO ACTION
			ON UPDATE CASCADE
)

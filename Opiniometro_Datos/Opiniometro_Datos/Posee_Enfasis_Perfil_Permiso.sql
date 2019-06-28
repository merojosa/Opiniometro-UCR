CREATE TABLE [dbo].[Posee_Enfasis_Perfil_Permiso]
(
	[NumeroEnfasis]	TINYINT			NOT NULL,
	[SiglaCarrera]	NVARCHAR(10)	NOT NULL,
	[NombrePerfil]	VARCHAR(30)		NOT NULL,
	[IdPermiso]		SMALLINT			NOT NULL,
	CONSTRAINT PK_PoseeEnfasisPerfil
		PRIMARY KEY (NumeroEnfasis, SiglaCarrera, [NombrePerfil], IdPermiso),
	CONSTRAINT FK_PoseeEnfasisPerfil_Perfil
		FOREIGN KEY ([NombrePerfil]) REFERENCES Perfil([Nombre])
			ON DELETE CASCADE
			ON UPDATE CASCADE,
	CONSTRAINT FK_PoseeEnfasisPerfil_Permiso
		FOREIGN KEY (IdPermiso) REFERENCES Permiso(Id)
			ON DELETE CASCADE
			ON UPDATE CASCADE,
	CONSTRAINT FK_PoseeEnfasisPerfil_Enfasis
		FOREIGN KEY (NumeroEnfasis, SiglaCarrera) REFERENCES Enfasis(Numero, SiglaCarrera)
			ON DELETE CASCADE
			ON UPDATE CASCADE
)

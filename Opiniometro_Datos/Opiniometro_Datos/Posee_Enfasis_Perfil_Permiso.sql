CREATE TABLE [dbo].[Posee_Enfasis_Perfil_Permiso]
(
	[NumeroEnfasis]	TINYINT			NOT NULL,
	[SiglaCarrera]	NVARCHAR(10)	NOT NULL,
	[IdPerfil]		VARCHAR(10)		NOT NULL,
	[IdPermiso]		TINYINT			NOT NULL,
	CONSTRAINT PK_PoseeEnfasisPerfil
		PRIMARY KEY (NumeroEnfasis, SiglaCarrera, IdPerfil, IdPermiso),
	CONSTRAINT FK_PoseeEnfasisPerfil_Perfil
		FOREIGN KEY (IdPerfil) REFERENCES Perfil(Id)
			ON DELETE NO ACTION
			ON UPDATE CASCADE,
	CONSTRAINT FK_PoseeEnfasisPerfil_Permiso
		FOREIGN KEY (IdPermiso) REFERENCES Permiso(Id)
			ON DELETE NO ACTION
			ON UPDATE CASCADE/*,
			--Lo siguiente tiene dependencias:
	CONSTRAINT FK_PoseeEnfasisPerfil_Enfasis
		FOREIGN KEY (NumeroEnfasis, SiglaCarrera) REFERENCES Enfasis(Numero, SiglaCarrera)
			ON DELETE NO ACTION
			ON UPDATE CASCADE*/
)

CREATE TABLE [dbo].[Tiene_Enfasis_Perfil_Usuario]
(
	[CorreoInstitucional]	NVARCHAR(50)	NOT NULL,
	[NumeroEnfasis]			TINYINT			NOT NULL,
	[SiglaCarrera]			NVARCHAR(10)	NOT NULL,
	[IdPerfil]				VARCHAR(10)		NOT NULL,
	CONSTRAINT PK_TieneEnfasisPerfilUsuario
		PRIMARY KEY (CorreoInstitucional, NumeroEnfasis, SiglaCarrera, IdPerfil),
	CONSTRAINT FK_TieneEnfasisPerfilUsuario_Usuario
		FOREIGN KEY (CorreoInstitucional) REFERENCES Usuario(CorreoInstitucional),
	/*CONSTRAINT FK_TieneEnfasisPerfilUsuario_Enfasis
		FOREIGN KEY (NumeroEnfasis, SiglaCarrera) REFERENCES Enfasis(NumeroEnfasis, SiglaCarrera),*/
	CONSTRAINT FK_TieneEnfasisPerfilUsuario_Perfil
		FOREIGN KEY (IdPerfil) REFERENCES Perfil(Id)
)

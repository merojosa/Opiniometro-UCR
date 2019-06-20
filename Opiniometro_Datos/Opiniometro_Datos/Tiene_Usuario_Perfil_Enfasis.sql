CREATE TABLE [dbo].[Tiene_Usuario_Perfil_Enfasis]
(
	[CorreoInstitucional] NVARCHAR(50) NOT NULL, 
    [NumeroEnfasis] TINYINT NOT NULL, 
    [SiglaCarrera] NVARCHAR(10) NOT NULL, 
    [NombrePerfil] VARCHAR(30) NOT NULL,

	CONSTRAINT PK_Tiene_Usuario_Perfil_Enfasis PRIMARY KEY ([CorreoInstitucional], [NumeroEnfasis], [SiglaCarrera], [NombrePerfil]),

	CONSTRAINT FK_CorreoInstitucional_Tiene_Usuario_Perfil_Enfasis FOREIGN KEY (CorreoInstitucional)
	REFERENCES Usuario(CorreoInstitucional),

	CONSTRAINT FK_NumeroEnfasis_SiglaCarrera_Tiene_Usuario_Perfil_Enfasis FOREIGN KEY (NumeroEnfasis, SiglaCarrera)
	REFERENCES Enfasis(Numero, SiglaCarrera),

	CONSTRAINT FK_IdPerfil_SiglaCarrera_Tiene_Usuario_Perfil_Enfasis FOREIGN KEY ([NombrePerfil])
	REFERENCES Perfil([Nombre])

)



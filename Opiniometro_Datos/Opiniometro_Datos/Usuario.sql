CREATE TABLE [dbo].[Usuario]
(
	[CorreoInstitucional]	NVARCHAR(50)	NOT NULL, 
    [Contrasena]			NVARCHAR(50)	NOT NULL, 
    [Activo]				BIT				NOT NULL, 
    [Cedula]				CHAR(9)			NOT NULL, 
    [Id]					UNIQUEIDENTIFIER NOT NULL
	CONSTRAINT PK_Usuario
		PRIMARY KEY (CorreoInstitucional),
	[RecuperarContrasenna] BIT NULL, 
    CONSTRAINT FK_Usu_Per
		FOREIGN KEY (Cedula) REFERENCES Persona(Cedula)
			ON DELETE NO ACTION
			ON UPDATE CASCADE
)

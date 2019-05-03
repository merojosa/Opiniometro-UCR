CREATE TABLE [dbo].[Estudiante]
(
	[Cedula_Estudiante] VARCHAR(9) NOT NULL PRIMARY KEY, 
    [Carne] VARCHAR(6) NOT NULL UNIQUE,
	
	CONSTRAINT [FK_Persona] FOREIGN KEY ([Cedula_Estudiante])
		REFERENCES [dbo].[Persona] (Cedula)	ON DELETE NO ACTION
											ON UPDATE CASCADE
)

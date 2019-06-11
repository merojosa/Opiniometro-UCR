CREATE TABLE [dbo].[Direccion_Persona]
(
	[Cedula]	CHAR(9) NOT NULL,
	[Provincia]	VARCHAR(50) NOT NULL,
	[Canton]	VARCHAR(50) NOT NULL,
	[Distrito]	VARCHAR(50) NOT NULL,
	CONSTRAINT PK_Direccion_Persona
		PRIMARY KEY (Cedula, Provincia, Canton, Distrito),
	CONSTRAINT FK_Dir_Dis 
		FOREIGN KEY (Provincia, Canton, Distrito) REFERENCES Distrito(NombreProvincia, NombreCanton, NombreDistrito),
	CONSTRAINT FK_Dir_Per
		FOREIGN KEY (Cedula) REFERENCES Persona(Cedula)
)

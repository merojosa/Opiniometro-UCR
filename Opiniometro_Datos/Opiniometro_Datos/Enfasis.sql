
CREATE TABLE [dbo].[Enfasis]
(
	[Numero] TINYINT NOT NULL , 
    [SiglaCarrera] NVARCHAR(10) NOT NULL,
	CONSTRAINT PK_Enfasis PRIMARY KEY (Numero,SiglaCarrera),
	CONSTRAINT FK_Enfasis_Carrera FOREIGN KEY (SiglaCarrera) 
	REFERENCES Carrera(Sigla) -- ON DELETE CASCADE ON UPDATE CASCADE
)
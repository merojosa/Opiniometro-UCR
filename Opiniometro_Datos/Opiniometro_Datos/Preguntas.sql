CREATE TABLE [dbo].[Preguntas]
(
	[Numero] INT NOT NULL, 
	[Planteamiento] NVARCHAR(120) NOT NULL, 
    [TipoPregunta] NCHAR(30) NULL, 
    [Categoria] NCHAR(30) NULL,
	PRIMARY KEY CLUSTERED ([Numero] ASC)
	)

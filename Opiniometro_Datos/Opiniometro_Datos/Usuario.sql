CREATE TABLE [dbo].[Usuario]
(
	[CorreoInstitucional] NVARCHAR(50) NOT NULL PRIMARY KEY, 
    [Contrasena] NVARCHAR(50) NOT NULL, 
    [Activo] BIT NOT NULL, 
    [Cedula] CHAR(9) NOT NULL, 
    [Id] UNIQUEIDENTIFIER NOT NULL
)

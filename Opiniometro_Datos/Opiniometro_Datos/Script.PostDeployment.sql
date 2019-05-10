EXEC sp_MSForEachTable 'DISABLE TRIGGER ALL ON ?'
GO
EXEC sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'
GO
EXEC sp_MSForEachTable 'DELETE FROM ?'
GO
EXEC sp_MSForEachTable 'ALTER TABLE ? CHECK CONSTRAINT ALL'
GO
EXEC sp_MSForEachTable 'ENABLE TRIGGER ALL ON ?'


-- PROCEDURES
IF OBJECT_ID('SP_AgregarUsuario') IS NOT NULL
	DROP PROCEDURE SP_AgregarUsuario
GO
CREATE PROCEDURE SP_AgregarUsuario
	@Correo			NVARCHAR(50),
	@Contrasenna	NVARCHAR(50),
	@Cedula			CHAR(9)
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Id UNIQUEIDENTIFIER=NEWID()

	INSERT INTO Usuario
	VALUES (@Correo, HASHBYTES('SHA2_512', @Contrasenna+CAST(@Id AS NVARCHAR(36))), 1, @Cedula, @Id)
END
GO

-- FUNCTIONS
IF OBJECT_ID('SF_LoginUsuario') IS NOT NULL
	DROP FUNCTION SF_LoginUsuario
GO
CREATE FUNCTION SF_LoginUsuario
	(@Correo NVARCHAR(50), @Contrasenna NVARCHAR(50))
RETURNS BIT
BEGIN
	DECLARE @CorreoBuscar NVARCHAR(50), @Result BIT

	-- Buscar que el correo y contrasenna calcen con lo que hay en la tabla Usuario.
	SET @CorreoBuscar =	(SELECT CorreoInstitucional 
						FROM Usuario
						WHERE CorreoInstitucional=@Correo AND Contrasena=HASHBYTES('SHA2_512', @Contrasenna+CAST(Id AS NVARCHAR(36))))
	IF(@CorreoBuscar IS NULL)	-- Si no calzan, no hay autenticacion.
		SET @Result = 0
	ELSE						-- Si hay autenticacion
		SET @Result = 1
	RETURN @Result
END;
	
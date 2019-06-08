CREATE VIEW ValorRandom
AS
SELECT randomvalue = CRYPT_GEN_RANDOM(10)
GO


CREATE FUNCTION SF_GenerarContrasena()
RETURNS NVARCHAR(10)
AS
BEGIN
	DECLARE @Resultado NVARCHAR(10);
	DECLARE @InfoBinario VARBINARY(10);
	DECLARE @DatosCaracteres NVARCHAR(10);

	SELECT @InfoBinario = randomvalue FROM ValorRandom;

	SET @DatosCaracteres = CAST ('' as xml).value('xs:base64Binary(sql:variable("@InfoBinario"))', 'varchar (max)');

	SET @Resultado = @DatosCaracteres;

	RETURN @Resultado;

END
GO

SELECT dbo.SF_GenerarContrasena()
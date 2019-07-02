-- Borrar todas las tuplas existentes en la base de datos para evitar repeticion de llaves primarias.
EXEC sp_MSForEachTable 'DISABLE TRIGGER ALL ON ?'
GO
EXEC sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'
GO
EXEC sp_MSForEachTable 'DELETE FROM ?'
GO
EXEC sp_MSForEachTable 'ALTER TABLE ? CHECK CONSTRAINT ALL'
GO
EXEC sp_MSForEachTable 'ENABLE TRIGGER ALL ON ?'

--PROCEDIMIENTOS

--The Strategists
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
	VALUES (@Correo, HASHBYTES('SHA2_512', @Contrasenna+CAST(@Id AS NVARCHAR(36))), 1, @Cedula, @Id, 0)
END
GO

IF OBJECT_ID('SP_LoginUsuario') IS NOT NULL
	DROP PROCEDURE SP_LoginUsuario
GO
CREATE PROCEDURE SP_LoginUsuario
	@Correo			NVARCHAR(50),
	@Contrasenna	NVARCHAR(50),
	@Resultado		BIT OUT
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @CorreoBuscar NVARCHAR(50)
	
	-- Buscar que el correo y la contrasenna sean correctos con lo que hay en la tabla Usuario.
	SET @CorreoBuscar =	(SELECT CorreoInstitucional 
						FROM Usuario
						WHERE CorreoInstitucional=@Correo AND Contrasena=HASHBYTES('SHA2_512', @Contrasenna+CAST(Id AS NVARCHAR(36))))

	IF(@CorreoBuscar IS NULL)	-- Si no calzan, no hay autenticacion.
		SET @Resultado = 0
	ELSE						-- Si hay autenticacion
		SET @Resultado = 1
END
GO

IF OBJECT_ID('SP_ExistenciaCorreo') IS NOT NULL
	DROP PROCEDURE SP_ExistenciaCorreo
GO
CREATE PROCEDURE SP_ExistenciaCorreo
	@Correo			NVARCHAR(50),
	@Resultado		BIT OUT
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @CorreoBuscar NVARCHAR(50)
	
	-- Buscar que el correo sea correcto con lo que hay en la tabla Usuario.
	SET @CorreoBuscar =	(SELECT CorreoInstitucional 
						FROM Usuario
						WHERE CorreoInstitucional=@Correo)

	IF(@CorreoBuscar IS NULL)	-- Si no calzan, no hay autenticacion.
		SET @Resultado = 0
	ELSE						-- Si hay autenticacion
		SET @Resultado = 1
END
GO

IF OBJECT_ID('SP_CambiarContrasenna') IS NOT NULL
	DROP PROCEDURE SP_CambiarContrasenna
GO
CREATE PROCEDURE SP_CambiarContrasenna
	@Correo					NVARCHAR(50),
	@Contrasenna_Nueva		NVARCHAR(50),
	@RecuperarContrasenna	BIT
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @CorreoBuscar NVARCHAR(50)
	
	-- Buscar que el correo y la contrasenna sean correctos con lo que hay en la tabla Usuario.
	SET @CorreoBuscar =	(SELECT CorreoInstitucional 
						FROM Usuario
						WHERE CorreoInstitucional=@Correo)

	IF(@CorreoBuscar IS NOT NULL)	-- Si existe el correo
	BEGIN
		UPDATE Usuario
		SET Contrasena = HASHBYTES('SHA2_512', @Contrasenna_Nueva+CAST(Id AS NVARCHAR(36))), RecuperarContrasenna = @RecuperarContrasenna
		WHERE CorreoInstitucional = @CorreoBuscar
	END	
END
GO

-- Modificar Persona
IF OBJECT_ID('SP_ModificarPersona') IS NOT NULL
	DROP PROCEDURE SP_ModificarPersona
GO
CREATE PROCEDURE SP_ModificarPersona
	@CedulaBusqueda		VARCHAR(9),
	@Cedula				CHAR(9),
	@Nombre				NVARCHAR(50),
	@Apellido1			NVARCHAR(50),
	@Apellido2			NVARCHAR(50),
	@Correo				NVARCHAR(100),
	@Direccion			NVARCHAR(256)
AS
BEGIN
	UPDATE Persona
	SET Cedula = @Cedula, Nombre = @Nombre, Apellido1 = @Apellido1, Apellido2 = @Apellido2, Direccion = @Direccion
	WHERE Cedula = @CedulaBusqueda;

	UPDATE Usuario
	SET CorreoInstitucional = @Correo
	WHERE Cedula = @CedulaBusqueda;
END
GO

--Trigger de validación de modificar Persona
IF OBJECT_ID('TR_ValidacionModificarPersona') IS NOT NULL
	DROP TRIGGER TR_ValidacionModificarPersona
GO
CREATE TRIGGER TR_ValidacionModificarPersona
ON Persona INSTEAD OF UPDATE
AS
BEGIN
	IF(UPDATE(Nombre) OR UPDATE(Apellido1) OR UPDATE(Apellido2) OR UPDATE(Direccion))
	BEGIN
		DECLARE @Cedula				CHAR(10)
		DECLARE @Nombre				NVARCHAR(51)
		DECLARE @Apellido1			NVARCHAR(51)
		DECLARE @Apellido2			NVARCHAR(51)
		DECLARE @Direccion			NVARCHAR(257)

		SET @Cedula				= (SELECT Cedula FROM inserted)
		SET @Nombre				= (SELECT Nombre FROM inserted)
		SET @Apellido1			= (SELECT Apellido1 FROM inserted)
		SET @Apellido2			= (SELECT Apellido2 FROM inserted)
		SET @Direccion			= (SELECT Direccion FROM inserted)

		BEGIN TRY
			IF((@nombre IS NOT NULL) AND (@Apellido1 IS NOT NULL) AND (@Apellido2 IS NOT NULL) AND (@Direccion IS NOT NULL) 
				AND (LEN(@Cedula) <= 9) AND (LEN(@Nombre) <= 50) AND (LEN(@Apellido1) <= 50) AND (LEN(@Apellido2) <= 50) AND (LEN(@Direccion) <= 256))
			BEGIN
				UPDATE Persona
				SET Cedula = @Cedula, Nombre = @Nombre, Apellido1 = @Apellido1, Apellido2 = @Apellido2, Direccion = @Direccion
			END
			ELSE
			BEGIN
				Raiserror('Los campos no pueden estar vacíos o exceden el tamaño permitido.', 16, 1)  
				Return  
			END
		END TRY

		BEGIN CATCH
			PRINT 'ERROR: ' + ERROR_MESSAGE();
		END CATCH
	END
END;

--Trigger de validación de modificar Usuario
IF OBJECT_ID('TR_ValidacionModificarUsuario') IS NOT NULL
	DROP TRIGGER TR_ValidacionModificarUsuario
GO
CREATE TRIGGER TR_ValidacionModificarUsuario
ON Usuario INSTEAD OF UPDATE
AS
BEGIN
	IF(UPDATE(CorreoInstitucional))
	BEGIN
		DECLARE @Correo				NVARCHAR(101)

		SET @Correo				= (SELECT Correo FROM inserted)

		BEGIN TRY
			IF((@correo IS NOT NULL) AND (@correo LIKE '%@ucr.ac.cr') AND (LEN(Correo) <= 100))
			BEGIN
				UPDATE Usuario
				SET Correo = @Correo
			END
			ELSE
			BEGIN
				RAISERROR('El correo no puede estar vacío y debe ser del tipo "nombre@ucr.ac.cr".', 16, 1)  
				RETURN  
			END
		END TRY

		BEGIN CATCH
			PRINT 'ERROR: ' + ERROR_MESSAGE();
		END CATCH
	END
END;

IF OBJECT_ID('SP_ObtenerPermisosUsuario') IS NOT NULL
	DROP PROCEDURE SP_ObtenerPermisosUsuario
GO
CREATE PROCEDURE SP_ObtenerPermisosUsuario
	@Correo		NVARCHAR(50),
	@Perfil		VARCHAR(30)
AS
	SELECT TU.SiglaCarrera, PE.NumeroEnfasis, PE.IdPermiso
	FROM Tiene_Usuario_Perfil_Enfasis TU	JOIN Perfil ON TU.NombrePerfil = Perfil.Nombre
											JOIN Posee_Enfasis_Perfil_Permiso PE ON Perfil.Nombre = PE.NombrePerfil
	WHERE TU.CorreoInstitucional=@Correo AND TU.NombrePerfil = @Perfil
GO

IF OBJECT_ID('SP_ObtenerNombre') IS NOT NULL
	DROP PROCEDURE SP_ObtenerNombre
GO
CREATE PROCEDURE SP_ObtenerNombre
	@Correo			NVARCHAR(50),
	@Nombre			NVARCHAR(50) OUT,
	@Apellido		NVARCHAR(50) OUT
AS
BEGIN
	SET NOCOUNT ON
	
	SET @Nombre = (SELECT Nombre
	FROM Usuario U	JOIN Persona P ON U.Cedula = p.Cedula
	WHERE U.CorreoInstitucional=@Correo)

	SET @Apellido = (SELECT Apellido1
	FROM Usuario U	JOIN Persona P ON U.Cedula = p.Cedula
	WHERE U.CorreoInstitucional=@Correo)
END
GO

--EXEC SP_ModificarPersona @CedulaBusqueda = '987654321', @Cedula='987654321', @Nombre='Barry2', @Apellido1='Allen2', @Apellido2='Garcia2', @Direccion='Central City2';

IF OBJECT_ID('ValorRandom') IS NOT NULL
	DROP VIEW ValorRandom
GO
CREATE VIEW ValorRandom
AS
SELECT randomvalue = CRYPT_GEN_RANDOM(10)
GO

IF OBJECT_ID('SF_GenerarContrasena') IS NOT NULL
	DROP FUNCTION SF_GenerarContrasena
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

IF OBJECT_ID('SP_AgregarPersonaUsuario') IS NOT NULL
	DROP PROCEDURE SP_AgregarPersonaUsuario
GO
CREATE PROCEDURE SP_AgregarPersonaUsuario
	@Correo			NVARCHAR(50),
	@Contrasenna	NVARCHAR(50),
	@Cedula			CHAR(9),
	@Nombre			NVARCHAR(50),
	@Apellido1		NVARCHAR(50),
	@Apellido2		NVARCHAR(50),
	@Direccion		NVARCHAR(256)
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Id UNIQUEIDENTIFIER=NEWID()

	INSERT INTO Persona
	VALUES (@Cedula, @Nombre, @Apellido1, @Apellido2, @Direccion)

	INSERT INTO Usuario
	VALUES (@Correo, HASHBYTES('SHA2_512', @Contrasenna+CAST(@Id AS NVARCHAR(36))), 1, @Cedula, @Id, 0)
END
GO


IF OBJECT_ID('ObtenerPerfilUsuario') IS NOT NULL
	DROP PROCEDURE ObtenerPerfilUsuario
GO
CREATE PROCEDURE ObtenerPerfilUsuario @correo nvarchar(100)
as 
	SELECT DISTINCT NombrePerfil
	FROM Tiene_Usuario_Perfil_Enfasis
	WHERE CorreoInstitucional = @correo;
go

IF OBJECT_ID('ObtenerPerfilPorDefecto') IS NOT NULL
	DROP PROCEDURE ObtenerPerfilPorDefecto
GO
CREATE PROCEDURE ObtenerPerfilPorDefecto @correo nvarchar(100)
as 
	SELECT TOP 1 NombrePerfil 
	FROM Tiene_Usuario_Perfil_Enfasis
	WHERE CorreoInstitucional = @correo;
go

--JJAPH
IF OBJECT_ID('MostrarEstudiantes', 'P') IS NOT NULL 
	DROP PROC MostrarEstudiantes

IF OBJECT_ID('NombrePersona', 'P') IS NOT NULL 
	DROP PROC NombrePersona

IF OBJECT_ID('DatosEstudiante', 'P') IS NOT NULL 
	DROP PROC DatosEstudiante

GO
--Pantalla 1, Home
CREATE PROCEDURE MostrarEstudiantes
AS 
	SELECT Nombre, Apellido1, Apellido2, Carne
	FROM Persona P JOIN Estudiante E ON P.Cedula = E.CedulaEstudiante;
GO

--Pantalla 2 solo el nombre para bienvenida
GO
CREATE PROCEDURE NombrePersona 
@Cedula VARCHAR(9)
AS
	SELECT Nombre
	FROM Persona
	WHERE Cedula = @Cedula;

--Pantalla 3, informacion de un estudiante
GO
CREATE PROCEDURE DatosEstudiante
@Cedula VARCHAR(9)
AS
	SELECT CONCAT(Nombre, ' ' ,Apellido1, ' ', Apellido2) as 'Nombre Completo', Carne, Cedula
	FROM Persona P JOIN Estudiante E ON P.Cedula = E.CedulaEstudiante
	WHERE Cedula = @Cedula;
GO

GO
IF OBJECT_ID('ObtenerPerfilesUsuario') IS NOT NULL
	DROP PROCEDURE ObtenerPerfilesUsuario
GO
CREATE PROC ObtenerPerfilesUsuario
	@Correo	NVARCHAR(50)
AS
	SELECT SiglaCarrera, NumeroEnfasis
	FROM Tiene_Usuario_Perfil_Enfasis
	WHERE CorreoInstitucional= @Correo
GO

--Inserciones

INSERT INTO Persona
VALUES	('116720500', 'Jose Andrés', 'Mejías', 'Rojas', 'Desamparados de Alajuela.'),
		('115003456', 'Daniel', 'Escalante', 'Perez', 'Desamparados de San José.'),
		('117720910', 'Jose Andrés', 'Mejías', 'Rojas', 'La Fortuna de San Carlos.'),
		('236724507', 'Jose Andrés', 'Mejías', 'Rojas', 'Sarchí, Alajuela.'),
		--Agregado de datos para visualizacion a cargo de CX Solutions
		('100000001', 'CX', 'Solutions', 'S.A.', 'San Pedro Montes de Oca'),
		('100000002', 'Marta', 'Rojas', 'Sanches', '300 metros norte de Pulmitan'),--Profesora
		--Estudiantes
		('100000003', 'Juan', 'Briceño', 'Lupon', '400 metros norte del Heraldo de la Grieta'),
		('100000005', 'Pepito', 'Fonsi', 'Monge', '20 metros norte del Blue del lado Rojo'),
		('100000004', 'Maria', 'Fallas', 'Merdi', 'Costado este del estandarte de top'),
		('117720912', 'Jorge', 'Solano', 'Carrillo', 'La Fortuna de San Carlos.'),
		('236724501', 'Carolina', 'Gutierrez', 'Lozano', 'Sarchí, Alajuela.'),
		('123456789', 'Ortencia', 'Cañas', 'Griezman', 'San Pedro de Montes de Oca');
		
INSERT INTO Estudiante VALUES 
 ('116720500', 'B11111')
,('115003456', 'B22222')
,('117720910', 'B33333') 
,('236724501', 'B44444');

--INSERT INTO Responde (ItemId,TituloSeccion, FechaRespuesta, CodigoFormularioResp, CedulaPersona, CedulaProfesor, AnnoGrupoResp, SemestreGrupoResp, NumeroGrupoResp, SiglaGrupoResp)
--VALUES (3,'Titulo Seccion 1','8-12-1994','CodF01','123456789','987654321',54,01,21,'SG1234'),
--	   (1,'Titulo Seccion 2','8-12-1998','CodF02','123789659','987678951',59,01,20,'SG2345'),
--	   (2,'Titulo Seccion 3','8-12-1997','CodF03','135786789','982455321',51,01,15,'SG3456');

EXEC SP_AgregarUsuario @Correo='jose.mejiasrojas@ucr.ac.cr', @Contrasenna='123456', @Cedula='116720500'
EXEC SP_AgregarUsuario @Correo='daniel.escalanteperez@ucr.ac.cr', @Contrasenna='Danielito', @Cedula='115003456'
EXEC SP_AgregarUsuario @Correo='rodrigo.cascantejuarez@ucr.ac.cr', @Contrasenna='contrasena', @Cedula='117720910'
EXEC SP_AgregarUsuario @Correo='luis.quesadaborbon@ucr.ac.cr', @Contrasenna='LigaDeportivaAlajuelense', @Cedula='236724501'
EXEC SP_AgregarUsuario @Correo='admin@ucr.ac.cr', @Contrasenna='adminUCR2019', @Cedula='123456789'
EXEC SP_AgregarUsuario @Correo= 'cx@cx.solutions', @Contrasenna= 'CXSolutions', @Cedula= '100000001'



--Script JJAPH

--Unidad academica
INSERT INTO Unidad_Academica (Codigo, Nombre)
VALUES ('UC-023874', 'ECCI')

INSERT INTO Unidad_Academica (Codigo, Nombre)
VALUES ('UC-485648', 'Derecho')

INSERT Curso
VALUES  ('CI1213', 'Ingenieria de Software', 1, 'UC-023874'),
('CI1223', 'Bases de Datos', 1, 'UC-023874'),
 ('CI1211', 'Proyecto Integrador', 1, 'UC-023874')

--Escuela
--INSERT INTO Escuela(CodigoUnidadAcademica,CodigoFacultad)
--VALUES ('UC-023874','UC-023874')

--INSERT INTO Escuela(CodigoUnidadAcademica,CodigoFacultad)
--VALUES ('UC-485648','UC-485648')

--Facultad
INSERT INTO Facultad (CodigoUnidadAcademica)
VALUES ('UC-023874')

INSERT INTO Facultad (CodigoUnidadAcademica)
VALUES ('UC-485648')

--Carrera
INSERT INTO Carrera(Sigla, Nombre, CodigoUnidadAcademica)
VALUES ('SC-01234', 'Ciencias de la Computación e Informática','UC-023874')

INSERT INTO Enfasis
VALUES	(0, 'SC-01234'),
		(1, 'SC-01234')

INSERT INTO Carrera(Sigla, Nombre, CodigoUnidadAcademica)
VALUES ('SC-01235', 'Computación con varios Énfasis','UC-023874')

INSERT INTO Carrera(Sigla, Nombre, CodigoUnidadAcademica)
VALUES ('SC-89457', 'Derecho','UC-485648')

--Cursos
INSERT INTO Curso (Sigla, Nombre, Tipo,CodigoUnidad)
VALUES ('CI1330', 'Ingenieria de software', 1,'UC-023874')

INSERT INTO Curso (Sigla, Nombre, Tipo,CodigoUnidad)
VALUES ('CI1331', 'Bases de datos', 1,'UC-023874')

INSERT INTO Curso (Sigla, Nombre, Tipo,CodigoUnidad)
VALUES ('CI1327', 'Programacion 1', 1,'UC-023874')

INSERT INTO Curso (Sigla, Nombre, Tipo,CodigoUnidad)
VALUES ('CI1328', 'Programacion Paralela y concurrente', 1,'UC-023874')

INSERT INTO Curso (Sigla, Nombre, Tipo,CodigoUnidad)
VALUES ('DE1001', 'INTRODUCCIÓN AL ESTUDIO DEL DERECHO I', 1,'UC-485648')

INSERT INTO Curso (Sigla, Nombre, Tipo,CodigoUnidad)
VALUES ('DE2001', 'PRINCIPIOS DEL DERECHO PRIVADO I', 1,'UC-485648')

----Grupos
insert into Ciclo_Lectivo (Anno, Semestre)
values (2018, 1), (2018, 2), (2019, 1);

insert into Grupo (SiglaCurso, Numero, AnnoGrupo, SemestreGrupo)
values 
	((select c.Sigla from Curso c where c.Sigla = 'CI1327'), 1, 2018, 1),
	((select c.Sigla from Curso c where c.Sigla = 'CI1327'), 2, 2018, 1),
	((select c.Sigla from Curso c where c.Sigla = 'CI1327'), 1, 2018, 2),
	((select c.Sigla from Curso c where c.Sigla = 'CI1327'), 2, 2018, 2),
	((select c.Sigla from Curso c where c.Sigla = 'CI1327'), 1, 2019, 1),
	((select c.Sigla from Curso c where c.Sigla = 'CI1327'), 2, 2019, 1),
--INSERT INTO Grupo(SiglaCurso, Numero, AnnoGrupo, SemestreGrupo)
--VALUES ('CI1330', 2, 2018, 1)

	((select c.Sigla from Curso c where c.Sigla = 'CI1328'), 1, 2018, 2),
	((select c.Sigla from Curso c where c.Sigla = 'CI1328'), 2, 2018, 2),
	((select c.Sigla from Curso c where c.Sigla = 'CI1328'), 1, 2019, 1),

	((select c.Sigla from Curso c where c.Sigla = 'CI1330'), 1, 2019, 1),

	((select c.Sigla from Curso c where c.Sigla = 'CI1331'), 1, 2019, 1),

	((select c.Sigla from Curso c where c.Sigla = 'DE1001'), 1, 2018, 1),
	((select c.Sigla from Curso c where c.Sigla = 'DE1001'), 1, 2019, 1),

	((select c.Sigla from Curso c where c.Sigla = 'DE2001'), 1, 2018, 2);

----Enfasis
INSERT INTO Enfasis(Numero, SiglaCarrera)
VALUES (100, 'SC-01234'), 
	   (101, 'SC-01234'),
	   (12, 'SC-89457');

----Curso-Enfasis
INSERT INTO Se_Encuentra_Curso_Enfasis(SiglaCurso, CodigoEnfasis, SiglaCarrera)
VALUES ('CI1330', 100, 'SC-01234'), 
	   ('CI1331', 100, 'SC-01234'),
	   ('CI1327', 101, 'SC-01234'),
	   ('CI1328', 101, 'SC-01234'),
	   ('DE1001', 12, 'SC-89457'),
	   ('DE2001', 12, 'SC-89457');

--DROP PROCEDURE CursosSegunCarrera
--Obtiene la lista de cursos que pertenecen a cierta carrera

IF OBJECT_ID('CursosSegunCarrera') IS NOT NULL
	DROP PROCEDURE CursosSegunCarrera
GO
CREATE PROCEDURE CursosSegunCarrera
@siglaCarrera NVARCHAR(10)
AS
BEGIN
SELECT C.Nombre, C.Sigla, C.Tipo, C.CodigoUnidad
FROM Curso C
WHERE C.Sigla IN (SELECT S.SiglaCurso
				FROM Se_Encuentra_Curso_Enfasis S
				WHERE S.SiglaCarrera = @siglaCarrera)
END
GO

--Obtiene la lista de cursos que pertenecen a cierto semestre
IF OBJECT_ID('CursosSegunSemestre') IS NOT NULL
	DROP PROCEDURE CursosSegunSemestre
GO
CREATE PROCEDURE CursosSegunSemestre
@semestre TINYINT
AS
BEGIN
SELECT C.Nombre, C.Sigla, C.Tipo, C.CodigoUnidad
FROM Curso C
WHERE C.Sigla IN (SELECT G.SiglaCurso
				FROM Grupo G
				WHERE G.SemestreGrupo = @semestre)
END
GO

--Obtiene la lista de cursos que pertenecen a cierto año
IF OBJECT_ID('CursosSegunAnno') IS NOT NULL
	DROP PROCEDURE CursosSegunAnno
GO
CREATE PROCEDURE CursosSegunAnno
@anno SMALLINT
AS
BEGIN
SELECT C.Nombre, C.Sigla, C.Tipo, C.CodigoUnidad
FROM Curso C
WHERE C.Sigla IN (SELECT G.SiglaCurso
				FROM Grupo G
				WHERE G.AnnoGrupo = @anno)
END
GO


--Script C.X. Solutions

--Ciclo Lectivo
INSERT INTO Ciclo_Lectivo (Anno, Semestre)
VALUES  (2017, 2);

--Grupo
INSERT INTO Grupo(SiglaCurso, Numero, AnnoGrupo, SemestreGrupo)
VALUES ('CI1330', 1, 2017, 2);

--Categoria
INSERT INTO Categoria
VALUES  ('Reglamento'),
		('Curso'),
		('Opinion');

--Item
INSERT INTO Item(ItemId, TextoPregunta, TieneObservacion, TipoPregunta, NombreCategoria)
VALUES  ('PRE303', '¿El profesor repuso clases cuando fue necesario?', 1, 3, 'Reglamento'),
		('PRE404', '¿El profesor entrego la carta del estudiante en las fechas indicadas por el reglamento?', 1, 3, 'Reglamento'),
		('PRE101', '¿Que opina del curso?', 0, 1, 'Opinion'),
		('PRE202', '¿Que opina del profesor?', 0, 1, 'Opinion'),
		---Se agregó
		('PRE505', 'Año de carrera que cursa', 0, 4, 'Opinion'),
		('PRE606', 'Condicion laboral', 0, 4, 'Opinion'),
		--Escalar y Escalar Estrella
		('PRE707', '¿Se prepara adecuadamente para las evaluaciones?', 1, 5, 'Opinion'),
		('PRE808', '¿Propone actividades que involucren investigacion?', 1, 6, 'Reglamento');

--Item-Texto Libre
INSERT INTO Texto_Libre (ItemId)
VALUES  ('PRE101'),
		('PRE202');

--Item-Si/no
INSERT INTO Seleccion_Unica (ItemId, IsaLikeDislike)
VALUES  ('PRE303', 1),
		('PRE404', 1);

--Seccion
INSERT INTO Seccion (Titulo, Descripcion)
VALUES  ('Evaluación de aspectos reglamentarios del profesor', 'Conteste a las preguntas relacionadas a aspectos reglamentarios que el profesor debe cumplir.'),
		('Opinion general del curso', 'Describa las opiniones que le han generado el profesor con respecto al curso tratado.'),
		--Nueva Seccion
		('Información del o la estudiante', 'Datos del estudiante'),
		('Tematicas transversales de la Universidad de Costa Rica', ' '),
		('Evaluacion de la participacion estudiantil', 'Autoevaluacion estudiantil');

--Formulario
INSERT INTO Formulario (CodigoFormulario, Nombre)
VALUES  ('131313', 'Evaluación de Profesores');

--Profesor
INSERT INTO Profesor (CedulaProfesor)
VALUES  ('100000002');

--Formulario Respuesta
INSERT INTO Formulario_Respuesta (Fecha, CodigoFormulario, CedulaPersona, CedulaProfesor, AnnoGrupo, SemestreGrupo, NumeroGrupo, SiglaGrupo, Completado)
VALUES  ('2017-4-5', '131313', '100000003', '100000002', 2017, 2, 1, 'CI1330', 1),
		('2017-3-6', '131313', '100000004', '100000002', 2017, 2, 1, 'CI1330', 1),
		('2017-4-18', '131313', '100000005', '100000002', 2017, 2, 1, 'CI1330', 1),
		('2017-3-20', '131313', '117720912', '100000002', 2017, 2, 1, 'CI1330', 1),
		('2017-3-20', '131313', '236724501', '100000002', 2017, 2, 1, 'CI1330', 1),
		('2017-3-21', '131313', '123456789', '100000002', 2017, 2, 1, 'CI1330', 1);

 --Conformado_Item_Sec_Form
INSERT INTO Conformado_Item_Sec_Form (ItemId, CodigoFormulario, TituloSeccion, NombreFormulario)
VALUES	('PRE101', '131313', 'Opinion general del curso', 'Evaluación de Profesores'),
		('PRE202', '131313', 'Opinion general del curso', 'Evaluación de Profesores'),
		('PRE303', '131313', 'Evaluación de aspectos reglamentarios del profesor', 'Evaluación de Profesores'),	
		('PRE404', '131313', 'Evaluación de aspectos reglamentarios del profesor', 'Evaluación de Profesores'),
		('PRE505', '131313', 'Información del o la estudiante', 'Evaluación de Profesores'),
		('PRE606', '131313', 'Información del o la estudiante', 'Evaluación de Profesores'),
		('PRE707', '131313', 'Evaluacion de la participacion estudiantil', 'Evaluación de Profesores'),
		('PRE808', '131313', 'Tematicas transversales de la Universidad de Costa Rica', 'Evaluación de Profesores');

--Responde
INSERT INTO Responde (ItemId, TituloSeccion, FechaRespuesta, CodigoFormularioResp, CedulaPersona, CedulaProfesor, AnnoGrupoResp, SemestreGrupoResp, NumeroGrupoResp, SiglaGrupoResp, Respuesta, Observacion)
VALUES  ('PRE303', 'Evaluación de aspectos reglamentarios del profesor', '2017-4-5', '131313', '100000003', '100000002', 2017, 2, 1, 'CI1330', '3', 'Nunca tuvimos que reponer clases'),
		('PRE404', 'Evaluación de aspectos reglamentarios del profesor', '2017-4-5', '131313', '100000003', '100000002', 2017, 2, 1, 'CI1330', '2', 'La profesora olvido enviar la carta del estudiante pero si la revisamos en la primera semana de clases'),
		('PRE101', 'Opinion general del curso', '2017-4-5', '131313', '100000003', '100000002', 2017, 2, 1, 'CI1330', '', 'La materia estuvo muy interesante y espero poder aplicarla en el futuro en el trabajo'),
		('PRE202', 'Opinion general del curso', '2017-4-5', '131313', '100000003', '100000002', 2017, 2, 1, 'CI1330', '', 'La profesora tardo mucho para devolver las evaluaciones'),
		--Segunda evaluacion
		('PRE303', 'Evaluación de aspectos reglamentarios del profesor', '2017-3-6', '131313', '100000004', '100000002', 2017, 2, 1, 'CI1330', '2', 'No fue necesario reponer clases'),
		('PRE404', 'Evaluación de aspectos reglamentarios del profesor', '2017-3-6', '131313', '100000004', '100000002', 2017, 2, 1, 'CI1330', '1', 'Revisamos la carta del estudiante en la primera semana'),
		('PRE101', 'Opinion general del curso', '2017-3-6', '131313', '100000004', '100000002', 2017, 2, 1, 'CI1330', '', 'No estoy seguro de si en el ambiente laboral me servira la materia'),
		('PRE202', 'Opinion general del curso', '2017-3-6', '131313', '100000004', '100000002', 2017, 2, 1, 'CI1330', '', 'La profesora logro que las clases fueran muy entretenidas y dinámicas'),
		--Tercera evaluacion
		('PRE303', 'Evaluación de aspectos reglamentarios del profesor', '2017-4-18', '131313', '100000005', '100000002', 2017, 2, 1, 'CI1330', '1', 'Me repuso una clase a la que falte'),
		('PRE404', 'Evaluación de aspectos reglamentarios del profesor', '2017-4-18', '131313', '100000005', '100000002', 2017, 2, 1, 'CI1330', '1', 'Sí se reviso'),
		('PRE101', 'Opinion general del curso', '2017-4-18', '131313', '100000005', '100000002', 2017, 2, 1, 'CI1330', '', 'Entretenido'),
		('PRE202', 'Opinion general del curso', '2017-4-18', '131313', '100000005', '100000002', 2017, 2, 1, 'CI1330', '', 'Muy buena profesora'),

		--Agregado
		--Cuarta Multiple
		('PRE505', 'Información del o la estudiante', '2017-4-18', '131313', '100000005', '100000002', 2017, 2, 1, 'CI1330', 'Primero', ''),
		('PRE606', 'Información del o la estudiante', '2017-4-18', '131313', '100000005', '100000002', 2017, 2, 1, 'CI1330', 'No Trabajo', ''),
		('PRE505', 'Información del o la estudiante', '2017-3-6', '131313', '100000004', '100000002', 2017, 2, 1, 'CI1330', 'Segundo', ''),
		('PRE505', 'Información del o la estudiante', '2017-4-5', '131313', '100000003', '100000002', 2017, 2, 1, 'CI1330', 'Tercero', ''),
		('PRE606', 'Información del o la estudiante', '2017-3-6', '131313', '100000004', '100000002', 2017, 2, 1, 'CI1330', 'Trabajo mas de 20 horas semanales', ''),
		('PRE606', 'Información del o la estudiante', '2017-4-5', '131313', '100000003', '100000002', 2017, 2, 1, 'CI1330', 'Trabajo menos de 20 horas semanales', ''),
		--Escalar 5 y 10
		('PRE707', 'Evaluacion de la participacion estudiantil', '2017-4-18', '131313', '100000005', '100000002', 2017, 2, 1, 'CI1330', 'Demasiado', 'Soy un sapazo'),
		('PRE707', 'Evaluacion de la participacion estudiantil', '2017-3-6', '131313', '100000004', '100000002', 2017, 2, 1, 'CI1330', 'Poco', 'Me da pereza estudiar'),
		('PRE707', 'Evaluacion de la participacion estudiantil', '2017-4-5', '131313', '100000003', '100000002', 2017, 2, 1, 'CI1330', 'A veces', 'Siempre le pongo'),
		('PRE707', 'Evaluacion de la participacion estudiantil', '2017-3-20', '131313', '117720912', '100000002', 2017, 2, 1, 'CI1330', 'Poco', 'Sí se reviso'),
		--1 a 10
		('PRE808', 'Tematicas transversales de la Universidad de Costa Rica', '2017-4-18', '131313', '100000005', '100000002', 2017, 2, 1, 'CI1330', '8', 'Me encanta la materia'),
		('PRE808', 'Tematicas transversales de la Universidad de Costa Rica', '2017-3-20', '131313', '117720912', '100000002', 2017, 2, 1, 'CI1330', '4', 'Mucho que investigar'),
		('PRE808', 'Tematicas transversales de la Universidad de Costa Rica', '2017-3-6', '131313', '100000004', '100000002', 2017, 2, 1, 'CI1330', '3', 'Soy muy vago'),
		('PRE808', 'Tematicas transversales de la Universidad de Costa Rica', '2017-4-5', '131313', '100000003', '100000002', 2017, 2, 1, 'CI1330', '1', 'Demasiado trabajo'),
		('PRE808', 'Tematicas transversales de la Universidad de Costa Rica', '2017-3-20', '131313', '236724501', '100000002', 2017, 2, 1, 'CI1330', '7', 'Me encanta la materia'),
		('PRE808', 'Tematicas transversales de la Universidad de Costa Rica', '2017-3-21', '131313', '123456789', '100000002', 2017, 2, 1, 'CI1330', '5', 'Mucho que investigar');

GO
IF OBJECT_ID('SP_ContarRespuestasPorGrupo') IS NOT NULL
	DROP PROCEDURE SP_ContarRespuestasPorGrupo

--REQ: La Base de datos creada.
--EFE: Retorna la cantidad de respuestas por pregunta de un grupo especifico.
--MOD:--
GO
CREATE PROCEDURE SP_ContarRespuestasPorGrupo
	@codigoFormulario	CHAR(6),
	@cedulaProfesor		CHAR(9),
	@annoGrupo			SMALLINT,
	@semestreGrupo		TINYINT,
	@numeroGrupo		TINYINT,
	@siglaCurso			CHAR(6),
	@itemId				NVARCHAR(10)
AS
BEGIN
	SET NOCOUNT ON
	SELECT e.Respuesta, COUNT(e.Respuesta) AS cntResp
	FROM Responde as e
	WHERE e.CodigoFormularioResp= @codigoFormulario AND e.CedulaProfesor= @cedulaProfesor AND e.AnnoGrupoResp= @annoGrupo AND e.SemestreGrupoResp= @semestreGrupo AND e.NumeroGrupoResp= @numeroGrupo AND e.SiglaGrupoResp= @siglaCurso AND e.ItemId= @itemId
	GROUP BY e.CodigoFormularioResp, e.CedulaProfesor, e.AnnoGrupoResp, e.SemestreGrupoResp, e.NumeroGrupoResp, e.SiglaGrupoResp, e.ItemId, e.Respuesta
END
GO

GO
IF OBJECT_ID('SP_DevolverObservacionesPorGrupo') IS NOT NULL
	DROP PROCEDURE SP_DevolverObservacionesPorGrupo

--REQ: La base de datos creada.
--EFE: Retorna las observaciones por pregunta de un grupo especifico.
--MOD:--
GO
CREATE PROCEDURE SP_DevolverObservacionesPorGrupo
	@codigoFormulario CHAR(6),
	@cedulaProfesor CHAR(9),
	@annoGrupo SMALLINT,
	@semestreGrupo TINYINT,
	@numeroGrupo TINYINT,
	@siglaCurso CHAR(6),
	@itemId NVARCHAR(10)
AS
BEGIN
	SET NOCOUNT ON
	SELECT e.Observacion
	FROM Responde as e
	WHERE e.CodigoFormularioResp= @codigoFormulario AND e.CedulaProfesor= @cedulaProfesor AND e.AnnoGrupoResp= @annoGrupo AND e.SemestreGrupoResp= @semestreGrupo AND e.NumeroGrupoResp= @numeroGrupo AND e.SiglaGrupoResp= @siglaCurso AND e.ItemId= @itemId
END
GO


--Permisos
INSERT INTO Permiso
VALUES	(1, 'Hacer todo'),
		(2, 'Asignar formulario'),
		(3, 'Calificar cursos'),
		(4, 'Ver cursos'),
		(202, 'VerInformacionPersonas'),
		(203, 'InsertarUsuario'),
		(204, 'AsignarFormulario'),
		(205, 'VisualizarResultadosDeEvaluaciones'),
		(206, 'VerSecciones'),
		(207, 'VerItems'),
		(208, 'InsertarFormulario'),
		(209, 'Eliminar perfiles');

INSERT INTO Perfil
VALUES	('Estudiante', 'Calificar y ver evaluaciones.'),
		('Administrador', 'Puede hacer cualquier cosa como asignar permisos.'),
		('Profesor', 'Ver sus evaluaciones.')


INSERT INTO Tiene_Usuario_Perfil_Enfasis
VALUES	('jose.mejiasrojas@ucr.ac.cr', 0, 'SC-01234', 'Estudiante'),
		('admin@ucr.ac.cr', 0, 'SC-01234', 'Administrador'),
		('jose.mejiasrojas@ucr.ac.cr', 0, 'SC-01234', 'Profesor'),
		('jose.mejiasrojas@ucr.ac.cr', 0, 'SC-01234', 'Administrador')


INSERT INTO Posee_Enfasis_Perfil_Permiso
VALUES	(0, 'SC-01234', 'Estudiante', 3),
		(0, 'SC-01234', 'Administrador', 1),
		(0, 'SC-01234', 'Administrador', 2),
		(0, 'SC-01234', 'Profesor', 2),
		(0,'SC-01234', 'Administrador', 202),
		(0,'SC-01234', 'Administrador', 203),
		(0,'SC-01234', 'Administrador', 204),
		(0,'SC-01234', 'Administrador', 205),
		(0,'SC-01234', 'Administrador', 206),
		(0,'SC-01234', 'Administrador', 207),
		(0,'SC-01234', 'Profesor', 204),
		(0,'SC-01234', 'Profesor', 205),
		(0,'SC-01234', 'Estudiante', 205),
		(0,'SC-01234', 'Administrador', 208),
		(0,'SC-01234', 'Profesor', 208),
		(0, 'SC-01234', 'Administrador', 209)

INSERT INTO Provincia
VALUES	('San José'),
		('Cartago'),
		('Heredia'),
		('Alajuela'),
		('Puntarenas'),
		('Guanacaste'),
		('Limón');

INSERT INTO Canton
VALUES	('San José', 'Acosta'),
		('San José', 'Alajuelita'),
		('San José', 'Aserrí'),
		('San José', 'Desamparados'),
		('San José', 'Curridabat'),
		('San José', 'Dota'),
		('San José', 'Escazú'),
		('San José', 'Goicoechea'),
		('San José', 'León Cortés Castro'),
		('San José', 'Montes de Oca'),
		('San José', 'Mora'),
		('San José', 'Moravia'),
		('San José', 'Puriscal'),
		('San José', 'San José'),
		('San José', 'Tarrazú'),
		('San José', 'Turrubares'),
		('San José', 'Vazquez de Coronado'),
		('Cartago', 'Cartago'),
		('Cartago', 'Paraíso'),
		('Cartago', 'La Unión'),
		('Cartago', 'Jiménez'),
		('Cartago', 'Turrialba'),
		('Cartago', 'Alvarado'),
		('Cartago', 'Oreamuno'),
		('Cartago', 'El Guarco')

INSERT INTO Distrito
VALUES	('San José', 'San José', 'Carmen'),
		('San José', 'San José', 'Merced'),
		('San José', 'San José', 'Hospital'),
		('San José', 'San José', 'Catedral'),
		('San José', 'San José', 'Zapote'),
		('San José', 'San José', 'San Francisco de Dos Ríos'),
		('San José', 'San José', 'Uruca'),
		('San José', 'San José', 'Mata Redonda'),
		('San José', 'San José', 'Pavas'),
		('San José', 'San José', 'Hatillo'),
		('San José', 'San José', 'San Sebastián'),
		('San José', 'Escazú', 'Escazú'),
		('San José', 'Escazú', 'San Antonio'),
		('San José', 'Escazú', 'San Rafael'),
		('San José', 'Desamparados', 'Desamparados'),
		('San José', 'Desamparados', 'San Miguel'),
		('San José', 'Desamparados', 'San Juan de Dios'),
		('San José', 'Desamparados', 'San Rafael Arriba'),
		('San José', 'Desamparados', 'San Rafael Abajo'),
		('San José', 'Desamparados', 'San Antonio'),
		('San José', 'Desamparados', 'Frailes'),
		('San José', 'Desamparados', 'Patarrá'),
		('San José', 'Desamparados', 'San Cristóbal'),
		('San José', 'Desamparados', 'Rosario'),
		('San José', 'Desamparados', 'Damas'),
		('San José', 'Desamparados', 'Gravilias'),
		('Cartago', 'Cartago', 'Oriental'),
		('Cartago', 'Cartago', 'Occidental'),
		('Cartago', 'Cartago', 'Carmen'),
		('Cartago', 'Cartago', 'San Nicilás'),
		('Cartago', 'Cartago', 'Agua Caliente'),
		('Cartago', 'Cartago', 'Guadalupe'),
		('Cartago', 'Cartago', 'Corralillo'),
		('Cartago', 'Cartago', 'Tierra Blanca'),
		('Cartago', 'Cartago', 'Dulce Nombre'),
		('Cartago', 'Cartago', 'Llano Grande'),
		('Cartago', 'Cartago', 'Quebradilla'),
		('Cartago', 'Paraíso', 'Paraíso'),
		('Cartago', 'Paraíso', 'Santiago'),
		('Cartago', 'Paraíso', 'Orosi'),
		('Cartago', 'Paraíso', 'Cachí'),
		('Cartago', 'Paraíso', 'Llanps de Santa Lucía'),
		('Cartago', 'La Unión', 'Tres Ríos'),
		('Cartago', 'La Unión', 'San Diego'),
		('Cartago', 'La Unión', 'San Juan'),
		('Cartago', 'La Unión', 'San Rafael'),
		('Cartago', 'La Unión', 'Concepción'),
		('Cartago', 'La Unión', 'Dulce Nombre'),
		('Cartago', 'La Unión', 'San Ramón'),
		('Cartago', 'La Unión', 'Río Azul'),
		('Cartago', 'Jiménez', 'Juan Viñas'),
		('Cartago', 'Jiménez', 'Tucurrique'),
		('Cartago', 'Jiménez', 'Pejibaye'),
		('Cartago', 'Turrialba', 'Turrialba'),
		('Cartago', 'Turrialba', 'La Suiza'),
		('Cartago', 'Turrialba', 'Peralta'),
		('Cartago', 'Turrialba', 'San Cruz'),
		('Cartago', 'Turrialba', 'Santa Teresita'),
		('Cartago', 'Turrialba', 'Pavones'),
		('Cartago', 'Turrialba', 'Tuis'),
		('Cartago', 'Turrialba', 'Tayitic'),
		('Cartago', 'Turrialba', 'Santa Rosa'),
		('Cartago', 'Turrialba', 'Tres Equis'),
		('Cartago', 'Turrialba', 'La Isabel'),
		('Cartago', 'Turrialba', 'Chirripó'),
		('Cartago', 'Alvarado', 'Pacayas'),
		('Cartago', 'Alvarado', 'Cervantes'),
		('Cartago', 'Alvarado', 'Capellades'),
		('Cartago', 'Oreamuno', 'San Rafael'),
		('Cartago', 'Oreamuno', 'Cot'),
		('Cartago', 'Oreamuno', 'Potrero Cerrado'),
		('Cartago', 'Oreamuno', 'Cipreses'),
		('Cartago', 'Oreamuno', 'Santa Rosa'),
		('Cartago', 'El Guarco', 'El Tejar'),
		('Cartago', 'El Guarco', 'San Isidro'),
		('Cartago', 'El Guarco', 'Tobosi'),
		('Cartago', 'El Guarco', 'Patio de Agua')
		

--select de prueba para la cnt de respuestas
--SELECT e.Respuesta, COUNT(e.Respuesta) as cantidadRespuestas
--FROM Responde as e
--WHERE e.CodigoFormularioResp= '131313' AND e.CedulaProfesor= '100000002' AND e.AnnoGrupoResp= 2017 AND e.SemestreGrupoResp= 2 AND e.NumeroGrupoResp= 1 AND e.SiglaGrupoResp= 'CI1330' AND e.ItemId= 2
--GROUP BY e.CodigoFormularioResp, e.CedulaProfesor, e.AnnoGrupoResp, e.SemestreGrupoResp, e.NumeroGrupoResp, e.SiglaGrupoResp, e.ItemId, e.Respuesta

--select de prueba para obtener las observaciones
--SELECT e.Observacion
--FROM Responde as e
--WHERE e.CodigoFormularioResp= '131313' AND e.CedulaProfesor= '100000002' AND e.AnnoGrupoResp= 2017 AND e.SemestreGrupoResp= 2 AND e.NumeroGrupoResp= 1 AND e.SiglaGrupoResp= 'CI1330' AND e.ItemId= 1
--GROUP BY e.CodigoFormularioResp, e.CedulaProfesor, e.AnnoGrupoResp, e.SemestreGrupoResp, e.NumeroGrupoResp, e.SiglaGrupoResp, e.ItemId, e.Observacion--, e.Respuesta

--Fin Por el momento de script CX Solutions
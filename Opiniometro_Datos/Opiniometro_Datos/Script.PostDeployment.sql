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
	VALUES (@Correo, HASHBYTES('SHA2_512', @Contrasenna+CAST(@Id AS NVARCHAR(36))), 1, @Cedula, @Id)
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
	@Correo				NVARCHAR(50),
	@Contrasenna_Nueva	NVARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @CorreoBuscar NVARCHAR(50)
	
	-- Buscar que el correo y la contrasenna sean correctos con lo que hay en la tabla Usuario.
	SET @CorreoBuscar =	(SELECT CorreoInstitucional 
						FROM Usuario
						WHERE CorreoInstitucional=@Correo)

	IF(@CorreoBuscar IS NOT NULL)	-- Si existe el correo
		UPDATE Usuario
		SET Contrasena = HASHBYTES('SHA2_512', @Contrasenna_Nueva+CAST(Id AS NVARCHAR(36)))
		
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
	@Direccion			NVARCHAR(256)
AS
BEGIN
	UPDATE Persona
	SET Cedula = @Cedula, Nombre = @Nombre, Apellido1 = @Apellido1, Apellido2 = @Apellido2, Direccion = @Direccion
	WHERE Cedula = @CedulaBusqueda;
END
GO

IF OBJECT_ID('SP_ObtenerPermisosUsuario') IS NOT NULL
	DROP PROCEDURE SP_ObtenerPermisosUsuario
GO
CREATE PROCEDURE SP_ObtenerPermisosUsuario
	@Correo		NVARCHAR(50)
AS
	SELECT PER.Id
	FROM Tiene_Usuario_Perfil_Enfasis TU	JOIN Perfil ON TU.IdPerfil = Perfil.Id
											JOIN Posee_Enfasis_Perfil_Permiso PE ON Perfil.Id = PE.IdPerfil
											JOIN Permiso PER ON PER.Id = PE.IdPermiso
	WHERE TU.CorreoInstitucional=@Correo
GO

EXEC SP_ModificarPersona @CedulaBusqueda = '987654321', @Cedula='987654321', @Nombre='Barry2', @Apellido1='Allen2', @Apellido2='Garcia2', @Direccion='Central City2';

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
	@Cedula			CHAR(9),
	@Nombre			NVARCHAR(50),
	@Apellido1		NVARCHAR(50),
	@Apellido2		NVARCHAR(50),
	@Correo			NVARCHAR(50),
	@Direccion		NVARCHAR(200)
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Id UNIQUEIDENTIFIER=NEWID()
	DECLARE @Contrasenna NVARCHAR(10)

	INSERT INTO Persona
	VALUES (@Cedula, @Nombre, @Apellido1, @Apellido2, @Direccion)
	SET @Contrasenna = (SELECT dbo.SF_GenerarContrasena());
	INSERT INTO Usuario
	VALUES (@Correo, HASHBYTES('SHA2_512', @Contrasenna+CAST(@Id AS NVARCHAR(36))), 1, @Cedula, @Id)
END
GO


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
VALUES (0, 'SC-01234')

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

--Script C.X. Solutions

--Ciclo Lectivo
INSERT INTO Ciclo_Lectivo (Anno, Semestre)
VALUES  (2017, 2);

--Grupo
INSERT INTO Grupo(SiglaCurso, Numero, AnnoGrupo, SemestreGrupo)
VALUES ('CI1330', 1, 2017, 2);

--Item
DELETE FROM Item;
DBCC CHECKIDENT ('Item', RESEED, 0);

INSERT INTO Item(TextoPregunta, Categoria, TieneObservacion, TipoPregunta)
VALUES  ('¿El profesor repuso clases cuando fue necesario?', 'Curso', 1, 3),
		('¿El profesor entrego la carta del estudiante en las fechas indicadas por el reglamento?', 'Responsabilidades', 1, 3),
		('¿Que opina del curso?', 'Opinion', 0, 1),
		('¿Que opina del profesor?', 'Opinion', 0, 1);

--Item-Texto Libre
INSERT INTO Texto_Libre (ItemId)
VALUES  (3),
		(4);

--Item-Si/no
INSERT INTO Seleccion_Unica (ItemId, IsaLikeDislike)
VALUES  (1, 1),
		(2, 1);

--Seccion
INSERT INTO Seccion (Titulo, Descripcion)
VALUES  ('Evaluación de aspectos reglamentarios del profesor', 'Conteste a las preguntas relacionadas a aspectos reglamentarios que el profesor debe cumplir.'),
		('Opinion general del curso', 'Describa las opiniones que le han generado el profesor con respecto al curso tratado.');

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
		('2017-4-18', '131313', '100000005', '100000002', 2017, 2, 1, 'CI1330', 1);

 --Conformado_Item_Sec_Form
INSERT INTO Conformado_Item_Sec_Form (ItemId, CodigoFormulario, TituloSeccion, NombreFormulario)
VALUES	(1, '131313', 'Opinion general del curso', 'Evaluación de Profesores'),
		(3, '131313', 'Opinion general del curso', 'Evaluación de Profesores'),
		(2, '131313', 'Evaluación de aspectos reglamentarios del profesor', 'Evaluación de Profesores'),	
		(4, '131313', 'Evaluación de aspectos reglamentarios del profesor', 'Evaluación de Profesores');


--Responde
INSERT INTO Responde (ItemId, TituloSeccion, FechaRespuesta, CodigoFormularioResp, CedulaPersona, CedulaProfesor, AnnoGrupoResp, SemestreGrupoResp, NumeroGrupoResp, SiglaGrupoResp, Respuesta, Observacion)
VALUES  (1, 'Evaluación de aspectos reglamentarios del profesor', '2017-4-5', '131313', '100000003', '100000002', 2017, 2, 1, 'CI1330', '3', 'Nunca tuvimos que reponer clases'),
		(2, 'Evaluación de aspectos reglamentarios del profesor', '2017-4-5', '131313', '100000003', '100000002', 2017, 2, 1, 'CI1330', '2', 'La profesora olvido enviar la carta del estudiante pero si la revisamos en la primera semana de clases'),
		(3, 'Opinion general del curso', '2017-4-5', '131313', '100000003', '100000002', 2017, 2, 1, 'CI1330', '', 'La materia estuvo muy interesante y espero poder aplicarla en el futuro en el trabajo'),
		(4, 'Opinion general del curso', '2017-4-5', '131313', '100000003', '100000002', 2017, 2, 1, 'CI1330', '', 'La profesora tardo mucho para devolver las evaluaciones'),
		--Segunda evaluacion
		(1, 'Evaluación de aspectos reglamentarios del profesor', '2017-3-6', '131313', '100000004', '100000002', 2017, 2, 1, 'CI1330', '2', 'No fue necesario reponer clases'),
		(2, 'Evaluación de aspectos reglamentarios del profesor', '2017-3-6', '131313', '100000004', '100000002', 2017, 2, 1, 'CI1330', '1', 'Revisamos la carta del estudiante en la primera semana'),
		(3, 'Opinion general del curso', '2017-3-6', '131313', '100000004', '100000002', 2017, 2, 1, 'CI1330', '', 'No estoy seguro de si en el ambiente laboral me servira la materia'),
		(4, 'Opinion general del curso', '2017-3-6', '131313', '100000004', '100000002', 2017, 2, 1, 'CI1330', '', 'La profesora logro que las clases fueran muy entretenidas y dinámicas'),
		--Tercera evaluacion
		(1, 'Evaluación de aspectos reglamentarios del profesor', '2017-4-18', '131313', '100000005', '100000002', 2017, 2, 1, 'CI1330', '1', 'Me repuso una clase a la que falte'),
		(2, 'Evaluación de aspectos reglamentarios del profesor', '2017-4-18', '131313', '100000005', '100000002', 2017, 2, 1, 'CI1330', '1', 'Sí se reviso'),
		(3, 'Opinion general del curso', '2017-4-18', '131313', '100000005', '100000002', 2017, 2, 1, 'CI1330', '', 'Entretenido'),
		(4, 'Opinion general del curso', '2017-4-18', '131313', '100000005', '100000002', 2017, 2, 1, 'CI1330', '', 'Muy buena profesora');

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
	@itemId				INT,
	@respuesta			NVARCHAR(500),
	@cntResp			INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON
	SELECT @cntResp= COUNT(e.Respuesta)
	FROM Responde as e
	WHERE e.CodigoFormularioResp= @codigoFormulario AND e.CedulaProfesor= @cedulaProfesor AND e.AnnoGrupoResp= @annoGrupo AND e.SemestreGrupoResp= @semestreGrupo AND e.NumeroGrupoResp= @numeroGrupo AND e.SiglaGrupoResp= @siglaCurso AND e.ItemId= @itemId AND e.Respuesta = @respuesta
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
	@itemId INT
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
		(4, 'Ver cursos')

INSERT INTO Perfil
VALUES	('Estudiante', 'Default'),
		('Admin', 'Default')

INSERT INTO Tiene_Usuario_Perfil_Enfasis
VALUES	('jose.mejiasrojas@ucr.ac.cr', 0, 'SC-01234', 'Estudiante'),
		('admin@ucr.ac.cr', 0, 'SC-01234', 'Admin')

INSERT INTO Posee_Enfasis_Perfil_Permiso
VALUES	(0, 'SC-01234', 'Estudiante', 3),
		(0, 'SC-01234', 'Estudiante', 4),
		(0, 'SC-01234', 'Admin', 1)

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
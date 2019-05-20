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

--En caso de error en la inserción es posiblemente el dato de CX, reescribir de ser necesario esa inserción.
INSERT INTO Persona
VALUES	('116720500', 'Jose Andrés', 'Mejías', 'Rojas', 'Desamparados de Alajuela.'),
		('115003456', 'Daniel', 'Escalante', 'Perez', 'Desamparados de San José.'),
		('117720910', 'Jose Andrés', 'Mejías', 'Rojas', 'La Fortuna de San Carlos.'),
		('236724501', 'Jose Andrés', 'Mejías', 'Rojas', 'Sarchí, Alajuela.'),
		--Agregado de datos para visualizacion a cargo de CX Solutions
		('100000001', 'CX', 'Solutions', 'S.A.', 'San Pedro Montes de Oca'),
		('100000002', 'Marta', 'Rojas', 'Sanches', '300 metros norte de Pulmitan'),--Profesora
		--Estudiantes
		('100000003', 'Juan', 'Briceño', 'Lupon', '400 metros norte del Heraldo de la Grieta'),
		('100000005', 'Pepito', 'Fonsi', 'Monge', '20 metros norte del Blue del lado Rojo'),
		('100000004', 'Maria', 'Fallas', 'Merdi', 'Costado este del estandarte de top');

--INSERT INTO Responde (ItemId,TituloSeccion, FechaRespuesta, CodigoFormularioResp, CedulaPersona, CedulaProfesor, AnnoGrupoResp, SemestreGrupoResp, NumeroGrupoResp, SiglaGrupoResp)
--VALUES (3,'Titulo Seccion 1','8-12-1994','CodF01','123456789','987654321',54,01,21,'SG1234'),
--	   (1,'Titulo Seccion 2','8-12-1998','CodF02','123789659','987678951',59,01,20,'SG2345'),
--	   (2,'Titulo Seccion 3','8-12-1997','CodF03','135786789','982455321',51,01,15,'SG3456');

EXEC SP_AgregarUsuario @Correo='jose.mejiasrojas@ucr.ac.cr', @Contrasenna='123456', @Cedula='116720500'
EXEC SP_AgregarUsuario @Correo='daniel.escalanteperez@ucr.ac.cr', @Contrasenna='Danielito', @Cedula='115003456'
EXEC SP_AgregarUsuario @Correo='rodrigo.cascantejuarez@ucr.ac.cr', @Contrasenna='contrasena', @Cedula='117720910'
EXEC SP_AgregarUsuario @Correo='luis.quesadaborbon@ucr.ac.cr', @Contrasenna='LigaDeportivaAlajuelense', @Cedula='236724501'

--Script JJAPH

--Unidad academica
INSERT INTO Unidad_Academica (Codigo, Nombre)
VALUES ('UC-023874', 'ECCI')

INSERT INTO Unidad_Academica (Codigo, Nombre)
VALUES ('UC-485648', 'Derecho')

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

--INSERT INTO Grupo(SiglaCurso, Numero, AnnoGrupo, SemestreGrupo)
--VALUES ('CI1330', 2, 2018, 1)

--INSERT INTO Grupo(Numero, SiglaCurso, Anno, Semestre)
--VALUES ('CI1331', 1, 2019, 1)

--INSERT INTO Grupo(Numero, SiglaCurso, Anno, Semestre)
--VALUES ('CI1331', 2, 2019, 1)

--INSERT INTO Grupo(Numero, SiglaCurso, Anno, Semestre)
--VALUES ('CI1327', 1, 2019, 1)

--INSERT INTO Grupo(Numero, SiglaCurso, Anno, Semestre)
--VALUES ('CI1327', 2, 2019, 1)

--INSERT INTO Grupo(Numero, SiglaCurso, Anno, Semestre)
--VALUES ('CI1328', 1, 2019, 1)

--INSERT INTO Grupo(Numero, SiglaCurso, Anno, Semestre)
--VALUES ('CI1328', 2, 2019, 1)

--INSERT INTO Grupo(Numero, SiglaCurso, Anno, Semestre)
--VALUES ('DE1001', 1, 2019, 1)

--INSERT INTO Grupo(Numero, SiglaCurso, Anno, Semestre)
--VALUES ('DE1001', 2, 2019, 1)

--INSERT INTO Grupo(Numero, SiglaCurso, Anno, Semestre)
--VALUES ('DE2001', 1, 2019, 1)

--INSERT INTO Grupo(Numero, SiglaCurso, Anno, Semestre)
--VALUES ('DE2001', 2, 2019, 1)

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

--Responde
INSERT INTO Responde (ItemId, TituloSeccion, FechaRespuesta, CodigoFormularioResp, CedulaPersona, CedulaProfesor, AnnoGrupoResp, SemestreGrupoResp, NumeroGrupoResp, SiglaGrupoResp, Observacion, Respuesta)
VALUES  (1, 'Evaluación de aspectos reglamentarios del profesor', '2017-4-5', '131313', '100000003', '100000002', 2017, 2, 1, 'CI1330', 'Nunca tuvimos que reponer clases', '3'),
		(2, 'Evaluación de aspectos reglamentarios del profesor', '2017-4-5', '131313', '100000003', '100000002', 2017, 2, 1, 'CI1330', 'La profesora olvido enviar la carta del estudiante pero si la revisamos en la primera semana de clases', '2'),
		(3, 'Opinion general del curso', '2017-4-5', '131313', '100000003', '100000002', 2017, 2, 1, 'CI1330', '', 'La materia estuvo muy interesante y espero poder aplicarla en el futuro en el trabajo'),
		(4, 'Opinion general del curso', '2017-4-5', '131313', '100000003', '100000002', 2017, 2, 1, 'CI1330', '', 'La profesora tardo mucho para devolver las evaluaciones'),
		--Segunda evaluacion
		(1, 'Evaluación de aspectos reglamentarios del profesor', '2017-3-6', '131313', '100000004', '100000002', 2017, 2, 1, 'CI1330', 'No fue necesario reponer clases', '2'),
		(2, 'Evaluación de aspectos reglamentarios del profesor', '2017-3-6', '131313', '100000004', '100000002', 2017, 2, 1, 'CI1330', 'Revisamos la carta del estudiante en la primera semana', '1'),
		(3, 'Opinion general del curso', '2017-3-6', '131313', '100000004', '100000002', 2017, 2, 1, 'CI1330', '', 'No estoy seguro de si en el ambiente laboral me servira la materia'),
		(4, 'Opinion general del curso', '2017-3-6', '131313', '100000004', '100000002', 2017, 2, 1, 'CI1330', '', 'La profesora logro que las clases fueran muy entretenidas y dinámicas'),
		--Tercera evaluacion
		(1, 'Evaluación de aspectos reglamentarios del profesor', '2017-4-18', '131313', '100000005', '100000002', 2017, 2, 1, 'CI1330', 'Me repuso una clase a la que falte', '1'),
		(2, 'Evaluación de aspectos reglamentarios del profesor', '2017-4-18', '131313', '100000005', '100000002', 2017, 2, 1, 'CI1330', '', '1'),
		(3, 'Opinion general del curso', '2017-4-18', '131313', '100000005', '100000002', 2017, 2, 1, 'CI1330', '', 'Entretenido'),
		(4, 'Opinion general del curso', '2017-4-18', '131313', '100000005', '100000002', 2017, 2, 1, 'CI1330', '', 'Muy buena profesora');

--Fin Por el momento de script CX Solutions
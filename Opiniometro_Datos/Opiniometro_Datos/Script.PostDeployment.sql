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
		('100000001', 'CX', 'Solutions', 'S.A.', 'San Pedro Montes de Oca');

INSERT INTO Responde (ItemId,TituloSeccion, FechaRespuesta, CodigoFormularioResp, CedulaPersona, CedulaProfesor, AñoGrupoResp, SemestreGrupoResp, NumeroGrupoResp, SiglaGrupoResp)
VALUES (3,'Titulo Seccion 1','8-12-1994','CodF01','123456789','987654321',54,01,21,'SG1234'),
	   (1,'Titulo Seccion 2','8-12-1998','CodF02','123789659','987678951',59,01,20,'SG2345'),
	   (2,'Titulo Seccion 3','8-12-1997','CodF03','135786789','982455321',51,01,15,'SG3456');

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
--INSERT INTO Grupo(Numero, SiglaCurso, Anno, Semestre)
--VALUES ('CI1330', 1, 2019, 1)

--INSERT INTO Grupo(Numero, SiglaCurso, Anno, Semestre)
--VALUES ('CI1330', 2, 2019, 1)

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
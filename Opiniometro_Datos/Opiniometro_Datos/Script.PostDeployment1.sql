--Eliminar todos los datos para que quede limpia a la hora de insertar nuevamente datos establecidos.

EXEC sp_MSForEachTable 'DISABLE TRIGGER ALL ON ?'
GO
EXEC sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'
GO
EXEC sp_MSForEachTable 'DELETE FROM ?'
GO
EXEC sp_MSForEachTable 'ALTER TABLE ? CHECK CONSTRAINT ALL'
GO
EXEC sp_MSForEachTable 'ENABLE TRIGGER ALL ON ?'
GO

--Inserciones

INSERT INTO Persona
VALUES ('111111', 'Juan', 'Carillo', 'Jimenez');
INSERT INTO Estudiante
VALUES ('111111', 'BXXXXX');

/* PENDING: PROCEDIMIENTOS ALMACENADOS
SELECT P.Nombre, P.Apellido1, P.Apellido2, E.Carne
FROM Persona P JOIN Estudiante E ON E.Cedula_Estudiante = P.Cedula
*/
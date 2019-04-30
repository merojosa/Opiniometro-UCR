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

INSERT INTO Persona VALUES 
 ('111111', 'Juan', 'Carillo', 'Jimenez')
,('222222', 'María', 'Mejías', 'Guierrez')
,('333333', 'Ellie', 'Rodríguez', 'Rojas')
,('444444', 'Abel', 'Hernández', 'Pérez');
INSERT INTO Estudiante VALUES 
 ('111111', 'B11111')
,('222222', 'B22222')
,('333333', 'B33333') 
,('444444', 'B44444');
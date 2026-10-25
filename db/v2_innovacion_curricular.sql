USE [innovacion_curricular];
GO

-- ============================================================================
-- 1. CREACI�N DE TABLAS DE LA V2 (CON CLAVES FOR�NEAS)
-- ============================================================================

-- Tabla facultad
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'facultad')
BEGIN
    CREATE TABLE facultad (
        id INT IDENTITY(1,1) PRIMARY KEY,
        nombre VARCHAR(200) NOT NULL,
        universidad_id INT NOT NULL FOREIGN KEY REFERENCES universidad(id),
        activo BIT NOT NULL DEFAULT 1
    );
END;
GO

-- Tabla programa
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'programa')
BEGIN
    CREATE TABLE programa (
        id INT IDENTITY(1,1) PRIMARY KEY,
        nombre VARCHAR(200) NOT NULL,
        facultad_id INT NOT NULL FOREIGN KEY REFERENCES facultad(id),
        activo BIT NOT NULL DEFAULT 1
    );
END;
GO

-- Tabla acreditacion
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'acreditacion')
BEGIN
    CREATE TABLE acreditacion (
        resolucion VARCHAR(100) PRIMARY KEY,
        tipo VARCHAR(100) NOT NULL,
        calificacion VARCHAR(50),
        fecha_inicio DATE,
        fecha_fin DATE,
        programa_id INT NOT NULL FOREIGN KEY REFERENCES programa(id),
        activo BIT NOT NULL DEFAULT 1
    );
END;
GO

-- Tabla registro_calificado
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'registro_calificado')
BEGIN
    CREATE TABLE registro_calificado (
        codigo VARCHAR(100) PRIMARY KEY,
        creditos INT NOT NULL,
        horas INT NOT NULL,
        metodologia VARCHAR(100),
        duracion VARCHAR(50),
        titulacion VARCHAR(200),
        programa_id INT NOT NULL FOREIGN KEY REFERENCES programa(id),
        activo BIT NOT NULL DEFAULT 1
    );
END;
GO

-- Tabla activ_academica
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'activ_academica')
BEGIN
    CREATE TABLE activ_academica (
        id INT IDENTITY(1,1) PRIMARY KEY,
        nombre VARCHAR(200) NOT NULL,
        creditos INT NOT NULL,
        horas INT NOT NULL,
        idioma VARCHAR(50) DEFAULT 'Espa�ol',
        es_espejo BIT DEFAULT 0,
        programa_id INT NOT NULL FOREIGN KEY REFERENCES programa(id),
        activo BIT NOT NULL DEFAULT 1
    );
END;
GO

-- Tabla pasantia
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'pasantia')
BEGIN
    CREATE TABLE pasantia (
        id INT IDENTITY(1,1) PRIMARY KEY,
        nombre VARCHAR(200) NOT NULL,
        descripcion VARCHAR(MAX),
        programa_id INT NOT NULL FOREIGN KEY REFERENCES programa(id),
        activo BIT NOT NULL DEFAULT 1
    );
END;
GO

-- Tabla premio
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'premio')
BEGIN
    CREATE TABLE premio (
        id INT IDENTITY(1,1) PRIMARY KEY,
        nombre VARCHAR(200) NOT NULL,
        entidad_otorgante VARCHAR(200) NOT NULL,
        fecha_otorgamiento DATE,
        programa_id INT NOT NULL FOREIGN KEY REFERENCES programa(id),
        activo BIT NOT NULL DEFAULT 1
    );
END;
GO

-- Tabla docente_departamento
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'docente_departamento')
BEGIN
    CREATE TABLE docente_departamento (
        id INT IDENTITY(1,1) PRIMARY KEY,
        docente_nombre VARCHAR(200) NOT NULL,
        dedicacion VARCHAR(100),
        modalidad VARCHAR(100),
        fecha_inicio DATE,
        fecha_fin DATE,
        programa_id INT NOT NULL FOREIGN KEY REFERENCES programa(id),
        activo BIT NOT NULL DEFAULT 1
    );
END;
GO

-- Tabla alianza (Alianza aliado <-> programa)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'alianza')
BEGIN
    CREATE TABLE alianza (
        id INT IDENTITY(1,1) PRIMARY KEY,
        aliado_nit VARCHAR(20) NOT NULL FOREIGN KEY REFERENCES aliado(nit),
        programa_id INT NOT NULL FOREIGN KEY REFERENCES programa(id),
        docente_responsable VARCHAR(200),
        fecha_inicio DATE,
        fecha_fin DATE,
        activo BIT NOT NULL DEFAULT 1
    );
END;
GO

-- Tablas Puente (Relaciones Mucho a Muchos con Clave Primaria Compuesta)

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'programa_ac')
BEGIN
    CREATE TABLE programa_ac (
        programa_id INT NOT NULL FOREIGN KEY REFERENCES programa(id),
        area_conocimiento_id INT NOT NULL FOREIGN KEY REFERENCES area_conocimiento(id),
        PRIMARY KEY (programa_id, area_conocimiento_id)
    );
END;
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'programa_pe')
BEGIN
    CREATE TABLE programa_pe (
        programa_id INT NOT NULL FOREIGN KEY REFERENCES programa(id),
        practica_estrategia_id INT NOT NULL FOREIGN KEY REFERENCES practica_estrategia(id),
        PRIMARY KEY (programa_id, practica_estrategia_id)
    );
END;
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'programa_ci')
BEGIN
    CREATE TABLE programa_ci (
        programa_id INT NOT NULL FOREIGN KEY REFERENCES programa(id),
        car_innovacion_id INT NOT NULL FOREIGN KEY REFERENCES car_innovacion(id),
        PRIMARY KEY (programa_id, car_innovacion_id)
    );
END;
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'an_programa')
BEGIN
    CREATE TABLE an_programa (
        aspecto_normativo_id INT NOT NULL FOREIGN KEY REFERENCES aspecto_normativo(id),
        programa_id INT NOT NULL FOREIGN KEY REFERENCES programa(id),
        PRIMARY KEY (aspecto_normativo_id, programa_id)
    );
END;
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'enfoque_rc')
BEGIN
    CREATE TABLE enfoque_rc (
        enfoque_id INT NOT NULL FOREIGN KEY REFERENCES enfoque(id),
        registro_calificado_codigo VARCHAR(100) NOT NULL FOREIGN KEY REFERENCES registro_calificado(codigo),
        PRIMARY KEY (enfoque_id, registro_calificado_codigo)
    );
END;
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'aa_rc')
BEGIN
    CREATE TABLE aa_rc (
        activ_academica_id INT NOT NULL FOREIGN KEY REFERENCES activ_academica(id),
        registro_calificado_codigo VARCHAR(100) NOT NULL FOREIGN KEY REFERENCES registro_calificado(codigo),
        componente VARCHAR(100),
        semestre INT,
        PRIMARY KEY (activ_academica_id, registro_calificado_codigo)
    );
END;
GO


-- ============================================================================
-- 2. TRIGGER DE CONTEO (Propuesta A)
-- ============================================================================

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('universidad') AND name = 'total_facultades')
BEGIN
    ALTER TABLE universidad ADD total_facultades INT DEFAULT 0;
END;
GO

CREATE OR ALTER TRIGGER trg_contar_facultades
ON facultad
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE u 
    SET u.total_facultades = (
        SELECT COUNT(*) 
        FROM facultad f 
        WHERE f.universidad_id = u.id AND f.activo = 1
    )
    FROM universidad u
    WHERE u.id IN (
        SELECT universidad_id FROM inserted UNION SELECT universidad_id FROM deleted
    );
END;
GO


-- ============================================================================
-- 3. PROCEDIMIENTOS ALMACENADOS BASE DE LA V2
-- ============================================================================

-- FACULTAD
CREATE OR ALTER PROCEDURE sp_listar_facultad
AS
BEGIN
    SET NOCOUNT ON;
    SELECT f.id, f.nombre, f.universidad_id, u.nombre AS universidad_nombre, f.activo 
    FROM facultad f
    INNER JOIN universidad u ON f.universidad_id = u.id
    WHERE f.activo = 1
    ORDER BY f.nombre;
END;
GO

CREATE OR ALTER PROCEDURE sp_consultar_facultad
    @id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT f.id, f.nombre, f.universidad_id, u.nombre AS universidad_nombre, f.activo 
    FROM facultad f
    INNER JOIN universidad u ON f.universidad_id = u.id
    WHERE f.id = @id AND f.activo = 1;
END;
GO

CREATE OR ALTER PROCEDURE sp_crear_facultad
    @nombre VARCHAR(200),
    @universidad_id INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @NuevoId TABLE (id INT);

    INSERT INTO facultad (nombre, universidad_id, activo)
    OUTPUT INSERTED.id INTO @NuevoId
    VALUES (@nombre, @universidad_id, 1);

    SELECT f.id, f.nombre, f.universidad_id, u.nombre AS universidad_nombre, f.activo
    FROM facultad f
    INNER JOIN universidad u ON f.universidad_id = u.id
    WHERE f.id = (SELECT id FROM @NuevoId);
END;
GO

CREATE OR ALTER PROCEDURE sp_actualizar_facultad
    @id INT,
    @nombre VARCHAR(200),
    @universidad_id INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE facultad
    SET nombre = @nombre,
        universidad_id = @universidad_id
    WHERE id = @id AND activo = 1;

    EXEC sp_consultar_facultad @id;
END;
GO

CREATE OR ALTER PROCEDURE sp_eliminar_facultad
    @id INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE facultad
    SET activo = 0
    WHERE id = @id;
END;
GO

-- PROGRAMA
CREATE OR ALTER PROCEDURE sp_listar_programa
AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.id, p.nombre, p.facultad_id, f.nombre AS facultad_nombre, p.activo
    FROM programa p
    INNER JOIN facultad f ON p.facultad_id = f.id
    WHERE p.activo = 1
    ORDER BY p.nombre;
END;
GO

CREATE OR ALTER PROCEDURE sp_consultar_programa
    @id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.id, p.nombre, p.facultad_id, f.nombre AS facultad_nombre, p.activo
    FROM programa p
    INNER JOIN facultad f ON p.facultad_id = f.id
    WHERE p.id = @id AND p.activo = 1;
END;
GO

CREATE OR ALTER PROCEDURE sp_crear_programa
    @nombre VARCHAR(200),
    @facultad_id INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @NuevoId TABLE (id INT);
    DECLARE @IdCreado INT;

    INSERT INTO programa (nombre, facultad_id, activo)
    OUTPUT INSERTED.id INTO @NuevoId
    VALUES (@nombre, @facultad_id, 1);

    -- Asignamos el ID insertado a una variable
    SELECT TOP 1 @IdCreado = id FROM @NuevoId;

    -- Pasamos la variable al procedimiento
    EXEC sp_consultar_programa @id = @IdCreado;
END;
GO

CREATE OR ALTER PROCEDURE sp_actualizar_programa
    @id INT,
    @nombre VARCHAR(200),
    @facultad_id INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE programa
    SET nombre = @nombre,
        facultad_id = @facultad_id
    WHERE id = @id AND activo = 1;

    EXEC sp_consultar_programa @id;
END;
GO

CREATE OR ALTER PROCEDURE sp_eliminar_programa
    @id INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE programa
    SET activo = 0
    WHERE id = @id;
END;
GO

-- ============================================================================
-- 4. PROCEDIMIENTOS ALMACENADOS PARA ACREDITACIONES, REGISTROS CALIFICADOS
--    Y ACTIVIDADES ACAD�MICAS (M�DULOS INTEGRADOS EN V2)
-- ============================================================================

-- ACREDITACIONES
CREATE OR ALTER PROCEDURE dbo.sp_listar_acreditacion
AS
BEGIN
    SET NOCOUNT ON;
    SELECT a.resolucion, a.tipo, a.calificacion, a.fecha_inicio, a.fecha_fin,
           a.programa_id, p.nombre AS programa_nombre, a.activo
    FROM dbo.acreditacion AS a
    INNER JOIN dbo.programa AS p ON a.programa_id = p.id
    WHERE a.activo = 1
    ORDER BY a.resolucion;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_consultar_acreditacion
    @resolucion VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT a.resolucion, a.tipo, a.calificacion, a.fecha_inicio, a.fecha_fin,
           a.programa_id, p.nombre AS programa_nombre, a.activo
    FROM dbo.acreditacion AS a
    INNER JOIN dbo.programa AS p ON a.programa_id = p.id
    WHERE a.resolucion = @resolucion AND a.activo = 1;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_crear_acreditacion
    @resolucion VARCHAR(100),
    @tipo VARCHAR(100),
    @calificacion VARCHAR(50),
    @fecha_inicio DATE,
    @fecha_fin DATE,
    @programa_id INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.acreditacion
        (resolucion, tipo, calificacion, fecha_inicio, fecha_fin, programa_id, activo)
    VALUES
        (@resolucion, @tipo, @calificacion, @fecha_inicio, @fecha_fin, @programa_id, 1);

    EXEC dbo.sp_consultar_acreditacion @resolucion = @resolucion;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_eliminar_acreditacion
    @resolucion VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.acreditacion SET activo = 0 WHERE resolucion = @resolucion;
END;
GO

-- REGISTROS CALIFICADOS
CREATE OR ALTER PROCEDURE dbo.sp_listar_registro_calificado
AS
BEGIN
    SET NOCOUNT ON;
    SELECT rc.codigo, rc.creditos, rc.horas, rc.metodologia, rc.duracion,
           rc.titulacion, rc.programa_id, p.nombre AS programa_nombre, rc.activo
    FROM dbo.registro_calificado AS rc
    INNER JOIN dbo.programa AS p ON rc.programa_id = p.id
    WHERE rc.activo = 1
    ORDER BY rc.codigo;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_consultar_registro_calificado
    @codigo VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT rc.codigo, rc.creditos, rc.horas, rc.metodologia, rc.duracion,
           rc.titulacion, rc.programa_id, p.nombre AS programa_nombre, rc.activo
    FROM dbo.registro_calificado AS rc
    INNER JOIN dbo.programa AS p ON rc.programa_id = p.id
    WHERE rc.codigo = @codigo AND rc.activo = 1;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_crear_registro_calificado
    @codigo VARCHAR(100),
    @creditos INT,
    @horas INT,
    @metodologia VARCHAR(100),
    @duracion VARCHAR(50),
    @titulacion VARCHAR(200),
    @programa_id INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.registro_calificado
        (codigo, creditos, horas, metodologia, duracion, titulacion, programa_id, activo)
    VALUES
        (@codigo, @creditos, @horas, @metodologia, @duracion, @titulacion, @programa_id, 1);

    EXEC dbo.sp_consultar_registro_calificado @codigo = @codigo;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_eliminar_registro_calificado
    @codigo VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.registro_calificado SET activo = 0 WHERE codigo = @codigo;
END;
GO

-- ACTIVIDADES ACAD�MICAS
CREATE OR ALTER PROCEDURE dbo.sp_listar_activ_academica
AS
BEGIN
    SET NOCOUNT ON;
    SELECT aa.id, aa.nombre, aa.creditos, aa.horas, aa.idioma, aa.es_espejo,
           aa.programa_id, p.nombre AS programa_nombre, aa.activo
    FROM dbo.activ_academica AS aa
    INNER JOIN dbo.programa AS p ON aa.programa_id = p.id
    WHERE aa.activo = 1
    ORDER BY aa.nombre;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_crear_activ_academica
    @nombre VARCHAR(200),
    @creditos INT,
    @horas INT,
    @idioma VARCHAR(50),
    @es_espejo BIT,
    @programa_id INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @NuevoId TABLE (id INT);

    INSERT INTO dbo.activ_academica
        (nombre, creditos, horas, idioma, es_espejo, programa_id, activo)
    OUTPUT INSERTED.id INTO @NuevoId
    VALUES (@nombre, @creditos, @horas, @idioma, @es_espejo, @programa_id, 1);

    SELECT aa.id, aa.nombre, aa.creditos, aa.horas, aa.idioma, aa.es_espejo,
           aa.programa_id, p.nombre AS programa_nombre, aa.activo
    FROM dbo.activ_academica AS aa
    INNER JOIN dbo.programa AS p ON aa.programa_id = p.id
    WHERE aa.id = (SELECT TOP (1) id FROM @NuevoId);
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_eliminar_activ_academica
    @id INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.activ_academica SET activo = 0 WHERE id = @id;
END;
GO

USE [innovacion_curricular];
GO

-- ============================================================================
-- SPs: PASANTIA, PREMIO, DOCENTE_DEPARTAMENTO, ALIANZA
-- ============================================================================
CREATE OR ALTER PROCEDURE sp_listar_pasantia AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.id, p.nombre, p.descripcion, p.programa_id, pr.nombre AS programa_nombre, p.activo
    FROM pasantia p INNER JOIN programa pr ON p.programa_id = pr.id WHERE p.activo = 1;
END;
GO

CREATE OR ALTER PROCEDURE sp_crear_pasantia @nombre VARCHAR(200), @descripcion VARCHAR(MAX), @programa_id INT AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @NuevoId TABLE (id INT);
    INSERT INTO pasantia (nombre, descripcion, programa_id, activo) OUTPUT INSERTED.id INTO @NuevoId VALUES (@nombre, @descripcion, @programa_id, 1);
    SELECT p.id, p.nombre, p.descripcion, p.programa_id, pr.nombre AS programa_nombre, p.activo
    FROM pasantia p INNER JOIN programa pr ON p.programa_id = pr.id WHERE p.id = (SELECT id FROM @NuevoId);
END;
GO

CREATE OR ALTER PROCEDURE sp_eliminar_pasantia @id INT AS
BEGIN
    SET NOCOUNT ON; UPDATE pasantia SET activo = 0 WHERE id = @id;
END;
GO

CREATE OR ALTER PROCEDURE sp_listar_premio AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.id, p.nombre, p.entidad_otorgante, p.fecha_otorgamiento, p.programa_id, pr.nombre AS programa_nombre, p.activo
    FROM premio p INNER JOIN programa pr ON p.programa_id = pr.id WHERE p.activo = 1;
END;
GO

CREATE OR ALTER PROCEDURE sp_crear_premio @nombre VARCHAR(200), @entidad_otorgante VARCHAR(200), @fecha_otorgamiento DATE, @programa_id INT AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @NuevoId TABLE (id INT);
    INSERT INTO premio (nombre, entidad_otorgante, fecha_otorgamiento, programa_id, activo) OUTPUT INSERTED.id INTO @NuevoId VALUES (@nombre, @entidad_otorgante, @fecha_otorgamiento, @programa_id, 1);
    SELECT p.id, p.nombre, p.entidad_otorgante, p.fecha_otorgamiento, p.programa_id, pr.nombre AS programa_nombre, p.activo
    FROM premio p INNER JOIN programa pr ON p.programa_id = pr.id WHERE p.id = (SELECT id FROM @NuevoId);
END;
GO

CREATE OR ALTER PROCEDURE sp_eliminar_premio @id INT AS
BEGIN
    SET NOCOUNT ON; UPDATE premio SET activo = 0 WHERE id = @id;
END;
GO

CREATE OR ALTER PROCEDURE sp_listar_docente_departamento AS
BEGIN
    SET NOCOUNT ON;
    SELECT d.id, d.docente_nombre, d.dedicacion, d.modalidad, d.fecha_inicio, d.fecha_fin, d.programa_id, p.nombre AS programa_nombre, d.activo
    FROM docente_departamento d INNER JOIN programa p ON d.programa_id = p.id WHERE d.activo = 1;
END;
GO

CREATE OR ALTER PROCEDURE sp_crear_docente_departamento @docente_nombre VARCHAR(200), @dedicacion VARCHAR(100), @modalidad VARCHAR(100), @fecha_inicio DATE, @fecha_fin DATE, @programa_id INT AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @NuevoId TABLE (id INT);
    INSERT INTO docente_departamento (docente_nombre, dedicacion, modalidad, fecha_inicio, fecha_fin, programa_id, activo) OUTPUT INSERTED.id INTO @NuevoId VALUES (@docente_nombre, @dedicacion, @modalidad, @fecha_inicio, @fecha_fin, @programa_id, 1);
    SELECT d.id, d.docente_nombre, d.dedicacion, d.modalidad, d.fecha_inicio, d.fecha_fin, d.programa_id, p.nombre AS programa_nombre, d.activo
    FROM docente_departamento d INNER JOIN programa p ON d.programa_id = p.id WHERE d.id = (SELECT id FROM @NuevoId);
END;
GO

CREATE OR ALTER PROCEDURE sp_eliminar_docente_departamento @id INT AS
BEGIN
    SET NOCOUNT ON; UPDATE docente_departamento SET activo = 0 WHERE id = @id;
END;
GO

CREATE OR ALTER PROCEDURE sp_listar_alianza AS
BEGIN
    SET NOCOUNT ON;
    SELECT a.id, a.aliado_nit, al.razon_social AS aliado_nombre, a.programa_id, p.nombre AS programa_nombre, a.docente_responsable, a.fecha_inicio, a.fecha_fin, a.activo
    FROM alianza a
    INNER JOIN aliado al ON a.aliado_nit = al.nit
    INNER JOIN programa p ON a.programa_id = p.id
    WHERE a.activo = 1;
END;
GO

CREATE OR ALTER PROCEDURE sp_crear_alianza @aliado_nit VARCHAR(20), @programa_id INT, @docente_responsable VARCHAR(200), @fecha_inicio DATE, @fecha_fin DATE AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @NuevoId TABLE (id INT);
    INSERT INTO alianza (aliado_nit, programa_id, docente_responsable, fecha_inicio, fecha_fin, activo) OUTPUT INSERTED.id INTO @NuevoId VALUES (@aliado_nit, @programa_id, @docente_responsable, @fecha_inicio, @fecha_fin, 1);
    SELECT a.id, a.aliado_nit, al.razon_social AS aliado_nombre, a.programa_id, p.nombre AS programa_nombre, a.docente_responsable, a.fecha_inicio, a.fecha_fin, a.activo
    FROM alianza a INNER JOIN aliado al ON a.aliado_nit = al.nit INNER JOIN programa p ON a.programa_id = p.id WHERE a.id = (SELECT id FROM @NuevoId);
END;
GO

CREATE OR ALTER PROCEDURE sp_eliminar_alianza @id INT AS
BEGIN
    SET NOCOUNT ON; UPDATE alianza SET activo = 0 WHERE id = @id;
END;
GO

CREATE OR ALTER PROCEDURE sp_actualizar_acreditacion
    @resolucion VARCHAR(100),
    @tipo VARCHAR(100),
    @calificacion VARCHAR(50),
    @fecha_inicio DATE,
    @fecha_fin DATE,
    @programa_id INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE acreditacion
    SET tipo = @tipo,
        calificacion = @calificacion,
        fecha_inicio = @fecha_inicio,
        fecha_fin = @fecha_fin,
        programa_id = @programa_id
    WHERE resolucion = @resolucion AND activo = 1;

    EXEC sp_consultar_acreditacion @resolucion;
END;
GO

CREATE OR ALTER PROCEDURE sp_actualizar_registro_calificado
    @codigo VARCHAR(100),
    @creditos INT,
    @horas INT,
    @metodologia VARCHAR(100),
    @duracion VARCHAR(50),
    @titulacion VARCHAR(200),
    @programa_id INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE registro_calificado
    SET creditos = @creditos,
        horas = @horas,
        metodologia = @metodologia,
        duracion = @duracion,
        titulacion = @titulacion,
        programa_id = @programa_id
    WHERE codigo = @codigo AND activo = 1;

    EXEC sp_consultar_registro_calificado @codigo;
END;
GO

CREATE OR ALTER PROCEDURE sp_actualizar_activ_academica
    @id INT,
    @nombre VARCHAR(200),
    @creditos INT,
    @horas INT,
    @idioma VARCHAR(50),
    @es_espejo BIT,
    @programa_id INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE activ_academica
    SET nombre = @nombre,
        creditos = @creditos,
        horas = @horas,
        idioma = @idioma,
        es_espejo = @es_espejo,
        programa_id = @programa_id
    WHERE id = @id AND activo = 1;

    SELECT aa.id, aa.nombre, aa.creditos, aa.horas, aa.idioma, aa.es_espejo, aa.programa_id, p.nombre AS programa_nombre, aa.activo
    FROM activ_academica aa
    INNER JOIN programa p ON aa.programa_id = p.id
    WHERE aa.id = @id;
END;
GO

CREATE OR ALTER PROCEDURE sp_actualizar_pasantia
    @id INT,
    @nombre VARCHAR(200),
    @descripcion VARCHAR(MAX),
    @programa_id INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE pasantia
    SET nombre = @nombre,
        descripcion = @descripcion,
        programa_id = @programa_id
    WHERE id = @id AND activo = 1;

    SELECT p.id, p.nombre, p.descripcion, p.programa_id, pr.nombre AS programa_nombre, p.activo
    FROM pasantia p
    INNER JOIN programa pr ON p.programa_id = pr.id
    WHERE p.id = @id;
END;
GO

CREATE OR ALTER PROCEDURE sp_actualizar_premio
    @id INT,
    @nombre VARCHAR(200),
    @entidad_otorgante VARCHAR(200),
    @fecha_otorgamiento DATE,
    @programa_id INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE premio
    SET nombre = @nombre,
        entidad_otorgante = @entidad_otorgante,
        fecha_otorgamiento = @fecha_otorgamiento,
        programa_id = @programa_id
    WHERE id = @id AND activo = 1;

    SELECT p.id, p.nombre, p.entidad_otorgante, p.fecha_otorgamiento, p.programa_id, pr.nombre AS programa_nombre, p.activo
    FROM premio p
    INNER JOIN programa pr ON p.programa_id = pr.id
    WHERE p.id = @id;
END;
GO

CREATE OR ALTER PROCEDURE sp_actualizar_docente_departamento
    @id INT,
    @docente_nombre VARCHAR(200),
    @dedicacion VARCHAR(100),
    @modalidad VARCHAR(100),
    @fecha_inicio DATE,
    @fecha_fin DATE,
    @programa_id INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE docente_departamento
    SET docente_nombre = @docente_nombre,
        dedicacion = @dedicacion,
        modalidad = @modalidad,
        fecha_inicio = @fecha_inicio,
        fecha_fin = @fecha_fin,
        programa_id = @programa_id
    WHERE id = @id AND activo = 1;

    SELECT d.id, d.docente_nombre, d.dedicacion, d.modalidad, d.fecha_inicio, d.fecha_fin, d.programa_id, p.nombre AS programa_nombre, d.activo
    FROM docente_departamento d
    INNER JOIN programa p ON d.programa_id = p.id
    WHERE d.id = @id;
END;
GO

CREATE OR ALTER PROCEDURE sp_actualizar_alianza
    @id INT,
    @aliado_nit VARCHAR(20),
    @programa_id INT,
    @docente_responsable VARCHAR(200),
    @fecha_inicio DATE,
    @fecha_fin DATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE alianza
    SET aliado_nit = @aliado_nit,
        programa_id = @programa_id,
        docente_responsable = @docente_responsable,
        fecha_inicio = @fecha_inicio,
        fecha_fin = @fecha_fin
    WHERE id = @id AND activo = 1;

    SELECT a.id, a.aliado_nit, al.razon_social AS aliado_nombre, a.programa_id, p.nombre AS programa_nombre, a.docente_responsable, a.fecha_inicio, a.fecha_fin, a.activo
    FROM alianza a
    INNER JOIN aliado al ON a.aliado_nit = al.nit
    INNER JOIN programa p ON a.programa_id = p.id
    WHERE a.id = @id;
END;
GO

-- ============================================================================
-- SPs: TABLAS PUENTE (Relaciones N a M)
-- ============================================================================
CREATE OR ALTER PROCEDURE sp_asignar_programa_ac @programa_id INT, @area_conocimiento_id INT AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO programa_ac (programa_id, area_conocimiento_id) VALUES (@programa_id, @area_conocimiento_id);
END;
GO

CREATE OR ALTER PROCEDURE sp_retirar_programa_ac @programa_id INT, @area_conocimiento_id INT AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM programa_ac WHERE programa_id = @programa_id AND area_conocimiento_id = @area_conocimiento_id;
END;
GO

CREATE OR ALTER PROCEDURE sp_asignar_programa_pe @programa_id INT, @practica_estrategia_id INT AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO programa_pe (programa_id, practica_estrategia_id) VALUES (@programa_id, @practica_estrategia_id);
END;
GO

CREATE OR ALTER PROCEDURE sp_retirar_programa_pe @programa_id INT, @practica_estrategia_id INT AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM programa_pe WHERE programa_id = @programa_id AND practica_estrategia_id = @practica_estrategia_id;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_asignar_programa_ci
    @programa_id INT,
    @car_innovacion_id INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.programa_ci (programa_id, car_innovacion_id)
    VALUES (@programa_id, @car_innovacion_id);
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_retirar_programa_ci
    @programa_id INT,
    @car_innovacion_id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM dbo.programa_ci
    WHERE programa_id = @programa_id
      AND car_innovacion_id = @car_innovacion_id;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_asignar_an_programa
    @aspecto_normativo_id INT,
    @programa_id INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.an_programa (aspecto_normativo_id, programa_id)
    VALUES (@aspecto_normativo_id, @programa_id);
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_retirar_an_programa
    @aspecto_normativo_id INT,
    @programa_id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM dbo.an_programa
    WHERE aspecto_normativo_id = @aspecto_normativo_id
      AND programa_id = @programa_id;
END;
GO

-- Verificacion: deben aparecer los cuatro procedimientos.
SELECT name AS procedimiento
FROM sys.procedures
WHERE name IN (
    'sp_asignar_programa_ci',
    'sp_retirar_programa_ci',
    'sp_asignar_an_programa',
    'sp_retirar_an_programa'
)
ORDER BY name;
GO

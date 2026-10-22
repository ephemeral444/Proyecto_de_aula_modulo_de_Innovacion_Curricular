USE [innovacion_curricular];
GO

-- ============================================================================
-- 1. CREACIÓN DE TABLAS DE LA V2 (CON CLAVES FORÁNEAS)
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
        idioma VARCHAR(50) DEFAULT 'Español',
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
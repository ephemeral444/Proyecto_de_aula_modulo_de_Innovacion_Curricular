-- =======================================================
-- Módulo de Innovación Curricular -- Versión 1
-- 7 Tablas independientes sin claves foráneas
-- Motor: SQL Server 2022
-- =======================================================

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'innovacion_curricular')
BEGIN
    CREATE DATABASE innovacion_curricular;
END
GO

USE innovacion_curricular;
GO

-- 1. area_conocimiento
IF OBJECT_ID('dbo.area_conocimiento', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.area_conocimiento (
        id INT NOT NULL,
        gran_area VARCHAR(150) NOT NULL,
        area VARCHAR(150) NOT NULL,
        disciplina VARCHAR(150) NOT NULL,
        activo BIT NOT NULL DEFAULT 1,
        CONSTRAINT pk_area_conocimiento PRIMARY KEY (id)
    );
END
GO

-- 2. universidad
IF OBJECT_ID('dbo.universidad', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.universidad (
        id INT NOT NULL,
        nombre VARCHAR(150) NOT NULL,
        tipo VARCHAR(50) NOT NULL,
        ciudad VARCHAR(100) NOT NULL,
        activo BIT NOT NULL DEFAULT 1,
        CONSTRAINT pk_universidad PRIMARY KEY (id)
    );
END
GO

-- 3. aspecto_normativo
IF OBJECT_ID('dbo.aspecto_normativo', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.aspecto_normativo (
        id INT NOT NULL,
        tipo VARCHAR(50) NOT NULL,
        descripcion VARCHAR(500) NOT NULL,
        fuente VARCHAR(200) NOT NULL,
        activo BIT NOT NULL DEFAULT 1,
        CONSTRAINT pk_aspecto_normativo PRIMARY KEY (id)
    );
END
GO

-- 4. practica_estrategia
IF OBJECT_ID('dbo.practica_estrategia', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.practica_estrategia (
        id INT NOT NULL,
        tipo VARCHAR(50) NOT NULL,
        nombre VARCHAR(150) NOT NULL,
        descripcion VARCHAR(500) NOT NULL,
        activo BIT NOT NULL DEFAULT 1,
        CONSTRAINT pk_practica_estrategia PRIMARY KEY (id)
    );
END
GO

-- 5. enfoque
IF OBJECT_ID('dbo.enfoque', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.enfoque (
        id INT NOT NULL,
        nombre VARCHAR(150) NOT NULL,
        descripcion VARCHAR(500) NOT NULL,
        activo BIT NOT NULL DEFAULT 1,
        CONSTRAINT pk_enfoque PRIMARY KEY (id)
    );
END
GO

-- 6. car_innovacion
IF OBJECT_ID('dbo.car_innovacion', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.car_innovacion (
        id INT NOT NULL,
        nombre VARCHAR(150) NOT NULL,
        descripcion VARCHAR(500) NOT NULL,
        tipo VARCHAR(50) NOT NULL,
        activo BIT NOT NULL DEFAULT 1,
        CONSTRAINT pk_car_innovacion PRIMARY KEY (id)
    );
END
GO

-- 7. aliado (clave primaria: nit)
IF OBJECT_ID('dbo.aliado', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.aliado (
        nit VARCHAR(20) NOT NULL,
        razon_social VARCHAR(150) NOT NULL,
        contacto VARCHAR(150) NOT NULL,
        ciudad VARCHAR(100) NOT NULL,
        activo BIT NOT NULL DEFAULT 1,
        CONSTRAINT pk_aliado PRIMARY KEY (nit)
    );
END
GO

-- Inserción inicial de semillas para validación de la v1
INSERT INTO dbo.universidad (id, nombre, tipo, ciudad, activo) VALUES 
(1, 'Institucion Universitaria ITM', 'Principal', 'Medellin', 1),
(2, 'Universidad de Antioquia', 'Seccional', 'Medellin', 1);

INSERT INTO dbo.aliado (nit, razon_social, contacto, ciudad, activo) VALUES 
('900123456-1', 'Ruta N Medellin', 'contacto@rutanmedellin.org', 'Medellin', 1);

INSERT INTO dbo.area_conocimiento (id, gran_area, area, disciplina, activo) VALUES 
(1, 'Ingenieria, Arquitectura y Urbanismo', 'Ingenieria de Sistemas', 'Ingenieria de Software', 1);
GO
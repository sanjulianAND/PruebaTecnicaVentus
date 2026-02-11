-- ============================================
-- Script de Creación de Base de Datos - Biblioteca
-- Prueba Técnica .NET - Versión Mejorada
-- ============================================

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'BibliotecaDB')
BEGIN
    CREATE DATABASE BibliotecaDB;
END
GO

USE BibliotecaDB;
GO

-- ============================================
-- Tabla: Autores
-- ============================================
IF OBJECT_ID('dbo.Autores', 'U') IS NOT NULL
    DROP TABLE dbo.Autores;
GO

CREATE TABLE dbo.Autores (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NombreCompleto NVARCHAR(200) NOT NULL,
    FechaNacimiento DATE NOT NULL,
    CiudadProcedencia NVARCHAR(100) NOT NULL,
    CorreoElectronico NVARCHAR(150) NOT NULL,
    FechaCreacion DATETIME2 DEFAULT GETDATE(),
    FechaActualizacion DATETIME2 DEFAULT GETDATE(),
    Activo BIT DEFAULT 1
);
GO

CREATE INDEX IX_Autores_CorreoElectronico ON dbo.Autores(CorreoElectronico);
CREATE INDEX IX_Autores_NombreCompleto ON dbo.Autores(NombreCompleto);
CREATE INDEX IX_Autores_Activo ON dbo.Autores(Activo);
GO

-- ============================================
-- Tabla: Libros
-- ============================================
IF OBJECT_ID('dbo.Libros', 'U') IS NOT NULL
    DROP TABLE dbo.Libros;
GO

CREATE TABLE dbo.Libros (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Titulo NVARCHAR(300) NOT NULL,
    Anio INT NOT NULL,
    Genero NVARCHAR(100) NOT NULL,
    NumeroPaginas INT NOT NULL,
    AutorId INT NOT NULL,
    FechaCreacion DATETIME2 DEFAULT GETDATE(),
    FechaActualizacion DATETIME2 DEFAULT GETDATE(),
    Activo BIT DEFAULT 1,
    CONSTRAINT FK_Libros_Autores FOREIGN KEY (AutorId) 
        REFERENCES dbo.Autores(Id)
);
GO

CREATE INDEX IX_Libros_AutorId ON dbo.Libros(AutorId);
CREATE INDEX IX_Libros_Titulo ON dbo.Libros(Titulo);
CREATE INDEX IX_Libros_Genero ON dbo.Libros(Genero);
CREATE INDEX IX_Libros_Activo ON dbo.Libros(Activo);
GO

-- ============================================
-- Tabla: Usuarios
-- ============================================
IF OBJECT_ID('dbo.Usuarios', 'U') IS NOT NULL
    DROP TABLE dbo.Usuarios;
GO

CREATE TABLE dbo.Usuarios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NombreUsuario NVARCHAR(100) NOT NULL,
    CorreoElectronico NVARCHAR(150) NOT NULL,
    PasswordHash NVARCHAR(500) NOT NULL,
    Rol NVARCHAR(50) DEFAULT 'Usuario',
    FechaCreacion DATETIME2 DEFAULT GETDATE(),
    FechaActualizacion DATETIME2 DEFAULT GETDATE(),
    Activo BIT DEFAULT 1
);
GO

CREATE UNIQUE INDEX IX_Usuarios_CorreoElectronico ON dbo.Usuarios(CorreoElectronico);
CREATE UNIQUE INDEX IX_Usuarios_NombreUsuario ON dbo.Usuarios(NombreUsuario);
CREATE INDEX IX_Usuarios_Activo ON dbo.Usuarios(Activo);
GO

-- ============================================
-- Datos de prueba - Autores
-- ============================================
INSERT INTO dbo.Autores (NombreCompleto, FechaNacimiento, CiudadProcedencia, CorreoElectronico, Activo)
VALUES 
    ('Gabriel García Márquez', '1927-03-06', 'Aracataca, Colombia', 'gabriel.garcia@literatura.com', 1),
    ('Isabel Allende', '1942-08-02', 'Lima, Perú', 'isabel.allende@literatura.com', 1),
    ('Julio Cortázar', '1914-08-26', 'Bruselas, Bélgica', 'julio.cortazar@literatura.com', 1),
    ('Mario Vargas Llosa', '1936-03-28', 'Arequipa, Perú', 'mario.vargas@literatura.com', 1),
    ('Pablo Neruda', '1904-07-12', 'Parral, Chile', 'pablo.neruda@poesia.com', 1),
    ('Jorge Luis Borges', '1899-08-24', 'Buenos Aires, Argentina', 'jorge.borges@literatura.com', 1);
GO

-- ============================================
-- Datos de prueba - Libros
-- ============================================
INSERT INTO dbo.Libros (Titulo, Anio, Genero, NumeroPaginas, AutorId, Activo)
VALUES 
    ('Cien años de soledad', 1967, 'Realismo mágico', 471, 1, 1),
    ('El amor en los tiempos del cólera', 1985, 'Novela romántica', 368, 1, 1),
    ('La casa de los espíritus', 1982, 'Realismo mágico', 448, 2, 1),
    ('Rayuela', 1963, 'Novela experimental', 736, 3, 1),
    ('La ciudad y los perros', 1963, 'Novela', 413, 4, 1),
    ('Veinte poemas de amor', 1924, 'Poesía', 156, 5, 1),
    ('Ficciones', 1944, 'Cuentos', 203, 6, 1);
GO

-- ============================================
-- Usuario Administrador por defecto
-- Password: Admin123!
-- ============================================
INSERT INTO dbo.Usuarios (NombreUsuario, CorreoElectronico, PasswordHash, Rol, Activo)
VALUES ('admin', 'admin@biblioteca.com', '$2a$11$qG3q7J8XqG3q7J8XqG3q7O8XqG3q7J8XqG3q7J8XqG3q7J8XqG3q7O', 'Administrador', 1);
GO

-- ============================================
-- Usuario Regular por defecto
-- Password: User123!
-- ============================================
INSERT INTO dbo.Usuarios (NombreUsuario, CorreoElectronico, PasswordHash, Rol, Activo)
VALUES ('usuario', 'usuario@biblioteca.com', '$2a$11$qG3q7J8XqG3q7J8XqG3q7O8XqG3q7J8XqG3q7J8XqG3q7J8XqG3q7O', 'Usuario', 1);
GO
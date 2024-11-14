IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Beneficios] (
    [Id] int NOT NULL IDENTITY,
    [Descripcion] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Beneficios] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Disciplinas] (
    [Id] int NOT NULL IDENTITY,
    [Nombre] nvarchar(max) NOT NULL,
    [Foto] nvarchar(max) NULL,
    CONSTRAINT [PK_Disciplinas] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Estados] (
    [Id] int NOT NULL IDENTITY,
    [Descripcion] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Estados] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Socios] (
    [Id] int NOT NULL IDENTITY,
    [Nombre] nvarchar(max) NOT NULL,
    [Apellido] nvarchar(max) NOT NULL,
    [DNI] int NOT NULL,
    [FechaNacimiento] datetime2 NOT NULL,
    [Foto] nvarchar(max) NULL,
    [FechaAlta] datetime2 NULL,
    [DisciplinaId] int NULL,
    [EstadoId] int NULL,
    [BeneficiosId] int NULL,
    CONSTRAINT [PK_Socios] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Socios_Beneficios_BeneficiosId] FOREIGN KEY ([BeneficiosId]) REFERENCES [Beneficios] ([Id]),
    CONSTRAINT [FK_Socios_Disciplinas_DisciplinaId] FOREIGN KEY ([DisciplinaId]) REFERENCES [Disciplinas] ([Id]),
    CONSTRAINT [FK_Socios_Estados_EstadoId] FOREIGN KEY ([EstadoId]) REFERENCES [Estados] ([Id])
);
GO

CREATE TABLE [Cuotas] (
    [Id] int NOT NULL IDENTITY,
    [SocioId] int NOT NULL,
    [Monto] int NOT NULL,
    [EmisionCuota] datetime2 NOT NULL,
    [FechaVencimiento] datetime2 NOT NULL,
    [Pagada] bit NOT NULL,
    CONSTRAINT [PK_Cuotas] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Cuotas_Socios_SocioId] FOREIGN KEY ([SocioId]) REFERENCES [Socios] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Cuotas_SocioId] ON [Cuotas] ([SocioId]);
GO

CREATE INDEX [IX_Socios_BeneficiosId] ON [Socios] ([BeneficiosId]);
GO

CREATE INDEX [IX_Socios_DisciplinaId] ON [Socios] ([DisciplinaId]);
GO

CREATE INDEX [IX_Socios_EstadoId] ON [Socios] ([EstadoId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20241114153936_inicial', N'8.0.10');
GO

COMMIT;
GO


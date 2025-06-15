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
CREATE TABLE [TodoItems] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(max) NULL,
    [IsCompleted] bit NOT NULL,
    CONSTRAINT [PK_TodoItems] PRIMARY KEY ([Id])
);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250615023202_InitialCreate', N'9.0.6');

COMMIT;
GO


-- =============================================
-- NASA APOD Gallery - Database Creation Script
-- =============================================

-- Create the database
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'NasaApodDb')
BEGIN
    CREATE DATABASE NasaApodDb;
END
GO

USE NasaApodDb;
GO

-- Create the Apod table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Apod')
BEGIN
    CREATE TABLE Apod (
        Id              INT IDENTITY(1,1) PRIMARY KEY,
        Date            DATE NOT NULL UNIQUE,          -- Unique so no duplicate dates
        Title           NVARCHAR(500) NOT NULL,
        Explanation     NVARCHAR(MAX) NULL,
        Url             NVARCHAR(1000) NOT NULL,
        MediaType       NVARCHAR(50) NULL,           -- "image" or "video"
        ServiceVersion  NVARCHAR(20) NULL,
        SavedAt         DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );
END
GO

PRINT 'Database NasaApodDb and table Apod created successfully.';
GO
-- =============================================
-- BATTLEGAME DATABASE
-- =============================================

USE master;
GO

IF DB_ID('BATTLEGAME') IS NULL
BEGIN
    CREATE DATABASE BATTLEGAME;
END
GO

USE BATTLEGAME;
GO


-- =============================================
-- DROP OLD TABLES
-- =============================================

IF OBJECT_ID('dbo.PlayerAsset', 'U') IS NOT NULL
    DROP TABLE dbo.PlayerAsset;

IF OBJECT_ID('dbo.Asset', 'U') IS NOT NULL
    DROP TABLE dbo.Asset;

IF OBJECT_ID('dbo.Player', 'U') IS NOT NULL
    DROP TABLE dbo.Player;
GO


-- =============================================
-- PLAYER TABLE
-- =============================================

CREATE TABLE dbo.Player
(
    PlayerId UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT PK_Player PRIMARY KEY
        DEFAULT NEWID(),

    PlayerName NVARCHAR(50) NOT NULL,
    FullName NVARCHAR(128) NOT NULL,
    Age INT NOT NULL,
    Level INT NOT NULL,
    Email NVARCHAR(64) NOT NULL
);
GO


-- =============================================
-- ASSET TABLE
-- =============================================

CREATE TABLE dbo.Asset
(
    AssetId UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT PK_Asset PRIMARY KEY
        DEFAULT NEWID(),

    AssetName NVARCHAR(64) NOT NULL,
    LevelRequire INT NOT NULL
);
GO


-- =============================================
-- PLAYER ASSET TABLE
-- =============================================

CREATE TABLE dbo.PlayerAsset
(
    PlayerId UNIQUEIDENTIFIER NOT NULL,
    AssetId UNIQUEIDENTIFIER NOT NULL,

    CONSTRAINT PK_PlayerAsset
        PRIMARY KEY (PlayerId, AssetId),

    CONSTRAINT FK_PlayerAsset_Player
        FOREIGN KEY (PlayerId)
        REFERENCES dbo.Player(PlayerId),

    CONSTRAINT FK_PlayerAsset_Asset
        FOREIGN KEY (AssetId)
        REFERENCES dbo.Asset(AssetId)
);
GO


-- =============================================
-- SAMPLE DATA
-- =============================================

DECLARE @Player1 UNIQUEIDENTIFIER = NEWID();
DECLARE @Player2 UNIQUEIDENTIFIER = NEWID();
DECLARE @Player3 UNIQUEIDENTIFIER = NEWID();

DECLARE @Hero1 UNIQUEIDENTIFIER = NEWID();
DECLARE @Hero2 UNIQUEIDENTIFIER = NEWID();


INSERT INTO dbo.Player
(
    PlayerId,
    PlayerName,
    FullName,
    Age,
    Level,
    Email
)
VALUES
(@Player1, 'Player 1', 'Nguyen Van A', 20, 10, 'player1@gmail.com'),
(@Player2, 'Player 2', 'Player 2', 19, 3, 'player2@gmail.com'),
(@Player3, 'Player 3', 'Player 3', 23, 10, 'player3@gmail.com');


INSERT INTO dbo.Asset
(
    AssetId,
    AssetName,
    LevelRequire
)
VALUES
(@Hero1, 'Hero 1', 5),
(@Hero2, 'Hero 2', 3);


INSERT INTO dbo.PlayerAsset
(
    PlayerId,
    AssetId
)
VALUES
(@Player1, @Hero1),
(@Player2, @Hero2),
(@Player3, @Hero1);

GO
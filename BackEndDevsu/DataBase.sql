CREATE DATABASE BankingDb;
GO
USE BankingDb;
GO

CREATE TABLE Person (
    PersonId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Gender NVARCHAR(10) NOT NULL,
    Age INT NOT NULL,
    Identification NVARCHAR(50) NOT NULL,
    Address NVARCHAR(200),
    Phone NVARCHAR(50)
);
GO

CREATE TABLE Client (
    ClientId INT IDENTITY(1,1) PRIMARY KEY,
    PersonId INT NOT NULL,
    Password NVARCHAR(100) NOT NULL,
    Status BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_Client_Person FOREIGN KEY (PersonId) REFERENCES Person(PersonId)
);
GO

CREATE TABLE Account (
    AccountId INT IDENTITY(1,1) PRIMARY KEY,
    ClientId INT NOT NULL,
    AccountNumber NVARCHAR(50) NOT NULL UNIQUE,
    AccountType NVARCHAR(50) NOT NULL,
    InitialBalance DECIMAL(18,2) NOT NULL,
    Status BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_Account_Client FOREIGN KEY (ClientId) REFERENCES Client(ClientId)
);
GO

CREATE TABLE Movement (
    MovementId INT IDENTITY(1,1) PRIMARY KEY,
    AccountId INT NOT NULL,
    Date DATETIME NOT NULL DEFAULT GETDATE(),
    MovementType NVARCHAR(20) NOT NULL,
    Value DECIMAL(18,2) NOT NULL,
    Balance DECIMAL(18,2) NOT NULL,
    CONSTRAINT FK_Movement_Account FOREIGN KEY (AccountId) REFERENCES Account(AccountId)
);
GO

IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'CalculatorDb')
BEGIN
    CREATE DATABASE CalculatorDb;
END

-- Creating Users table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'User')
BEGIN
CREATE TABLE [User]
(
    Id INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(10) NOT NULL UNIQUE
);
END

-- Creating CalculationHistory table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'CalculationHistory')
BEGIN
CREATE TABLE CalculationHistory
(
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT FOREIGN KEY REFERENCES [User](Id),
    FirstValue DECIMAL(12,2),
    SecondValue DECIMAL(12,2), 
    OperationType NVARCHAR(50), 
    Result DECIMAL(12,2) NOT NULL,
    CalculationDate DATETIME NOT NULL DEFAULT GETDATE()
);
END
GO

-- Creating CreateUser stored procedure
CREATE OR ALTER PROCEDURE CreateUser
        @Username NVARCHAR(10),
        @UserId INT OUTPUT
    AS
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM [User] WHERE Username = @Username)
        BEGIN
            INSERT INTO [User] (Username) VALUES (@Username)
            SET @UserId = SCOPE_IDENTITY()
        END
        ELSE
        BEGIN
            SET @UserId = NULL
        END
    END
GO
-- Creating AddCalculationHistory stored procedure
CREATE OR ALTER PROCEDURE AddCalculationHistory
                    @UserId INT,
                    @FirstValue DECIMAL(12,2),
                    @SecondValue DECIMAL(12,2),
                    @OperationType NVARCHAR(50), 
                    @Result DECIMAL(12,2)
                  AS
                  BEGIN
                    INSERT INTO CalculationHistory (UserId, FirstValue, SecondValue, OperationType, Result) 
                    VALUES (@UserId, @FirstValue, @SecondValue, @OperationType, @Result)
                  END;
GO

-- Creating GetUserById stored procedure
CREATE OR ALTER PROCEDURE GetUserById
    @UserId INT
AS
BEGIN
    SELECT * FROM [User] WHERE Id = @UserId
END;
GO
-- Creating GetCalculationHistoryByUserId stored procedure
CREATE OR ALTER PROCEDURE GetCalculationHistoryByUserId 
    @UserId INT
AS
BEGIN
    SELECT * FROM CalculationHistory WHERE UserId = @UserId
END;
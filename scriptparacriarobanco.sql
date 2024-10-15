-- Create database 'ContactManagment'
CREATE DATABASE ContactManagment;
GO

-- Select database 'ContactManagment'
USE ContactManagment;
GO

-- Create table 'Contact'
CREATE TABLE Contacts (
  Id INT IDENTITY(1,1) PRIMARY KEY,
  FirstName NVARCHAR(70) NOT NULL,
  LastName NVARCHAR(70) NOT NULL,
  AreaCode CHAR(2) NOT NULL,
  PhoneNumber INT NOT NULL,
  Email NVARCHAR(100) NOT NULL
);
GO
GO

-- Add sample records in Contact table -- Insere um contato na tabela 'Contatct'
INSERT INTO Contacts (FirstName, LastName, AreaCode, PhoneNumber, Email)
VALUES ('João', 'Silva', '11', '777799999', 'joao.silva@email.com');
GO
INSERT INTO Contacts (FirstName, LastName, AreaCode, PhoneNumber, Email)
VALUES ('Maria', 'Silva', '11', '888899999', 'maria.silva@email.com');
GO
INSERT INTO Contacts (FirstName, LastName, AreaCode, PhoneNumber, Email)
VALUES ('Jose', 'Silva', '11', '999999999', 'jose.silva@email.com');
GO
-- 5 Lista os contatos inseridos
SELECT TOP 10 * FROM Contacts
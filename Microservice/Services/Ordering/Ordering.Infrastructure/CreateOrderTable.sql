CREATE TABLE Orders (
    Id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    UserName NVARCHAR(255) NOT NULL,
    TotalPrice DECIMAL(18, 2) NOT NULL,
    
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    EmailAddress NVARCHAR(255) NOT NULL,
    PhoneNumber NVARCHAR(20) NOT NULL,
    Address NVARCHAR(255) NOT NULL,
    City NVARCHAR(100) NOT NULL,
    State NVARCHAR(100) NOT NULL,
    ZipCode NVARCHAR(20) NOT NULL,
    
    CardName NVARCHAR(255) NOT NULL,
    CardNumber NVARCHAR(16) NOT NULL,
    CVV NVARCHAR(4) NOT NULL,
    Expiration NVARCHAR(5) NOT NULL,                               
    
    CreatedBy NVARCHAR(255) NOT NULL,                        
    CreatedDate DATETIME NOT NULL DEFAULT GETUTCDATE(),      
    UpdatedBy NVARCHAR(255) NULL,                            
    UpdatedDate DATETIME NULL,
    
    CONSTRAINT PK_Orders PRIMARY KEY (Id)
);

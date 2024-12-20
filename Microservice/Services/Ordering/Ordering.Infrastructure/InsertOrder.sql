USE [Order]
GO
/****** Object:  StoredProcedure [dbo].[InsertOrder]    Script Date: 12/3/2024 12:10:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[InsertOrder]
    @UserName NVARCHAR(255),
    @TotalPrice DECIMAL(18, 2),
    @FirstName NVARCHAR(100),
    @LastName NVARCHAR(100),
    @EmailAddress NVARCHAR(255),
    @PhoneNumber NVARCHAR(20),
    @Address NVARCHAR(255),
    @City NVARCHAR(100),
    @State NVARCHAR(100),
    @ZipCode NVARCHAR(20),
    @CardName NVARCHAR(255),
    @CardNumber NVARCHAR(16),
    @CVV NVARCHAR(4),
    @Expiration NVARCHAR(5),
    @CreatedBy NVARCHAR(255)
AS
BEGIN
    DECLARE @InsertedOrder TABLE (
        Id UNIQUEIDENTIFIER
    );

    INSERT INTO Orders (
        UserName,
        TotalPrice,
        FirstName,
        LastName,
        EmailAddress,
        PhoneNumber,
        Address,
        City,
        State,
        ZipCode,
        CardName,
        CardNumber,
        CVV,
        Expiration,
        CreatedBy
    )
    OUTPUT inserted.Id INTO @InsertedOrder
    VALUES (
        @UserName,
        @TotalPrice,
        @FirstName,
        @LastName,
        @EmailAddress,
        @PhoneNumber,
        @Address,
        @City,
        @State,
        @ZipCode,
        @CardName,
        @CardNumber,
        @CVV,
        @Expiration,
        @CreatedBy
    );

    -- Return only the Id of the newly inserted order
    SELECT Id FROM @InsertedOrder;
END;

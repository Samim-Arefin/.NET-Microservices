USE [Order]
GO
/****** Object:  StoredProcedure [dbo].[UpdateOrder]    Script Date: 12/3/2024 2:41:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[UpdateOrder]
    @Id UNIQUEIDENTIFIER,
    @UserName NVARCHAR(255) = NULL,
    @TotalPrice DECIMAL(18, 2) = NULL,
    @FirstName NVARCHAR(100) = NULL,
    @LastName NVARCHAR(100) = NULL,
    @EmailAddress NVARCHAR(255) = NULL,
    @PhoneNumber NVARCHAR(20) = NULL,
    @Address NVARCHAR(255) = NULL,
    @City NVARCHAR(100) = NULL,
    @State NVARCHAR(100) = NULL,
    @ZipCode NVARCHAR(20) = NULL,
    @CardName NVARCHAR(255) = NULL,
    @CardNumber NVARCHAR(16) = NULL,
    @CVV NVARCHAR(4) = NULL,
    @Expiration NVARCHAR(5) = NULL,
    @UpdatedBy NVARCHAR(255)
AS
BEGIN
    BEGIN TRANSACTION;
    UPDATE Orders
    SET
        UserName = COALESCE(@UserName, UserName),
        TotalPrice = COALESCE(@TotalPrice, TotalPrice),
        FirstName = COALESCE(@FirstName, FirstName),
        LastName = COALESCE(@LastName, LastName),
        EmailAddress = COALESCE(@EmailAddress, EmailAddress),
        PhoneNumber = COALESCE(@PhoneNumber, PhoneNumber),
        Address = COALESCE(@Address, Address),
        City = COALESCE(@City, City),
        State = COALESCE(@State, State),
        ZipCode = COALESCE(@ZipCode, ZipCode),
        CardName = COALESCE(@CardName, CardName),
        CardNumber = COALESCE(@CardNumber, CardNumber),
        CVV = COALESCE(@CVV, CVV),
        Expiration = COALESCE(@Expiration, Expiration),
        UpdatedBy = @UpdatedBy,
        UpdatedDate = GETUTCDATE()
    WHERE Id = @Id;
    IF @@ROWCOUNT = 0
    BEGIN
        ROLLBACK TRANSACTION;
    END
    ELSE
    BEGIN
        COMMIT TRANSACTION;
    END
END;

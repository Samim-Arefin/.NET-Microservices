CREATE PROCEDURE GetOrdersByUserName
    @UserName NVARCHAR(255)
AS
BEGIN
    SELECT
        Id,
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
        CreatedBy,
        CreatedDate,
        UpdatedBy,
        UpdatedDate
    FROM Orders
    WHERE UserName = @UserName;
END;

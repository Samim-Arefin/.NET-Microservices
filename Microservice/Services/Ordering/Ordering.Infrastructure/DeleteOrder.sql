USE [Order]
GO
/****** Object:  StoredProcedure [dbo].[DeleteOrder]    Script Date: 12/3/2024 3:01:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[DeleteOrder]
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    BEGIN TRANSACTION;
    DELETE FROM Orders
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

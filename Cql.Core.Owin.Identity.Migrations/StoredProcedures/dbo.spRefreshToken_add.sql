IF EXISTS(SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[spRefreshToken_Add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE [dbo].[spRefreshToken_Add];
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.spRefreshToken_add
    @Id nvarchar(128),
    @Subject nvarchar(255),
    @ClientId nvarchar(128),
    @IssuedUtc datetime,
    @ExpiresUtc datetime,
    @ProtectedTicket nvarchar(4000)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DELETE t
    FROM dbo.RefreshTokens t
    WHERE ExpiresUtc < GETUTCDATE()

    INSERT INTO dbo.RefreshTokens (Id, Subject, ClientId, IssuedUtc, ExpiresUtc, ProtectedTicket)
    VALUES (@Id, @Subject, @ClientId, @IssuedUtc, @ExpiresUtc, @ProtectedTicket);

    RETURN 1;
END
GO

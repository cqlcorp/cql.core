IF EXISTS(SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[spUserRoles_exists]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE [dbo].[spUserRoles_exists];
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.spUserRoles_exists
    @UserId int,
    @RoleName nvarchar(255)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @roleId int,
            @exists bit = 0;

    SELECT @roleId = r.Id
    FROM dbo.AspNetRoles r
    WHERE r.Name = @RoleName;

    IF @RoleId IS NOT NULL
    BEGIN
        IF EXISTS (
            SELECT TOP 1 1
            FROM dbo.AspNetUserRoles r
            WHERE r.UserId = @UserId AND r.RoleId = @RoleId
        )
        BEGIN
            SET @exists = 1;
        END
    END

    SELECT @exists;
END
GO

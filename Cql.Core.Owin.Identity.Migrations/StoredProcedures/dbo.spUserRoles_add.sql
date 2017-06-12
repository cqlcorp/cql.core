IF EXISTS(SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[spUserRoles_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE [dbo].[spUserRoles_add];
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.spUserRoles_add
    @UserId int,
    @RoleName nvarchar(255)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @RoleId nvarchar(128);

    SELECT @RoleId = r.Id
    FROM dbo.AspNetRoles r
    WHERE r.Name = @RoleName;

    IF @RoleId IS NOT NULL
    BEGIN
        IF NOT EXISTS (
            SELECT TOP 1 1
            FROM dbo.AspNetUserRoles r
            WHERE r.UserId = @UserId AND r.RoleId = @RoleId
        )
        BEGIN
            INSERT INTO dbo.AspNetUserRoles (UserId, RoleId)
            VALUES (@UserId, @RoleId);
        END
    END
END
GO

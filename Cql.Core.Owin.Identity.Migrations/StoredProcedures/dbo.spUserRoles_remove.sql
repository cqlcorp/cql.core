IF EXISTS(SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[spUserRoles_remove]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE [dbo].[spUserRoles_remove];
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.spUserRoles_remove
    @UserId int,
    @RoleName nvarchar(255)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DELETE userRole
    FROM dbo.AspNetUserRoles userRole
    JOIN dbo.AspNetRoles roles ON userRole.RoleId = roles.Id
    WHERE userRole.UserId = @UserId AND roles.Name = @RoleName;

END

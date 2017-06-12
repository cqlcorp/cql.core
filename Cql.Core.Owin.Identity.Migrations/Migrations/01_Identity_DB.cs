using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;

using Cql.FluentMigrator.Extensions;

using FluentMigrator;

namespace Cql.Core.Owin.Identity.Migrations.Migrations
{
    [Migration(01)]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class _01_Identity_DB : Migration
    {
        public override void Down() {}

        public override void Up()
        {
            Step_01_CreateTables();

            Step_02_CreateForeignKeys();

            Step_03_DeployStoredProcs();

            Step_04_SeedData();
        }

        private void Step_01_CreateTables()
        {
            const int stringIdLength = 128;
            const int userNameLength = 255;

            Create.Table("Clients")
                .WithColumn("Id").AsStringIdColumn()
                .WithColumn("Secret").AsString(255).NotNullable()
                .WithColumn("Name").AsString(100).NotNullable()
                .WithColumn("ApplicationType").AsInt32().NotNullable()
                .WithColumn("Active").AsBoolean().NotNullable()
                .WithColumn("RefreshTokenLifeTime").AsInt32().NotNullable()
                .WithColumn("AllowedOrigin").AsString(4000);

            Create.Table("RefreshTokens")
                .WithColumn("Id").AsStringIdColumn()
                .WithColumn("Subject").AsString(userNameLength).NotNullable()
                .WithColumn("ClientId").AsString(stringIdLength).NotNullable()
                .WithColumn("IssuedUtc").AsDateTime().NotNullable()
                .WithColumn("ExpiresUtc").AsDateTime().NotNullable()
                .WithColumn("ProtectedTicket").AsString(4000).NotNullable();

            Create.Table("AspNetRoles")
                .WithColumn("Id").AsIdColumn()
                .WithColumn("Name").AsString(255).NotNullable().Unique("RoleNameIndex");

            Create.Table("AspNetUserRoles")
                .WithColumn("UserId").AsInt32().PrimaryKey()
                .WithColumn("RoleId").AsInt32().PrimaryKey();

            Create.Table("AspNetUsers")
                .WithColumn("Id").AsIdColumn()
                .WithIndexedGuidColumn()
                .WithColumn("Email").AsString(userNameLength)
                .WithColumn("EmailConfirmed").AsBoolean().NotNullable()
                .WithColumn("PasswordHash").AsString(255).NotNullable()
                .WithColumn("SecurityStamp").AsString(255).NotNullable()
                .WithColumn("PhoneNumber").AsString(255).NotNullable()
                .WithColumn("PhoneNumberConfirmed").AsBoolean().NotNullable()
                .WithColumn("TwoFactorEnabled").AsBoolean().NotNullable()
                .WithColumn("LockoutEndDate").AsDateTimeOffset().Nullable()
                .WithColumn("LockoutEnabled").AsBoolean().NotNullable()
                .WithColumn("AccessFailedCount").AsInt32().NotNullable()
                .WithColumn("UserName").AsString(userNameLength).NotNullable().Unique("UserNameIndex")
                .WithColumn("AccessRevokedDate").AsDateTimeOffset().Nullable()
                .WithColumn("AccessRevokedBy").AsInt32().Nullable()
                .WithColumn("LastActivityDate").AsDateTimeOffset().Nullable()
                .WithColumn("CreatedDate").AsDateTimeOffset().NotNullable().WithDefaultValue(SqlFunctions.SysDateTimeOffset);

            Create.Table("AspNetUserClaims")
                .WithColumn("Id").AsIdColumn()
                .WithColumn("UserId").AsInt32().NotNullable().Indexed()
                .WithColumn("ClaimType").AsString(1024).NotNullable()
                .WithColumn("ClaimValue").AsString(4000).NotNullable();

            Create.Table("AspNetUserLogins")
                .WithColumn("LoginProvider").AsString(128).NotNullable().PrimaryKey()
                .WithColumn("ProviderKey").AsString(128).NotNullable().PrimaryKey()
                .WithColumn("UserId").AsInt32().NotNullable().PrimaryKey();
        }

        private void Step_02_CreateForeignKeys()
        {
            Create.ForeignKey()
                .FromTable("AspNetUserRoles").ForeignColumn("UserId")
                .ToTable("AspNetUsers").PrimaryColumn("Id")
                .OnDelete(Rule.Cascade);

            Create.ForeignKey()
                .FromTable("AspNetUserRoles").ForeignColumn("RoleId")
                .ToTable("AspNetRoles").PrimaryColumn("Id")
                .OnDelete(Rule.Cascade);

            Create.ForeignKey()
                .FromTable("AspNetUserClaims").ForeignColumn("UserId")
                .ToTable("AspNetUsers").PrimaryColumn("Id")
                .OnDelete(Rule.Cascade);

            Create.ForeignKey()
                .FromTable("AspNetUserLogins").ForeignColumn("UserId")
                .ToTable("AspNetUsers").PrimaryColumn("Id")
                .OnDelete(Rule.Cascade);
        }

        private void Step_03_DeployStoredProcs()
        {
            this.DeployStoredProcedure("dbo.spRefreshToken_add");
            this.DeployStoredProcedure("dbo.spUserRoles_add");
            this.DeployStoredProcedure("dbo.spUserRoles_remove");
            this.DeployStoredProcedure("dbo.spUserRoles_exists");
        }

        private void Step_04_SeedData()
        {
            if (1 == int.Parse("1"))
            {
                // TODO: Seed your own data.
                throw new NotImplementedException("You should remove this line and put in your own seed data for roles, clients, etc...");
            }

            Insert.IntoTable("AspNetRoles")
                .Row(new {Name = "Admin"})
                .Row(new {Name = "User"})
                .Row(new {Name = "Account Manager"});

            Insert.IntoTable("Clients")
                .Row(
                    new
                    {
                        Id = "web",
                        Secret = "7f386d20fc0a46f4af8f0259963db4b97eeff7b22a694bd8806bb59c4da6fee4",
                        Name = "Web Application",
                        ApplicationType = 0,
                        Active = true,
                        RefreshTokenLifetime = 1440,
                        AllowedOrigin = "*"
                    })
                .Row(
                    new
                    {
                        Id = "svc",
                        Secret = "b7c66f497abb47ea9d3af53fac590dfa23b127e091b34e01a1da7e8496fc2a5c",
                        Name = "External Service",
                        ApplicationType = 1,
                        Active = true,
                        RefreshTokenLifetime = 1440,
                        AllowedOrigin = "*"
                    })
                .Row(
                    new
                    {
                        Id = "swagger",
                        Secret = "4f8e1feef0704a64bc397b1c4d2734b680eb6191597a4b9ba59d424fbf3bd973",
                        Name = "Swagger",
                        ApplicationType = 2,
                        Active = true,
                        RefreshTokenLifetime = 3600,
                        AllowedOrigin = "*"
                    });
        }
    }
}

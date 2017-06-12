using FluentMigrator;
using FluentMigrator.Builders.Alter.Table;
using FluentMigrator.Builders.Create.Table;

namespace Cql.Core.Owin.Identity.Migrations
{
    public static class ExtensionsForDbMigrations
    {
        /// <summary>
        /// Creates column as a standard auto-increment 64 bit integer ID primary key column
        /// </summary>
        public static ICreateTableColumnOptionOrWithColumnSyntax AsBigIdColumn(this ICreateTableColumnAsTypeSyntax syntax)
        {
            return syntax.AsInt64().Identity().NotNullable().PrimaryKey();
        }

        public static ICreateTableColumnOptionOrWithColumnSyntax AsDateTimeOffset(this ICreateTableColumnAsTypeSyntax syntax)
        {
            return syntax.AsCustom("DateTimeOffset");
        }

        public static IAlterTableColumnOptionOrAddColumnOrAlterColumnSyntax AsDateTimeOffset(this IAlterTableColumnAsTypeSyntax syntax)
        {
            return syntax.AsCustom("DateTimeOffset");
        }

        /// <summary>
        /// Creates column as a standard auto-increment 32 bit integer ID primary key column
        /// </summary>
        public static ICreateTableColumnOptionOrWithColumnSyntax AsIdColumn(this ICreateTableColumnAsTypeSyntax syntax)
        {
            return syntax.AsInt32().Identity().NotNullable().PrimaryKey();
        }

        public static ICreateTableColumnOptionOrWithColumnSyntax AsMaxString(this ICreateTableColumnAsTypeSyntax syntax)
        {
            return syntax.AsCustom("nvarchar(MAX)");
        }

        public static IAlterTableColumnOptionOrAddColumnOrAlterColumnSyntax AsMaxString(this IAlterTableColumnAsTypeSyntax syntax)
        {
            return syntax.AsCustom("nvarchar(MAX)");
        }

        public static ICreateTableColumnOptionOrWithColumnSyntax AsStringIdColumn(this ICreateTableColumnAsTypeSyntax syntax)
        {
            return syntax.AsString(128).NotNullable().PrimaryKey();
        }

        public static ICreateTableWithColumnSyntax WithChangeTrackingColumns(this ICreateTableWithColumnSyntax syntax)
        {
            return syntax
                .WithModifiedColumns()
                .WithCreatedColumns();
        }

        public static ICreateTableWithColumnSyntax WithCreatedColumns(this ICreateTableWithColumnSyntax syntax)
        {
            return syntax
                .WithColumn("CreatedDate").AsDateTimeOffset().NotNullable().WithDefaultValue(SqlFunctions.SysDateTimeOffset)
                .WithColumn("CreatedBy").AsInt32().NotNullable();
        }

        public static ICreateTableColumnOptionOrWithColumnSyntax WithIndexedGuidColumn(this ICreateTableWithColumnSyntax syntax, string name = "Guid")
        {
            return syntax.WithColumn(name).AsGuid().NotNullable().WithDefaultValue(SystemMethods.NewGuid).Unique();
        }

        public static ICreateTableWithColumnSyntax WithModifiedColumns(this ICreateTableWithColumnSyntax syntax)
        {
            return syntax
                .WithColumn("LastModifiedDate").AsDateTimeOffset().Nullable().WithDefaultValue(SqlFunctions.SysDateTimeOffset)
                .WithColumn("LastModifiedBy").AsInt32().Nullable();
        }

        public static ICreateTableColumnOptionOrWithColumnSyntax WithNonNullableAnsiStringColumn(this ICreateTableWithColumnSyntax syntax, string name, int size = 255)
        {
            return syntax.WithColumn(name).AsAnsiString(size).NotNullable().WithDefaultValue(string.Empty);
        }

        public static ICreateTableColumnOptionOrWithColumnSyntax WithNonNullableStringColumn(this ICreateTableWithColumnSyntax syntax, string name, int size = 255)
        {
            return syntax.WithColumn(name).AsString(size).NotNullable().WithDefaultValue(string.Empty);
        }
    }
}

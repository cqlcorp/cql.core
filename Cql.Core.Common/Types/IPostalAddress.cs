namespace Cql.Core.Common.Types
{
    public interface IPostalAddress
    {
        string Address1 { get; set; }

        string Address2 { get; set; }

        string City { get; set; }

        string Country { get; set; }

        string State { get; set; }

        string Zip { get; set; }
    }
}

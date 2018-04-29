namespace Peschu.Web.Models.Contracts
{
    public interface IPageListing
    {
        string Area { get; }

        string Controller { get; }

        string Action { get; }

        int CurrentPage { get; }

        int TotalPages { get; }

        int PreviousPage { get; }

        int NextPage { get; }

        string PreviousDisabled { get; }

        string NextDisabled { get; }

        int RouteId { get; }
    }
}

namespace Peschu.Web.Models
{
    using Contracts;

    public abstract class PageListingBaseModel : IPageListing
    {
        public string Area { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int PreviousPage => this.CurrentPage <= 1 ? 1 : this.CurrentPage - 1;

        public int NextPage
        {
            get
            {
                if (this.TotalPages == 0)
                {
                    return 1;
                }
                else
                {
                    return this.CurrentPage == this.TotalPages ? this.TotalPages : this.CurrentPage + 1;
                }
            }
        }

        public string PreviousDisabled => this.CurrentPage <= 1 ? "disabled" : string.Empty;

        public string NextDisabled => this.CurrentPage >= this.TotalPages ? "disabled" : string.Empty;

        public int RouteId { get; set; }
    }
}

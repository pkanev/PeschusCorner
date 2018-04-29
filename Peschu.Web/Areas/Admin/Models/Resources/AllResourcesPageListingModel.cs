namespace Peschu.Web.Areas.Admin.Models.Resources
{    
    using Services.Models;
    using System.Collections.Generic;
    using Web.Models;

    public class AllResourcesPageListingModel : PageListingBaseModel
    {
        public IEnumerable<ResourceListingModel> Resources { get; set; }
    }
}

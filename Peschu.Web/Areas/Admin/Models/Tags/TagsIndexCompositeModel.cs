namespace Peschu.Web.Areas.Admin.Models.Tags
{
    using Services.Models;
    using System.Collections.Generic;

    public class TagsIndexCompositeModel
    {
        public TagFormModel TagForm { get; set; }

        public IEnumerable<TagListingModel> Tags { get; set; }
    }
}

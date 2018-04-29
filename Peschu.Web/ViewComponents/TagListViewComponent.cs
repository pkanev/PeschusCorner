namespace Peschu.Web.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;
    using Services;
    using System.Threading.Tasks;

    public class TagListViewComponent : ViewComponent
    {
        private readonly ITagService tags;

        public TagListViewComponent(ITagService tags)
        {
            this.tags = tags;
        }

        public async Task<IViewComponentResult> InvokeAsync()
            => View(await this.tags.GetAllAsync());
    }
}

namespace Peschu.Services.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    
    public class ArticleDetailsModel : ArticleListingModel
    {
        public string Contents { get; set; }

        public IEnumerable<string> Paragraphs 
            => this.Contents.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
    }
}

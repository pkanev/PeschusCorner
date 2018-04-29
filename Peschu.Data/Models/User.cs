namespace Peschu.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;

    public class User : IdentityUser
    {
        public ICollection<Article> Articles { get; set; } = new HashSet<Article>();

    }
}
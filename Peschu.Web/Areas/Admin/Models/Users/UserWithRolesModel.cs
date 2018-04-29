namespace Peschu.Web.Areas.Admin.Models.Users
{
    using System.Collections.Generic;

    public class UserWithRolesModel
    {
        public string Id { get; set; }

        public string Email { get; set; }
        
        public IEnumerable<string> Roles { get; set; }
    }
}

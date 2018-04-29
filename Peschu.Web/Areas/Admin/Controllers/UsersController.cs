namespace Peschu.Web.Areas.Admin.Controllers
{
    using Data;
    using Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using Models.Users;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using static WebConstants;

    public class UsersController : BaseAdminController
    {
        private readonly PeschuDbContext db;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UsersController(PeschuDbContext db, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.db = db;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await this.db
                .Users
                .OrderBy(u => u.UserName)
                .Select(u => new ListUserModel
                {
                    Id = u.Id,
                    Username = u.UserName,
                    Email = u.Email
                })
                .ToListAsync();

            return View(users);
        }

        public async Task<IActionResult> Roles(string id)
        {
            var user = await this.GetUserById(id);

            var roles = await this.userManager.GetRolesAsync(user);
            var model = new UserWithRolesModel
            {
                Id = id,
                Email = user.Email,
                Roles = roles
            };
            return View(model);
        }

        public IActionResult AddRoleTo(string id)
        {
            var rolesSelectListItems = this.roleManager
                .Roles
                .Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name
                })
                .ToList();
            return View(rolesSelectListItems);
        }

        [HttpPost]
        public async Task<IActionResult> AddRoleTo(string id, [FromForm] string role)
        {
            var user = await this.GetUserById(id);
            var roleExists = await this.roleManager.RoleExistsAsync(role);
            if (!roleExists)
            {
                throw new ApplicationException($"Unable to load the role '{role}'.");
            }

            var result = await this.userManager.AddToRoleAsync(user, role);
            if (result.Succeeded)
            {
                TempData[TempDataSuccessMessageKey] = $"{user.Email} was assigned to role '{role}'.";
            }
            else
            {
                TempData[TempDataErrorMessageKey] = $"The role '{role}' was not added to the {user.Email}.";
            }

            return RedirectToAction(nameof(Roles), new { Id = id });
        }

        public IActionResult Create() => View();

        public async Task<IActionResult> RemoveRoleFrom(string id, string role)
        {
            var user = await this.GetUserById(id);
            var roleExists = await this.roleManager.RoleExistsAsync(role);
            if (!roleExists)
            {
                throw new ApplicationException($"Unable to load the role '{role}'.");
            }

            var result = await this.userManager.RemoveFromRoleAsync(user, role);
            if (result.Succeeded)
            {
                TempData[TempDataSuccessMessageKey] = $"{user.Email} was removed from role '{role}'.";
            }
            else
            {
                TempData[TempDataErrorMessageKey] = $"The role '{role}' was not removed from the {user.Email}.";
            }

            return RedirectToAction(nameof(Roles), new { Id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var newUser = new User
            {
                Email = model.Email,
                UserName = model.Username
            };

            var result = await this.userManager.CreateAsync(newUser, model.Password);
            if (!result.Succeeded)
            {
                AddErrors(result);
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ChangePassword(string id)
        {
            var user = await this.GetUserById(id);

            var model = new AdminChangePasswordModel
            {
                Email = user.Email
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(AdminChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await this.GetUserByEmail(model.Email);
            var token = await this.userManager.GeneratePasswordResetTokenAsync(user);
            var result = await this.userManager.ResetPasswordAsync(user, token, model.NewPassword);
            if (!result.Succeeded)
            {
                this.AddErrors(result);
                return View(model);
            }

            TempData[TempDataSuccessMessageKey] = $"The password was reset successfully for user {user.Email}";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            var user = await this.GetUserById(id);

            var model = new DestroyUserModel
            {
                Id = id,
                Email = user.Email
            };
            return View(model);
        }

        public async Task<IActionResult> Destroy(string id)
        {
            var user = await this.GetUserById(id);

            try
            {
                this.db.Users.Remove(user);
                this.db.SaveChanges();
            }
            catch (Exception ex)
            {
                TempData[TempDataErrorMessageKey] = $"User {user.Email} could not be deleted from the database. Perform a clean up on the user's articles first.";
                return RedirectToAction(nameof(Index));
            }

            TempData[TempDataSuccessMessageKey] = $"User {user.Email} was deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private async Task<User> GetUserById(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{id}'.");
            }

            return user;
        }

        private async Task<User> GetUserByEmail(string email)
        {
            var user = await this.userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with email '{email}'.");
            }

            return user;
        }
    }
}

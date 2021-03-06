﻿namespace DreamFactory.AddressBook.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using DreamFactory.AddressBook.Models;
    using DreamFactory.Api;

    [Authorize]
    [HandleError]
    public class ManageController : Controller
    {
        private readonly ISystemAdminApi adminApi;
        private readonly IUserApi userApi;

        public ManageController(ISystemAdminApi adminApi, IUserApi userApi)
        {
            this.adminApi = adminApi;
            this.userApi = userApi;
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ClaimsIdentity identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;

            Task<bool> result;

            if (claims.Any(x => x.Type == ClaimTypes.Role && x.Value == DreamFactoryContext.Roles.SysAdmin))
            {
                result = adminApi.ChangeAdminPasswordAsync(model.OldPassword, model.NewPassword);
            }
            else
            {
                result = userApi.ChangePasswordAsync(model.OldPassword, model.NewPassword);
            }

            if (await result)
            {
                ViewBag.StatusMessage = "Your password has been changed.";
            }
            else
            {
                ViewBag.StatusMessage = "Password was not changed. Please try again.";
            }

            return View(model);
        }
    }
}
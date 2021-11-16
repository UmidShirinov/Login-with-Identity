using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UDemyAuth.Models;
using UDemyAuth.ViewModels;

namespace UDemyAuth.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : BaseController
    {

        //private  UserManager<AppUser> _userManager { get; set; }

        public AdminController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager ,RoleManager<AppRole> roleManager) : base(userManager, signInManager, roleManager)
        {

        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Users()
        {
            return View(_userManager.Users.ToList());
        }


        public IActionResult Roles()
        {
            return View(_roleManager.Roles.ToList());
        }

        public IActionResult RoleCreate()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RoleCreate(RoleViewModel roleViewModel)
        {
            AppRole appRole = new AppRole();

            appRole.Name = roleViewModel.Name;

            var result = _roleManager.CreateAsync(appRole).Result;


            if (result.Succeeded)
            {
                return RedirectToAction("Roles");
            }

            else
            {
                AddModelError(result);
            }

            return View(roleViewModel);
        }

        [HttpPost]
        public IActionResult RoleDelete(string id)
        {
            var role = _roleManager.FindByIdAsync(id).Result;
            if (role != null)
            {
                var result = _roleManager.DeleteAsync(role).Result;


            }
            return RedirectToAction("Roles");
        }

        public IActionResult RoleUpdate(string id)
        {
            var role = _roleManager.FindByIdAsync(id).Result;
            if (role != null)
            {
                RoleViewModel roleViewModel = new RoleViewModel();

                roleViewModel.Id = role.Id;
                roleViewModel.Name = role.Name;

                return View(roleViewModel);
            }

            return View();
        }


        [HttpPost]
        public IActionResult RoleUpdate(RoleViewModel roleViewModel)
        {

            var role = _roleManager.FindByIdAsync(roleViewModel.Id).Result;

            if (role != null)
            {
                role.Name = roleViewModel.Name;
                var result = _roleManager.UpdateAsync(role).Result;

                if (result.Succeeded)
                {
                    return RedirectToAction("Roles");
                }
                else
                {
                    AddModelError(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "Deyisdirme Prosesi ugursuz oldu");
            }
            return View(roleViewModel);

        }


        public async Task<IActionResult> RoleAssign(string id)
         {
            TempData["userid"] = id;
            var user = await _userManager.FindByIdAsync(id);

            ViewBag.username = user.UserName;

            var roles = _roleManager.Roles;

            var userrole = await _userManager.GetRolesAsync(user);

            List<RoleAssignViewModel> roleAssignViewModel = new List<RoleAssignViewModel>();

            foreach (var role in roles)
            {
                RoleAssignViewModel r = new RoleAssignViewModel();

                r.RoleId = role.Id;
                r.RoleName = role.Name;

                if (userrole.Contains(role.Name))
                {
                    r.Exist = true;
                }
                else
                {
                    r.Exist = false;
                }
                roleAssignViewModel.Add(r);
            }



            return View(roleAssignViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> RoleAssign(List<RoleAssignViewModel> roleAssignViewModels)
        {

            var user =  await _userManager.FindByIdAsync(TempData["userid"].ToString());

            foreach (var item in roleAssignViewModels)
            {
                if (item.Exist)
                {
                    await _userManager.AddToRoleAsync(user, item.RoleName);
                    //await _userManager.UpdateSecurityStampAsync(user);
                    await _signInManager.RefreshSignInAsync(user);
                    //await _signInManager.PasswordSignInAsync(user)
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(user, item.RoleName);
                    //await _userManager.UpdateSecurityStampAsync(user);
                    await _signInManager.RefreshSignInAsync(user);


                }
            }



            return RedirectToAction("Users");
        }



        public IActionResult Claims()
        {
            return View(User.Claims.ToList());
        }

    }
}

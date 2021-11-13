using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UDemyAuth.Models;
using Mapster;
using UDemyAuth.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using UDemyAuth.Enums;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace UDemyAuth.Controllers
{
    [Authorize]
    public class MemberController : BaseController
    {
        public MemberController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) :
            base(userManager, signInManager)
        {

        }
        public IActionResult Index()
        {

            AppUser user = CurrentUser;   // her istifadeci ucun 
                                          // User.Identity yaranir


            UserViewModel userViewModel = user.Adapt<UserViewModel>();                // Mapster yukluyursen sora
                                                                                      // Adapt metodu userin icindekileri 
                                                                                      // UserViewModele uygunlasdirib elave elir.

            return View(userViewModel);
        }

        public IActionResult Home()
        {
            return View();
        }


        public IActionResult UserEdit()
        {

            var user = CurrentUser;

            UserViewModel userViewModel = user.Adapt<UserViewModel>();

            //userViewModel.Pictures = user.Picture;


            ViewBag.gender = new SelectList(Enum.GetNames(typeof(Gender)));


            return View(userViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> UserEdit(UserViewModel userViewModel, IFormFile userPicture)
        {
            ModelState.Remove("Password");

            ViewBag.gender = new SelectList(Enum.GetNames(typeof(Gender)));


            if (ModelState.IsValid)
            {

                var user = await _userManager.FindByNameAsync(User.Identity.Name);




                if (userPicture != null && userPicture.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(userPicture.FileName);

                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserPictures", fileName);  // gedib bele bir sekil var yoxdu onu yoxluyacaq


                    FileStream stream = new FileStream(path, FileMode.Create); //sora gedib ora elave edecek

                    await userPicture.CopyToAsync(stream);

                    user.Picture = "UserPictures/" + fileName;





                }

                user.UserName = userViewModel.UserName;
                user.Email = userViewModel.Email;
                user.PhoneNumber = userViewModel.PhoneNumber;
                user.City = userViewModel.City;
                user.BirthDay = userViewModel.BirthDay;
                user.Gender = (int)userViewModel.Gender;


                var result = await _userManager.UpdateAsync(user);


                if (result.Succeeded)
                {

                    await _userManager.UpdateSecurityStampAsync(user);
                    await _signInManager.SignOutAsync();

                    await _signInManager.SignInAsync(user, true);


                    ViewBag.success = "true";
                }
                else
                {
                    AddModelError(result);


                }
            }


            return View(userViewModel);

        }
        public IActionResult PasswordChange()
        {


            return View();
        }

        [HttpPost]
        public IActionResult PasswordChange(PasswordChangeViewModel passwordChangeViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = CurrentUser;

                var passwordExist = _userManager.CheckPasswordAsync(user, passwordChangeViewModel.PasswordOld).Result;


                if (passwordExist)
                {

                    var newpassword = _userManager.ChangePasswordAsync
                        (user, passwordChangeViewModel.PasswordOld, passwordChangeViewModel.PasswordNew).Result;


                    if (newpassword.Succeeded)
                    {
                        _userManager.UpdateSecurityStampAsync(user);


                        _signInManager.SignOutAsync();   // user logout edib sora yeniden giris edirrikki cookiesi yenilensin
                                                         // cunki cookie mudeti bitennen sora Identity api useri Login sehifesine atacaq
                        _signInManager.PasswordSignInAsync(user, passwordChangeViewModel.PasswordNew, true, false);
                        ViewBag.success = "True";
                    }
                    else
                    {

                        AddModelError(newpassword);



                    }

                }
                else
                {
                    ModelState.AddModelError("", "Kohne Parol sef yazilib");
                }


            }

            return View(passwordChangeViewModel);
        }


        public void LogOut()
        {
            _signInManager.SignOutAsync();
        }

        public IActionResult AccessDenied()
        {

            return View();
        }



        [Authorize(Roles ="editor,admin")]
        public IActionResult Editor()
        {
            return View();
        }

        [Authorize(Roles = "manager,admin")]
        public IActionResult Manager()
        {
            return View();
        }




    }
}

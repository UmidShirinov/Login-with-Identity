using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UDemyAuth.Helper;
using UDemyAuth.Models;
using UDemyAuth.ViewModels;

namespace UDemyAuth.Controllers
{
    public class HomeController : BaseController
    {





        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) :
            base(userManager, signInManager)
        {

        }
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Member");
            }


            return View();
        }


        public IActionResult SignUp()
        {
            return View();
        }

        public IActionResult LogIn(string ReturnUrl)
        {

            TempData["ReturnUrl"] = ReturnUrl;  //action alarasi kicik hecmli datalari dasimaq ucun isdifade olunur

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LoginViewModel loginViewModel)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(loginViewModel.Email);

                if (user != null)
                {

                    if (await _userManager.IsLockedOutAsync(user))
                    {
                        ModelState.AddModelError("", "Hesabiniz mueyyen vaxta qeder baglanilib");
                        return View(loginViewModel);


                    }

                    await _signInManager.SignOutAsync(); //cookieni silir ve cixis edir.

                    var signInResult = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, loginViewModel.RememberMe, false);   //FailLockOut 4-5 defe
                                                                                                                                                    // parolu sef girende ki
                                                                                                                                                    //kilitliyir
                    if (signInResult.Succeeded)
                    {

                        await _userManager.ResetAccessFailedCountAsync(user);   // sef gedislerin sayini sifirliyir

                        if (User.IsInRole("Admin")==true)
                        {
                            return RedirectToAction("Index", "Admin");
                        }
                        else
                        {

                            if (TempData["ReturnUrl"] != null)
                            {
                                return Redirect(TempData["ReturnUrl"].ToString());
                            }
                            return RedirectToAction("Index", "Member");
                        }

                    }
                    else
                    {


                        await _userManager.AccessFailedAsync(user); // her sef girende 1 vahid artirir


                        var Failed = await _userManager.GetAccessFailedCountAsync(user);  // ne qeder sef girilib onu alir
                        if (4 - Failed > 0)
                        {
                            ModelState.AddModelError("", $"{4 - Failed} dene giris şansiniz qaldi");

                        }

                        if (Failed == 4)
                        {
                            await _userManager.SetLockoutEndDateAsync(user, DateTime.Now.AddMinutes(10));  // blokluyur

                            ModelState.AddModelError("", "Hesaba 4 defe yanlis parol girdiyine gore 10 deqiqe muddetinde baglanmisdir , zehmet olmasa sonra yeniden cehd edin ");
                        }
                        else if (Failed < 4)
                        {
                            ModelState.AddModelError("", "Email yada Parol Yanlisdir");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(nameof(loginViewModel.Email), "Bele bir hesab yoxdur");
                }

            }
            return View(loginViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> SignUp(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {

                AppUser _appUser = new AppUser();

                _appUser.UserName = userViewModel.UserName;
                _appUser.Email = userViewModel.Email;
                _appUser.PhoneNumber = userViewModel.PhoneNumber;


                var result = await _userManager.CreateAsync(_appUser, userViewModel.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Home");
                }

                else
                {
                    AddModelError(result);
                }



            }
            return View(userViewModel);
        }



        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(PasswordResetViewModel passwordResetViewModel)
        {
            var user = await _userManager.FindByEmailAsync(passwordResetViewModel.Email);

            if (user != null)
            {
                string passwordResettoken = await _userManager.GeneratePasswordResetTokenAsync(user); // userin 

                var passwordResetLink = Url.Action("ResetPasswordConfirm", "Home", new
                {

                    userId = user.Id,
                    token = passwordResettoken,
                }, HttpContext.Request.Scheme);

                PasswordReset.sendToMail(passwordResetLink, user.Email);

                ViewBag.status = "success";
            }

            else
            {
                ModelState.AddModelError("", "Bele bir email adresi movcud deil");
            }
            return View(passwordResetViewModel);
        }

        public IActionResult ResetPasswordConfirm(string userid, string token)
        {

            TempData["userid"] = userid;
            TempData["token"] = token;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ResetPasswordConfirm([Bind("PasswordNew")] PasswordResetViewModel passwordResetViewModel) // Bind metodu Viewmodel 
                                                                                                                                   // icerisinde hanisini
                                                                                                                                   // isdiyirsen onu getirir

        {
            string token = TempData["token"].ToString();

            string userid = TempData["userid"].ToString();

            var user = await _userManager.FindByIdAsync(userid);


            if (userid != null)
            {
                var result = await _userManager.ResetPasswordAsync(user, token, passwordResetViewModel.PasswordNew);

                if (result.Succeeded) // eger userin vacib neyinse deyisirsense  SecurityStampinda deyismelisen.
                                      // cunki cookie security stampa gore yaranir
                {


                    await _userManager.UpdateSecurityStampAsync(user);

                    ViewBag.success = "success";


                }
                else
                {
                    AddModelError(result);
                }

            }

            else
            {
                ModelState.AddModelError("", "Bir xeta bas verdi , daha sora yeniden cehd edin");
            }

            return View(passwordResetViewModel);
        }

    }

}

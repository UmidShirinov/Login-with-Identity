using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UDemyAuth.Models;

namespace UDemyAuth.CustomValidation
{
    public class CustomPasswordValidator : IPasswordValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string password)
        {

            List<IdentityError> errors = new List<IdentityError>();


            if (password.ToLower().Contains(user.UserName.ToLower()))
            {

                if (!user.Email.Contains(user.UserName))
                {

                    errors.Add(new IdentityError { Code = "PasswordContainsUserName", Description = "Şifre Username-dən istifadə  oluna bilməz" });
                }


            }
            else if (password.ToLower().Contains(user.Email.ToLower().ToString()))
            {
                errors.Add(new IdentityError { Code = "PasswordContainsEmail", Description = "Şifre Email ile eyni ola bilməz" });

            }

            if (errors.Count == 0)
            {
                return Task.FromResult(IdentityResult.Success);
            }
            else
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }

        }
    }
}

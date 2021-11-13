using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UDemyAuth.Models;

namespace UDemyAuth.CustomValidation
{
    public class CustomUserValidator : IUserValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user)
        {

            List<IdentityError> errors = new List<IdentityError>();

            string[] st = new string[] { "1", "2", "3", "4", "5", "6", "6", "7", "8", "9" };


            foreach (var item in st)
            {
                if (user.UserName[0].ToString()==item)
                {
                    errors.Add(new IdentityError { Code = "FirstLetterCanNotBeDigit", Description = "Basliq rəqəmlə baslıya bilməz" });
                }
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

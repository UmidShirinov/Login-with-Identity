using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UDemyAuth.ViewModels
{
    public class PasswordResetViewModel
    {
        [Display(Name ="Email Address")]
        [Required(ErrorMessage ="Email girmelisiniz")]
        [EmailAddress]
        public string Email { get; set; }



        [Required(ErrorMessage = "Parol girməyiniz vacibdir")]
        [DataType(DataType.Password)]
        [Display(Name = "Yeni Parolunuz")]
        public string PasswordNew { get; set; }

        [DataType(DataType.Password)]
        [Compare("PasswordNew",ErrorMessage ="Yazdiginiz Parol yuxaridaki ile eyni deil!")]
        public string ConfirmPassword { get; set; }
    }
}

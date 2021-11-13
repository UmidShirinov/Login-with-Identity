using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UDemyAuth.ViewModels
{
    public class LoginViewModel
    {


        [Required(ErrorMessage ="Email girmək vacibdir")]
        [Display(Name ="Email unvaniniz")]
        [EmailAddress]
        public string Email { get; set; }


        [Required(ErrorMessage ="Parol girməyiniz vacibdir")]
        [DataType(DataType.Password)]
        [Display(Name ="Parolunuz")]
        public string Password { get; set; }


        public bool RememberMe { get; set; }







    }
}

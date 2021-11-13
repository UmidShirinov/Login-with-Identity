using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UDemyAuth.ViewModels
{
    public class PasswordChangeViewModel
    {

        [Required(ErrorMessage ="Kohne parolu girmelisiz")]
        [DataType(DataType.Password)]
        [Display(Name ="Kohne Parol")]
        public string PasswordOld { get; set; }

        [Required(ErrorMessage = "Yeni parolu girmelisiz")]
        [DataType(DataType.Password)]
        [Display(Name = "Yeni Parol")]
        public string PasswordNew { get; set; }


        [Required(ErrorMessage = "Yeni parolu yeniden yazmalisan")]
        [DataType(DataType.Password)]
        [Display(Name = "Tekrar Yeni Parol")]
        [Compare("PasswordNew" , ErrorMessage ="Yazdgin parol yeni paroldan ferqlidir")]
        public string PasswordConfirm { get; set; }
    }
}

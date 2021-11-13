using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UDemyAuth.Enums;

namespace UDemyAuth.ViewModels
{
    public class UserViewModel
    {

        [Required(ErrorMessage = "Username qeyd edilmelidi")]
        [Display(Name ="Username")]
        public string UserName { get; set; }

        [Display(Name = "Telefon Nomresi")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email qeyd edilmelidi")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage ="Email adressi duzgun qeyd edilmiyib")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Parol qeyd edilmelidi")]
        [Display(Name = "Parol")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [DataType(DataType.Date)]
        [Display(Name ="Tarix")]
        public DateTime? BirthDay { get; set; }

        [Display(Name = "Sekil")]

        public string Picture { get; set; }


        [Display(Name = "Cins")]

        public Gender  Gender { get; set; }


        [Display(Name = "Seher")]

        public string City { get; set; }

    }
}

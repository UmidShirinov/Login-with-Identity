using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UDemyAuth.ViewModels
{
    public class RoleViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage ="Role adi girilmelidi")]
        [Display(Name="Role adi")]
        public string Name { get; set; }
    }
}

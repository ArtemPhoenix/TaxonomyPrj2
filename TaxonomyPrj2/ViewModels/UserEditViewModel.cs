using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaxonomyPrj2.ViewModels
{
    public class UserEditViewModel
    {
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [Display(Name = "EMail")]
        public string Role { get; set; }

        [Display(Name = "EMail")]
        [Required(ErrorMessage = "Не указана почта")]
        public string EMail { get; set; }

        [Display(Name = "Год начала исследований")]
        [Required(ErrorMessage = "Не указан год начала исследований")]
        public int Year { get; set; }

        public List<IdentityRole> AllRoles { get; set; }
    }
}

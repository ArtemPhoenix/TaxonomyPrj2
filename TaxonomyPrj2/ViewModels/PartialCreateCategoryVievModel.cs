using DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TaxonomyPrj2.Models;

namespace TaxonomyPrj2.ViewModels
{
    public class PartialCreateCategoryVievModel
    {
        public List<Category> Categories { get; set; }

        public int Id { get; set; }
        public int Parent { get; set; }

        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Не указано имя")]
        public string Name { get; set; }

        [Display(Name = "Приставочное имя")]
        [Required(ErrorMessage = "Не указано приставочное имя")]
        public string NameCat { get; set; }

        [Display(Name = "Описание")]
        [Required(ErrorMessage = "Не указано описание")]
        public string Description { get; set; }
       
    }
}

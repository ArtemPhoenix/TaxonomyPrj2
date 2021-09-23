using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaxonomyPrj2.ViewModels
{
    public class PartialDescriptionViewModel
    {
        public int Id { get; set; }
        public int Parent { get; set; }
        [Required(ErrorMessage = "Не указано имя2")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Не указано приставочное имя")]
        public string NameCat { get; set; }
        [Required(ErrorMessage = "Не указано описание")]
        public string Description { get; set; }
    }
}

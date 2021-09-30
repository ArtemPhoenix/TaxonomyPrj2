using DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TaxonomyPrj2.Models;

namespace TaxonomyPrj2.ViewModels
{
    public class PartialRedactOrganismViewModel
    {
        public List<Category> Categories { get; set; }   
        public int Id { get; set; }

        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Не указано имя")]
        public string Name { get; set; }

        [Display(Name = "Дата начала исследований")]
        [Required(ErrorMessage = "Не указана дата начала исследований")]
        public DateTime StartResearch { get; set; }

        [Display(Name = "Количество образцов")]
        [Required(ErrorMessage = "Не указано количество образцов")]
        public int CountSample { get; set; }

        [Display(Name = "Описание")]
        [Required(ErrorMessage = "Не указано описание")]
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int FlagEvent { get; set; }
       
    }
}

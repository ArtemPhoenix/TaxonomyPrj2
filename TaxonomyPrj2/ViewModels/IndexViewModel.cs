using DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TaxonomyPrj2.Models;

namespace TaxonomyPrj2.ViewModels
{
    public class IndexViewModel
    {
        
        public List<Category> Categories { get; set; }  
        public List<Organism> Organisms { get; set; }

        public int CurrenCategoryId { get; set; }
       

        [Display(Name = "Имя искомого организма")]
        public string SearchNameOrganism { get; set; }
        [Display(Name = "Нижняя планка количества образцов искомого организма")]
        public int? SearchCountFromtOrganism { get; set; }
        [Display(Name = "Верхняя планка количества образцов искомого организма")]
        public int? SearchCountTotOrganism { get; set; }
        [Display(Name = "Нижняя планка начала исследования искомого организма")]
        public DateTime SearchDateFromOrganism { get; set; }
        [Display(Name = "Верхняя планка начала исследования искомого организма")]
        public DateTime SearchDateToOrganism { get; set; }
        
      

       
    }
}

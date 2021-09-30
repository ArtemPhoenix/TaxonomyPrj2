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
        //public string Role { get; set; }
        //public bool Admin { get; set; }
        //public bool CommonUser { get; set; }
        public List<Category> Categories { get; set; }   // prop + tab
        public List<Organism> Organisms { get; set; }

        public int CurrenCategoryId { get; set; }
        public int CurrenOrganismId { get; set; }
        public Organism OneOrganism { get; set; }

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
        
        public int FlagEvent { get; set; }
        // 0 =  Организм выбран из списка.
        // 1 =  Организм был изменен.
        // 2 =  Организм был удален.
        // 3 =  Организм был добавлен.
       /* public int returnIndexCategoryes(List<Category> categoryes, int id)  // убрать потом в репозит
        {
            int index = -1;
            for (int i = 0; i < categoryes.Count; i++)
            {
                if (categoryes[i].Id == id)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }*/

       
    }
}

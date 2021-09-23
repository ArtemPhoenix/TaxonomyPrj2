using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxonomyPrj2.Models;

namespace TaxonomyPrj2.ViewModels
{
    public class RedactOrganismViewModel
    {
        public List<Category> Categories { get; set; }   // prop + tab
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartResearch { get; set; }
        public int CountSample { get; set; }
        public int CategoryId { get; set; }
        public int FlagEvent { get; set; }
        // 0 =  Организм выбран из списка.
        // 1 =  Организм был изменен.
        // 2 =  Организм был удален.
        // 3 =  Организм был добавлен.

        //public List<Organism> Organisms { get; set; }
        /*
        public int CurrenCategoryId { get; set; }
        public int CurrenOrganismId { get; set; }

        /**/
        //public Organism OneOrganism { get; set; }
        /*
        public int flagEvent { get; set; }
        // 0 =  Организм выбран из списка.
        // 1 =  Организм был изменен.
        // 2 =  Организм был удален.
        // 3 =  Организм был добавлен.*/
    }
}

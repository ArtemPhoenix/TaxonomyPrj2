using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxonomyPrj2.Models;

namespace TaxonomyPrj2.ViewModels
{
    public class OrganismTableViewModel
    {
        public string Role { get; set; }
        public bool Admin { get; set; }
        public bool CommonUser { get; set; }
        public List<Organism> Organisms { get; set; }
        public int CurrenCategoryId { get; set; }

    }
}

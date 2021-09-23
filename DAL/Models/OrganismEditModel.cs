using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxonomyPrj2.Models
{
    public class OrganismEditModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartResearch { get; set; }
        public int CountSample { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }

       public OrganismEditModel(Organism organism)
        {
            Id = organism.Id;
            Name = organism.Name;
            Description = organism.Description;
            CategoryId = organism.CategoryId;
        }

        public OrganismEditModel()
        {

        }
    }

}

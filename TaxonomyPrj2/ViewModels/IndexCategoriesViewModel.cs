using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxonomyPrj2.Helpers;
using TaxonomyPrj2.Models;

namespace TaxonomyPrj2.ViewModels
{
    public class IndexCategoriesViewModel
    {
        // public List<CategoryTree> CategoryTree { get; set; }
        public List<Category> CategoryTree { get; set; }
        public CategoryTree TopTree { get; set; }
        
        public string InView { get; set; }



    }
}

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
        public List<CategoryTree> CategoryTree { get; set; }
        public string Indent { get; set; }
        
    }
}

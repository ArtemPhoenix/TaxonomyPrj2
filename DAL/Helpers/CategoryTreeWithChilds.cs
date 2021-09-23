using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxonomyPrj2.Models;

namespace TaxonomyPrj2.Helpers
{
    public class CategoryTreeWithChilds
    {
        public List<Category> childs { get; set; }

        public Category data { get; set; }
    }

   
}

using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxonomyPrj2.Models;

namespace TaxonomyPrj2.Helpers
{
    public class CategoryTree : Category
    {
        public int level { get; set; } // уровень узла дерева
        public string indent { get; set; } // отступ 
    }
}

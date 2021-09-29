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
        public List<CategoryTree> CategoryTree { get; set; }
        public List<Category> List { get; set; }
        public string Indent { get; set; }
        //public int Test { get; set; }
        public string InView { get; set; }
        public void MakeTreeInViev(Category top)
        {



            if (top.InverseParentNavigation.Count != 0)
            {
                InView += "<li> <span class=\"drop\" onclick=\"clickUL(this)\">-</span>";
                InView += top.NameCat + top.Name;

                InView += "<ul>";

                foreach (var item in top.InverseParentNavigation)
                {
                    MakeTreeInViev(item);
                }
                InView += "</ul>";

            }
            else
            {
                InView += "<li>";
                InView += top.NameCat + top.Name;
            }
            InView += "</li>";

        }

    }
}

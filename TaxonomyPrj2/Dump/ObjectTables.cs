using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxonomyPrj2.Models
{
    public class ObjectTables
    {
        public List<Category> Categoryes;
        public List<Organism> Organisms;
        private int CurrenCategory { get; set; }
        public int GetCurrenCategory()
        {
            return CurrenCategory;
        }
        public void SetCurrenCategory(int newCurren)
        {
            CurrenCategory = newCurren;
        }


        public int returnIndexCategoryes(List<Category> categoryes, int id)
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
        }

        public string returnParentName(List<Category> categoryes, int id)
        {
            string name = "<предка нет>";
            int idParent = categoryes[returnIndexCategoryes(categoryes, id)].Parent; //id предка
            int indexParent = returnIndexCategoryes(categoryes, idParent); // index предка


            if (indexParent >= 0) // если id=-1, то предка нет
            {
                name = categoryes[indexParent].Name;
            }
            return name;
        }
    }
}

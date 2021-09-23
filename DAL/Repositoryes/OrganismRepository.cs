using DAL.Data;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TaxonomyPrj2.Models;

namespace TaxonomyPrj2.interfaces
{
    public class OrganismRepository : IDisposable
    {
        private taxonomydbContext db;

        public OrganismRepository()
        {
            this.db = new taxonomydbContext();
        }

               

        public void Create(OrganismEditModel editOrganism)
        {
            Organism newOrganism = new Organism();
            //newOrganism.Id = editOrganism.Id;
            newOrganism.CategoryId = editOrganism.CategoryId;
            newOrganism.Name = editOrganism.Name;
            newOrganism.StartResearch = editOrganism.StartResearch;
            newOrganism.CountSample = editOrganism.CountSample;
            newOrganism.Description = editOrganism.Description;
            db.Organisms.Add(newOrganism);

            db.SaveChanges();
        }

        public void Update(OrganismEditModel editOrganism)
        {
            var organism = db.Organisms.Find(editOrganism.Id);

            organism.CategoryId = editOrganism.CategoryId;
            organism.Name = editOrganism.Name;
            organism.StartResearch = editOrganism.StartResearch;
            organism.CountSample = editOrganism.CountSample;
            organism.Description = editOrganism.Description;
            

            db.SaveChanges();
        }

        public void Delete(int id)
        {
            Organism organism = db.Organisms.Find(id);
            if (organism != null)
            {
                db.Organisms.Remove(organism);
                db.SaveChanges();
            }
        }

       

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public List<Organism> GetList()
        {
            return db.Organisms.ToList();
        }

        public Organism Get(int id)
        {
            return db.Organisms.Find(id);
        }

        /// <summary>
        /// фильтр отсеивания организма, если параметр  null, тогда игнор его
        /// </summary>
        /// <param name="searchName"></param>
        /// <param name="searchCountFrom"></param>
        /// <param name="searchCountTo"></param>
        /// <param name="searchDateFrom"></param>
        /// <param name="searchDateTo"></param>
        /// <param name="SearchIdCategory"></param>
        /// <returns></returns>
        public List<Organism> SearchOrganismsByFilter(string searchName, int? searchCountFrom, int? searchCountTo, DateTime? searchDateFrom, DateTime? searchDateTo, int? SearchIdCategory)
        {
           var query = db.Organisms.AsQueryable(); 
            if (!string.IsNullOrWhiteSpace(searchName))
            {
                query = query.Where(x => x.Name.Contains(searchName));
            }
            if (searchCountFrom != null)
            {
                query = query.Where(x => x.CountSample > (searchCountFrom.Value));
            }
            if (searchCountTo != null)
            {
                query = query.Where(x => x.CountSample < (searchCountTo.Value));
            }
            if (searchDateFrom != null)
            {
                query = query.Where(x => x.StartResearch > (searchDateFrom.Value));
            }
            if (searchDateTo != null)
            {
                query = query.Where(x => x.StartResearch < (searchDateTo.Value));
            }
            if (SearchIdCategory != null)
            {
                query = query.Where(x => x.CategoryId == (SearchIdCategory.Value));
            }
            var resSearch = query.ToList(); // для памяти дублируется

           int flagWork = 0;
            //bool FlagSearchTrue = true;
           var SearchList = GetList();
           if (searchName!=null) { SearchByName(searchName,ref SearchList, ref flagWork); }
           if ((searchCountFrom != null) && (flagWork !=0)) { SearchByCountFrom(searchCountFrom.Value, ref SearchList, ref flagWork); }
           if ((searchCountTo != null) && (flagWork != 0)) { SearchByCountTo(searchCountTo.Value, ref SearchList, ref flagWork); }
           if ((searchDateFrom != null) && (flagWork != 0)) { SearchByDateFrom(searchDateFrom.Value, ref SearchList, ref flagWork); }
           if ((searchDateTo != null) && (flagWork != 0)) { SearchByDateTo(searchDateTo.Value, ref SearchList, ref flagWork); }
           if ((SearchIdCategory != null) && (flagWork != 0)) { SearchByIdCategory(SearchIdCategory.Value, ref SearchList, ref flagWork); }

            if (flagWork == 0) { SearchList = new List<Organism>(); }

           return SearchList.ToList();
        }

        /// <summary>
        /// фильтруем searchList, изначально там все организмы
        /// </summary>
        /// <param name="searchIdCategiry"></param>
        /// <param name="searchList"></param>
        /// <param name="flagWork"></param>
        private void SearchByIdCategory(int searchIdCategiry, ref List<Organism> searchList, ref int flagWork)
        {
            List<Organism> result = new List<Organism>();

            foreach (var item in searchList) // перебор элементов
            {
                if (item.CategoryId == searchIdCategiry) // если элемент проходит ограничение - добавляем в лист прошедших фильтр
                {
                    result.Add(item);
                }
            }
            if (result.Count != 0) // если хоть что-нибудь прошло фильтр.
            {
                searchList = result; // обновляем лист для фильтрования
                flagWork++; // делаем флаг не ноль, если запуск первый, а так же подсчитываем пройденные фильтры
            }
            else
            {
                flagWork = 0; //фильтр не пройден - флаг в ноль
            }
        }
        private void SearchByDateTo(DateTime searchDateTo, ref List<Organism> searchList, ref int flagWork)
        {
            List<Organism> result = new List<Organism>();

            foreach (var item in searchList)
            {
                if (item.StartResearch <= searchDateTo)
                {
                    result.Add(item);
                }
            }
            if (result.Count != 0) 
            {
                searchList = result;
                flagWork++;
            }
            else
            {
                flagWork = 0;
            }
        }
        private void SearchByDateFrom(DateTime searchDateFrom, ref List<Organism> searchList, ref int flagWork)
        {
            List<Organism> result = new List<Organism>();

            foreach (var item in searchList)
            {
                if (item.StartResearch >= searchDateFrom) 
                {
                    result.Add(item);
                }
            }
            if (result.Count != 0) 
            {
                searchList = result;
                flagWork++;
            }
            else
            {
                flagWork = 0;
            }
        }
        private void SearchByCountTo(int searchCountTo, ref List<Organism> searchList, ref int flagWork)
        {
            List<Organism> result = new List<Organism>();

            foreach (var item in searchList)
            {
                if (item.CountSample <= searchCountTo)
                {
                    result.Add(item);
                }
            }
            if (result.Count != 0) 
            {
                searchList = result;
                flagWork++;
            }
            else
            {
                flagWork = 0;
            }
        }
        private void SearchByCountFrom(int searchCountFrom, ref List<Organism> searchList, ref int flagWork)
        {
            List<Organism> result = new List<Organism>();

            foreach (var item in searchList)
            {
                if (item.CountSample >= searchCountFrom) 
                {
                    result.Add(item);
                }
            }
            if (result.Count != 0)
            {
                searchList = result;
                flagWork++;
            }
            else
            {
                flagWork = 0;
            }
        }
        private void SearchByName(string searchName,ref List<Organism> searchList, ref int flagWork)
        {
            List<Organism> result = new List<Organism>();
            foreach (var item in searchList)
            {
                if (item.Name == searchName) 
                {
                    result.Add(item);
                }
            }
            if (result.Count != 0) 
            {
                searchList = result;
                flagWork++;
            }
            else
            {
                flagWork = 0;
            }
        }

        /// <summary>
        /// взять все организмы определенной категории
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public List<Organism> GetListByCategoryId(int categoryId/*, List<Category> categories*/) // по 1-й категории
        {

            return /*makeResult(categoryId, categories);*/db.Organisms.Where(x => x.CategoryId == categoryId).ToList();
            
        }
        public List<Organism> GetListByCategoryId(int categoryId, List<Category> categories) //по веткам, не используется уже
        {

            return MakeResult(categoryId, categories);

        }

        private List<Organism> MakeResult(int categoryId, List<Category> categories) // не используется уже
        {
            List<int> currenViews = new List<int>();
            CurrenListViews(categoryId, categories, ref currenViews);
            List<Organism> result = new List<Organism>();
            foreach (var item in currenViews)
            {
                foreach (var itemOrganism in db.Organisms)
                {
                    if (itemOrganism.CategoryId == item) { result.Add(itemOrganism); }
                }
            }
            return result;
        }
        private void  CurrenListViews(int categoryId, List<Category> categories, ref List<int> currenViews) // не используется уже
        {
            foreach (var category in categories)
            {
                if (category.Parent == categoryId) { CurrenListViews(category.Id, categories,ref currenViews); }
            }
            currenViews.Add(categoryId);
        }

        /*
        private int returnIndexOrganism(List<Organism> organisms, int id)
        {
            int index = 0;
            for (int i = 0; i < organisms.Count; i++)
            {
                if (organisms[i].Id == id)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }
        */
        
        /* static void GetListID(int currenCat, List<Category> categories, ref List<int> views)
         {
             foreach (var category in categories)
             {
                 if (category.parentId == currenCat) { GetListID(category.id, categories, ref views); }

             }
             views.Add(currenCat);

         }*/

    }
}

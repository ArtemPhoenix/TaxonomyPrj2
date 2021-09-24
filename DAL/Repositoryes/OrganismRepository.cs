using DAL.Data;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TaxonomyPrj2.Models;

namespace TaxonomyPrj2.interfaces
{
    public class OrganismRepository : IDisposable, IRepository<Organism>
    {
        private taxonomydbContext db;

        public OrganismRepository()
        {
            this.db = new taxonomydbContext();
        }

               

        public void Create(OrganismEditModel editOrganism)
        {
            Organism newOrganism = new Organism
            {
                //newOrganism.Id = editOrganism.Id;
                CategoryId = editOrganism.CategoryId,
                Name = editOrganism.Name,
                StartResearch = editOrganism.StartResearch,
                CountSample = editOrganism.CountSample,
                Description = editOrganism.Description
            };
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
            return db.Organisms.Include(x => x.Category).ToList();
        }

        public Organism Get(int id)
        {
            return db.Organisms.Include(x => x.Category).FirstOrDefault(x => x.Id == id);
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
          
           return resSearch;
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

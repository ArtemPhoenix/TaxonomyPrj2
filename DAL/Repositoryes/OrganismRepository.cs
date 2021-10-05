using DAL.Data;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
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


        public List<OrganismAndCategory> getAllInformation()
        {
            // var tt = resulta.Select(x => new _Example() { CategoryId=x.CategoryId, Id=x.Id, Name=x.Name}).ToList();  // памятка
            return db.OAC.FromSqlRaw("Information").ToList();

        }
        public List<OrganismAndCategory> getFiltrInformation(int? CountFrom, int? CountTo, DateTime? DateFrom, DateTime? DateTo)
        {
            //Microsoft.Data.SqlClient.SqlParameter param1 = new Microsoft.Data.SqlClient.SqlParameter("CountFrom", CountFrom ?? 0);

            //SqlParameter[] parameters = new SqlParameter[5];
            SqlParameter param1 = new SqlParameter("@CountFrom", SqlDbType.Int);
            param1.Value = (object)CountFrom ?? DBNull.Value;
            //  parameters[0] = param1;

            //Microsoft.Data.SqlClient.SqlParameter param2 = new Microsoft.Data.SqlClient.SqlParameter("@CountTo", CountTo ?? 0);
            SqlParameter param2 = new SqlParameter("@CountTo", SqlDbType.Int);
            param2.Value = (object)CountTo ?? DBNull.Value;

            Microsoft.Data.SqlClient.SqlParameter param5 = new Microsoft.Data.SqlClient.SqlParameter("@DateFlag", 99);
            if ((DateFrom == null) || (DateFrom.Value.Year < 1753))
            {
                if ((DateTo == null) || (DateTo.Value.Year < 1753)) { param5 = new Microsoft.Data.SqlClient.SqlParameter("@DateFlag", 12); }
                else { param5 = new Microsoft.Data.SqlClient.SqlParameter("@DateFlag", 10); }
            }
            else
            {
                if ((DateTo == null) || (DateTo.Value.Year < 1753)) { param5 = new Microsoft.Data.SqlClient.SqlParameter("@DateFlag", 02); }
                else { param5 = new Microsoft.Data.SqlClient.SqlParameter("@DateFlag", 99); }
            }

            

            //Microsoft.Data.SqlClient.SqlParameter param3 = new Microsoft.Data.SqlClient.SqlParameter("@DateFrom", ((DateFrom==null) || (DateFrom.Value.Year < 1753) ? new DateTime(1753, 1, 1):DateFrom));
              
            SqlParameter param3 = new SqlParameter("@DateFrom", SqlDbType.DateTime);
            param3.Value = (object)DateFrom ?? DBNull.Value;
            if (DateFrom != null && DateFrom.Value.Year < 1753)
            {
                param3.Value = (object)DBNull.Value;
            }
            //Microsoft.Data.SqlClient.SqlParameter param4 = new Microsoft.Data.SqlClient.SqlParameter("@DateTo", ((DateTo == null) || (DateTo.Value.Year < 1753) ? new DateTime(1753, 1, 1) : DateTo));
            SqlParameter param4 = new SqlParameter("@DateTo", SqlDbType.DateTime);
            param4.Value = (object)DateTo ?? DBNull.Value;
           
           if (DateTo != null && DateTo.Value.Year < 1753) 
            {
                param4.Value = (object)DBNull.Value;
            }

            /*parameters[1] = param2;
            parameters[2] = param3;
            parameters[3] = param4;
            parameters[4] = param3;*/


            var result = db.OAC.FromSqlRaw("InformationFiltr @CountFrom ,@CountTo, @DateFrom, @DateTo, @DateFlag", param1 ,param2, param3, param4, param5).ToList();
         

            return result;

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

        public string Delete(int id)
        {
            string result = "error:";

            Organism organism = db.Organisms.FirstOrDefault(x => x.Id == id);//Find(id);
            if (organism != null)
            {
                try
                {
                    db.Organisms.Remove(organism);
                }
                catch (Exception)
                {

                    result += " удаление из БД не поизошло";
                }
                try
                {
                    db.SaveChanges();
                }
                catch (Exception)
                {

                    result += " сохранить измениния в БД не удалось";
                }
                if (result == "error:") { result = "успешно"; }
            }
            else
            {
                result = "error: категория не найдена";
            }

            return result;
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
        public List<Organism> SearchOrganismsByFilter(string Name, int? CountFrom, int? CountTo, DateTime? DateFrom, DateTime? DateTo, int? IdCategory)
        {
            var query = db.Organisms.AsQueryable(); 
            if (!string.IsNullOrWhiteSpace(Name))
            {
                query = query.Where(x => x.Name.Contains(Name));
            }
            if (CountFrom != null)
            {
                query = query.Where(x => x.CountSample > (CountFrom.Value));
            }
            if (CountTo != null)
            {
                query = query.Where(x => x.CountSample < (CountTo.Value));
            }
            if (DateFrom != null)
            {
                query = query.Where(x => x.StartResearch > (DateFrom.Value));
            }
            if (DateTo != null)
            {
                query = query.Where(x => x.StartResearch < (DateTo.Value));
            }
            if (IdCategory != null)
            {
                query = query.Where(x => x.CategoryId == (IdCategory.Value));
            }
            var resSearch = query.ToList(); // для памяти дублируется
          
           return resSearch;
        }

       
       

        /// <summary>
        /// взять все организмы определенной категории
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public List<Organism> GetListByCategoryId(int categoryId) 
        {

            return db.Organisms.Where(x => x.CategoryId == categoryId).ToList();
            
        }
      


    }
}

using DAL.Data;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TaxonomyPrj2.Helpers;
using TaxonomyPrj2.Models;

namespace TaxonomyPrj2.interfaces
{
    public class CategoryRepository : IDisposable, IRepository<Category>
    {
        private taxonomydbContext db;

        public CategoryRepository()
        {
            this.db = new taxonomydbContext();
        }

               
        public void Create(CategoryEditModel editCategory) // +
        {
            var newCategory = new Category
            {
                
                Name = editCategory.Name,
                NameCat = editCategory.NameCat,
                Parent = editCategory.Parent,
                Description = editCategory.Description
            };
            db.Categories.Add(newCategory);
            db.SaveChanges();
        }

        public void Update(CategoryEditModel editCategory) // +
        {
                               
            var category = db.Categories.Find(editCategory.Id);
            category.Name = editCategory.Name;
            category.NameCat = editCategory.NameCat;
            category.Parent = editCategory.Parent;
            category.Description = editCategory.Description;

            
            db.SaveChanges();
           
        }

        public string Delete(int id) // +
        {
            string result = "error:";


            Category category = db.Categories.Include(x => x.InverseParentNavigation)
                .Include(x => x.Organisms).FirstOrDefault(x => x.Id == id);

            if (category != null)
            {
                if (category.InverseParentNavigation.Count != 0 || category.Organisms.Count != 0)
                {
                   
                    if (category.InverseParentNavigation.Count != 0) 
                    {
                        result += " существуют подкатегории";
                    }
                    if (category.InverseParentNavigation.Count != 0)
                    {
                        result += " существуют организмы";
                    }
                }
                else
                {
                    try
                    {
                        db.Categories.Remove(category);
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

            }
            else
            {
                result = "error: категория не найдена";
            }


            return result;
        }

        public Category Get(int id)//****
        {
            return db.Categories.Include( x => x.ParentNavigation).Include(x => x.InverseParentNavigation).FirstOrDefault(x=> x.Id==id);
        }

        public List<Category> GetList()
        {
            return db.Categories.Include(x => x.InverseParentNavigation).ToList();
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

        
        /// <summary>
        /// вернуть лист цепочки родительских категорий в порядке начинай со старшей, зная Id самой младшей
        /// </summary>
        /// <param name="id">Id рассматриваемой категории</param>
        /// <returns></returns>
        public List<Category> GetListParent(int id)
        {
            // создаем лист родительских поднимаясь вверх
            var listParent = new List<Category>(); 
            var list = GetList(); // взять лист категорий
            var newCategory = list.Find(x => x.Id == id);  // взять первый родительский
            while (newCategory.Parent != null) // если ноль, значит вершина достигнута
            {
                listParent.Add(newCategory); // добавить родителя
               // newCategory = list.Find(x => x.Id == newCategory.Parent); // найти следующего
                newCategory = newCategory.ParentNavigation; 
            }
            listParent.Add(newCategory);  // добавить вершину
            listParent.Reverse(); // сделать обратный порядок родителей

            return listParent;
        }
        /// <summary>
        /// получить лист категорий не являщимися дочерними для категории Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Category> GetAnotherBranches(int id) 
        {
            var list = db.Categories.ToList();
            var top = list.Find(x => x.Id == id);
            var childsList = new List<Category>();

            MakeListChildsBranches(top,ref childsList);

            var result = list.Except(childsList); // убрать дочерние из листа

            return list.Except(childsList).ToList();
        }

        private void MakeListChildsBranches(Category top,ref List<Category> childsList) // получить лист всех дочерних на всех уровнях
        {
          
            foreach (var item in top.InverseParentNavigation)
            {
                childsList.Add(item);
                MakeListChildsBranches(item, ref childsList);
            }
        }
        

       
       
    }
}

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
                //newCategory.Id = editCategory.Id;
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

        public Category Get(int id)//****************************************************************
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
        

       
        /// <summary>
        /// Делает лист категорий для вывода в виде дерева добавляется уровень узла и отступ
        /// </summary>
        /// <returns></returns>
        public List<CategoryTree> GetListTree()  
        {
            // потом переделать нормально ?
            // var tree = new List<CategoryTreeWithChilds>(); // лист, где каждый элемент хранит всех своих потомков
            var list = db.Categories.Include(x=>x.InverseParentNavigation).ToList(); // просто лист всех категорий
            var top = list.Find(x => x.Id == 1); // вершина дерева

            // makeCategoryTreeWC(ref tree,top,list); // наполнение элементами листа, где каждый элемент хранит всех своих потомков
            
            // var topTreeWC = tree.Find(x => x.data.Id == 1); // вершина дерева, где каждый элемент хранит всех своих потомков
            var topTree = list.Find(x => x.Id == 1);
            var treeCategory = new List<CategoryTree>(); // пустой лист категорий с отступом и уровнем
            int level = 0; // начальный уровень
            treeCategory.Add(new CategoryTree // записываем вершину листа дерева с наследниками, но без них
            {
                Id = topTree.Id,
                Name = topTree.Name,
                NameCat = topTree.NameCat,
                Parent = topTree.Parent,
                Description = topTree.Description,
                level = level,
                InverseParentNavigation = topTree.InverseParentNavigation
            }) ;

            SortTree(ref treeCategory, topTree, level); // записываем в treeCategory категории порядке их отображения на странице

            foreach (var item in treeCategory) // формирование отступа
            {
                for (int i = 0; i < item.level; i++)
                {
                    item.indent = item.indent + "....";
                }
            }

          //  string alt="";
            
           // MakeTreeInViev(topTree, ref alt);
            
           // treeCategory.FirstOrDefault().indent = "<ul> " + alt + "</ul>" + "<h1 class=\"redText\">Стиль подключен</h1>";
            //treeCategory.FirstOrDefault().indent = "<ul class=\"treeline\">";
            
            return treeCategory;
        }

        /// <summary>
        /// записываем в treeCategory категории порядке их отображения на странице
        /// </summary>
        /// <param name="treeCategory">формируемый лист, передается по ссылке</param>
        /// <param name="tree">дерево, где каждый элемент хранит всех своих потомков</param>
        /// <param name="top">вершина, где каждый элемент хранит всех своих потомков</param>
        /// <param name="level">уровень вершины</param>
        private void SortTree(ref List<CategoryTree> treeCategory, Category top, int level)
        {
            // спуск вниз по веткам с присвоением уровня
            level++; // добавляет уровень
            if (top.InverseParentNavigation != null) // у вершины должны быть потомки
            {
                foreach (var item in top.InverseParentNavigation) // берем каждого потомка вершины
                {
                    
                        treeCategory.Add(new CategoryTree   /*записываем потомка в итоговый лист*/
                        {
                            Id = item.Id,
                            Name = item.Name,
                            NameCat = item.NameCat,
                            Parent = item.Parent,
                            Description = item.Description,
                            level = level
                        });
                        
                        SortTree(ref treeCategory, item, level);
                    // рекурсия. переходим на потомков.next этого потомка, что бы они следовали в итоговом листе за текущим потомком 
                    // уровень там станет уже на 1 больше, так же передается как вершина текущий потомок



                }
            }
          
        }

        /*
        public void MakeTreeInViev(Category top, ref string inView)
        {
            
           
            
            if (top.InverseParentNavigation.Count != 0)
            {
                inView += "<li> <span class=\"drop\" onclick=\"clickUL(this)\">-</span>";
                inView += top.NameCat + top.Name;

                inView += "<ul>";

                foreach (var item in top.InverseParentNavigation)
                {
                    MakeTreeInViev(item, ref inView);
                }
                inView += "</ul>";

            }
            else
            {
                inView += "<li>";
                inView += top.NameCat + top.Name;
            }
            inView += "</li>";
           
        }
        */
        
        /// <summary>
        /// формируем лист категорий, где у каждого элемента вдобавок хранится его потомки
        /// </summary>
        /// <param name="tree">формируемый лист</param>
        /// <param name="top">вершина</param>
        /// <param name="list">лист всех категорий</param>
        /// 
        /*
        private void makeCategoryTreeWC(ref List<CategoryTreeWithChilds> tree, Category top, List <Category> list)
        {
            // создание листа типа элемент + лист потомков
            CategoryTreeWithChilds newElem = new CategoryTreeWithChilds(); // новый элемент листа категорий с потомками
            newElem.data = top; // .data -это сам элемент, новый элемент ему присваивается вершина 
            foreach (var item in list) // перебор всех категорий
            {
                if (item.Parent==top.Id) // ищем потомков вершины
                {
                    if (newElem.childs == null) 
                    { 
                        newElem.childs = new List<Category>(); 
                    }
                    newElem.childs.Add(item); // добавляем найденного потомка
                   
                    makeCategoryTreeWC(ref tree, item,list); // рекурсия для найденного потомка повторяем всю операцию поиска
                   // узнать как лучше передавать list, с ref или без
                   
                }
                                    
            }

            tree.Add(newElem); // в newElem все потомки добавлены, можно добавлять в формируемый лист
           

        }
        */
    }
}

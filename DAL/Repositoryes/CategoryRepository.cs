using DAL.Data;
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
    public class CategoryRepository : IDisposable
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

        public bool Delete(int id) // +
        {
            bool result = true;
           
            Category category = db.Categories.Find(id);

            if (category != null)
            {
                foreach (var item in GetList())
                {
                    if (category.Id == item.Parent) 
                    {
                        result = false;
                        break;
                    }
                }

                if (result)
                {
                    db.Categories.Remove(category);
                    db.SaveChanges();
                }
                
            }


            return result;
        }

        public Category Get(int id)
        {
            return db.Categories.Find(id);
        }

        public List<Category> GetList()
        {
            return db.Categories.ToList();
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
            while (newCategory.Parent != 0) // если ноль, значит вершина достигнута
            {
                listParent.Add(newCategory); // добавить родителя
                newCategory = list.Find(x => x.Id == newCategory.Parent); // найти следующего
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
        public List<Category> GetListAltParent(int id) 
        {
            var list = db.Categories.ToList();
            var top = list.Find(x => x.Id == id);
            var notAltList = new List<Category>();

            MakeListNotAltParent(top, ref list, ref notAltList);

            var result = list.Except(notAltList); // убрать дочерние из листа

            return list.Except(notAltList).ToList();
        }

        private void MakeListNotAltParent(Category top, ref List<Category> list, ref List<Category> notAltList) // получить лист всех дочерних на всех уровнях
        {
            foreach (var item in list)
            {
                if (item.Parent == top.Id) 
                {
                    notAltList.Add(item);
                    MakeListNotAltParent(item, ref list, ref notAltList);
                }
            }
        }
        

        private int returnIndexOrganism(List<Organism> organisms, int id)  // АЛЬТ
        {
            int index = -1;
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
        public int returnIndexCategory(List<Category> categoryes, int id) // АЛЬТ
        {
            int index = 0;
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
        public List<Category> chainCategoriesToOrganism(int idOrganism, List<Category> categories, List<Organism> organisms) // АЛЬТ
        {
            
            int categoryIndex = returnIndexCategory(categories, organisms[returnIndexOrganism(organisms,idOrganism)].CategoryId);  
             List<Category> result = new List<Category>();  // тут дублеж!! потом исправить
            while (categories[categoryIndex].Parent != 0) // цепочка родительских категорий снизу-вверх
            {
                result.Add(categories[categoryIndex]);
                categoryIndex = returnIndexCategory(categories, categories[categoryIndex].Parent);
            }

            result.Add(categories[0]);

            for (int i = 0; i < (result.Count / 2); i++) // Это реверс
            {
                Category buf = new Category();
                Category buf2 = new Category();

                buf = result[i];
                buf2 = result[result.Count - i -1];  // на всяк пожарный, чтоб не по ссылке

                result[result.Count - i -1] = buf;
                result[i] = buf2;

            }
            return result;
            
        }
        /*
        public List<Category> chainCategories(List<Category> categories, int idCategory) // не нужно
        {
            int categoryIndex = categories[returnIndexCategory(categories ,idCategory)].Parent;
            List<Category> result = new List<Category>();
            while (categories[categoryIndex].Parent != 0)
            {
                result.Add(categories[categoryIndex]);
                categoryIndex = returnIndexCategory(categories, categories[categoryIndex].Parent);
            }

            result.Add(categories[0]);

            for (int i = 0; i < (result.Count / 2); i++)
            {
                Category buf = new Category();
                Category buf2 = new Category();

                buf = result[i];
                buf2 = result[result.Count - i - 1];  // на всяк пожарный, чтоб не по ссылке

                result[result.Count - i - 1] = buf;
                result[i] = buf2;

            }
            return result;
        }
        */
       
        /*
        private int getIdParentCategoryForIdOrganism(int idOrganism, List<Category> categories, List<Organism> organisms)
        {
            foreach (var organism in organisms)
            {
                if (organism.Id == idOrganism)
                {
                    foreach (var category in categories)
                    {
                        if (category.Id == organism.CategoryId)
                        {
                            return category.Parent;
                        }
                    }
                }
            }
            return 0;
        }
        */
        /// <summary>
        /// Делает лист категорий для вывода в виде дерева добавляется уровень узла и отступ
        /// </summary>
        /// <returns></returns>
        public List<CategoryTree> GetListTree()  
        {
            // потом переделать нормально ?
            var tree = new List<CategoryTreeWithChilds>(); // лист, где каждый элемент хранит всех своих потомков
            var list = db.Categories.ToList(); // просто лист всех категорий
            var top = list.Find(x => x.Id == 1); // вершина дерева

            makeCategoryTreeWC(ref tree,top,list); // наполнение элементами листа, где каждый элемент хранит всех своих потомков

            var topTreeWC = tree.Find(x => x.data.Id == 1); // вершина дерева, где каждый элемент хранит всех своих потомков
            var treeCategory = new List<CategoryTree>(); // пустой лист категорий с отступом и уровнем
            int level = 0; // начальный уровень
            treeCategory.Add(new CategoryTree // записываем вершину листа дерева с наследниками, но без них
            {
                Id = topTreeWC.data.Id,
                Name = topTreeWC.data.Name,
                NameCat = topTreeWC.data.NameCat,
                Parent = topTreeWC.data.Parent,
                Description = topTreeWC.data.Description,
                level = level
            });

            SortTree(ref treeCategory, tree, topTreeWC, level); // записываем в treeCategory категории порядке их отображения на странице

            foreach (var item in treeCategory) // формирование отступа
            {
                for (int i = 0; i < item.level; i++)
                {
                    item.indent = item.indent + "....";
                }
            }

            return treeCategory;
        }

        /// <summary>
        /// записываем в treeCategory категории порядке их отображения на странице
        /// </summary>
        /// <param name="treeCategory">формируемый лист, передается по ссылке</param>
        /// <param name="tree">дерево, где каждый элемент хранит всех своих потомков</param>
        /// <param name="top">вершина, где каждый элемент хранит всех своих потомков</param>
        /// <param name="level">уровень вершины</param>
        private void SortTree(ref List<CategoryTree> treeCategory, List<CategoryTreeWithChilds> tree, CategoryTreeWithChilds top, int level)
        {
            // спуск вниз по веткам с присвоением уровня
            level++; // добавляет уровень
            if (top.childs != null) // у вершины должны быть потомки
            {
                foreach (var item in top.childs) // берем каждого потомка вершины
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
                        
                        SortTree(ref treeCategory, tree, tree.Find(x => x.data == item), level);
                    // рекурсия. переходим на потомков.next этого потомка, что бы они следовали в итоговом листе за текущим потомком 
                    // уровень там станет уже на 1 больше, так же передается как вершина текущий потомок



                }
            }
          
        }
        /// <summary>
        /// формируем лист категорий, где у каждого элемента вдобавок хранится его потомки
        /// </summary>
        /// <param name="tree">формируемый лист</param>
        /// <param name="top">вершина</param>
        /// <param name="list">лист всех категорий</param>
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

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using TaxonomyPrj2.interfaces;
using TaxonomyPrj2.Models;
using TaxonomyPrj2.ViewModels;

namespace TaxonomyPrj2.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
              
        public IActionResult index()
        {
           var model = new IndexCategoriesViewModel();
             return View(model);
        }
              
       

        [HttpGet]
        public IActionResult PartialInfoTree()
        {
            var model = new IndexCategoriesViewModel();

            using (var repozitCategory = new CategoryRepository())
            {
                model.CategoryTree = repozitCategory.GetList();
            }
            return View(model);
        }

       
       
        public IActionResult PartialCreate(int? id)   ////
        {
            var model = new PartialCreateCategoryVievModel();

                using (var repozitCategory = new CategoryRepository())
                {
                    model.Categories = repozitCategory.GetList();
                    var isCorrectId = id.HasValue && repozitCategory.GetList().Any(x => x.Id == id);
                    model.Id = isCorrectId ? id.Value : 0;
                    if (model.Id != 0)
                    {
                        model.Name = repozitCategory.Get(model.Id).Name;
                        model.NameCat = repozitCategory.Get(model.Id).NameCat;
                        model.Parent = repozitCategory.Get(model.Id).Parent;
                        model.Description = repozitCategory.Get(model.Id).Description;
                    }


                }
          
           
               
            return PartialView(model);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult PartialCreate(int parentId, string name, string nameCat, string description)
        {
            
            using (var repozitCategory = new CategoryRepository())
            {
                
                var newCategory = new CategoryEditModel();
                newCategory.Parent = parentId;
                newCategory.Name = name;
                newCategory.NameCat = nameCat;
                newCategory.Description = description;
                repozitCategory.Create(newCategory);

            }
            
            return Json(new { save = true });
        }

        public IActionResult PartialEdit(int? id)
        {
            var model = new PartialCreateCategoryVievModel();
            using (var repozitCategory = new CategoryRepository())
            {
                
                var isCorrectId = id.HasValue && repozitCategory.GetList().Any(x => x.Id == id);
                model.Id = isCorrectId ? id.Value : 0;
                model.Categories = isCorrectId ? repozitCategory.GetAnotherBranches(id.Value) : repozitCategory.GetList();
                //var cadasd = repozitCategory.GetList();

                if (model.Id != 0)
                {
                    model.Name = repozitCategory.Get(model.Id).Name;
                    model.NameCat = repozitCategory.Get(model.Id).NameCat;
                    model.Parent = repozitCategory.Get(model.Id).Parent;
                    model.Description = repozitCategory.Get(model.Id).Description;
                }


            }

            return PartialView(model);
        }

        [HttpPost]
        public IActionResult PartialEdit(int id, int parentId, string name, string nameCat, string description)
        {
            using (var repozitCategory = new CategoryRepository())
            {

                var newCategory = new CategoryEditModel();

                newCategory.Id = id;
                newCategory.Parent = parentId;
                newCategory.Name = name;
                newCategory.NameCat = nameCat;
                newCategory.Description = description;
                repozitCategory.Update(newCategory);

            }

            return Json(new { save = true });
        }

        [HttpGet]
        public IActionResult PartialDelete(int id)
        {
            var model = new QuestionViewModel();
            model.Id = id;

            return PartialView(model);

            
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                using (var repozitCategory = new CategoryRepository())
                {


                    var result = repozitCategory.Delete(id);
                    return Json(new { save = result });
                }
            }
            catch (Exception ex)
            {

                return Json(new { save = false, error = ex.Message });
            }
           
            
          
        }

       



    }

    
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaxonomyPrj2.interfaces;
using TaxonomyPrj2.Models;
using TaxonomyPrj2.ViewModels;

namespace TaxonomyPrj2.Controllers
{
    [Authorize]
    public class OrganismController : Controller
    {
       

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult PartialCreate()  // СКРИПТ создание организма ГЕТ
        {
            var model = new PartialRedactOrganismViewModel();
           

            using (var repozitCategory = new CategoryRepository())
            {
                model.Categories = repozitCategory.GetList();
            }
            model.Id = 0;
           
            return PartialView("PartialEdit", model);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult PartialEdit(int id)  // скрипт редактирование ГЕТ
        {
            var model = new PartialRedactOrganismViewModel();
           

           
            using (var repozitOrganizm = new OrganismRepository())
            {
                

                var organism = repozitOrganizm.Get(id);

                model.Id = organism.Id;
                model.Name = organism.Name;
                model.CategoryId = organism.CategoryId;
                model.StartResearch = organism.StartResearch;
                model.CountSample = organism.CountSample;
                model.Description = organism.Description;

                using (var repozitCategory = new CategoryRepository())
                {
                    model.Categories = repozitCategory.GetListParent(organism.CategoryId);
                }
            }
            return PartialView(model);

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult PartialEdit(int id, string name, int categoryId, DateTime startResearch, int countSample, string description) // скрипт редактирования/создания ПОСТ
        {
            
            OrganismEditModel newOrganism = new OrganismEditModel
            {
                Id = id,
                Name = name,
                CategoryId = categoryId,
                StartResearch = startResearch,
                CountSample = countSample,
                Description = description
            };
            using (var repozitOrganizm = new OrganismRepository())
            {
                if (id == 0) // создание нового организма
                {
                    try
                    {
                        repozitOrganizm.Create(newOrganism);
                    }
                    catch (Exception)
                    {
                        return Json(new { save = false });
                    }
                }
                else
                {                    
                    try
                    {
                        repozitOrganizm.Update(newOrganism);
                    }
                    catch (Exception)
                    {
                        return Json(new { save = false });
                    }
                }
            }

              


            return Json(new { save = true });
        }


        [HttpGet]
        public IActionResult PartialOrganismTable(int categoryId)  // скрипт отображение списка организмов по категории ГЕТ
        {
            var model = new OrganismTableViewModel();
           
            using (var repozitOrganism = new OrganismRepository())
            {

                model.Organisms = repozitOrganism.GetListByCategoryId(categoryId);
                
            }
            return PartialView(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult PartialConfirmDelete(int id)  // 
        {
            
            var model = new QuestionViewModel();
            using (var repozitOrganism = new OrganismRepository())
            {
                model.Id = repozitOrganism.Get(id).CategoryId;
            }
            
            return PartialView(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Delete(int id)  // 
        {
            try
            {
                using (var repozitOrganism = new OrganismRepository())
                {
                  var result = repozitOrganism.Delete(id);
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
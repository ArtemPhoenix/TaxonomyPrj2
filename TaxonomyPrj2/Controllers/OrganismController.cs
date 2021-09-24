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
        private readonly UserManager<User> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public OrganismController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;

        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult PartialCreate()  // СКРИПТ создание организма ГЕТ
        {
            var model = new PartialRedactOrganismViewModel();
            model.FlagEvent = 0;

            using (var repozitCategory = new CategoryRepository())
            {
                model.Categories = repozitCategory.GetList();
            }
            model.Id = 0;
           /* model.Name = "Название нового организма";
            model.Description = "Краткое описание нового организма";*/
            return PartialView("PartialEdit", model);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult PartialEdit(int id)  // скрипт редактирование ГЕТ
        {
            var model = new PartialRedactOrganismViewModel();
            model.FlagEvent = 0;

           
            using (var repozitOrganizm = new OrganismRepository())
            {
                using (var repozitCategory = new CategoryRepository())
                {
                    var idOrg = repozitOrganizm.GetList().Find(x => x.Id==id).CategoryId;
                    model.Categories = repozitCategory.GetListParent(idOrg);
                }

                var organism = repozitOrganizm.Get(id);
                model.Id = organism.Id;
                model.Name = organism.Name;
                model.CategoryId = organism.CategoryId;
                model.StartResearch = organism.StartResearch;
                model.CountSample = organism.CountSample;
                model.Description = organism.Description;


            }
            return PartialView(model);

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult PartialEdit(int id, string name, int categoryId, DateTime startResearch, int countSample, string description) // скрипт редактирования/создания ПОСТ
        {
            var model = new PartialRedactOrganismViewModel();
            OrganismEditModel newOrganism = new OrganismEditModel
            {
                Id = id,
                Name = name,
                CategoryId = categoryId,
                StartResearch = startResearch,
                CountSample = countSample,
                Description = description
            };

            if (id == 0) // создание новогоорганизма
            {

                model.FlagEvent = 3;
                using (var repozitOrganizm = new OrganismRepository())
                {
                    repozitOrganizm.Create(newOrganism);

                    var newOrg = repozitOrganizm.GetList().Last();
                    model.Id = newOrg.Id;
                    model.Name = newOrg.Name;
                    model.CategoryId = newOrg.CategoryId;
                    model.StartResearch = newOrg.StartResearch;
                    model.CountSample = newOrg.CountSample;
                    model.Description = newOrg.Description;

                    using (var repozitCategory = new CategoryRepository())
                    {
                        var idOrg = repozitOrganizm.GetList().Find(x => ((x.Name == newOrganism.Name) && (x.Description == newOrganism.Description))).CategoryId;
                        model.Categories = repozitCategory.GetListParent(idOrg);
                    }
                }

              

            }
            else  // обновление организма
            {

                model.FlagEvent = 1;

                using (var repozitOrganizm = new OrganismRepository())
                {
                    repozitOrganizm.Update(newOrganism);

                    var organism = repozitOrganizm.Get(newOrganism.Id);
                    model.Id = organism.Id;
                    model.Name = organism.Name;
                    model.CategoryId = organism.CategoryId;
                    model.StartResearch = organism.StartResearch;
                    model.CountSample = organism.CountSample;
                    model.Description = organism.Description;
                }

                using (var repozitCategory = new CategoryRepository())
                {
                    var idCat = repozitCategory.GetList().Find(x => x.Id == newOrganism.CategoryId).Id;
                    model.Categories = repozitCategory.GetListParent(idCat);
                }
            }

           

            return PartialView(model);
        }


        [HttpGet]
        public async Task<IActionResult> PartialOrganismTable(int categoryId)  // скрипт отображение списка организмов по категории ГЕТ
        {
            var elem = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            var userRoles = await _userManager.GetRolesAsync(elem);

            //int categoryId = 1;
            ViewBag.currenCategoryId = categoryId;
           
            var model = new OrganismTableViewModel();
            //роли
            model.Role = userRoles.First();
            if (model.Role == "Admin")
            {
                model.Admin = true;
                model.CommonUser = false;
            }
            else
            {
                model.CommonUser = true;
                model.Admin = false;
            }
            //
            using (var repozitOrganism = new OrganismRepository())
            {

                model.Organisms = repozitOrganism.GetListByCategoryId(categoryId);
                //var allOrganisms = repozitOrganism.GetList().Where(x => x.CategoryId.Equals(categoryId));
                model.CurrenCategoryId = categoryId;
                //return View(model);
            }
            return PartialView(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult PartialQuestion(int id)  
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
        public IActionResult PartialQuestion(int? id)  // спросить, может ли id быть null?
        {

            using (var repozitOrganism = new OrganismRepository())
            {
                var isCorrectId = id.HasValue && repozitOrganism.GetList().Any(x => x.Id == id);

                repozitOrganism.Delete(isCorrectId ? id.Value : 0);
            }

            return Json(new { save = true });
        }

     




    }
}
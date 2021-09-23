﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly UserManager<User> _userManager;
      //  private RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;

        public CategoryController(UserManager<User> userManager, SignInManager<User> signInManager/*, RoleManager<IdentityRole> roleManager*/)
        {
            _userManager = userManager;
            //_roleManager = roleManager;
            _signInManager = signInManager;
        }
       
        public IActionResult index()
        {
           var model = new IndexCategoriesViewModel();

            using (var repozitCategory = new CategoryRepository())
            {
                model.CategoryTree = repozitCategory.GetListTree();
            }

            
                return View(model);
        }

        [HttpGet]
        public IActionResult PartialDescription(int id)
        {
            
            var model = new PartialDescriptionViewModel();
            using (var repozitCategory = new CategoryRepository())
            {
                model.Id = repozitCategory.Get(id).Id;
                model.Name = repozitCategory.Get(id).Name;
                model.NameCat = repozitCategory.Get(id).NameCat;
                model.Parent = repozitCategory.Get(id).Parent;
                model.Description = repozitCategory.Get(id).Description;
            }
                return PartialView(model); 
        }

       

        [HttpGet]
        public IActionResult PartialInfoTree()
        {
            var model = new IndexCategoriesViewModel();

            using (var repozitCategory = new CategoryRepository())
            {
                model.CategoryTree = repozitCategory.GetListTree();
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult PartialInfoCategory(int id)
        {
            var model = new PartialDescriptionViewModel();
            using (var repozitCategory = new CategoryRepository())
            {
                model.Id = repozitCategory.Get(id).Id;
                model.Name = repozitCategory.Get(id).Name;
                model.NameCat = repozitCategory.Get(id).NameCat;
                model.Parent = repozitCategory.Get(id).Parent;
                model.Description = repozitCategory.Get(id).Description;
            }

            return PartialView(model);
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
                model.Categories = isCorrectId ? repozitCategory.GetListAltParent(id.Value) : repozitCategory.GetList();
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
        //[ValidateAntiForgeryToken]
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
        public IActionResult PartialDelete(int? id)
        {

            using (var repozitCategory = new CategoryRepository())
            {

                var isCorrectId = id.HasValue && repozitCategory.GetList().Any(x => x.Id == id);

               // var result =isCorrectId ? repozitCategory.Delete(isCorrectId ? id.Value : 0): false;

                return Json(new { save = isCorrectId ? repozitCategory.Delete(isCorrectId ? id.Value : 0) : false }); 
            }
            
          
        }

        [HttpGet]
        public async Task<IActionResult> returnRole(string role) // проверка на неизменность роли
        {
            var elem = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            var userRoles = await _userManager.GetRolesAsync(elem);
            if (userRoles.First() == role)
            {
                return Json(new { result = true });
            }
            else
            {
                await _signInManager.SignOutAsync();
                return Json(new { result = false });
            }

        }



    }

    
}
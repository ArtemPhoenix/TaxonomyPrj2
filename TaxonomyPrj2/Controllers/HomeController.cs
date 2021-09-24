﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using TaxonomyPrj2.interfaces;
using TaxonomyPrj2.Models;
using TaxonomyPrj2.ViewModels;

namespace TaxonomyPrj2.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UserManager<User> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;
        public HomeController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager  )
        {
          _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;

        }
       
        //[Authorize(Roles = "CommonUser")]
        [HttpGet]
        public async Task<IActionResult> Index(int? id)
        {
           
            var elem = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            var userRoles = await _userManager.GetRolesAsync(elem);

            using (var repozitCategory = new CategoryRepository())
            {

                var model = new IndexViewModel {
                    Categories = repozitCategory.GetList()
                };
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
                var isCorrectId = id.HasValue && model.Categories.Any(x => x.Id == id);
                model.CurrenCategoryId = isCorrectId ? id.Value : model.Categories.First().Id;  // тернарные операторы

                using (var repozitOrganism = new OrganismRepository())
                {
                    model.Organisms = repozitOrganism.GetListByCategoryId(model.CurrenCategoryId/*, repozitCategory.GetList()*//*,true*/);
                    ViewBag.currenCategoryId = model.CurrenCategoryId;

                    model.SearchNameOrganism = "Организм";
                    model.SearchCountFromtOrganism = 5;
                    model.SearchCountTotOrganism = 15;
                    model.SearchDateFromOrganism = new DateTime(1922, 12, 29);
                    model.SearchDateToOrganism = new DateTime(1991, 12, 26);
                    return View(model);
                }
            }
        }

        [HttpGet]
        public IActionResult PartialSearchStart()
        {
            var model = new IndexViewModel();
            model.SearchNameOrganism = "Организм";
            model.SearchCountFromtOrganism = 5;
            model.SearchCountTotOrganism = 15;
            model.SearchDateFromOrganism = new DateTime(1922, 12, 29);
            model.SearchDateToOrganism = new DateTime(1991, 12, 26);

            return PartialView(model);
        }


        [HttpGet]
        public IActionResult PartialSearchResult(string searchNameOrganism, int? searchCountFromtOrganism, int? searchCountTotOrganism, string searchDateFromOrganism, string searchDateToOrganism, int? searchIdCategiryOrganism)
        {
            DateTime? searchDateFromOrganismC;
            if (DateTime.TryParse(searchDateFromOrganism, out var t)) { searchDateFromOrganismC = t; }
            else { searchDateFromOrganismC = null; }

            DateTime? searchDateToOrganismC;
            if (searchDateFromOrganism != null)
            {
                try
                {
                    searchDateToOrganismC = Convert.ToDateTime(searchDateToOrganism);
                }
                catch (Exception)
                {

                    searchDateToOrganismC = null;
                }
            }
            else
            {
                searchDateToOrganismC = null;
            }

            var model = new OrganismTableViewModel();
            using (var repozitOrganism = new OrganismRepository())
            {
               model.Organisms = repozitOrganism.SearchOrganismsByFilter(searchNameOrganism, searchCountFromtOrganism, searchCountTotOrganism, searchDateFromOrganismC, searchDateToOrganismC, searchIdCategiryOrganism);
            }

            if (searchIdCategiryOrganism != null)
            {
                model.CurrenCategoryId = searchIdCategiryOrganism.Value;
                // return PartialOrganismTable(model);
            }
            else
            {
                model.CurrenCategoryId = 0;
            }

            
            return PartialView(model);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult PartialWorkWithUser()
        {
            return PartialView();
        }

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








        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
        public IActionResult Example()
        {
            return View();
        }

        

        

        public IActionResult Examp(int categoryId)
        {
            int exa = categoryId + 100;
            return View(exa);
        }


    }
}
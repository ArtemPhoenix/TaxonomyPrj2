using System;
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
        public HomeController(UserManager<User> userManager )
        {
            _userManager = userManager;
        }
       
        
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
                
                var isCorrectId = id.HasValue && model.Categories.Any(x => x.Id == id);
                model.CurrenCategoryId = isCorrectId ? id.Value : model.Categories.First().Id;  // тернарные операторы

                using (var repozitOrganism = new OrganismRepository())
                {
                    model.Organisms = repozitOrganism.GetListByCategoryId(model.CurrenCategoryId);
                    ViewBag.currenCategoryId = model.CurrenCategoryId;
                    
                    return View(model);
                }
            }
        }

        [HttpGet]
        public IActionResult PartialSearchStart() 
        {
            var model = new IndexViewModel();
            return PartialView(model);
        }


        [HttpGet]
        public IActionResult PartialSearchResult(string NameOrganism, int? CountFromtOrganism, int? CountTotOrganism, string DateFromOrganism, string DateToOrganism, int? IdCategiryOrganism)
        {
            DateTime? DateFromOrganismC = null;
            if (DateTime.TryParse(DateFromOrganism, out var t)) { DateFromOrganismC = t; }

            DateTime? DateToOrganismC = null;
            if (DateTime.TryParse(DateToOrganism, out var x)) { DateToOrganismC = x; }

            var model = new OrganismTableViewModel();
            using (var repozitOrganism = new OrganismRepository())
            {
               model.Organisms = repozitOrganism.SearchOrganismsByFilter(NameOrganism, CountFromtOrganism, CountTotOrganism, DateFromOrganismC, DateToOrganismC, IdCategiryOrganism);
            }

            if (IdCategiryOrganism != null) // на основе его формируется нужный вывод для фильтра
            {
                model.CurrenCategoryId = IdCategiryOrganism.Value;
            }
            else
            {
                model.CurrenCategoryId = null;  
            }

            
            return PartialView(model);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult PartialWorkWithUser()
        {
            return PartialView();
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

        

        

       


    }
}

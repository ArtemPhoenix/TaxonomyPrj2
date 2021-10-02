using DAL.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxonomyPrj2.interfaces;
using TaxonomyPrj2.ViewModels;

namespace TaxonomyPrj2.Controllers
{
    [Authorize]
    public class InformationController : Controller
    {
        public IActionResult Index()
        {
            using (var repozitOrganism = new OrganismRepository())
            {
                var model = new IndexInformationViewModel() { Info = repozitOrganism.getAllInformation() };
                return View(model);
            }
               
           

           
            
        }
    }
}

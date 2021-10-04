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
            using var repozitOrganism = new OrganismRepository();
            var model = new IndexInformationViewModel()
            {
                Info = repozitOrganism.getAllInformation()
            };

            return View(model);

        }

        public IActionResult PartialSearch( int? CountFromOrganism, int? CountToOrganism, string DateFromOrganism, string DateToOrganism)
        {
            DateTime? DateFromOrganismC = null;
            if (DateTime.TryParse(DateFromOrganism, out var t)) { DateFromOrganismC = t; }

            DateTime? DateToOrganismC = null;
            if (DateTime.TryParse(DateToOrganism, out var x)) { DateToOrganismC = x; }

            using var repozitOrganism = new OrganismRepository();
            var model = new IndexInformationViewModel()
            {
                Info = repozitOrganism.getFiltrInformation(CountFromOrganism, CountToOrganism, DateFromOrganismC, DateToOrganismC)
            };

            return PartialView(model);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    [Keyless]
    public class OrganismAndCategory 
    {
        public int Id { get; set; }  //+
        public int CategoryId { get; set; }
        public string NameOrganism { get; set; }  //+
        public string DescriptionOrganism { get; set; }
      //  public DateTime StartResearch { get; set; }

        public int CountSample { get; set; }
        public string NameCategory { get; set; }
           
        public string NameCat { get; set; }
         public string DescriptionCategory { get; set; }


    }
}


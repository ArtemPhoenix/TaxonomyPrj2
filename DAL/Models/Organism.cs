using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class Organism
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public DateTime StartResearch { get; set; }
        public int CountSample { get; set; }

        public virtual Category Category { get; set; }
    }
}

using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class Category
    {
        public Category()
        {
            InverseParentNavigation = new HashSet<Category>();
            Organisms = new HashSet<Organism>();
        }

        public int Id { get; set; }
        public int? Parent { get; set; }
        public string Name { get; set; }
        public string NameCat { get; set; }
        public string Description { get; set; }

        public virtual Category ParentNavigation { get; set; }
        public virtual ICollection<Category> InverseParentNavigation { get; set; }
        public virtual ICollection<Organism> Organisms { get; set; }
    }
}

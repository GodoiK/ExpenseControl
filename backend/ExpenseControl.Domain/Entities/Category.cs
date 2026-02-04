using ExpenseControl.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseControl.Domain.Entities
{
    public class Category
    {
        public Guid Id { get; set; }
        public required string Description { get; set; }
        public CategoryPurpose Purpose { get; set; }

        public Category() { }

        public Category(string name, CategoryPurpose purpose)
        {
            Id = Guid.NewGuid();
            Description = name;
            Purpose = purpose;
        }
    }
}

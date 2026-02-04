using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseControl.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public int Age { get; private set; }

        private readonly List<FinancialRecord> _records = new();
        public IReadOnlyCollection<FinancialRecord> Records => _records;

        public User(string name, int age)
        {
            Id = Guid.NewGuid();
            Name = name;
            Age = age;
        }

        public void Update(string name, int age)
        {
            Name = name;
            Age = age;
        }

        public bool IsMinor()
            => Age < 18;
    }
}

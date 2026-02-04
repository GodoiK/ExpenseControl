using ExpenseControl.Domain.Enums;
using ExpenseControl.Domain.Exceptions;
using System;

namespace ExpenseControl.Domain.Entities
{
    public class FinancialRecord
    {
        public Guid Id { get; private set; }

        public User User { get; private set; } = null!;
        public Guid UserId { get; private set; }

        public Category Category { get; private set; } = default!;
        public Guid CategoryId { get; private set; }

        public string Description { get; private set; } = string.Empty;
        public FinancialRecordType Type { get; private set; }
        public decimal Amount { get; private set; }
        public DateTime CreatedAt { get; private set; }

        // EF Core
        protected FinancialRecord() { }

        public FinancialRecord(
            User user,
            FinancialRecordType type,
            decimal amount,
            string description)
        {
            if (user is null)
                throw new BusinessException("User is required.");

            Id = Guid.NewGuid();
            User = user;
            UserId = user.Id;
            Type = type;
            Amount = amount;
            Description = description ?? string.Empty;
            CreatedAt = DateTime.UtcNow;
        }

        public FinancialRecord(
            User user,
            Category category,
            FinancialRecordType type,
            decimal amount,
            string description)
            : this(user, type, amount, description)
        {
            if (category is null)
                throw new BusinessException("Category is required.");

            Category = category;
            CategoryId = category.Id;
        }
    }
}

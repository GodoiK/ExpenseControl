using ExpenseControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseControl.Infrastructure.Data.Configurations
{
    public class FinancialRecordConfiguration : IEntityTypeConfiguration<FinancialRecord>
    {
        public void Configure(EntityTypeBuilder<FinancialRecord> builder)
        {
            builder.ToTable("FinancialRecords");

            builder.HasKey(fr => fr.Id);

            builder.Property(fr => fr.Description)
                .IsRequired()
                .HasMaxLength(400);

            builder.Property(fr => fr.Amount)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(fr => fr.Type)
                .IsRequired();

            builder.Property(fr => fr.CreatedAt)
                .IsRequired();
            
            builder.Property(x => x.CategoryId)
                .HasColumnName("CategoryId")
                .IsRequired();

            builder.HasOne(fr => fr.User)
                .WithMany()
                .HasForeignKey(fr => fr.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(fr => fr.Category)
                .WithMany()
                .HasForeignKey(fr => fr.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

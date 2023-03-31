using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todo_App.Domain.Entities;

namespace Todo_App.Infrastructure.Persistence.Configurations;
public class TodoTagConfiguration : IEntityTypeConfiguration<TodoTag>
{
    public void Configure(EntityTypeBuilder<TodoTag> builder)
    {
        builder.Property(t => t.Name)
                  .HasMaxLength(25)
                  .IsRequired();

    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Repository.Data.Configurations
{
    internal class TaskConfigurations : IEntityTypeConfiguration<Tasky>
    {
        public void Configure(EntityTypeBuilder<Tasky> builder)
        {
            builder.Property(T => T.Name).IsRequired();
            builder.Property(T => T.Description).HasMaxLength(100).IsRequired();
           
        }
    }
}

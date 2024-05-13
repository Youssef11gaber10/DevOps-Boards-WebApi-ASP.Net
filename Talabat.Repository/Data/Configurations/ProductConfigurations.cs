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
    internal class ProductConfigurations : IEntityTypeConfiguration<Product>
    {


        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasOne(P => P.ProductBrand)
                .WithMany()//now i make it one to many not one to one
                .HasForeignKey(P => P.ProductBrandId);//if i change name of it
                                                      //.OnDelete(DeleteBehavior.Cascade);

            

            builder.HasOne(P => P.ProductType)
                .WithMany()
                .HasForeignKey(P => P.ProductTypeId);

          
            builder.Property(P => P.Name).IsRequired().HasMaxLength(100);
            builder.Property(P => P.Description).IsRequired();
            builder.Property(P => P.PictureUrl).IsRequired();





            //solve problem of decimal property when migrations

            #region Error
            //No store type was specified for the decimal property 'Price' on entity type 'Product'.
            //This will cause values to be silently truncated if they do not fit in the default precision and scale.
            //Explicitly specify the SQL server column type that can accommodate all the values in 'OnModelCreating' using 'HasColumnType', specify precision and scale using 'HasPrecision', or configure a value converter using 'HasConversion' 
            #endregion
            //he was assume it was decimal 18,2  i ensure i want decimal(18,2)
            builder.Property(P => P.Price).HasColumnType("decimal(18,2)");




        }
    }
}

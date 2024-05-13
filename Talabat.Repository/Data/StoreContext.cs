using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Repository.Data.Configurations;

namespace Talabat.Repository.Data
{
    public class StoreContext : IdentityDbContext<Users>
    {


        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Tasky> Tasks { get; set; }
        public DbSet<Comment> Comments { get; set; }





        #region Fluent Api

        //now i can add my fluent api
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            //modelBuilder.ApplyConfiguration(new ProductConfigurations());//make it for 3 either , i have another way
            //func apply all classes inhert interface IEntityTypeConfiguraions
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());//must give him project the have the configurations class
                                                                                          //Assembly.GetExecutingAssembly() => this give me the current project execute
                                                                                          //so if i add any configration in same projects its automatic applied

            #region configure m-m commentUser


            modelBuilder.Entity<CommentUser>()
                           .HasKey(SC => new { SC.CommentId, SC.UserId });


            modelBuilder.Entity<Comment>()
                .HasMany(S => S.CommentUser)
                .WithOne(SC => SC.Comment)
                .IsRequired(true)
            .HasForeignKey(SC => SC.CommentId);


            modelBuilder.Entity<Users>()
                .HasMany(C => C.CommentUser)
                .WithOne(SC => SC.User);


            #endregion



        }

        #endregion



    }
}

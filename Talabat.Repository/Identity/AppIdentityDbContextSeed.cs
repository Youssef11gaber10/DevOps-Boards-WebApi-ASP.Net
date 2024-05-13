using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
    public static class AppIdentityDbContextSeed
    {

        //public static async Task SeedUserAsync(AppIdentityDbContext dbContext)//instead of creating instance of dbcontext it dosn't have a fucntion the Identiy pachage have these fucntions 
        public static async Task SeedUserAsync(UserManager<Users> userManger)//user manger its like a  repo have some fucntion operate with database  like normal repo
        {
            if(!userManger.Users.Any())
            {
                var user = new Users
                {
                    DisplayName = "Aliaa Tarek",
                    Email = "aliaatarek.route@gmail.com",
                    UserName = "aliaatarek.route",
                    PhoneNumber = "01234343443",
                    RoleName ="TeamLeader"

                };
                await userManger.CreateAsync(user, "Pa$$w0rd");

            }





        }
    }
}

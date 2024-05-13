using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;
using Talabat.Service;

namespace Talabat.APIs.Extensions
{
    public static class IdentityServicesExtension
    {

        public static IServiceCollection AddIdentityServices(this IServiceCollection Services,IConfiguration configuration)
        {
            //1 first allow it for dependencies inside interfaces & repos like _dbContext inside the repos


            Services.AddIdentity<Users, IdentityRole>()//add Interfaces interface
                            .AddEntityFrameworkStores<StoreContext>(); //add implementation

            //2 then allow it for repos its self
            //builder.Services.AddScoped<UserManager<AppUser>>();//instead of that 
            Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)//for all repos in Idnetity package => userManger/Signin Manger/ RoleManger
            //this just for suring if front-end send token and suring if i create this token
            .AddJwtBearer(Options=>
            {
                Options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["JWT:Issure"],

                    ValidateAudience = true,
                    ValidAudience = configuration["JWT:Audience"],

                    ValidateLifetime = true,//he take it from there

                    ValidateIssuerSigningKey =true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))
            };

            });

            #region Allow Di for object from ITokenService to call func to create token
 
              Services.AddScoped<ITokenService, TokenServices>();

            #endregion


            return Services;
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.APIs.Helpers;
using Talabat.APIs.Middlewares;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;

namespace Talabat.APIs
{
    public class Program
    {

        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region  Add services to the container.
            // Add services to the container.
            builder.Services.AddControllers();//add service for api

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();//those for swagger
            builder.Services.AddSwaggerGen();//those for swagger

            #region allow DI For DBContext
            //builder.Services.AddDbContext<StoreContext>();//here allow DI for DBContext but in its paramterless ctor need options(connection string) so i must send it
            builder.Services.AddDbContext<StoreContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });


            //builder.Services.AddDbContext<AppIdentityDbContext>(Options =>
            //{
            //    Options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            //});




            #endregion


            #region Allow DI for Generic class
            //builder.Services.AddScoped<IGenericRepository<Product>,GenericRepository<Product>> ();//insted of do it for each model 
            //make it genreic

            builder.Services.AddScoped<IProjectRepository, ProjectRepostiory>();
            builder.Services.AddScoped<ITaskRepository, TaskRepository>();

            //builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));//when ask for class impelemnts Igeneric of any type give him Class of Generic of any type


            #endregion


            #region Allow Auto Mapper
            //builder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));// this map will include all profiles in its ctor

            //builder.Services.AddAutoMapper(typeof(MappingProfiles));

            #endregion


            #region Configure Validation Error
            //builder.Services.Configure<ApiBehaviorOptions>(Options =>
            //  {
            //      Options.InvalidModelStateResponseFactory = (actionContext) =>//model state factory is thing that create obj of error
            //      {
            //          //modelState => dictionary [key,value]
            //          //key=> name of pramter that error cause in it like => int id
            //          //value => the error it self
            //          //action context => carry the context of request 
            //          var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count > 0)//if param has error return it
            //                                              .SelectMany(P => P.Value.Errors)
            //                                              .Select(E => E.ErrorMessage)
            //                                              .ToArray();

            //          var ValidationErrorResponse = new ApiValidationErrorResponse()
            //          {
            //              Errors = errors
            //          };
            //          return new BadRequestObjectResult(ValidationErrorResponse);//this will work in any validation error happen
            //                                                                     //we standarize the shape of error will return at all application
            //                                                                     //this configure happen one per app 

            //      };
            //  });


            #endregion


            #region Insted of adding multi services gather them in Extension method and invoke it
            builder.Services.AddApplicationServices();
            #endregion


            #region Allow DI for Identity Package repos function => inside extension method

            builder.Services.AddIdentityServices(builder.Configuration);//the caller will be the Services

            #endregion

            //builder.Services.AddScoped<IProjectRepository, ProjectRepostiory>();
           
            #endregion



            var app = builder.Build();

            //after build before add middle ware
            #region Update-Database
            //StoreContext dbContext = new StoreContext();//invalid i can't create obj from obj and can't create pramterless ctor it will not use the other to open connection with db
            //await dbContext.Database.MigrateAsync();



            //must ask clr to create obj from dbocontext explicitly
            //i was ask it to create obj implicitly buy injected in ctor

            //first i need to get all service thats life time is  scoped that includes dbcontext 
            using var Scope = app.Services.CreateScope();//now i get container of all services thats life time is scoped

            var Services = Scope.ServiceProvider;
            //the Serivces its self



            var LoggerFactory = Services.GetRequiredService<ILoggerFactory>();
            //logger factory is also scoped so i can  create one from it with my Services
            //why i can't inject this object in ctor of Program ? ..

            try
            {
                //those may happen to cause exception

                var dbContext = Services.GetRequiredService<StoreContext>();//now ask him to create obj from my dbContext
                //var identityDbContext = Services.GetRequiredService<AppIdentityDbContext>();


                await dbContext.Database.MigrateAsync();//Update-Database default for app
                //await identityDbContext.Database.MigrateAsync();//update-database for the identity

                //and close the Scope
                //Scope.Dispose();//or use using



                //after update-database do the data seeding
                #region Callfunc of data  seeding

                await StoreContextSeed.SeedAsync(dbContext);//give here the obj used in previous phase of updating-database
                                                            // and we made it inside the try because we can't access the dbcontext outside the try{}
                                                            //i have problem this will call each time i run the progrem ? ok check in func if the table already have data or not if not return


                var userManger = Services.GetRequiredService<UserManager<Users>>();
                await AppIdentityDbContextSeed.SeedUserAsync(userManger);

                #endregion

            }
            catch (Exception ex)
            {//to show exception in console must use Logger Factory
                //like we when do Update-database  if any proplem happen genereted in power chell console we do this for 
                // if happen any problem in update-database appear in console
                var Logger = LoggerFactory.CreateLogger<Program>();//will show the error on log at erros that happen in the class program
                Logger.LogError(ex, "An Error Occured During Appling The Migraitions");
            }

            #endregion



            #region  Configure the HTTP request pipeline.
            // Configure the HTTP request pipeline.

            if (app.Environment.IsDevelopment())
            {
                //i want the serverside error happen just in development env
                app.UseMiddleware<ExceptionMiddleWare>();


                //app.UseSwagger();s
                //app.UseSwaggerUI();
                //instead of this i put them in extension method and invoke it
                app.UseSwaggerMiddlewares();
            }



            #region Handle endPoint Not found
            //app.UseStatusCodePagesWithRedirects("/errors/{0}");//if not found end point will redirect to errors/withstatusCode
            //Console.WriteLine("{0} , {1}", hamada, gg); 
            app.UseStatusCodePagesWithReExecute("/errors/{0}");//we use this cause this send one request instead of use 2 req in ReDirect
            #endregion

            app.UseHttpsRedirection();

            app.UseStaticFiles();//to go open to images or files

            app.UseCors("AllowReact");

            app.UseAuthentication();

            app.UseAuthorization();



            app.MapControllers();

            #endregion






            app.Run();

        }

    }
}

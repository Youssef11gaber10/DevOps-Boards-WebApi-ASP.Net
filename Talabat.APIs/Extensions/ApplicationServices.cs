using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Repositories;
using Talabat.Repository;

namespace Talabat.APIs.Extensions
{
    public static class ApplicationServices
    {
        //this is extension method must be static the pramter its also a caller or Invoker
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services) //IServiceColletion to accept all added Services
        {

            #region   Allow DI for Generic class
            Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            #endregion

            #region Allow For TaskRepository
            Services.AddScoped<ITaskRepository, TaskRepository>();
            #endregion

            #region Allow  AutoMapper
            Services.AddAutoMapper(typeof(MappingProfiles));
            #endregion

            #region Configure Validation Error
            Services.Configure<ApiBehaviorOptions>(Options =>
             {
                 Options.InvalidModelStateResponseFactory = (actionContext) =>//model state factory is thing that create obj of error
                 {
                     //modelState => dictionary [key,value]
                     //key=> name of pramter that error cause in it like => int id
                     //value => the error it self
                     //action context => carry the context of request 
                     var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count > 0)//if param has error return it
                                                         .SelectMany(P => P.Value.Errors)
                                                         .Select(E => E.ErrorMessage)
                                                         .ToArray();

                     var ValidationErrorResponse = new ApiValidationErrorResponse()
                     {
                         Errors = errors
                     };
                     return new BadRequestObjectResult(ValidationErrorResponse);//this will work in any validation error happen
                                                                                //we standarize the shape of error will return at all application
                                                                                //this configure happen one per app 

                 };
             });


            #endregion


            #region Cors try

            Services.AddCors(options =>
            {
                options.AddPolicy("AllowReact", builder =>
                {
                    builder
                        .WithOrigins("http://localhost:3000") // Replace with your React app origin
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    // You might need to adjust allowed methods and headers based on your API
                });
            });

            #endregion


            return Services;
        }
    }
}

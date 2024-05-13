using System.Net;
using System.Text.Json;
using Talabat.APIs.Errors;

namespace Talabat.APIs.Middlewares
{
    public class ExceptionMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleWare> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleWare(RequestDelegate Next, ILogger<ExceptionMiddleWare> logger, IHostEnvironment env)
        {
            _next = Next;
            _logger = logger;
            _env = env;
        }


        // each middleware must have function called "Invoke"

        public async Task InvokeAsync(HttpContext context)
        {
            //make him try to pass this middle ware
            try
            {//the Next have the next middle ware

                await _next.Invoke(context);//try to invoke the func of next middle ware but he cant so fill in catch

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                //Production => Log ex in Database
                // developing => 
                context.Response.ContentType = "application/json";
                //context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                context.Response.StatusCode = 500;
                //if(_env.IsDevelopment()) {
                //    var Response = new ApiExceptionResponse(500, ex.Message, ex.StackTrace.ToString());
                //}
                //else
                //{
                //    var Response = new ApiExceptionResponse(500);//in any eviroment except the development not need to show him the stack trace will not under stand any thing
                //}
                //its syntac sugar

                var Response = _env.IsDevelopment() ? new ApiExceptionResponse(500, ex.Message, ex.StackTrace.ToString()) :
                     new ApiExceptionResponse(500);
                var Options = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase//cause javascript understand it in front-end
                };
                var JsonResponse = JsonSerializer.Serialize(Response, Options);
                context.Response.WriteAsync(JsonResponse);

  
                // SO VALIDATION ERROR & EXCEPTION ERROR HANDLED ONE TIME PER APP
                // BUT NOT FOUND & BADREQUEST MAKE THE MODEL AND CALL IT EACH TIME IN PLACE (MAY THROW ONE OF THEM)
            }








        }


    }
}

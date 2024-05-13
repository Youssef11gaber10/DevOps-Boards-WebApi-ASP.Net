using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;

namespace Talabat.APIs.Controllers
{
    [Route("errors/{Code}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi =true)]//to ignore swagger documentation cause swagger need to each end point have function type
    public class ErrorController : ControllerBase
    {
        //not need to be get or post cause it will not invoke like that i will redirect the user to it if not found its route
        public  ActionResult Error (int Code)
        {
            //return NotFound(new ApiResponse(Code,"End Point Not Found"));
            return NotFound(new ApiResponse(Code));
        }
    }
}

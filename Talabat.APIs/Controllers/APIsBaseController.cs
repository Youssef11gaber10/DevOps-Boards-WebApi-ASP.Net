using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Talabat.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIsBaseController : ControllerBase
    {
        //if there is common endPoints shared to all controller also all controllers inherts them and inhert its route [Route("api/[controller]")] [ApiController]
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Repository.Data;

namespace Talabat.APIs.Controllers
{

    public class BuggyController : APIsBaseController
    {
        private readonly StoreContext _storeContext;

        public BuggyController(StoreContext storeContext)//we can get it from ref from IGeneric<Product>
        {
            _storeContext = storeContext;
        }

        [HttpGet("NotFound")]
        public ActionResult GetNotFoundRequest()
        {
            var Product = _storeContext.Products.Find(100);
            if(Product is null)
                return NotFound(new ApiResponse(404));
                //return NotFound(new ApiResponse(404,"hamada"));// if you want custom message
               
            return Ok(Product);  
        }


        [HttpGet("ServerError")]// server error handling happen one time per project
        public ActionResult GetServerError()
        {
            var Product = _storeContext.Products.Find(100);//will return null
            var ProductToReturn = Product.ToString(); //null.toString() //error will throw [Null Reference Exception]

            return Ok(ProductToReturn);
        }

        [HttpGet("BadRequest")]
        public ActionResult GetBadRequest()
        {
            //return BadRequest(new ApiResponse(400)); 
            return BadRequest(new ApiResponse(400,"its your fault"));
        }

        [HttpGet("BadRequest/{id}")]
        public ActionResult GetBadRequest(int id)
        {
            return Ok();//not found the item
        }


    }
}

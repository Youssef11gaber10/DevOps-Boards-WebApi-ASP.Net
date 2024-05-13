using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;


namespace Talabat.APIs.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    //public class ProductsController : ControllerBase//inherts from Controller base not Controller like mvc
    //[EnableCors("AllowReact")]
    public class ProductsController : APIsBaseController //inherts your route and common endpoints from baseController
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<ProductType> _productType;
        private readonly IGenericRepository<ProductBrand> _productBrand;

        public ProductsController(IGenericRepository<Product> ProductRepository, IMapper mapper,
            IGenericRepository<ProductType> ProductType, IGenericRepository<ProductBrand> ProductBrand
            )//need to talk generic repo to get its generic getall func
        {
            _productRepository = ProductRepository;
            _mapper = mapper;

            _productType = ProductType;
            _productBrand = ProductBrand;
        }



        #region Get all Product

        //[Authorize(AuthenticationSchemes = "Bearer" )]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme ,Roles ="")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme ,Roles ="TeamLeader")]
        //[EnableCors("AllowReact")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts(string? Sort,int? BrandId ,int? TypeId)
        {
           var Products= await _productRepository.GetAllAsync(null,null, null,null);

            var ProductsDTO = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(Products);
            return Ok(ProductsDTO);

        }


        #endregion

        #region Get Product By id

        
        [HttpGet("{id}")]//{to make it variable}
        [ProducesResponseType(typeof(ProductToReturnDto), 200)]//those for improve  swagger documentations to include model if error 
        //[ProducesResponseType(typeof( ApiResponse),404)]//those for improve  swagger documentations to include model if  error 
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]

        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var Product = await _productRepository.GetByIdAsync(id);

            if (Product is null)
                return NotFound(new ApiResponse(404));

            var ProductDTO = _mapper.Map<Product, ProductToReturnDto>(Product);

            return Ok(ProductDTO);
        }

        #endregion


        #region Get ALL Types


        [HttpGet("Types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
        {
            var Types=  await _productType.GetAllAsync(null,null, null,null);
            return Ok(Types);
        }

        #endregion

        #region Get ALL Brands

        [HttpGet("Brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var Brands = await _productBrand.GetAllAsync(null,null, null,null);
            return Ok(Brands);
        }

        #endregion




    }
}

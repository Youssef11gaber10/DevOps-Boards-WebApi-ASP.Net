using Talabat.Core.Entities;

namespace Talabat.APIs.DTOs
{
    public class ProductToReturnDto
    {
        public int Id { get; set; }//added he will map it cause he inhert it
        public string Name { get; set; }

        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }



        public int ProductBrandId { get; set; }
        public string ProductBrand { get; set; }//need to map manualy from type of model ProductBrand to string ProductBrand.name


        public int ProductTypeId { get; set; }
        public string ProductType { get; set; }

    }
}

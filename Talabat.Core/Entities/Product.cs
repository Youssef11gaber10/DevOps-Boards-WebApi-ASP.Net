using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Core.Entities
{
    public class Product : BaseEntity
    {
        //here i didn't add any data annotation even it relate with db any thing you want do it with fluentApi
        public string Name { get; set; }

        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }



        public int ProductBrandId { get; set; }
        public ProductBrand ProductBrand { get; set; }//navigational property =>one
        //must add [fk] in fluentApi if i Change name of attr
        //must add [inverseProperty] in fluent api cause there are many relation in same class


        public int ProductTypeId { get; set; }
        public  ProductType ProductType{ get; set; }//navigational property =>one


        //public string UsersId { get; set; }
        //public Users Users { get; set; }




    }
}

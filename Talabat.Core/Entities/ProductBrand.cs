using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities
{
    public class ProductBrand : BaseEntity
    {
        //here i didn't add any data annotation even it relate with db any thing you want do it with fluentApi
        public string Name { get; set; }

        
        //public ICollection<Product> products { get; set; }//navigational property =>many
        //if i don't need to represent all products in ProductBrand don't do it .
        //the relation will be one to one 
        // do it with fluent Api 

    }
}

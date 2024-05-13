using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Repository.Data.Configrations;

namespace Talabat.Repository.Data
{
    public static class StoreContextSeed
    {
        //Seeding
        public static async Task SeedAsync(StoreContext dbContext)//this class is static so its ctor not visited // so ask for dbContext obj at level of function not whole class 
        {

            //must insert data of brands and types first casue Products has fk depends on it => tree of insert
            #region Seed BRANDS 

            if (!dbContext.ProductBrands.Any())//if the table don't have any data// not each time to run seed the data
            {
                var BrandsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");//seriallize => make it string
                var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandsData);//de seriallise => return to json file

                //if (Brands is not null && Brands.Count>0)//instead of use 2 conditions
                if (Brands?.Count > 0)// if it null will but false and not get count and if not null will check its count
                {
                    foreach (var Brand in Brands)

                        await dbContext.ProductBrands.AddAsync(Brand);
                    //await dbContext.Set<ProductBrand>().AddAsync(Brand);

                    //after finishes foreach
                    await dbContext.SaveChangesAsync();
                }

            }

            #endregion


            #region Seed Types

            if (!dbContext.ProductTypes.Any())//if the table don't have any data// not each time to run seed the data
            {
                var TypesData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/types.json");
                var Types = JsonSerializer.Deserialize<List<ProductType>>(TypesData);

                if (Types?.Count > 0)
                {
                    foreach (var Type in Types)
                        await dbContext.ProductTypes.AddAsync(Type);
                    await dbContext.SaveChangesAsync();
                }

            }
            #endregion


            #region Seed Products

            if (!dbContext.Products.Any())//if the table don't have any data// not each time to run seed the data
            {
                var ProductsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");
                var Products = JsonSerializer.Deserialize<List<Product>>(ProductsData);

                if (Products?.Count > 0)
                {
                    foreach (var product in Products)
                        await dbContext.Products.AddAsync(product);
                    await dbContext.SaveChangesAsync();
                }

            }
            #endregion




        }


    }
}

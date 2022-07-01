using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context, ILoggerFactory loggerFactory){
            try
            {
                if(!context.ProductBrands!.Any()){
                    var brandsData = File.ReadAllText("../Infrastructure/Data/SeedData/brands.json");
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                    brands!.ForEach(item =>
                    {
                        context.ProductBrands!.Add(item);
                    });

                    await context.SaveChangesAsync();
                }

                if(!context.ProductTypes!.Any()){
                    var typesData = File.ReadAllText("../Infrastructure/Data/SeedData/types.json");
                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

                    types!.ForEach(item =>
                    {
                        context.ProductTypes!.Add(item);
                    });

                    await context.SaveChangesAsync();
                }

                if(!context.Products!.Any()){
                    var productsData = File.ReadAllText("../Infrastructure/Data/SeedData/products.json");
                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                    products!.ForEach(item =>
                    {
                        context.Products!.Add(item);
                    });

                    await context.SaveChangesAsync();
                } 
                
                if(!context.DeliveryMethods!.Any()){
                    var deliverMethodsData = File.ReadAllText("../Infrastructure/Data/SeedData/delivery.json");
                    var deliverMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliverMethodsData);

                    deliverMethods!.ForEach(item =>
                    {
                        context.DeliveryMethods!.Add(item);
                    });

                    await context.SaveChangesAsync();
                }
            }
            catch(Exception ex){
                var logger = loggerFactory.CreateLogger<StoreContextSeed>();
                logger.LogError(ex.Message);
            }
        }
    }
}
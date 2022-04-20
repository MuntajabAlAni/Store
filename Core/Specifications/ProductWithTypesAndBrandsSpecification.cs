using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductWithTypesAndBrandsSpecification : BaseSpecification<Product>
    {
        public ProductWithTypesAndBrandsSpecification(ProductSpecificationParameters productParameters)
        :base(x => 
         (string.IsNullOrEmpty(productParameters.Search) || x.Name!.ToLower().Contains(productParameters.Search)) &&
         (!productParameters.BrandId.HasValue || x.ProductBrandId == productParameters.BrandId) &&
         (!productParameters.TypeId.HasValue || x.ProductTypeId == productParameters.TypeId))
        {
            AddInclude(x => x.productType!);
            AddInclude(x => x.productBrand!);
            AddOrderBy(p => p.Name!);
            ApplyPaging(productParameters.PageSize * (productParameters.PageIndex - 1), productParameters.PageSize);

            if(!string.IsNullOrEmpty(productParameters.Sort)){
                switch (productParameters.Sort){
                    case "priceAsc":
                    AddOrderBy(p => p.Price);
                    break; 
                    case "priceDesc":
                    AddOrderByDescending(p => p.Price);
                    break; 
                    default:
                    AddOrderBy(p => p.Name!);
                    break; 
                }
            }
        }

        public ProductWithTypesAndBrandsSpecification(int id) : base(p => p.Id == id)
        {
            AddInclude(x => x.productType!);
            AddInclude(x => x.productBrand!);
        }
    }
}
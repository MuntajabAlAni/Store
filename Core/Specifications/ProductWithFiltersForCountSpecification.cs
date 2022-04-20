using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductWithFiltersForCountSpecification : BaseSpecification<Product>
    {
        public ProductWithFiltersForCountSpecification(ProductSpecificationParameters productParameters)
        : base(x =>
          (string.IsNullOrEmpty(productParameters.Search) || x.Name!.ToLower().Contains(productParameters.Search)) &&
          (!productParameters.BrandId.HasValue || x.ProductBrandId == productParameters.BrandId) &&
          (!productParameters.TypeId.HasValue || x.ProductTypeId == productParameters.TypeId))
        {
        }
    }
}
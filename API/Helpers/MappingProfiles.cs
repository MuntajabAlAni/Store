using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
            .ForMember(dto => dto.productBrand, o => o.MapFrom(s => s.productBrand!.Name))
            .ForMember(dto => dto.productType, o => o.MapFrom(s => s.productType!.Name))
            .ForMember(dto => dto.PictureUrl, o => o.MapFrom<ProductUrlResolver>());
        }
    }
}
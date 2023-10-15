using AutoMapper;
using BarcodeService.Application.Dtos;
using BarcodeService.Application.Models;
using BarcodeService.Domain.Entities;

namespace BarcodeService.Application.Mappings;

public class ProductsMappingProfile : Profile
    {
        public ProductsMappingProfile()
        {
            CreateMap<OpenApiResponse, Product>()
                .ForMember(m => m.Name, c => c.MapFrom(s => s.product.product_name))
                .ForMember(m => m.Ean, c => c.MapFrom(s => s.code));
            CreateMap<Product, ProductDto>();
            CreateMap<GroupedPotentialProduct, Product>();
            CreateMap<Product, GroupedPotentialProduct>();
        }
    }



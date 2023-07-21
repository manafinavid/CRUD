using System.Reflection;
using AutoMapper;
using CRUD.Application.Products.Commands.CreateProduct;
using CRUD.Application.Products.Commands.UpdateProduct;
using CRUD.Application.Users.Commands.SignUp;
using CRUD.Domain.Entities;

namespace CRUD.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        ApplyMappings();
    }

    private void ApplyMappings()
    {
        CreateMap<CreateProductCommand, Product>();

        CreateMap<UpdateProductCommand, Product>()
            .ReverseMap();
    }
}

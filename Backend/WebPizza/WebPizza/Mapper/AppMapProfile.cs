﻿using AutoMapper;
using WebPizza.Data.Entities;
using WebPizza.ViewModel.Category;
using WebPizza.ViewModel.Pizza;
using WebPizza.ViewModels.Category;
using WebPizza.ViewModels.Ingredient;
using WebPizza.ViewModels.Pizza;
using WebPizza.ViewModels.PizzaSizes;
using WebPizza.ViewModels.Sizes;

namespace WebPizza.Mapper;
public class AppMapProfile : Profile
{
    public AppMapProfile()
    {
        CreateMap<CategoryCreateVM, CategoryEntity>()
            .ForMember(c => c.Image, opt => opt.Ignore());

        CreateMap<CategoryEntity, CategoryVm>();

        // Ingredient
        CreateMap<IngredientCreateVm, IngredientEntity>()
            .ForMember(c => c.Image, opt => opt.Ignore());

        CreateMap<IngredientEntity, IngredientVm>();
        CreateMap<IngredientVm, IngredientEntity>();

        CreateMap<PizzaIngredientEntity, IngredientVm>()
              .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Ingredient.Id))
              .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Ingredient.Name))
              .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Ingredient.Image));


        // Pizza
        CreateMap<PizzaEntity, PizzaVm>();

        CreateMap<PizzaSizePriceEntity, PizzaSizePriceVm>()
               .ForMember(dest => dest.SizeName, opt => opt.MapFrom(src => src.Size.Name));

        CreateMap<PizzaPhotoEntity, PizzaPhotoVm>();

        CreateMap<PizzaCreateVm, PizzaEntity>()
            .ForMember(c => c.IsAvailable, opt => opt.Ignore())
            .ForMember(c => c.Rating, opt => opt.Ignore())
            .ForMember(c => c.Photos, opt => opt.Ignore())
            .ForMember(c => c.Sizes, opt => opt.Ignore());

        // Sizes
        CreateMap<PizzaSizeEntity, SizeVm>();
    }

}

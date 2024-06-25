﻿using AutoMapper;
using WebPizza.Data.Entities;
using WebPizza.Data;
using WebPizza.Interfaces;
using WebPizza.Services.ControllerServices.Interfaces;
using WebPizza.ViewModels.Category;
using Microsoft.EntityFrameworkCore;

namespace WebPizza.Services.ControllerServices;

public class CategoryControllerService(
PizzaDbContext pizzaContext,
IMapper mapper,
IImageService imageService
) : ICategoryControllerService
{
    public async Task CreateAsync(CategoryCreateVM vm)
    {
        var category = mapper.Map<CategoryEntity>(vm);

        try
        {
            category.Image = await imageService.SaveImageAsync(vm.Image);
            //category.DateCreated = DateTime.UtcNow;

            await pizzaContext.Categories.AddAsync(category);
            await pizzaContext.SaveChangesAsync();
        }
        catch (Exception)
        {
            imageService.DeleteImageIfExists(category.Image);
            throw new Exception("Error category created");
        }
    }

    public async Task DeleteIfExistsAsync(int id)
    {
        var category = await pizzaContext.Categories.FirstOrDefaultAsync(c => c.Id == id);

        try
        {
            if (category == null)
            {
                throw new Exception("Category not found");
            }

            imageService.DeleteImageIfExists(category.Image);
            pizzaContext.Categories.Remove(category);
            await pizzaContext.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task UpdateAsync(CategoryEditVm vm)
    {
        var category = await pizzaContext.Categories.FirstOrDefaultAsync(c => c.Id == vm.Id);

        try
        {
            if (category == null)
            {
                throw new Exception("Category not found");
            }

            var oldImage = category.Image;

            if (vm.Name != null)
            {
                category.Name = vm.Name;
            }

            if (vm.Image != null)
            {
                category.Image = await imageService.SaveImageAsync(vm.Image);
                imageService.DeleteImageIfExists(oldImage);
            }

            pizzaContext.Categories.Update(category);
            await pizzaContext.SaveChangesAsync();

        }
        catch (Exception)
        {
            imageService.DeleteImageIfExists(category.Image);
            throw;
        }
    }
}

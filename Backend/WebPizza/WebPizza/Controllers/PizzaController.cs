﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebPizza.Data;
using WebPizza.Interfaces;
using WebPizza.Services.ControllerServices.Interfaces;
using WebPizza.ViewModel.Pizza;
using WebPizza.ViewModels.Pizza;

namespace WebPizza.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class PizzaController(IMapper mapper,
        IValidator<PizzaCreateVm> createValidator,
        IPizzaControllerService service,
        IPaginationService<PizzaVm, PizzaFilterVm> pagination,
        PizzaDbContext pizzaContext
    ) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var list = await pizzaContext.Pizzas
               //.Include(x => x.Photos)
               .ProjectTo<PizzaVm>(mapper.ConfigurationProvider)
               .ToArrayAsync();

            return Ok(list);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetPage([FromQuery] PizzaFilterVm vm)
    {
        try
        {
            return Ok(await pagination.GetPageAsync(vm));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var pizza = await pizzaContext.Pizzas
            .ProjectTo<PizzaVm>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (pizza is null)
            return NotFound();

        return Ok(pizza);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] PizzaCreateVm vm)
    {

        var validationResult = await createValidator.ValidateAsync(vm);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        try
        {
            await service.CreateAsync(vm);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await service.DeleteIfExistsAsync(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

}

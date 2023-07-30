using CRUD.Application.Common.Interfaces;
using CRUD.Application.Products.Commands.CreateProduct;
using CRUD.Application.Products.Commands.DeleteProduct;
using CRUD.Application.Products.Commands.GetProduct;
using CRUD.Application.Products.Commands.UpdateProduct;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRUD.WebApi.Controllers;

[Authorize]
[Route("/")]
[ApiController]
public class ProductsController : Controller
{
    readonly IMediator Mediator;
    readonly IApplicationDbContext DbContext;
    public ProductsController(IMediator mediator, IApplicationDbContext dbContext)
    {
        Mediator = mediator;
        DbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductViewModel>>> Index([FromQuery] GetProductCommand command)
    {
        return await Mediator.Send(command);
    }


    [HttpGet]
    [Route("/Create")]
    public async Task<ActionResult<Guid>> Create([FromQuery] CreateProductCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpGet]
    [Route("/Update")]
    public async Task<ActionResult<Guid>> Update([FromQuery] UpdateProductCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpGet]
    [Route("/Delete")]
    public async Task<IActionResult> Delete(string id)
    {
        await Mediator.Send(new DeleteProductCommand()
        {
            Id = Guid.Parse(id)
        });

        return NoContent();
    }
}

using CRUD.Application.Products.Commands.CreateProduct;
using CRUD.Application.Products.Commands.DeleteProduct;
using CRUD.Application.Products.Commands.UpdateProduct;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRUD.WebApi.Controllers;

[Authorize]
[Route("/Products")]
// [Authorize(AuthenticationSchemes = "Bearer")]
public class ProductsController : Controller
{
    readonly IMediator Mediator;
    public ProductsController(IMediator mediator)
    {
        Mediator = mediator;
    }

    [Route("/Products/Index")]
    public ActionResult Index()
        => View();

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateProductCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(UpdateProductCommand command)
    {
        await Mediator.Send(command);

        return NoContent();
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Delete(string id)
    {
        await Mediator.Send(new DeleteProductCommand()
        {
            Id = Guid.Parse(id)
        });

        return NoContent();
    }
}

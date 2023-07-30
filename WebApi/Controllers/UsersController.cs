using CRUD.Application.Products.Commands.CreateProduct;
using CRUD.Application.Users.Commands.SignUp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using CRUD.Application.Users.Commands.Login;

namespace CRUD.WebApi.Controllers;

[ApiController]
[AllowAnonymous]
public class UsersController : ControllerBase
{
    readonly IMediator Mediator;
    public UsersController(IMediator mediator)
    {
        Mediator = mediator;
    }

    [HttpPost]
    [Route("/Register")]
    public async Task<ActionResult<string>> Register(SignUpCommand command)
    {
        return await Mediator.Send(command);
    }


    [HttpGet]
    [Route("/SignInTest")]
    public async Task<IActionResult> SignInTest()
        => await SignIn(new()
        {
            UserName = "Helena",
            Password = "Helena@000",
        });

    [HttpPost]
    [Route("/SignIn")]
    public async Task<ActionResult> SignIn(LoginCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }

    [HttpGet]
    [Route("/SignOut")]
    public override SignOutResult SignOut()
    {
        return base.SignOut();
    }
}

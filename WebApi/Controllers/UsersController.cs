using CRUD.Application.Products.Commands.CreateProduct;
using CRUD.Application.Users.Commands.SignUp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using CRUD.Application.Users.Commands.Login;

namespace CRUD.WebApi.Controllers;

[Route("/")]
public class UsersController : Controller
{
    readonly IMediator Mediator;
    public UsersController(IMediator mediator)
    {
        Mediator = mediator;
    }

    [HttpGet]
    public IActionResult Register()
        => View();

    [HttpGet]
    [Route("/RegisterTest")]
    public async Task<IActionResult> RegisterTest()
        => await Register(new()
        {
            Email = "Helena@ir.com",
            UserName = "Helena",
            Password = "Helena@000",
            ConfrimPassword = "Helena@000",
        });



    [HttpPost]
    public async Task<IActionResult> Register(SignUpCommand command)
    {
        return Json(await Mediator.Send(command));
    }


    [HttpGet]
    public IActionResult SignIn()
        => View();


    [HttpGet]
    [Route("/SignInTest")]
    public async Task<IActionResult> SignInTest()
        => await SignIn(new()
        {
            UserName = "Helena",
            Password = "Helena@000",
        });

    [HttpPost]
    public async Task<IActionResult> SignIn(LoginCommand command)
    {
        await Mediator.Send(command);
        //HttpContext.Session.Clear();

        return RedirectToAction("Index", "Products");
    }

    [HttpGet]
    public override SignOutResult SignOut()
    {
        return base.SignOut();
    }
}

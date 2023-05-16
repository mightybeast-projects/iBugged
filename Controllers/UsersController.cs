using iBugged.Models;
using iBugged.Services;
using Microsoft.AspNetCore.Mvc;

namespace iBugged.Controllers;

public class UsersController : ControllerBase
{
    private readonly UsersService usersService;

    public UsersController(UsersService usersService) =>
        this.usersService = usersService;

    [HttpGet]
    public IActionResult List() => View(nameof(List), usersService.GetAll());

    [HttpGet]
    public IActionResult Edit(string id) =>
        PartialView(nameof(Edit), usersService.Get(id));

    [HttpGet]
    public IActionResult Delete(string id)
    {
        usersService.DeleteUser(id);

        return RedirectToAction(nameof(List));
    }

    [HttpPost]
    public IActionResult List(string searchString)
    {
        ViewData["SearchString"] = searchString;
    
        return View(nameof(List), usersService.GetAll(searchString));
    }

    [HttpPost]
    public IActionResult Edit(User user)
    {
        usersService.Edit(user.id, user);

        return RedirectToAction(nameof(List));
    }
}
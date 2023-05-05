using iBugged.Models;
using iBugged.Services;
using Microsoft.AspNetCore.Mvc;

namespace iBugged.Controllers;

public class UsersController : Controller
{
    private readonly UsersService usersService;

    public UsersController(UsersService usersService) =>
        this.usersService = usersService;

    [HttpGet]
    public IActionResult List() =>
        View(nameof(List), usersService.GetAll());

    [HttpGet]
    public IActionResult Edit(string id) =>
        View(nameof(Edit), usersService.Get(u => u.id == id));

    [HttpGet]
    public IActionResult Delete(string id)
    {
        usersService.DeleteUser(id);
        
        return RedirectToAction(nameof(List));
    }

    [HttpPost]
    public IActionResult Edit(User user)
    {
        usersService.Edit(user.id, user);

        return RedirectToAction(nameof(List));
    }
}
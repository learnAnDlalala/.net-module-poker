using Microsoft.AspNetCore.Mvc;
using ScrumPoker.Data;
using ScrumPoker.Data.Models;
using ScrumPoker.Services;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;


namespace ScrumPoker.Controllers
{
  /// <summary>
  /// Контроллер пользователей.
  /// </summary>
  [ApiController]
  [Route("api/[controller]")]
  public class UserController : Controller
  {
    /// <summary>
    /// Сервис пользователей.
    /// </summary>
    public readonly UserService userService;

    /// <summary>
    /// Конструктор пользователей.
    /// </summary>
    /// <param name="userService">сервис пользователей.</param>
    public UserController(UserService userService)
    {
      this.userService = userService;
    }

    /// <summary>
    /// Запрос на полученя списка пользователей.
    /// </summary>
    /// <returns>Список пользователей.</returns>
    [HttpGet]
    public async Task<ActionResult<List<User>>> GetUsersList()
    {
      return await this.userService.ShowAll();
    }

    /// <summary>
    /// Запрос на вход в учетную запись.
    /// </summary>
    /// <param name="user">инстанс класса пользователь.</param>
    /// <returns>true / false.</returns>
    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(User user)
    {
      return await this.userService.CheckRegistration(user);
    }

    [HttpPost("auf")]
    public async Task<User> CreateUserAndAuth(User user)
    {
      if (await userService.UserExists( user.Name) == false)
      {
        var newUser = await userService.Create(user);
        var mainClaim = new List<Claim>()
        {
          new Claim(ClaimTypes.Name, newUser.Name)
      };
        var idenity = new ClaimsIdentity(mainClaim, "scrum idenity");
        var principle = new ClaimsPrincipal(idenity);
        await HttpContext.SignInAsync("CookieAuth", principle, new AuthenticationProperties
        {
          IsPersistent = true
        });
        return newUser;
      }
      return user;
    }
  }
}



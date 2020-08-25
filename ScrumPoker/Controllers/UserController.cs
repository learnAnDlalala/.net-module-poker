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
    /// Контекст бд.
    /// </summary>
    public readonly ModelContext db;
    /// <summary>
    /// Сервис пользователей.
    /// </summary>
    public readonly UserService userService;

    /// <summary>
    /// Конструктор пользователей.
    /// </summary>
    /// <param name="db">контекст бд.</param>
    /// <param name="userService">сервис пользователей.</param>
    public UserController(ModelContext db, UserService userService)
    {
      this.db = db;
      this.userService = userService;
    }

    /// <summary>
    /// Запрос на создания нового пользователя.
    /// </summary>
    /// <param name="newUser">инстанс класса пользователь.</param>
    /// <returns>id пользователя.</returns>
    //[HttpPost("auf")]

    //public async Task Post(User newUser)
    //{
    //  var test = await userService.UserExists(db, newUser.Name);
    //  if (!test)
    //  {
    //    var user = await userService.Create(db, newUser);
    //    var claim = new List<Claim>
    //    {
    //      new Claim(ClaimTypes.Name, user.Name)
    //    };
    //    var identity = new ClaimsIdentity(
    //    claim, CookieAuthenticationDefaults.AuthenticationScheme);
    //    var principle = new ClaimsPrincipal(identity);
    //    var props = new AuthenticationProperties();
    //    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principle, props);
    //  };

    //}

    /// <summary>
    /// Запрос на полученя списка пользователей.
    /// </summary>
    /// <returns>Список пользователей.</returns>
    [HttpGet]
    public async Task<ActionResult<List<User>>> GetUsersList()
    {
      return await this.userService.ShowAll(this.db);
    }

    /// <summary>
    /// Запрос на вход в учетную запись.
    /// </summary>
    /// <param name="user">инстанс класса пользователь.</param>
    /// <returns>true / false.</returns>
    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(User user)
    {
      return await this.userService.CheckRegistration(this.db, user);
    }

    [HttpPost("auf")]
    public async Task<User> CreateUserAndAuth(User user)
    {
      if (await userService.UserExists(this.db, user.Name) == false)
      {
        var newUser = await userService.Create(this.db, user);
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



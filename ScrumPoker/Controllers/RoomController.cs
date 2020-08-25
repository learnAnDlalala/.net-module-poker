using Microsoft.AspNetCore.Mvc;
using ScrumPoker.Data;
using ScrumPoker.Data.Models;
using ScrumPoker.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScrumPoker.Controllers
{
  /// <summary>
  /// Контроллер комнат.
  /// </summary>
  [ApiController]
  [Route("api/[controller]")]
  public class RoomController : Controller
  {
    /// <summary>
    /// контекст бд.
    /// </summary>
    private readonly ModelContext db;

    /// <summary>
    /// сервис комнат.
    /// </summary>
    private readonly RoomService roomService;

    /// <summary>
    /// Конструктор комнат.
    /// </summary>
    /// <param name="context">контекст бд.</param>
    /// <param name="roomService">сервис комнат.</param>
    public RoomController(ModelContext context, RoomService roomService)
    {
      this.db = context;
      this.roomService = roomService;
    }

    /// <summary>
    /// Запрос на создание новой комнаты.
    /// </summary>
    /// <param name="newRoom">инстанс класса комнаты.</param>
    /// <returns>ничего не возвращает</returns>
    [HttpPost]
    public async Task<IActionResult> Post(Room newRoom)
    {
      var id = await this.roomService.Create(this.db, newRoom);
      return new OkObjectResult(id);
    }

    /// <summary>
    /// Запрос на получения список комнат.
    /// </summary>
    /// <returns>список комнат.</returns>
    [HttpGet]
    public async Task<ActionResult<List<Room>>> Get()
    {
      return await this.roomService.ShowAll(this.db);
    }

    /// <summary>
    /// Запрос на вход в комнату.
    /// </summary>
    /// <param name="user">инстанс класса пользователей.</param>
    /// <param name="id">id комнаты.</param>
    /// <returns>ничего не возвращает.</returns>
    [HttpPost("{id}")]
    public async Task<Room> Enter(User user ,int id)
    {
      return await this.roomService.Enter(this.db, user.ID, id);
    }

    /// <summary>
    /// Запрос на получения списка пользователей в комнате.
    /// </summary>
    /// <param name="id">id комнаты</param>
    /// <returns>список пользователей.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<List<User>>> Info(int id)
    {
      return await this.roomService.ShowUsers(this.db, id);
    }

    /// <summary>
    /// Запрос на выход из комнаты.
    /// </summary>
    /// <param name="user">id пользователя</param>
    /// <param name="id">id комнаты</param>
    /// <returns>ничего не возвращает.</returns>
    [HttpPost("{id}/exit")]
    public async Task Exit(int user, int id)
    {
       await this.roomService.Exit(this.db, user, id);
     
    }

    /// <summary>
    /// Запрос на удаления пользователя из комнаты.
    /// </summary>
    /// <param name="id">id пользователя.</param>
    /// <param name="room">id комнаты.</param>
    /// <returns>ничего не возвращает.</returns>
    [HttpDelete("{id}")]
    public async Task Delete(int id, int room)
    {
      await this.roomService.Delete(this.db, id, room);
    }
  }
}

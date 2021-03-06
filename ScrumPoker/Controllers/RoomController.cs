﻿using Microsoft.AspNetCore.Mvc;
using ScrumPoker.DataService.Models;
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
    /// сервис комнат.
    /// </summary>
    private readonly RoomService roomService;

    /// <summary>
    /// Конструктор комнат.
    /// </summary>
    /// <param name="roomService">сервис комнат.</param>
    public RoomController( RoomService roomService)
    {
      this.roomService = roomService;
    }

    /// <summary>
    /// Запрос на создание новой комнаты.
    /// </summary>
    /// <param name="newRoom">инстанс класса комнаты.</param>
    /// <returns>ничего не возвращает</returns>
    [HttpPost]
    public async Task<IActionResult> CreateRoom(Room newRoom)
    {
      var id = await this.roomService.Create(newRoom);
      return new OkObjectResult(id);
    }

    /// <summary>
    /// Запрос на получения список комнат.
    /// </summary>
    /// <returns>список комнат.</returns>
    [HttpGet]
    public async Task<ActionResult<List<Room>>> GetRoomsList()
    {
      return await this.roomService.ShowAll();
    }

    /// <summary>
    /// Запрос на вход в комнату.
    /// </summary>
    /// <param name="user">инстанс класса пользователей.</param>
    /// <param name="id">id комнаты.</param>
    /// <returns>ничего не возвращает.</returns>
    [HttpPost("{id}")]
    public async Task<Room> EnterInRoomAndGetRoomInfo(User user ,int id)
    {
      return await this.roomService.Enter(user.ID, id);
    }

    /// <summary>
    /// Запрос на получения списка пользователей в комнате.
    /// </summary>
    /// <param name="id">id комнаты</param>
    /// <returns>список пользователей.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<List<User>>> GetUsersInRoom(int id)
    {
      return await this.roomService.ShowUsers(id);
    }

    /// <summary>
    /// Запрос на выход из комнаты.
    /// </summary>
    /// <param name="user">id пользователя</param>
    /// <param name="id">id комнаты</param>
    /// <returns>ничего не возвращает.</returns>
    [HttpPost("{id}/exit")]
    public async Task ExitFromRoom(int user, int id)
    {
       await this.roomService.ExitFromRoom(user, id);
     
    }

    /// <summary>
    /// Запрос на удаления пользователя из комнаты.
    /// </summary>
    /// <param name="id">id пользователя.</param>
    /// <param name="room">id комнаты.</param>
    /// <returns>ничего не возвращает.</returns>
    [HttpDelete("{id}")]
    public async Task DeleteRoom(int id, int room)
    {
      await this.roomService.DeleteUser(id, room);
    }
  }
}

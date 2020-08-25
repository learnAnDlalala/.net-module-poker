using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ScrumPoker.Data;
using ScrumPoker.Data.Models;
using ScrumPoker.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumPoker.Services
{
  /// <summary>
  /// Класс сервиса комнаты.
  /// </summary>
  public class RoomService
  {
    /// <summary>
    /// контекст Хаба комнаты.
    /// </summary>
    private IHubContext<RoomsHub> ctx;

    /// <summary>
    /// Сервис пользователей.
    /// </summary>
    private UserService userService;

    /// <summary>
    /// Конструктор класса.
    /// </summary>
    /// <param name="context">контекст хаба.</param>
    /// <param name="userService">сервис пользователей.</param>
    public RoomService(IHubContext<RoomsHub> context, UserService userService)
    {
      this.ctx = context;
      this.userService = userService;
    }

    /// <summary>
    /// Создание комнаты.
    /// </summary>
    /// <param name="db">контекст бд.</param>
    /// <param name="newRoom">инстанс класса комнаты.</param>
    /// <returns></returns>
    public async Task<int> Create(ModelContext db, Room newRoom)
    {
      //if (await this.RoomExists(db, newRoom.Name))
      //{
      //  return null;
      //}

      var entity = await db.Rooms.AddAsync(newRoom);
      //room.Users.Add(await db.Users.FindAsync(newRoom.OwnerID));
      //db.Rooms.Add(newRoom);
      await db.SaveChangesAsync();
      return entity.Entity.ID;
    }

    /// <summary>
    /// Показать все комнаты.
    /// </summary>
    /// <param name="db">контекст бд.</param>
    /// <returns>Список комнат.</returns>
    public async Task<ActionResult<List<Room>>> ShowAll(ModelContext db)
    {
      return await db.Rooms.Include(d => d.Users).Include(d=>d.Rounds).ToListAsync();
    }

    /// <summary>
    /// Вход в комнату.
    /// </summary>
    /// <param name="db">контекст бд.</param>
    /// <param name="userId">id пользователя.</param>
    /// <param name="roomId">id  комнаты/</param>
    /// <returns>ничего не возвращает.</returns>
    public async Task<Room> Enter(ModelContext db, int userId, int roomId)
    {

      var room = await db.Rooms.Include(t => t.Users).Include(t => t.Rounds).FirstOrDefaultAsync(t => t.ID == roomId);
      var user = await db.Users.FindAsync(userId);
      var connectinID = this.userService.FindConnectionID(user.Name);
      if (!room.Users.Contains(user))
      {
        room.Users.Add(user);
      }
      await db.SaveChangesAsync();
      await this.ctx.Groups.RemoveFromGroupAsync(connectinID, $"room={roomId}");
      await this.ctx.Groups.AddToGroupAsync(connectinID, $"room={roomId}");
      await this.ctx.Clients.Group($"room={roomId}").SendAsync("UpdateUsersList", room);
      return room;
      //return await db.Rooms.Include(t => t.Users).Include(t => t.Rounds).FirstOrDefaultAsync(t=>t.ID == roomId);
    }

    /// <summary>
    /// Удаление пользователя.
    /// </summary>
    /// <param name="db">контекст бд.</param>
    /// <param name="userId">id пользователя.</param>
    /// <param name="roomId">id комнаты.</param>
    /// <returns>ничего не возвращает.</returns>
    public async Task Delete(ModelContext db, int userId, int roomId)
    {
      var room = await db.Rooms.FindAsync(roomId);
      var user = await db.Users.FindAsync(userId);
      var connectinID = this.userService.FindConnectionID(user.Name);
      room.Users.Remove(user);
      await db.SaveChangesAsync();
      await this.ctx.Groups.RemoveFromGroupAsync(connectinID, $"room={roomId}");
      await this.ctx.Clients.Group($"room={roomId}").SendAsync("UpdateUsersList");
    }

    /// <summary>
    /// Выход из комнаты.
    /// </summary>
    /// <param name="db">контекст бд.</param>
    /// <param name="userId">id пользователя.</param>
    /// <param name="roomId">id комнаты.</param>
    /// <returns>ничего не возвращает.</returns>
    public async Task Exit(ModelContext db, int userId, int roomId)
    {
      await this.Delete(db, userId, roomId);
    }

    /// <summary>
    /// Показать пользователей.
    /// </summary>
    /// <param name="db">контекст бд.</param>
    /// <param name="id">id комнаты.</param>
    /// <returns>список пользователей в комнате.</returns>
    public async Task<List<User>> ShowUsers(ModelContext db, int id)
    {
      return await db.Users.Where(t => t.Room.ID == id).ToListAsync();
    }

    /// <summary>
    /// Наличие комнаты в бд.
    /// </summary>
    /// <param name="db">контекст бд.</param>
    /// <param name="name">имя комнаты.</param>
    /// <returns></returns>
    public async Task<bool> RoomExists(ModelContext db, string name)
    {
      return await db.Rooms.AnyAsync(e => e.Name == name);
    }
  }
}
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

    private ModelContext db;

    /// <summary>
    /// Конструктор класса.
    /// </summary>
    /// <param name="context">контекст хаба.</param>
    /// <param name="userService">сервис пользователей.</param>
    public RoomService(IHubContext<RoomsHub> context, UserService userService, ModelContext dbContex)
    {
      this.ctx = context;
      this.userService = userService;
      this.db = dbContex;
    }

    /// <summary>
    /// Создание комнаты.
    /// </summary>
    /// <param name="newRoom">инстанс класса комнаты.</param>
    /// <returns>возвращает инстанс комнаты.</returns>
    public async Task<int> Create(Room newRoom)
    {
      //if (await this.RoomExists(db, newRoom.Name))
      //{
      //  return null;
      //}

      var entity = await db.Rooms.AddAsync(newRoom);
      //room.Users.Add(await db.Users.FindAsync(newRoom.OwnerID));
      //db.Rooms.Add(newRoom);
      await this.db.SaveChangesAsync();
      return entity.Entity.ID;
    }

    /// <summary>
    /// Показать все комнаты.
    /// </summary>ram>
    /// <returns>Список комнат.</returns>
    public async Task<List<Room>> ShowAll()
    {
      return await this.db.Rooms.Include(d => d.Users).Include(d=>d.Rounds).ToListAsync();
    }

    /// <summary>
    /// Вход в комнату.
    /// </summary>
    /// <param name="userId">id пользователя.</param>
    /// <param name="roomId">id  комнаты/</param>
    /// <returns>ничего не возвращает.</returns>
    public async Task<Room> Enter(int userId, int roomId)
    {

      var room = await this.db.Rooms.Include(t => t.Users).Include(t => t.Rounds).FirstOrDefaultAsync(t => t.ID == roomId);
      var user = await this.db.Users.FindAsync(userId);
      var connectinID = this.userService.FindConnectionID(user.Name);
      if (!room.Users.Contains(user))
      {
        room.Users.Add(user);
      }
      await this.db.SaveChangesAsync();
      await this.ctx.Groups.RemoveFromGroupAsync(connectinID, this.GetGroupKey(roomId));
      await this.ctx.Groups.AddToGroupAsync(connectinID, this.GetGroupKey(roomId));
      await this.ctx.Clients.Group(this.GetGroupKey(roomId)).SendAsync("UpdateUsersList", room);
      return room;
    }

    /// <summary>
    /// Удаление пользователя из комнаты (доступно только владельцу комнаты).
    /// </summary>
    /// <param name="userId">id пользователя.</param>
    /// <param name="roomId">id комнаты.</param>
    /// <returns>ничего не возвращает.</returns>
    public async Task DeleteUser(int userId, int roomId)
    {
      var room = await this.db.Rooms.FindAsync(roomId);
      var user = await this.db.Users.FindAsync(userId);
      var connectinID = this.userService.FindConnectionID(user.Name);
      room.Users.Remove(user);
      await this.db.SaveChangesAsync();
      await this.ctx.Groups.RemoveFromGroupAsync(connectinID, this.GetGroupKey(roomId));
      await this.ctx.Clients.Group(this.GetGroupKey(roomId)).SendAsync("UpdateUsersList");
    }

    /// <summary>
    /// Выход из комнаты (доступен всем).
    /// </summary>
    /// <param name="userId">id пользователя.</param>
    /// <param name="roomId">id комнаты.</param>
    /// <returns>ничего не возвращает.</returns>
    public async Task ExitFromRoom(int userId, int roomId)
    {
      await this.DeleteUser(userId, roomId);
    }

    /// <summary>
    /// Показать пользователей.
    /// </summary>
    /// <param name="id">id комнаты.</param>
    /// <returns>список пользователей в комнате.</returns>
    public async Task<List<User>> ShowUsers(int id)
    {
      return await this.db.Users.Where(t => t.Room.ID == id).ToListAsync();
    }

    /// <summary>
    /// Наличие комнаты в бд.
    /// </summary>
    /// <param name="name">имя комнаты.</param>
    /// <returns></returns>
    public async Task<bool> RoomExists(string name)
    {
      return await this.db.Rooms.AnyAsync(e => e.Name == name);
    }

    public string GetGroupKey (int id)
    {
      return $"roomId={id}";
    }
  }
}
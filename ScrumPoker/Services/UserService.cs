using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ScrumPoker.Data;
using ScrumPoker.Data.Models;
using ScrumPoker.SignalR;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumPoker.Services
{
  /// <summary>
  /// Класс сервиса пользователей.
  /// </summary>
  public class UserService
  {
    /// <summary>
    /// контекст Хаба комнаты.
    /// </summary>
    private IHubContext<RoomsHub> ctx;

    private ModelContext db;

    /// <summary>
    /// Список SignalRconnections.
    /// </summary>
    private readonly ConcurrentDictionary<string, string> usersConnections;

    /// <summary>
    /// Конструктор класса.
    /// </summary>
    /// <param name="context">контекст хаба.</param>
    public UserService(IHubContext<RoomsHub> context, ModelContext dbContext)
    {
      this.db = dbContext;
      this.ctx = context;
      this.usersConnections = new ConcurrentDictionary<string, string>();
    }

    /// <summary>
    /// Создание нового пользователя.
    /// </summary>
    /// <param name="newUser">инстанс класса пользователя.</param>
    /// <returns>возвращает id пользователя</returns>
    public async Task<User> Create(User newUser)
    {
      var entity = this.db.Users.Add(newUser);
      await this.db.SaveChangesAsync();
      return newUser;
    }

    /// <summary>
    /// Список пользователей.
    /// </summary>
    /// <returns>список пользователей.</returns>
    public async Task<ActionResult<List<User>>> ShowAll()
    {
      return await this.db.Users.ToListAsync();
    }

    /// <summary>
    /// Проверка наличия пользователя в бд.
    /// </summary>
    /// <param name="checkUser">инстанс класса пользователя.</param>
    /// <returns>Строку с результатом.</returns>
    public async Task<string> CheckRegistration(User checkUser)
    {
      if (await this.UserExists(checkUser.Name))
      {
        return "Success";
      }

      return "Fail";
    }

    /// <summary>
    /// Добавление SignalRconnection в список.
    /// </summary>
    /// <param name="user">имя пользователя.</param>
    /// <param name="id">SignalR id connection</param>
    public void AddUserToConnection(string user, string id)
    {
      if (this.usersConnections.ContainsKey(user))
      {
        string previousConnection;
        this.usersConnections.TryRemove(user, out previousConnection);
      }

      this.usersConnections.TryAdd(user, id);
    }

    /// <summary>
    /// Удаление SiglanRconnection из списка.
    /// </summary>
    /// <param name="identityName">имя пользователя</param>
    public void DeleteUserConnection(string identityName)
    {
      string previousConnection;
      this.usersConnections.TryRemove(identityName, out previousConnection);
    }

    /// <summary>
    /// Найти SignalRConnection в списке.
    /// </summary>
    /// <param name="name">имя пользователя.</param>
    /// <returns>SignalR id connection.</returns>
    public string FindConnectionID(string name)
    {
      return this.usersConnections
        .Where(с => с.Key == name)
        .Select(с => с.Value)
        .FirstOrDefault();
    }

    /// <summary>
    /// Существует ли пользователь.
    /// </summary>
    /// <param name="name">имя пользователя.</param>
    /// <returns>true/ false.</returns>
    public async Task<bool> UserExists(string name)
    {
      return await this.db.Users.AnyAsync(e => e.Name == name);
    }
  }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using ScrumPoker.Data.Models;
using ScrumPoker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumPoker.SignalR
{
  /// <summary>
  /// Хаб комнаты.
  /// </summary>
  [Authorize]
  public class RoomsHub : Hub
  {
    /// <summary>
    /// Сервис пользователей.
    /// </summary>
    private readonly UserService userService;

    /// <summary>
    /// Конструктор класса.
    /// </summary>
    /// <param name="service">Сервис пользователей.</param>
    public RoomsHub(UserService service)
    {
      this.userService = service;
    }

    /// <summary>
    /// Перезапись события подключения нового пользователя.
    /// </summary>
    /// <returns>Результат работы базового метода.</returns>
    
    public override Task OnConnectedAsync()
    {
      this.userService.AddUserToConnection(this.Context.User.Identity.Name, this.Context.ConnectionId);
      return base.OnConnectedAsync();
     }

    /// <summary>
    /// Перезапись события отключения пользователя.
    /// </summary>
    /// <param name="exception">исключение.</param>
    /// <returns>Результат работы базового метода.</returns>
    public override Task OnDisconnectedAsync(Exception exception)
    {
      userService.DeleteUserConnection(this.Context.User.Identity.Name);
      return base.OnDisconnectedAsync(exception);
    }
  }
}

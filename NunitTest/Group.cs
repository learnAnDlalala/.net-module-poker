using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace NunitTest.UtilsContext
{
  /// <summary>
  /// Реализация интефейса IGroupManager
  /// </summary>
  class Group : IGroupManager
  { 
    /// <summary>
    /// Конструктор класса.
    /// </summary>
    private Group()
    {
      this.groupsConnections = new Dictionary<string, ISet<string>>();
    }

    /// <summary>
    /// Cоздание инстанса саомго себя.
    /// </summary>
    public static Group GetGroupManager { get; } = new Group();

    /// <summary>
    /// Хранилище групп соединений.
    /// </summary>
    private Dictionary<string, ISet<string>> groupsConnections;

    /// <summary>
    /// Добавления соединения в группу.
    /// </summary>
    /// <param name="connectionId">ID connection.</param>
    /// <param name="groupName">название группы.</param>
    /// <param name="cancellationToken">признак отмены.</param>
    /// <returns>асинхронная задача.</returns>
    public Task AddToGroupAsync(string connectionId, string groupName,
        CancellationToken cancellationToken = new CancellationToken())
    {
      return Task.Run(() =>
      {
        if (!this.groupsConnections.ContainsKey(groupName))
        {
          this.groupsConnections.Add(groupName, new HashSet<string>());
        }

        ISet<string> connections = this.groupsConnections.GetValueOrDefault(groupName);
        if (!connections.Contains(connectionId))
        {
          connections.Add(connectionId);
        }
      });
    }

    /// <summary>
    /// Удаление соединения из группы.
    /// </summary>
    /// <param name="connectionId">ID connection.</param>
    /// <param name="groupName">название группы.</param>
    /// <param name="cancellationToken">признак отмены.</param>
    /// <returns>асинхронная задача</returns>
    public Task RemoveFromGroupAsync(string connectionId, string groupName,
        CancellationToken cancellationToken = new CancellationToken())
    {
      return Task.Run(() =>
      {
        if (this.groupsConnections.ContainsKey(groupName))
        {
          this.groupsConnections[groupName].Remove(connectionId);
        }
      });
    }
  }
}

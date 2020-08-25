using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace NunitTest.UtilsContext
{
  class Group: IGroupManager
  { private Group()
    {
      this.groupsConnections = new Dictionary<string, ISet<string>>();
    }
    public static Group GetGroupManager { get; } = new Group();
    private Dictionary<string, ISet<string>> groupsConnections;
    public Task AddToGroupAsync(string connectionId, string groupName,
        CancellationToken cancellationToken = new CancellationToken())
    {
      return Task.Run(() =>
      {
        if (!groupsConnections.ContainsKey(groupName))
        {
          groupsConnections.Add(groupName, new HashSet<string>());
        }

        ISet<string> connections = groupsConnections.GetValueOrDefault(groupName);
        if (!connections.Contains(connectionId))
        {
          connections.Add(connectionId);
        }
      });
    }
    public Task RemoveFromGroupAsync(string connectionId, string groupName,
        CancellationToken cancellationToken = new CancellationToken())
    {
      return Task.Run(() =>
      {
        if (groupsConnections.ContainsKey(groupName))
        {
          this.groupsConnections[groupName].Remove(connectionId);
        }
      });
    }
  }
}

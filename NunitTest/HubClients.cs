using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;

namespace NunitTest.UtilsContext
{
  class HubClients:IHubClients
  {
    private HubClients()
    {
      this.All = new Client();
    }
    public static IHubClients GetHubClients { get; } = new HubClients();
    public IClientProxy AllExcept(IReadOnlyList<string> excludedConnectionIds)
    {
      return new Client();
    }
    public IClientProxy Client(string connectionId)
    {
      return new Client();
    }
    public IClientProxy Clients(IReadOnlyList<string> connectionIds)
    {
      return new Client();
    }
    public IClientProxy Group(string groupName)
    {
      return new Client();
    }
    public IClientProxy GroupExcept(string groupName, IReadOnlyList<string> excludedConnectionIds)
    {
      return new Client();
    }
    public IClientProxy Groups(IReadOnlyList<string> groupNames)
    {
      return new Client();
    }
    public IClientProxy User(string userId)
    {
      return new Client();
    }
    public IClientProxy Users(IReadOnlyList<string> userIds)
    {
      return new Client();
    }
    public IClientProxy All { get; }
  }
}

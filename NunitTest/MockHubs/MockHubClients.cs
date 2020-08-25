using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;

namespace NunitTest.UtilsContext
{
  class MockHubClients:IHubClients
  {
    private MockHubClients()
    {
      this.All = new MockClient();
    }
    public static IHubClients GetHubClients { get; } = new MockHubClients();
    public IClientProxy AllExcept(IReadOnlyList<string> excludedConnectionIds)
    {
      return new MockClient();
    }
    public IClientProxy Client(string connectionId)
    {
      return new MockClient();
    }
    public IClientProxy Clients(IReadOnlyList<string> connectionIds)
    {
      return new MockClient();
    }
    public IClientProxy Group(string groupName)
    {
      return new MockClient();
    }
    public IClientProxy GroupExcept(string groupName, IReadOnlyList<string> excludedConnectionIds)
    {
      return new MockClient();
    }
    public IClientProxy Groups(IReadOnlyList<string> groupNames)
    {
      return new MockClient();
    }
    public IClientProxy User(string userId)
    {
      return new MockClient();
    }
    public IClientProxy Users(IReadOnlyList<string> userIds)
    {
      return new MockClient();
    }
    public IClientProxy All { get; }
  }
}

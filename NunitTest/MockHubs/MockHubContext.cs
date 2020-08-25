using Microsoft.AspNetCore.SignalR;
using ScrumPoker.SignalR;


namespace NunitTest.UtilsContext
{
  class MockHubContext : IHubContext<RoomsHub>
  {
    private MockHubContext()
    {
      this.Groups = MockGroup.GetGroupManager;
      this.Clients = MockHubClients.GetHubClients;
      CallingMethod = string.Empty;
    }
    public static IHubContext<RoomsHub> GetContext { get; } = new MockHubContext();
    public IHubClients Clients { get; }
    public IGroupManager Groups { get; }
    public static string CallingMethod = string.Empty;
  }
}

using Microsoft.AspNetCore.SignalR;
using ScrumPoker.SignalR;


namespace NunitTest.UtilsContext
{
  class HubContext : IHubContext<RoomsHub>
  {
    private HubContext()
    {
      this.Groups = Group.GetGroupManager;
      this.Clients = HubClients.GetHubClients;
      CallingMethod = string.Empty;
    }
    public static IHubContext<RoomsHub> GetContext { get; } = new HubContext();
    public IHubClients Clients { get; }
    public IGroupManager Groups { get; }
    public static string CallingMethod = string.Empty;
  }
}

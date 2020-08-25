using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NunitTest.UtilsContext
{
  class Client: IClientProxy
  {
    public Task SendCoreAsync(string method, object[] args, CancellationToken cancellationToken = new CancellationToken())
    {
      return Task.Run(() =>
      {
        //BaseTest.InvokedSignalRMethod = method;
      });
    }
  }
}

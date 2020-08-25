using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NunitTest.UtilsContext
{
  /// <summary>
  /// Реализация интерфейса.
  /// </summary>
  class Client : IClientProxy
  {
    /// <summary>
    /// Реализация метода записывающего названия вызываемого метода.
    /// </summary>
    /// <param name="method">название метода.</param>
    /// <param name="args">массив аргументов.</param>
    /// <param name="cancellationToken">признак отмены.</param>
    /// <returns>асинхронная задание.</returns>
    public Task SendCoreAsync(string method, object[] args, CancellationToken cancellationToken = new CancellationToken())
    {
      return Task.Run(() =>
      {
        HubContext.CallingMethod = method;
      });
    }
  }
}

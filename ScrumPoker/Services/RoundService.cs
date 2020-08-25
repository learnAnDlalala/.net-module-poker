using DataService;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ScrumPoker.DataService.Models;
using ScrumPoker.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;

namespace ScrumPoker.Services
{
  /// <summary>
  /// Класс сервиса раундов.
  /// </summary>
  public class RoundService
  {
    /// <summary>
    /// контекст Хаба комнаты.
    /// </summary>
    private IHubContext<RoomsHub> ctx;
    private readonly ConcurrentDictionary<int, Timer> roundTimers;
    private readonly ModelContext db;

    /// <summary>
    /// Конструктор класса.
    /// </summary>
    /// <param name="context">контекст хаба.</param>
    public RoundService(IHubContext<RoomsHub> context, ModelContext dbContext)
    {
      this.ctx = context;
      this.roundTimers = new ConcurrentDictionary<int, Timer>();
      this.db = dbContext;
    }

    /// <summary>
    /// Старт раунда.
    /// </summary>
    /// <param name="db">контекст бд.</param>
    /// <param name="newRound">инстанс класса раунда.</param>
    /// <returns>ничего не возвращает.</returns>
    public async Task Start(Round newRound)
    {
      newRound.Start = DateTime.Now;
      this.db.Rounds.Add(newRound);
      await this.db.SaveChangesAsync();
      this.CreateTimer(newRound);
      await this.ctx.Clients.Group($"room={newRound.RoomID}").SendAsync("StartRoundEvent", newRound);
    }

    /// <summary>
    /// Информация о раунде.
    /// </summary>
    /// <param name="id">id раунда.</param>
    /// <returns>инстант раунда.</returns>
    public async Task<Round> GetRoundInfo(int id)
    {
      return await this.db.Rounds.Include(t => t.Cards).FirstOrDefaultAsync(t => t.ID == id);
    }

    /// <summary>
    /// Перезапуск раунда.
    /// </summary>
    /// <param name="round">id раунда.</param>
    /// <returns>ничего не возвращает.</returns>
    public async Task Restart(int id)
    {
      var currentRound = await this.db.Rounds.FirstOrDefaultAsync(t => t.ID == id);
      currentRound.Start = DateTime.Now;
      var timer = this.roundTimers.GetValueOrDefault(id);
      timer?.Stop();
      timer?.Dispose();
      this.roundTimers.TryRemove(id, out timer);
      await db.SaveChangesAsync();
      CreateTimer(currentRound);
      // timer сделать
      await this.ctx.Clients.Group($"room={currentRound.RoomID}").SendAsync("StartRoundEvent", currentRound);
    }

    /// <summary>
    /// Окончание раунда.
    /// </summary>
    /// <param name="round">инстант класса раунда.</param>
    /// <returns>ничего не возвращает.</returns>
    public async Task EndRound(int id)
    {
      var currentRoom = await this.db.Rooms.Include(t => t.Rounds).FirstOrDefaultAsync(t => t.ID == id);
      //var currentRound = currentRoom.Rounds.Last();
      var currentRound = await this.db.Rounds.Include(t => t.Cards).FirstOrDefaultAsync(t => t.ID == id);
      currentRound.End = DateTime.Now;
      var timer = this.roundTimers.GetValueOrDefault(id);
      timer?.Stop();
      timer?.Dispose();
      this.roundTimers.TryRemove(id, out timer);
      await this.db.SaveChangesAsync();
      await this.ctx.Clients.Group($"room={currentRound.RoomID}").SendAsync("EndRoundEvent", currentRound);
    }

    /// <summary>
    /// Создания таймера(принимает секунды).
    /// </summary>
    /// <param name="round">инстанс класса раунда.</param>
    /// <returns>таймер в секундах</returns>
    private void CreateTimer(Round round)
    {
      var id = round.ID;
      var timer = new Timer(round.Timer * Math.Pow(10, 3));
      timer.AutoReset = false;
      timer.Elapsed += (sender, e) => this.EndRound(id);
      timer.Enabled = true;
      timer.Start();
      this.roundTimers.TryAdd(id, timer);
    }
  }
}

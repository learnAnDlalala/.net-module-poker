using Microsoft.EntityFrameworkCore;
using ScrumPoker.Data;
using ScrumPoker.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumPoker.Services
{
  /// <summary>
  /// Класс сервис выбранных карт.
  /// </summary>
  public class RoundCardService
  {

    /// <summary>
    /// Выбор карты.
    /// </summary>
    /// <param name="db">контекст бд.</param>
    /// <param name="card">инстанс класса карты.</param>
    /// <returns>ничего не возвращает.</returns>
    public async Task UserChoose(ModelContext db, RoundCard card)
    {
      if (await this.RoundCardExist(db, card.UserID, card.Round.ID))
      {
        var currentCard = await db.RoundCards.FindAsync(card.ID);
        currentCard.CardValue = card.CardValue;
      }
      else
      {
        db.RoundCards.Add(card);
      }

      await db.SaveChangesAsync();
    }

    /// <summary>
    /// Проверка выбирал ли пользователь карту или нет.
    /// </summary>
    /// <param name="db">контекст бд.</param>
    /// <param name="user">id пользователя.</param>
    /// <param name="round">id раунда.</param>
    /// <returns>карту выбранную пользователем.</returns>
    public async Task<bool> RoundCardExist(ModelContext db, int user, int round)
    {
      var currentRound = await db.Rounds.FindAsync(round);
      return currentRound.Cards.Any(s => s.UserID == user);
    }
  }
}

using DataService;
using ScrumPoker.DataService.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumPoker.Services
{ 
  /// <summary>
  /// Класс сервис выбранных карт.
  /// </summary>
  public class RoundResultsService
  {

    private readonly ModelContext db;

    public RoundResultsService(ModelContext dbContex)
    {
      this.db = dbContex;
    }

    /// <summary>
    /// Выбор карты.
    /// </summary>>
    /// <param name="card">инстанс класса карты.</param>
    /// <returns>ничего не возвращает.</returns>
    public async Task UserChoose(RoundResults card)
    {
      if (await this.RoundCardExist(card.UserID, card.Round.ID))
      {
        var currentCard = await this.db.RoundCards.FindAsync(card.ID);
        currentCard.CardValue = card.CardValue;
      }
      else
      {
        this.db.RoundCards.Add(card);
      }

      await this.db.SaveChangesAsync();
    }

    /// <summary>
    /// Проверка выбирал ли пользователь карту или нет.
    /// </summary>
    /// <param name="user">id пользователя.</param>
    /// <param name="round">id раунда.</param>
    /// <returns>карту выбранную пользователем.</returns>
    public async Task<bool> RoundCardExist(int user, int round)
    {
      var currentRound = await this.db.Rounds.FindAsync(round);
      return currentRound.Cards.Any(s => s.UserID == user);
    }
  }
}

using ScrumPoker.Data.Models;
using System.Linq;

namespace ScrumPoker.Data
{
  /// <summary>
  /// Класс заполнения бд стартовыми значениями.
  /// </summary>
  public class DbInitializer
  {
    /// <summary>
    /// Метод заполнения бд значениями.
    /// </summary>
    /// <param name="context">контекст бд.</param>
    public static void Initialize(ModelContext context)
    {
      context.Database.EnsureCreated();
      if (context.Decks.Any())
      {
        return;
      }

      var users = new User[]
      {
        new User { Name = "Alex" }, new User { Name = "Dima" }, new User { Name = "Andrey" }, new User { Name = "Ilia" }
      };
      foreach (User user in users)
      {
        context.Users.Add(user);
      }

      context.SaveChanges();

      var rooms = new Room[]
     {
        new Room { Name = "Room1", OwnerID = 1 }, new Room { Name = "Room2", OwnerID = 2 }, new Room { Name = "Room3", OwnerID = 3 }, new Room { Name = "Room4", OwnerID = 4}
     };
      foreach (Room room in rooms)
      {
        context.Rooms.Add(room);
      }

      context.SaveChanges();
      var cards = new Card[]
      {
        new Card { Value = 0, Description = "zero" }, new Card { Value = 1, Description = "one" }, new Card { Value = 2, Description = "two" }, new Card { Value = 4, Description = "four" }
      };
      var deck = new Deck { Name = "1st deck", Description = "this is 1st deck" };
      foreach (Card card in cards)
      {
        context.Cards.Add(card);
        deck.Cards.Add(card);
      }

      context.Decks.Add(deck);
      context.SaveChanges();
      var round = new Round { Subject = "test round 2", RoomID = 2 , DeckID = 1 };
      context.Rounds.Add(round);
      context.SaveChanges();

      var roundCards = new RoundCard[]
      {
        new RoundCard { CardValue = cards[1].Value , UserID = 1 ,RoundID = 1}, new RoundCard { CardValue = cards[2].Value , UserID = 2 , RoundID =1}
      };
      foreach (RoundCard card  in roundCards)
      {
        context.RoundCards.Add(card);
      }
      context.SaveChanges();
    }
  }
}

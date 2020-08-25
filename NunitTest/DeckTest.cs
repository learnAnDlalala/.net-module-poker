using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ScrumPoker.Services;
using System.Threading.Tasks;
using ScrumPoker.DataService.Models;
using DataService;
using System.Linq;

namespace NunitTest
{
  class DeckTest
  {
    private ModelContext db;
    private DeckService deck;
    [SetUp]
    public void Setup()
    {
      var dbContextoptions = new DbContextOptionsBuilder<ModelContext>().UseInMemoryDatabase("TestDB");
      db = new ModelContext(dbContextoptions.Options);
      db.Database.EnsureCreated();
      deck = new DeckService(db);
    }
    [TearDown]
    public void TearDown()
    {
      db.Database.EnsureDeleted();
    }
    [Test]
    public async Task ShowAllDecks()
    {
      var cards = new Card[]
     {
        new Card {Value = 0, Description="zero"}, new Card {Value = 1, Description="one"}, new Card {Value = 2, Description="two"}, new Card {Value = 4, Description="four"}
     };
      var newDeck = new Deck { Name = "testDeck" };
      var newDeck2 = new Deck { Name = "testDeck2" };
      db.Decks.AddRange(newDeck, newDeck2);
      await db.SaveChangesAsync();
      foreach (Card card in cards)
      {
        db.Cards.Add(card);
        
        newDeck2.Cards.Add(card);
        newDeck.Cards.Add(card);
      }
      await db.SaveChangesAsync();
      var result = await deck.ShowAll();
      var length = result.Count;
      Assert.That(2, Is.EqualTo(length));
    }

    [Test]
    public async Task GetDeck()
    {
      var cards = new Card[]
     {
        new Card {Value = 0, Description="zero"}, new Card {Value = 1, Description="one"}, new Card {Value = 2, Description="two"}, new Card {Value = 4, Description="four"}
     };
      var newDeck = new Deck { Name = "testDeck" };
      var newDeck2 = new Deck { Name = "testDeck2" };
      db.Decks.AddRange(newDeck, newDeck2);
      await db.SaveChangesAsync();
      foreach (Card card in cards)
      {
        db.Cards.Add(card);
        newDeck.Cards.Add(card);
        newDeck2.Cards.Add(card);
      }
      db.SaveChanges();
      newDeck2.Cards.Add(cards[1]);
      await db.SaveChangesAsync();
      var result = await deck.getDeck(1);
      var result2 = await deck.getDeck(2);
      Assert.That(result2.Cards.Count, Is.EqualTo(newDeck2.Cards.Count));
      Assert.That(cards[1].Value, Is.EqualTo(newDeck2.Cards.Last().Value));
    }
  }
}

using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ScrumPoker.Data;
using ScrumPoker.Data.Models;
using ScrumPoker.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
      deck = new DeckService();
    }
    [TearDown]
    public void TearDown()
    {
      db.Database.EnsureDeleted();
    }
    [Test]
    public async Task ShowAllCards()
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
      await db.SaveChangesAsync();
      var result = deck.ShowAll(db);
      var length = result.Result.Value.Count;
      Assert.That(2, Is.EqualTo(length));
    }
  }
}

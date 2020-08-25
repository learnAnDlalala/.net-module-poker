using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
//using NunitTest.UtilsContext;
using ScrumPoker.Data;
using ScrumPoker.Data.Models;
using ScrumPoker.Services;
using ScrumPoker.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NunitTest
{
  class RoundCardsTest
  {
    private ModelContext db;
    private RoundCardService roundCard;
    [SetUp]
    public void Setup()
    {
      var dbContextoptions = new DbContextOptionsBuilder<ModelContext>().UseInMemoryDatabase("TestDB");
      db = new ModelContext(dbContextoptions.Options);
      db.Database.EnsureCreated();
      roundCard = new RoundCardService ();
    }
    [TearDown]
    public void TearDown()
    {
      db.Database.EnsureDeleted();
    }
    [Test]
    public async Task ChooseCard ()
    {
      var newUser = new User { Name = "CommonUser" };
      await db.Users.AddAsync(newUser);
      var newRoom = new Room { Name = "TestRoom", OwnerID = 1 };
      await db.Rooms.AddAsync(newRoom);
      var newRound = new Round { Room = newRoom, Subject = "Some text", Timer = 2 };
      await db.Rounds.AddAsync(newRound);
      await db.SaveChangesAsync();
      var cards = new Card[]
      {
        new Card {Value = 0, Description="zero"}, new Card {Value = 1, Description="one"}, new Card {Value = 2, Description="two"}, new Card {Value = 4, Description="four"}
      };
      var newDeck = new Deck { Name = "testDeck" };
      foreach (Card card in cards)
      {
        db.Cards.Add(card);
        newDeck.Cards.Add(card);

      }
      var newRoundCard = new RoundCard { Round = newRound, UserID = 1 , CardValue = 3 };
      await roundCard.UserChoose(db, newRoundCard);
      var length = newRound.Cards.Count;
      Assert.That(1, Is.EqualTo(length));
      Assert.That(3, Is.EqualTo(newRoundCard.Card.ID));
    }
    [Test]
    public async Task CardAlreadyExist ()
    {
      var newUser = new User { Name = "CommonUser" };
      await db.Users.AddAsync(newUser);
      var newRoom = new Room { Name = "TestRoom", OwnerID = 1 };
      await db.Rooms.AddAsync(newRoom);
      var newRound = new Round { Room = newRoom, Subject = "Some text", Timer = 2 };
      await db.Rounds.AddAsync(newRound);
      await db.SaveChangesAsync();
      var cards = new Card[]
      {
        new Card {Value = 0, Description="zero"}, new Card {Value = 1, Description="one"}, new Card {Value = 2, Description="two"}, new Card {Value = 4, Description="four"}
      };
      var newDeck = new Deck { Name = "testDeck" };
      foreach (Card card in cards)
      {
        db.Cards.Add(card);
        newDeck.Cards.Add(card);

      }
      var newRoundCard = new RoundCard { Round = newRound, User = 1, Card = cards[2] };
      await db.RoundCards.AddAsync(newRoundCard);
      newRoundCard.Card = cards[1];
      await roundCard.UserChoose(db, newRoundCard);
      var length = newRound.Cards.Count;
      Assert.That(1, Is.EqualTo(length));
      Assert.That(2, Is.EqualTo(newRoundCard.Card.ID));
    }
  }
}

﻿using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
//using NunitTest.UtilsContext;
using System.Threading.Tasks;
using DataService;
using ScrumPoker.DataService.Models;
using System.Linq;

namespace NunitTest
{
  class RoundCardsTest
  {
    private ModelContext db;
    private ScrumPoker.Services.RoundResultsService roundCard;
    [SetUp]
    public void Setup()
    {
      var dbContextoptions = new DbContextOptionsBuilder<ModelContext>().UseInMemoryDatabase("TestDB");
      db = new ModelContext(dbContextoptions.Options);
      db.Database.EnsureCreated();
      roundCard = new ScrumPoker.Services.RoundResultsService(db);
    }
    [TearDown]
    public void TearDown()
    {
      db.Database.EnsureDeleted();
    }
    [Test]
    public async Task ChooseCard()
    {
      var newUser = new User { Name = "CommonUser" };
      await db.Users.AddAsync(newUser);
      var newUser2 = new User { Name = "CommonUser2" };
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
      var newRoundCard = new RoundResults { Round = newRound, UserID = 1, CardValue = 3 };
      var newRoundCard2 = new RoundResults { Round = newRound, UserID = 2, CardValue = 1 };
      await roundCard.UserChoose(newRoundCard);
      await roundCard.UserChoose(newRoundCard2);
      var length = newRound.Cards.Count;
      Assert.That(2, Is.EqualTo(length));
      Assert.That(2, Is.EqualTo(newRound.Result));
    }


    public async Task CardAlreadyExist()
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
      var newRoundCard = new RoundResults { Round = newRound, UserID = 1, CardValue = cards[2].Value };
      await db.RoundCards.AddAsync(newRoundCard);
      newRoundCard.CardValue = cards[3].Value;
      await roundCard.UserChoose(newRoundCard);
      var length = newRound.Cards.Count;
      Assert.That(1, Is.EqualTo(length));
      Assert.That(cards[3].Value, Is.EqualTo(newRound.Cards.Last().CardValue));
      Assert.That(newRoundCard.UserID, Is.EqualTo(newRound.Cards.Last().UserID));
    }
  }
}

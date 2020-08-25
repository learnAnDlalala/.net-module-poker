using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

using ScrumPoker.Data;
using ScrumPoker.Data.Models;
using ScrumPoker.Services;
using ScrumPoker.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NunitTest.UtilsContext;

namespace NunitTest
{
  class RoundTest
  {
    private ModelContext db;
    private RoundService round;
    private IHubContext<RoomsHub> context;
    [SetUp]
    public void Setup()
    {
      var dbContextoptions = new DbContextOptionsBuilder<ModelContext>().UseInMemoryDatabase("TestDB");
      db = new ModelContext(dbContextoptions.Options);
      db.Database.EnsureCreated();

      context = HubContext.GetContext;
      round = new RoundService((IHubContext<RoomsHub>)context);
    }
    [TearDown]
    public void TearDown()
    {
      db.Database.EnsureDeleted();
    }
    [Test]
    public async Task CreateRound()
    {
      var newRoom = new Room { Name = "TestRoom", OwnerID = 1 };
      var newRound = new Round { Subject = "lorem sdsd", Timer=2, Room=newRoom };
      await round.Start(db, newRound);
      var result = await db.Rounds.FirstOrDefaultAsync(t => t.Subject == "lorem sdsd");
      var actualMethod = HubContext.CallingMethod;
      Assert.That("StartRound", Is.EqualTo(actualMethod));
      Assert.That(1, Is.EqualTo(result.ID));
    }
    [Test]
    public async Task RestartRound()
    {
      var newRoom = new Room { Name = "TestRoom", OwnerID = 1 };
      var newRound = new Round { Subject = "lorem sdsd", Timer = 2, Room = newRoom };
      await round.Start(db, newRound);
      var restartRound = await db.Rounds.FirstOrDefaultAsync(t => t.ID == 1);
      restartRound.Timer = 3;
      restartRound.Subject = "LETS TRY";
      await round.Restart(db,restartRound);
      var result = await db.Rounds.FirstOrDefaultAsync(t => t.Subject == "LETS TRY");
      var actualMethod = HubContext.CallingMethod;
      Assert.That("StartRound", Is.EqualTo(actualMethod));
      Assert.That(1, Is.EqualTo(result.ID));
    }
    [Test]
    public async Task EndRound()
    {
      var newRoom = new Room { Name = "TestRoom", OwnerID = 1 };
      var newRound = new Round { Subject = "lorem sdsd", Timer = 2, Room = newRoom };
      await round.Start(db, newRound);
      await round.EndRound(db, newRound);
      var result = newRound.End.ToString("d");
      var expect = DateTime.Now.ToString("d");
      var actualMethod = HubContext.CallingMethod;
      Assert.That("EndRound", Is.EqualTo(actualMethod));
      Assert.That(expect, Is.EqualTo(result));
    }
    [Test]
    public async Task GetRoundInfo()
    {
      var newRoom = new Room { Name = "TestRoom", OwnerID = 1 };
      var newDeck = new Deck { Name = "1st Deck", Description = "test text" };
      var newRound = new Round { Subject = "lorem sdsd", Timer = 2, Room = newRoom, Deck=newDeck};
      await round.Start(db, newRound);
      var result = await round.GetRoundInfo(db, 1);
      Assert.That("lorem sdsd", Is.EqualTo(result.Value.Subject));
      Assert.That(1, Is.EqualTo(result.Value.ID));
      Assert.That("TestRoom", Is.EqualTo(result.Value.Room.Name));
      Assert.That("1st Deck", Is.EqualTo(result.Value.Deck.Name));
    }
  }
}

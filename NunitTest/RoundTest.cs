using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ScrumPoker.Services;
using ScrumPoker.SignalR;
using System;
using System.Threading.Tasks;
using NunitTest.UtilsContext;
using DataService;
using ScrumPoker.DataService.Models;

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

      context = MockHubContext.GetContext;
      round = new RoundService((IHubContext<RoomsHub>)context, db);
    }
    [TearDown]
    public void TearDown()
    {
      db.Database.EnsureDeleted();
    }
    [Test]
    public async Task CreateRound()
    {
      var newRoom = new Room { Name = "TestRoom", OwnerID = 1,  ID = 1};
      var newRound = new Round { Subject = "lorem sdsd", Timer=2, RoomID=newRoom.ID};
      await round.Start(newRound);
      var result = await db.Rounds.FirstOrDefaultAsync(t => t.Subject == "lorem sdsd");
      var actualMethod = MockHubContext.CallingMethod;
      Assert.That("StartRoundEvent", Is.EqualTo(actualMethod));
      Assert.That(1, Is.EqualTo(result.ID));
      Assert.That(newRoom.ID, Is.EqualTo(result.RoomID));
    }
    [Test]
    public async Task RestartRound()
    {
      var newRoom = new Room { Name = "TestRoom", OwnerID = 1, ID = 1 };
      var newRound = new Round { Subject = "lorem sdsd", Timer = 2, RoomID = newRoom.ID };
      await round.Start(newRound);
      var restartRound = await db.Rounds.FirstOrDefaultAsync(t => t.ID == 1);
      restartRound.Timer = 3;
      restartRound.Subject = "LETS TRY";
      await round.Restart(restartRound.ID);
      var result = await db.Rounds.FirstOrDefaultAsync(t => t.Subject == "LETS TRY");
      var actualMethod = MockHubContext.CallingMethod;
      Assert.That("StartRoundEvent", Is.EqualTo(actualMethod));
      Assert.That(1, Is.EqualTo(result.ID));
    }
    [Test]
    public async Task EndRound()
    {
      var newRoom = new Room { Name = "TestRoom", OwnerID = 1 };
      var newRound = new Round { Subject = "lorem sdsd", Timer = 2, Room = newRoom };
      await round.Start(newRound);
      await round.EndRound(newRound.ID);
      var result = newRound.End.ToString("d");
      var expect = DateTime.Now.ToString("d");
      var actualMethod = MockHubContext.CallingMethod;
      Assert.That("EndRoundEvent", Is.EqualTo(actualMethod));
      Assert.That(expect, Is.EqualTo(result));
    }
    [Test]
    public async Task GetRoundInfo()
    {
      var newRoom = new Room { Name = "TestRoom", OwnerID = 1 };
      var newDeck = new Deck { Name = "1st Deck", Description = "test text" };
      var newRound = new Round { Subject = "lorem sdsd", Timer = 2, RoomID = 1, DeckID = 1 };
      await round.Start(newRound);
      var result = await round.GetRoundInfo(1);
      Assert.That(newRound.Subject, Is.EqualTo(result.Subject));
      Assert.That(newRound.ID, Is.EqualTo(result.ID));
      Assert.That(newRound.RoomID, Is.EqualTo(result.RoomID));
      Assert.That(newRound.DeckID, Is.EqualTo(result.DeckID));
    }
  }
}

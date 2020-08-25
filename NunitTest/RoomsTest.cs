using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using NunitTest.UtilsContext;
using ScrumPoker.Data;
using ScrumPoker.Data.Models;
using ScrumPoker.Services;
using ScrumPoker.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NunitTest
{
  class RoomsTest
  {
    private ModelContext db;
    private RoomService room;
    private UserService user;
    private IHubContext<RoomsHub> context;
    [SetUp]
    public void Setup()
    {
      var dbContextoptions = new DbContextOptionsBuilder<ModelContext>().UseInMemoryDatabase("TestDB");
      this.db = new ModelContext(dbContextoptions.Options);
      db.Database.EnsureCreated();
      context = HubContext.GetContext;
      var a = new Mock<IHubClients<RoomsHub>>();
       this.user = new UserService(context);
      this.room = new RoomService(context,user);
    }
    [TearDown]
    public void TearDown()
    {
      db.Database.EnsureDeleted();
    }
    [Test]
    public async Task CreateNewRoom()
    {
      var userCreator = new User
      {
        Name = "Master"
      };
      await user.Create(db,userCreator);
      var newRoom = new Room
      {
        Name = "TestRoom",
        OwnerID = 1
      };
      await room.Create(db, newRoom);
      var length = await db.Rooms.CountAsync();
      Assert.That(1, Is.EqualTo(length));
    }
    [Test]
    public async Task ShowAllRooms()
    {
      var userCreator = new User
      {
        Name = "Master"
      };
      await db.Users.AddAsync(userCreator);      
      var Room1 = new Room
      {
        Name = "TestRoom1",
        OwnerID = 1
      };
      var Room2 = new Room
      {
        Name = "TestRoom2",
        OwnerID = 1
      };
      await room.Create(db,Room1);
      await room.Create(db,Room2);
      var result = await room.ShowAll(db);
      var length = result.Value.Count;
      Assert.That(2, Is.EqualTo(length));
    }
    [Test]
    public async Task ShowAllUsers()
    {
      await db.Users.AddAsync(new User { Name = "Master" });
      
      var Room1 = new Room
      {
        Name = "TestRoom1",
        OwnerID = 1
      };
      
      await room.Create(db, Room1);
      await db.Users.AddAsync(new User { Name = "CommonUser", Room = Room1 });
      await db.SaveChangesAsync();
      var result = await room.ShowUsers(db, 1);
      var length = result.Count;
      Assert.That(2, Is.EqualTo(length));
    }
    [Test]
    public async Task EnterInRoom()
    {
      var userCreator = new User
      {
        Name = "Master"
      };
      await user.Create(db, userCreator);
      var newRoom = new Room
      {
        Name = "TestRoom",
        OwnerID = 1
      };
      await room.Create(db, newRoom);
      await db.Users.AddAsync(new User { Name = "CommonUser" });
      await room.Enter(db, 2, 1);
      await db.SaveChangesAsync();
      var result = await db.Users.Where(t => t.Room.ID == 1).ToListAsync();
      var length = result.Count;
      var actualMethod = HubContext.CallingMethod;
      Assert.That("UpdateUsersList",Is.EqualTo(actualMethod));
      Assert.That(2, Is.EqualTo(length));
    }
    [Test]
    public async Task DeleteUser ()
    {
      var userCreator = new User
      {
        Name = "Master"
      };
      await user.Create(db, userCreator);
      var newRoom = new Room
      {
        Name = "TestRoom",
        OwnerID = 1
      };
      await room.Create(db, newRoom);
      await db.Users.AddAsync(new User { Name = "CommonUser", Room = newRoom });
      await room.Delete(db, 2, 1);
      var result = await db.Users.Where(t => t.Room.ID == 1).ToListAsync();
      var length = result.Count;
      var actualMethod = HubContext.CallingMethod;
      Assert.That("UpdateUsersList", Is.EqualTo(actualMethod));
      Assert.That(1, Is.EqualTo(length));
    }
    [Test]
    public async Task RoomExist ()
    {
      var userCreator = new User
      {
        Name = "Master"
      };
      await user.Create(db, userCreator);
      var newRoom = new Room
      {
        Name = "TestRoom",
        OwnerID = 1
      };
      await room.Create(db, newRoom);
      var result = await room.RoomExists(db, newRoom.ID);
      Assert.IsTrue(result);
    }
  }
}

using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using NunitTest.UtilsContext;
using ScrumPoker.Services;
using ScrumPoker.SignalR;
using System.Linq;
using System.Threading.Tasks;
using DataService;
using ScrumPoker.DataService.Models;

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
      context = MockHubContext.GetContext;
      var a = new Mock<IHubClients<RoomsHub>>();
      this.user = new UserService(context,db);
      this.room = new RoomService(context,user,db);
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
      await user.Create(userCreator);
      var newRoom = new Room
      {
        Name = "TestRoom",
        OwnerID = 1
      };
      await room.Create(newRoom);
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
      await room.Create(Room1);
      await room.Create(Room2);
      var result = await room.ShowAll();
      var length = result.Count;
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
      
      await room.Create(Room1);
      await db.Users.AddAsync(new User { Name = "CommonUser", Room = Room1 });
      await db.SaveChangesAsync();
      var result = await room.ShowUsers(1);
      var length = result.Count;
      Assert.That(1, Is.EqualTo(length));
      Assert.That(2, Is.EqualTo(result[0].ID));
    }
    [Test]
    public async Task EnterInRoom()
    {
      var userCreator = new User
      {
        Name = "Master"
      };
      await user.Create(userCreator);
      var newRoom = new Room
      {
        Name = "TestRoom",
        OwnerID = 1
      };
      await room.Create(newRoom);
      await db.Users.AddAsync(new User { Name = "CommonUser" });
      var result = await room.Enter(2, 1);
      var actualMethod = MockHubContext.CallingMethod;
      Assert.That("UpdateUserEvent",Is.EqualTo(actualMethod));
      Assert.That(1, Is.EqualTo(result.Users.Count));
    }
    [Test]
    public async Task DeleteUser ()
    {
      var userCreator = new User
      {
        Name = "Master"
      };
      await user.Create(userCreator);
      var newRoom = new Room
      {
        Name = "TestRoom",
        OwnerID = 1
      };
      await room.Create(newRoom);
      await db.Users.AddAsync(new User { Name = "CommonUser", Room = newRoom });
      await room.DeleteUser(2, 1);
      var result = await db.Users.Where(t => t.Room.ID == 1).ToListAsync();
      var length = result.Count;
      var actualMethod = MockHubContext.CallingMethod;
      Assert.That("UpdateUsersEvent", Is.EqualTo(actualMethod));
      Assert.That(0, Is.EqualTo(length));
    }
    [Test]
    public async Task RoomExist ()
    {
      var userCreator = new User
      {
        Name = "Master"
      };
      await user.Create(userCreator);
      var newRoom = new Room
      {
        Name = "TestRoom",
        OwnerID = 1
      };
      await room.Create(newRoom);
      var result = await room.RoomExists(newRoom.Name);
      Assert.IsTrue(result);
    }
  }
}

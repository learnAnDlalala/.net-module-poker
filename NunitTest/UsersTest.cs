using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
//using NunitTest.UtilsContext;
using ScrumPoker.Services;
using ScrumPoker.SignalR;
using System.Threading.Tasks;
using System.Linq;
using NunitTest.UtilsContext;
using DataService;
using ScrumPoker.DataService.Models;

namespace NunitTest
{
  class UsersTest
  {
    private ModelContext db;
    private UserService user;
    private IHubContext<RoomsHub> context;
    [SetUp]
    public void Setup()
    {
      var dbContextoptions = new DbContextOptionsBuilder<ModelContext>().UseInMemoryDatabase("TestDB");
      db = new ModelContext(dbContextoptions.Options);
      db.Database.EnsureCreated();
      context = MockHubContext.GetContext;
      user = new UserService(context, db);
    }
    [TearDown]
    public void TearDown()
    {
      db.Database.EnsureDeleted();
    }
    [Test]
    public async Task CreateNewUser()
    {
      var newUser = new User
      {
        Name = "T-1000"
      };
      var dbUser = await user.Create(newUser);
      var currentUser = await db.Users.FirstOrDefaultAsync(t => t.ID == dbUser.ID);
      Assert.That(currentUser.Name, Is.EqualTo(newUser.Name), "Names are euqal");
    }
    [Test]
    public async Task ShowAllUsers()
    {
      var user1 = new User { Name = "Roma" };
      var user2 = new User { Name = "Alex" };
      await user.Create(user1);
      await user.Create(user2);
      var result = await user.ShowAll();
      var length = result.Count;
      Assert.That(2, Is.EqualTo(length));
    }
    [Test]
    public void AddUserToConnection()
    {
      user.AddUserToConnection("Test User", "1");
      var result = user.FindConnectionID("Test User");
      Assert.That("1", Is.EqualTo(result));
    }
    [Test]
    public void DeleteUserСonnection()
    {
      user.AddUserToConnection("Old User", "2");
      user.DeleteUserConnection("Old User");
      var result = user.FindConnectionID("Old User");
      Assert.That(null, Is.EqualTo(null));
    }
    [Test]
    public void GetUserConnectionNegative()
    {
      user.AddUserToConnection("old User", "1");
      var result = user.FindConnectionID("Old user");
      Assert.AreNotEqual("1", result);
    }
    [Test]
    public async Task UserExist()
    {
      var user1 = new User { Name = "Alex" };
      await user.Create(user1);
      var result = await user.UserExists(user1.Name);
      Assert.IsTrue(result);
    }

  }
}

using DataService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ScrumPoker.Data;
using ScrumPoker.Services;
using ScrumPoker.SignalR;

namespace ScrumPoker
{
  /// <summary>
  /// Конфигурация приложения.
  /// </summary>
  public class Startup
  {
    /// <summary>
    /// Конструктор класса Startup.
    /// </summary>
    /// <param name="configuration">Инстанс интерфейса конфигурация.</param>
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    /// <summary>
    /// Инстанс интерфейса конфигурация.
    /// </summary>
    public IConfiguration Configuration { get; }

    /// <summary>
    /// Конфигурация сервисов.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers();
      string connection = this.Configuration.GetConnectionString("DefaultConnection");
      services.AddDbContext<ModelContext>(options => options.UseSqlServer(connection), ServiceLifetime.Singleton);
      services.AddSingleton<UserService>();
      services.AddSingleton<RoomService>();
      services.AddSingleton<DeckService>();
      services.AddSingleton<RoundService>();


      services.AddAuthentication("CookieAuth").AddCookie("CookieAuth", options =>
       {
         options.LoginPath = "/user/auf";

         options.Cookie = new CookieBuilder
         {
           Name = "HUI",
           HttpOnly = true,
           Path = "/",
           SameSite = SameSiteMode.Lax,
           SecurePolicy = CookieSecurePolicy.None
         };
         options.SlidingExpiration = true;
       });
      services.AddAuthorization();
      services.AddSignalR();
    }

    /// <summary>
    /// Конфигурация HTTP.
    /// </summary>
    /// <param name="app">инстанс интерфейса билдера.</param>
    /// <param name="env">инстантс интерфейса окружения.</param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors(x => x
        .AllowAnyMethod()
        .AllowAnyHeader()
        .SetIsOriginAllowed(origin => true)
        .AllowCredentials());
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseCookiePolicy(new CookiePolicyOptions
        {
          HttpOnly = HttpOnlyPolicy.None,
          Secure = CookieSecurePolicy.None
        });

        app.UseEndpoints(endpoints =>
        {
          endpoints.MapControllers();
          endpoints.MapHub<RoomsHub>("/roomsHub");
        });
      }
    }
  }
}
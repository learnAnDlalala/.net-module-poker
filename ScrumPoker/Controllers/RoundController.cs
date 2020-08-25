using Microsoft.AspNetCore.Mvc;
using ScrumPoker.Data;
using ScrumPoker.Data.Models;
using ScrumPoker.Services;
using System.Threading.Tasks;

namespace ScrumPoker.Controllers
{
  /// <summary>
  /// Контроллер раундов.
  /// </summary>
  [Route("api/[controller]")]
  [ApiController]
    public class RoundController : Controller
  {
    /// <summary>
    /// Контекст бд.
    /// </summary>
    private readonly ModelContext db;

    /// <summary>
    /// Сервис раундов.
    /// </summary>
    private readonly RoundService roundService;

    /// <summary>
    /// Конструктор класса.
    /// </summary>
    /// <param name="context">контекст бд.</param>
    /// <param name="roundService">сервис раундов.</param>
    public RoundController (ModelContext context, RoundService roundService)
    {
      this.db = context;
      this.roundService = roundService;
    }

    /// <summary>
    /// Запрос на получения информации о раунде.
    /// </summary>
    /// <param name="id">id раунда.</param>
    /// <returns>инстанс класса раунд</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Round>> Get(int id)
    {
      return await this.roundService.GetRoundInfo(id);
    }

    /// <summary>
    /// Запрос на создания нового раунда.
    /// </summary>
    /// <param name="round">инстанс класса раунд.</param>
    /// <returns>ничего не возвращает.</returns>
    [HttpPost]
    public async Task Create(Round round)
    {
      await this.roundService.Start(round);
    }

    /// <summary>
    /// Запрос на рестарт раунда.
    /// </summary>
    /// <param name="round">инстанс класса раунд.</param>
    /// <returns>ничего не возвращает.</returns>
    [HttpPost("restart")]
    public async Task Restarts(Round round)
    {
      await this.roundService.Restart(round.ID);
    }

    /// <summary>
    /// Запрос на окончания раунда.
    /// </summary>
    /// <param name="round">инстанс класса раунд.</param>
    /// <returns>ничего не возвращает.</returns>
    [HttpPost("end")]
    public async Task End(Room room)
    {
      this.roundService.EndRound(room.ID);
    }
  }
}

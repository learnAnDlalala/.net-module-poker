using Microsoft.AspNetCore.Mvc;
using ScrumPoker.Data;
using ScrumPoker.Data.Models;
using ScrumPoker.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScrumPoker.Controllers
{
  /// <summary>
  /// Контролер колод.
  /// </summary>
  [Route("api/[controller]")]
  [ApiController]
  public class DeckController : Controller
  {
    /// <summary>
    /// Контекст бд.
    /// </summary>
    private readonly ModelContext db;

    /// <summary>
    /// Сервис колод.
    /// </summary>
    private readonly DeckService deckService;

    /// <summary>
    /// Конструктор контролера.
    /// </summary>
    /// <param name="contex">контекст бд.</param>
    /// <param name="deckService">сервис колод.</param>
    public DeckController(ModelContext contex, DeckService deckService)
    {
      this.db = contex;
      this.deckService = deckService;
    }

    /// <summary>
    /// Запрос на получения списка колод.
    /// </summary>
    /// <returns>Список доступных колод.</returns>
    [HttpGet]
    public async Task<ActionResult<List<Deck>>> Get()
    {
      return await this.deckService.ShowAll(this.db);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<Deck>> GetDeck(int id)
    {
      return await this.deckService.getDeck(this.db, id);
    }
  }
}

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
    /// Сервис колод.
    /// </summary>
    private readonly DeckService deckService;

    /// <summary>
    /// Конструктор контролера.
    /// </summary>
    /// <param name="contex">контекст бд.</param>
    /// <param name="deckService">сервис колод.</param>
    public DeckController(DeckService deckService)
    {
      this.deckService = deckService;
    }

    /// <summary>
    /// Запрос на получения списка колод.
    /// </summary>
    /// <returns>Список доступных колод.</returns>
    [HttpGet]
    public async Task<ActionResult<List<Deck>>> GetDecksList()
    {
      return await this.deckService.ShowAll();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Deck>> GetDeckInfo(int id)
    {
      return await this.deckService.getDeck(id);
    }
  }
}

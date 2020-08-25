using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScrumPoker.Data;
using ScrumPoker.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScrumPoker.Services
{
  /// <summary>
  /// Класс сервиса колод.
  /// </summary>
  public class DeckService
  {
    /// <summary>
    /// Показать все колоды.
    /// </summary>
    /// <param name="db">контекст бд.</param>
    /// <returns>список колод.</returns>
    public async Task<ActionResult<List<Deck>>> ShowAll(ModelContext db)
    {
      return await db.Decks.Include(d => d.Cards).ToListAsync();
    }
      public async Task<Deck> getDeck (ModelContext db,int id)
    {

      return await db.Decks.Include(d => d.Cards).FirstOrDefaultAsync(t => t.ID == id);
    }
      
  }
}

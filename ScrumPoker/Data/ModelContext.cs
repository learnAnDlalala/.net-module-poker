using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ScrumPoker.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumPoker.Data
{
  /// <summary>
  /// Класс МоделиБД.
  /// </summary>
  public class ModelContext : DbContext
  {
    /// <summary>
    /// Сущность представляющая пользователей.
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// Сущность представляющая раунды.
    /// </summary>
    public DbSet<Round> Rounds { get; set; }

    /// <summary>
    /// Сущность представляющая комнаты.
    /// </summary>
    public DbSet<Room> Rooms { get; set; }

    /// <summary>
    /// Сущность представляющая колоды.
    /// </summary>
    public DbSet<Deck> Decks { get; set; }

    /// <summary>
    /// Сущность представляющая карты.
    /// </summary>
    public DbSet<Card> Cards { get; set; }

    /// <summary>
    /// Cущность представляющая карты выбранные в раунде.
    /// </summary>
    public DbSet<RoundCard> RoundCards { get; set; }

    /// <summary>
    /// Конструктор класса.
    /// </summary>
    /// <param name="options">настройки модели бд.</param>
    public ModelContext(DbContextOptions<ModelContext> options)
      : base(options)
    {
      this.Database.EnsureCreated();
    }
   
  }
}

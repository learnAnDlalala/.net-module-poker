using System.Collections.Generic;

namespace ScrumPoker.Data.Models
{
  /// <summary>
  /// Класс представляющий колоду.
  /// </summary>
  public class Deck
  {
    /// <summary>
    /// Конструткор класса.
    /// </summary>
    public Deck ()
    {
      this.Cards = new List<Card>();
    }

    /// <summary>
    /// ID колоды.
    /// </summary>
    public int ID { get;set; }

    /// <summary>
    /// Имя колоды.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Описание колоды.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Список карт входящий в колоду.
    /// </summary>
    public virtual List<Card> Cards { get; set; }
  }
}

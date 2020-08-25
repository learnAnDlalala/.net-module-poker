using System.Collections.Generic;

namespace ScrumPoker.DataService.Models
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
    public virtual ICollection<Card> Cards { get; set; }
  }
}

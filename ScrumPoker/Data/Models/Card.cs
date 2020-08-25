using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumPoker.Data.Models
{
  /// <summary>
  /// Класс представляющий карты.
  /// </summary>
  public class Card
  {
    /// <summary>
    /// ID карты.
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    /// Значение карты.
    /// </summary>
    public int Value { get; set; }

    /// <summary>
    /// Изображение карты.
    /// </summary>
    public string Img { get; set; }

    /// <summary>
    /// Описание карты.
    /// </summary>
    public string Description { get; set; }
  }
}

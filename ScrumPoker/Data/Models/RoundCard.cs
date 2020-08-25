using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumPoker.Data.Models
{
  /// <summary>
  /// Класс представляющий карты выбранные в раунде.
  /// </summary>
  public class RoundCard
  {
    /// <summary>
    /// ID инстанса.
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    /// ID пользователя выбравшего карту.
    /// </summary>
    public int UserID { get; set; }

    /// <summary>
    /// Выбранная карта.
    /// </summary>
    public int CardValue { get; set; }

    public int RoundID { get; set; }

    /// <summary>
    /// Раунд в котором была выбранна карта.
    /// </summary>
    [JsonIgnore]
    virtual public Round Round { get; set; }
    virtual public User User { get; set; }
    
  }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ScrumPoker.Data.Models
{
  /// <summary>
  /// Класс представляющий пользователей.
  /// </summary>
  public class User
  {
    /// <summary>
    /// ID пользователя.
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    /// Имя пользователя.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Комната в которой находится пользователь.
    /// </summary>
    [JsonIgnore]
    public virtual Room Room { get; set; }
  }
}

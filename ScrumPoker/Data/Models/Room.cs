using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumPoker.Data.Models
{
  /// <summary>
  /// Класс представляющий комнату.
  /// </summary>
  public class Room
  {
    /// <summary>
    /// Конструктор класса.
    /// </summary>
    public Room()
    {
      this.Users = new List<User>();
      this.Rounds = new List<Round>();
    }

    /// <summary>
    /// ID комнаты.
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    /// Имя комнаты.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// ID пользователя создавшего комнату.
    /// </summary>
    public int OwnerID { get; set; }

    /// <summary>
    /// Список пользователей находящихся в комнате.
    /// </summary>
    public ICollection<User> Users { get; set; }

    /// <summary>
    /// Список раундов провоеденных в комнате.
    /// </summary>
    public ICollection<Round> Rounds { get; set; }
  }
}

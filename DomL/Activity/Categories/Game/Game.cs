using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomL.Business.Entities
{
    [Table("GameActivity")]
    public class GameActivity
    {
        [Key]
        [ForeignKey("Activity")]
        public int Id { get; set; }
        [ForeignKey("Game")]
        public int GameId { get; set; }

        public string Description { get; set; }

        public virtual Activity Activity { get; set; }
        public virtual Game Game { get; set; }
    }

    [Table("Game")]
    public class Game
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Series { get; set; }
        public string Number { get; set; }
        public string Person { get; set; }
        public string Company { get; set; }
        public string Year { get; set; }
        public string Score { get; set; }
    }
}
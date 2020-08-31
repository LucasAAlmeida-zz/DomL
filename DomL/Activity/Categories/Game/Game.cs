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
        public int PlatformId { get; set; }
        public int? SeriesId { get; set; }
        public string NumberInSeries { get; set; }
        public int? DirectorId { get; set; }
        public int? PublisherId { get; set; }
        public int? ScoreId { get; set; }

        [ForeignKey("DirectorId")]
        public Person Director { get; set; }
        [ForeignKey("SeriesId")]
        public Series Series { get; set; }
        [ForeignKey("PlatformId")]
        public MediaType Platform { get; set; }
        [ForeignKey("PublisherId")]
        public Company Publisher { get; set; }
        [ForeignKey("ScoreId")]
        public Score Score { get; set; }
    }
}
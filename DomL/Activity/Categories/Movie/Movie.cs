using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomL.Business.Entities
{
    [Table("MovieActivity")]
    public class MovieActivity
    {
        [Key]
        [ForeignKey("Activity")]
        public int Id { get; set; }
        [ForeignKey("Movie")]
        public int MovieId { get; set; }

        public string Description { get; set; }

        public virtual Activity Activity { get; set; }
        public virtual Movie Movie { get; set; }
    }

    [Table("Movie")]
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public int? DirectorId { get; set; }
        public int? SeriesId { get; set; }
        public string NumberInSeries { get; set; }
        public int? ScoreId { get; set; }

        [ForeignKey("DirectorId")]
        public Person Director { get; set; }
        [ForeignKey("SeriesId")]
        public Series Series { get; set; }
        [ForeignKey("ScoreId")]
        public Score Score { get; set; }
    }
}

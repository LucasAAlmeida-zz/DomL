using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomL.Business.Entities
{
    [Table("ShowActivity")]
    public class ShowActivity
    {
        [Key]
        [ForeignKey("Activity")]
        public int Id { get; set; }
        [ForeignKey("ShowSeason")]
        public int ShowSeasonId { get; set; }

        public string Description { get; set; }

        public virtual Activity Activity { get; set; }
        public virtual ShowSeason Show { get; set; }
    }

    [Table("Show")]
    public class ShowSeason
    {
        [Key]
        public int Id { get; set; }
        public int SeriesId { get; set; }
        public string Season { get; set; }
        public int? DirectorId { get; set; }
        public int? TypeId { get; set; }
        public int? ScoreId { get; set; }

        [ForeignKey("SeriesId")]
        public Series Series { get; set; }
        [ForeignKey("DirectorId")]
        public Person Director { get; set; }
        [ForeignKey("TypeId")]
        public MediaType Type { get; set; }
        [ForeignKey("ScoreId")]
        public Score Score { get; set; }
    }
}

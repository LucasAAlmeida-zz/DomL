using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomL.Business.Entities
{
    [Table("ComicActivity")]
    public class ComicActivity
    {
        [Key]
        [ForeignKey("Activity")]
        public int Id { get; set; }
        [ForeignKey("ComicVolume")]
        public int ComicVolumeId { get; set; }

        public string Description { get; set; }

        public virtual Activity Activity { get; set; }
        public virtual ComicVolume ComicVolume { get; set; }
    }

    [Table("ComicVolume")]
    public class ComicVolume
    {
        [Key]
        public int Id { get; set; }
        public int SeriesId { get; set; }
        public string Chapters { get; set; }
        public int? AuthorId { get; set; }
        public int? TypeId { get; set; }
        public int? ScoreId { get; set; }

        [ForeignKey("SeriesId")]
        public Series Series { get; set; }
        [ForeignKey("AuthorId")]
        public Person Author { get; set; }
        [ForeignKey("TypeId")]
        public MediaType Type { get; set; }
        [ForeignKey("ScoreId")]
        public Score Score { get; set; }
    }
}

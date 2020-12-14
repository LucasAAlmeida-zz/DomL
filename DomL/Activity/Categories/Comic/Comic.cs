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
        [ForeignKey("Comic")]
        public int ComicId { get; set; }

        public string Description { get; set; }

        public virtual Activity Activity { get; set; }
        public virtual Comic Comic { get; set; }
    }

    [Table("Comic")]
    public class Comic
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Type { get; set; }
        public string Person { get; set; }
        public int? SeriesId { get; set; }
        public string Number { get; set; }
        public string Company { get; set; }
        public int Year { get; set; }
        public string Score { get; set; }

        [ForeignKey("SeriesId")]
        public Series Series { get; set; }
    }
}

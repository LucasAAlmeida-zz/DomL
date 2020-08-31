using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomL.Business.Entities
{
    [Table("BookActivity")]
    public class BookActivity
    {
        [Key]
        [ForeignKey("Activity")]
        public int Id { get; set; }
        [ForeignKey("Book")]
        public int BookId { get; set; }

        public string Description { get; set; }

        public virtual Activity Activity { get; set; }
        public virtual Book Book { get; set; }
    }

    [Table("Book")]
    public class Book
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public int? AuthorId { get; set; }
        public int? SeriesId { get; set; }
        public string NumberInSeries { get; set; }
        public int? ScoreId { get; set; }

        [ForeignKey("AuthorId")]
        public Person Author { get; set; }
        [ForeignKey("SeriesId")]
        public Series Series { get; set; }
        [ForeignKey("ScoreId")]
        public Score Score { get; set; }
    }
}

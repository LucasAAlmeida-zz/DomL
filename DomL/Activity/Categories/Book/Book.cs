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
        public string Author { get; set; }
        public int? SeriesId { get; set; }
        public string Number { get; set; }
        public string Publisher { get; set; }
        public int Year { get; set; }
        public string Score { get; set; }

        [ForeignKey("SeriesId")]
        public Series Series { get; set; }
    }
}

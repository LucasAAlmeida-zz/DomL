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
        public string Person { get; set; }
        public string Company { get; set; }
        public string Series { get; set; }
        public string Number { get; set; }
        public string Year { get; set; }
        public string Score { get; set; }
    }
}

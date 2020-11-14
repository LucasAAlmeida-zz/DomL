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
        [ForeignKey("Show")]
        public int ShowId { get; set; }

        public string Description { get; set; }

        public virtual Activity Activity { get; set; }
        public virtual Show Show { get; set; }
    }

    [Table("Show")]
    public class Show
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public int SeriesId { get; set; }
        public string Number { get; set; }
        public string Person { get; set; }
        public string Company { get; set; }
        public int Year { get; set; }
        public string Score { get; set; }

        [ForeignKey("SeriesId")]
        public Series Series { get; set; }
    }
}

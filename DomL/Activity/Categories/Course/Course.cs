using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomL.Business.Entities
{
    [Table("CourseActivity")]
    public class CourseActivity
    {
        [Key]
        [ForeignKey("Activity")]
        public int Id { get; set; }
        [ForeignKey("Course")]
        public int CourseId { get; set; }

        public string Description { get; set; }

        public virtual Activity Activity { get; set; }
        public virtual Course Course { get; set; }
    }

    [Table("Course")]
    public class Course
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; } // Area (Japones, Computacao, Preparo)
        public string Series { get; set; } // Materia (Python, Elasticsearch) (franchise seria o degree, ex pos, graduacao, etc)
        public string Number { get; set; }
        public string Person { get; set; }
        public string Company { get; set; }
        public int Year { get; set; }
        public string Score { get; set; }
    }
}
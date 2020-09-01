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
        public string Name { get; set; }
        public int SchoolId { get; set; }
        public int? TeacherId { get; set; }
        public int? ScoreId { get; set; }

        [ForeignKey("TeacherId")]
        public Person Teacher { get; set; }
        [ForeignKey("SchoolId")]
        public Company School { get; set; }
        [ForeignKey("ScoreId")]
        public Score Score { get; set; }
    }
}
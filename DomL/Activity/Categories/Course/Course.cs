﻿using System.ComponentModel.DataAnnotations;
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
        public string Professor { get; set; }
        public string Area { get; set; }
        public string Degree { get; set; }
        public string Number { get; set; }
        public string School { get; set; }
        public int Year { get; set; }
        public string Score { get; set; }
    }
}
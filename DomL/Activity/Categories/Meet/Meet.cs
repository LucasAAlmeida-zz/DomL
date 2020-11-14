using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomL.Business.Entities
{
    [Table("MeetActivity")]
    public class MeetActivity
    {
        [Key]
        [ForeignKey("Activity")]
        public int Id { get; set; }
        [Required]
        public string Person { get; set; }
        [Required]
        public string Origin { get; set; }
        public string Description { get; set; }

        public virtual Activity Activity { get; set; }
    }
}

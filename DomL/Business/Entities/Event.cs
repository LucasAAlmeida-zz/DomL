using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomL.Business.Entities
{
    [Table("EventActivity")]
    public class EventActivity
    {
        [Key]
        [ForeignKey("Activity")]
        public int Id { get; set; }
        [Required]
        public string Description { get; set; }
        public bool IsImportant { get; set; }

        public virtual Activity Activity { get; set; }
    }
}
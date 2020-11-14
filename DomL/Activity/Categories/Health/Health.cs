using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomL.Business.Entities
{
    [Table("HealthActivity")]
    public class HealthActivity
    {
        [Key]
        [ForeignKey("Activity")]
        public int Id { get; set; }
        public string Specialty { get; set; }
        [Required]
        public string Description { get; set; }

        public virtual Activity Activity { get; set; }
    }
}

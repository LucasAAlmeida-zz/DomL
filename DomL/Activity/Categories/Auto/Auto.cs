using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomL.Business.Entities
{
    [Table("AutoActivity")]
    public class AutoActivity
    {
        [Key]
        [ForeignKey("Activity")]
        public int Id { get; set; }
        [Required]
        public string Auto { get; set; }
        [Required]
        public string Description { get; set; }

        public virtual Activity Activity { get; set; }
    }
}

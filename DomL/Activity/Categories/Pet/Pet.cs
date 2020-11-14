using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomL.Business.Entities
{
    [Table("PetActivity")]
    public class PetActivity
    {
        [Key]
        [ForeignKey("Activity")]
        public int Id { get; set; }
        [Required]
        public string Pet { get; set; }
        [Required]
        public string Description { get; set; }

        public virtual Activity Activity { get; set; }
    }
}

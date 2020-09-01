using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomL.Business.Entities
{
    [Table("GiftActivity")]
    public class GiftActivity
    {
        [Key]
        [ForeignKey("Activity")]
        public int Id { get; set; }
        [Required]
        public string Gift { get; set; }
        public bool IsFrom { get; set; }
        [Required]
        public string Who { get; set; }

        public string Description { get; set; }

        public virtual Activity Activity { get; set; }
    }
}

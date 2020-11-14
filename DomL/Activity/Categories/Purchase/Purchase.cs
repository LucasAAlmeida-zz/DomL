using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomL.Business.Entities
{
    [Table("PurchaseActivity")]
    public class PurchaseActivity
    {
        [Key]
        [ForeignKey("Activity")]
        public int Id { get; set; }
        [Required]
        public string Store { get; set; }
        [Required]
        public string Product { get; set; }
        public int Value { get; set;}
        public string Description { get; set; }

        public virtual Activity Activity { get; set; }
    }
}

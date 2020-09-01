using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomL.Business.Entities
{
    [Table("PlayActivity")]
    public class PlayActivity
    {
        [Key]
        [ForeignKey("Activity")]
        public int Id { get; set; }
        [Required]
        public string Who { get; set; }
        public string Description { get; set; }

        public virtual Activity Activity { get; set; }
    }
}

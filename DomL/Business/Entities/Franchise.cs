using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomL.Business.Entities
{
    [Table("Franchise")]
    public class Franchise
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int? CreatorId { get; set; }

        [ForeignKey("CreatorId")]
        public Person Creator { get; set; }
    }
}

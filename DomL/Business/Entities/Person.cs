using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomL.Business.Entities
{
    [Table("Person")]
    public class Person
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Origin { get; set; }
    }
}

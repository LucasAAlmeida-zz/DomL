using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomL.Business.Entities
{
    [Table("Score")]
    public class Score
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int Value { get; set; }

        public override string ToString()
        {
            return (Value != 0) ? Value + " - " + Name : "";
        }

        public const int WORST_THING = 10;
        public const int HORRIBLE = 20;
        public const int REALLY_BAD = 30;
        public const int BAD = 40;
        public const int MEH = 50;
        public const int EHHH = 55;
        public const int OK = 60;
        public const int FINE = 65;
        public const int GOOD = 70;
        public const int NICE = 75;
        public const int REALLY_GOOD = 80;
        public const int WOW = 85;
        public const int AWESOME = 90;
        public const int OMG = 95;
        public const int BEST_THING = 100;
    }
}

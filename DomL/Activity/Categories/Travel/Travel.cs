using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomL.Business.Entities
{
    [Table("TravelActivity")]
    public class TravelActivity
    {
        [Key]
        [ForeignKey("Activity")]
        public int Id { get; set; }
        public int TransportId { get; set; }
        public int? OriginId { get; set; }
        public int DestinationId { get; set; }
        public string Description { get; set; }

        public virtual Activity Activity { get; set; }
        [ForeignKey("TransportId")]
        public Transport Transport { get; set; }
        [ForeignKey("OriginId")]
        public Location Origin { get; set; }
        [ForeignKey("DestinationId")]
        public Location Destination { get; set; }
    }
}

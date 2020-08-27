﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomL.Business.Entities
{
    [Table("Series")]
    public class Series
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int? FranchiseId { get; set; }

        [ForeignKey("FranchiseId")]
        public Franchise Franchise { get; set;}
    }
}
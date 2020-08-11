using DomL.Business.Utils.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomL.Business.Activities
{
    public abstract class Activity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int DayOrder { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [MaxLength(50)]
        public string Subject { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        public int? ActivityBlockId { get; set; }

        [ForeignKey("ActivityBlockId")]
        public ActivityBlock ActivityBlock { get; set; }

        public Activity(ActivityDTO atividadeDTO)
        {
            this.DayOrder = atividadeDTO.DayOrder;
            this.Date = atividadeDTO.Dia;
            this.ActivityBlockId = atividadeDTO.ActivityBlockId;
        }

        protected abstract void PopulateActivity(IReadOnlyList<string> segments);
        public abstract string ParseToString();
        public abstract void Save();
    }
}

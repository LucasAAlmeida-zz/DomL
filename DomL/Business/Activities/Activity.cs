using DomL.Business.Utils.DTOs;
using DomL.Business.Utils.Enums;
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

        public DateTime Dia { get; set; }

        [MaxLength(50)]
        public string Assunto { get; set; }

        [MaxLength(255)]
        public string Descricao { get; set; }
        
        [NotMapped]
        public Category Categoria { get; set; }
        [NotMapped]
        public bool IsInBlocoEspecial { get; set; }
        [NotMapped]
        public string FullLine { get; set; }

        public Activity(ActivityDTO atividadeDTO)
        {
            this.DayOrder = atividadeDTO.DayOrder;
            this.Dia = atividadeDTO.Dia;
            this.IsInBlocoEspecial = atividadeDTO.IsInBlocoEspecial;
            this.FullLine = atividadeDTO.FullLine;
        }

        protected abstract void ParseAtividade(IReadOnlyList<string> segmentos);
        protected abstract string ConsolidateActivity();
    }
}

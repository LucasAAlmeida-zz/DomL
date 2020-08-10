using DomL.Business.Utils;
using DomL.Business.Utils.DTOs;
using DomL.Business.Utils.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomL.Business.Activities.SingleDayActivities
{
    [Table("Movie")]
    public class Movie : SingleDayActivity
    {
        [Required]
        public string Nota { get; set; }

        public Movie(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos) 
        {
            this.Categoria = Category.Movie;
        }

        protected override void ParseAtividade(IReadOnlyList<string> segmentos)
        {
            // FILME; (Assunto) Título; (Nota)
            // FILME; (Assunto) Título; (Nota); (Descrição) O que achei

            this.Assunto = segmentos[1];
            this.Nota = segmentos[2];
            if (segmentos.Count == 4) {
                this.Descricao = segmentos[3];
            }
        }

        protected override string ConsolidateActivity()
        {
            return Util.GetDiaMes(this.Dia) + "\t" + this.Assunto + "\t" + this.Nota + "\t" + this.Descricao;
        }
    }
}

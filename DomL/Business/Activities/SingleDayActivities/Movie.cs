using DomL.Business.Utils;
using DomL.Business.Utils.DTOs;
using DomL.Business.Utils.Enums;
using System;
using System.Collections.Generic;

namespace DomL.Business.Activities.SingleDayActivities
{
    public class Movie : SingleDayActivity
    {
        public Movie(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos) 
        {
            this.Categoria = Category.Movie;
        }

        protected override void ParseAtividade(IReadOnlyList<string> segmentos)
        {
            // FILME; (Assunto) Título; (Valor) Nota
            // FILME; (Assunto) Título; (Valor) Nota; (Descrição) O que achei

            this.Assunto = segmentos[1];
            this.Valor = segmentos[2];
            if (segmentos.Count == 4) {
                this.Descricao = segmentos[3];
            }
        }

        protected override string ConsolidateActivity()
        {
            return Util.GetDiaMes(this.Dia) + "\t" + this.Assunto + "\t" + this.Valor + "\t" + this.Descricao;
        }
    }
}

using DomL.Business.Utils.DTOs;
using System.Collections.Generic;

namespace DomL.Business.Activities.SingleDayActivities
{
    public class Auto : SingleDayActivity
    {
        public Auto(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos) { }

        protected override void ParseAtividade(IReadOnlyList<string> segmentos)
        {
            //AUTO; (Assunto) Qual automovel; (Descricao) O que Aconteceu
            //AUTO; (Descricao) O que Aconteceu

            if (segmentos.Count == 2)
            {
                Descricao = segmentos[1];
            }
            else
            {
                Assunto = segmentos[1];
                Descricao = segmentos[2];
            }
        }

        protected override string ConsolidateActivity()
        {
            string diaMes = Dia.Day.ToString("00") + "/" + Dia.Month.ToString("00");
            return diaMes + "\t" + Assunto + "\t" + Descricao;
        }

        
    }
}

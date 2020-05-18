using DomL.Business.Utils.DTOs;
using System.Collections.Generic;

namespace DomL.Business.Activities.SingleDayActivities
{
    public class Pet : SingleDayActivity
    {
        public Pet(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos) { }

        protected override void ParseAtividade(IReadOnlyList<string> segmentos)
        {
            //PET; (Assunto) Qual Pet; (Descricao) O que Aconteceu
            //PET; (Descricao) O que Aconteceu

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

using DomL.Business.Utils.DTOs;
using System.Collections.Generic;

namespace DomL.Business.Activities.SingleDayActivities
{
    public class Play : SingleDayActivity
    {
        public Play(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos) { }

        protected override void ParseAtividade(IReadOnlyList<string> segmentos)
        {
            //PLAY; (Descricao) O que Aconteceu

            Assunto = segmentos[1];
            Descricao = segmentos[2];
        }

        protected override string ConsolidateActivity()
        {
            string diaMes = Dia.Day.ToString("00") + "/" + Dia.Month.ToString("00");
            return diaMes + "\t" + Assunto + "\t" + Descricao;
        }
    }
}

using DomL.Business.Utils.DTOs;
using System.Collections.Generic;

namespace DomL.Business.Activities.SingleDayActivities
{
    public class Work : SingleDayActivity
    {
        public Work(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos) { }

        protected override void ParseAtividade(IReadOnlyList<string> segmentos)
        {
            //WORK; (Descrição) O que aconteceu

            Descricao = segmentos[1];
        }

        protected override string ConsolidateActivity()
        {
            string diaMes = Dia.Day.ToString("00") + "/" + Dia.Month.ToString("00");
            return diaMes + "\t" + Descricao;
        }
    }
}

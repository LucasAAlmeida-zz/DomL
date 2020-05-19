using DomL.Business.Utils.DTOs;
using DomL.Business.Utils.Enums;
using System.Collections.Generic;

namespace DomL.Business.Activities.SingleDayActivities
{
    public class Travel : SingleDayActivity
    {
        public Travel(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos)
        {
            Categoria = Category.Travel;
        }

        protected override void ParseAtividade(IReadOnlyList<string> segmentos)
        {
            //VIAGEM; (Assunto) De onde pra onde; (MeioTransporte) Meio de transporte
            //VIAGEM; (Assunto) De onde pra onde; (MeioTransporte) Meio de transporte; (Descrição) o que aconteceu

            Assunto = segmentos[1];
            MeioTransporte = segmentos[2];
            if (segmentos.Count == 4)
            {
                Descricao = segmentos[3];
            }
        }

        protected override string ConsolidateActivity()
        {
            string diaMes = Dia.Day.ToString("00") + "/" + Dia.Month.ToString("00");
            return diaMes + "\t" + Assunto + "\t" + MeioTransporte + "\t" + Descricao;
        }
    }
}

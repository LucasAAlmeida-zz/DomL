using DomL.Business.Utils.DTOs;
using System.Collections.Generic;

namespace DomL.Business.Activities.SingleDayActivities
{
    public class Purchase : SingleDayActivity
    {
        public Purchase(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos) { }

        protected override void ParseAtividade(IReadOnlyList<string> segmentos)
        {
            //COMPRA; (De Quem) Loja; (Assunto) O que comprei; (Valor) Quanto custou
            //COMPRA; (De Quem) Loja; (Assunto) O que comprei; (Valor) Quanto custou; (Descrição) Misc

            DeQuem = segmentos[1];
            Assunto = segmentos[2];
            Valor = segmentos[3];
            if (segmentos.Count == 5)
            {
                Descricao = segmentos[4];
            }
        }

        protected override string ConsolidateActivity()
        {
            string diaMes = Dia.Day.ToString("00") + "/" + Dia.Month.ToString("00");
            return diaMes + "\t" + DeQuem + "\t" + Assunto + "\t" + Valor + "\t" + Descricao;
        }
    }
}

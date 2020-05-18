using DomL.Business.Utils.Enums;
using System.Collections.Generic;
using System.IO;

namespace DomL.Business.Activities.SingleDayActivities
{
    public class Travel : SingleDayActivity
    {
        readonly static Category categoria = Category.Travel;

        public void Parse(IReadOnlyList<string> segmentos)
        {
            //VIAGEM; (Assunto) De onde pra onde; (MeioTransporte) Meio de transporte
            //VIAGEM; (Assunto) De onde pra onde; (MeioTransporte) Meio de transporte; (Descrição) o que aconteceu

            Categoria = categoria;
            Assunto = segmentos[1];
            MeioTransporte = segmentos[2];
            if (segmentos.Count == 4)
            {
                Descricao = segmentos[3];
            }
        }

        protected override void ParseAtividadeVelha(string[] segmentos)
        {
            Assunto = segmentos[1];
            MeioTransporte = segmentos[2];
            Descricao = segmentos[3];
        }

        protected override string ConsolidateActivity()
        {
            string diaMes = Dia.Day.ToString("00") + "/" + Dia.Month.ToString("00");
            return diaMes + "\t" + Assunto + "\t" + MeioTransporte + "\t" + Descricao;
        }
    }
}

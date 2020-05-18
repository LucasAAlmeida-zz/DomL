using DomL.Business.Utils.DTOs;
using System.Collections.Generic;

namespace DomL.Business.Activities.SingleDayActivities
{
    public class Health : SingleDayActivity
    {
        public Health(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos) { }

        protected override void ParseAtividade(IReadOnlyList<string> segmentos)
        {
            //SAUDE; (Descrição) o que aconteceu
            //SAUDE; (Assunto) Especialidade médica; (Descrição) o que aconteceu

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
            string assunto = !string.IsNullOrWhiteSpace(Assunto) ? Assunto : "-";
            return diaMes + "\t" + assunto + "\t" + Descricao;
        }

    }
}

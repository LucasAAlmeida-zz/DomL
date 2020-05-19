using DomL.Business.Utils.DTOs;
using DomL.Business.Utils.Enums;
using System.Collections.Generic;

namespace DomL.Business.Activities.SingleDayActivities
{
    public class Gift : SingleDayActivity
    {
        public Gift(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos)
        {
            Categoria = Category.Gift;
        }

        protected override void ParseAtividade(IReadOnlyList<string> segmentos)
        {
            //GIFT; (Assunto) O que ganhei; (DeQuem) De quem ganhei o presente
            //GIFT; (Assunto) O que ganhei; (DeQuem) De quem ganhei o presente; (Descrição) o que aconteceu

            Assunto = segmentos[1];
            DeQuem = segmentos[2];
            if (segmentos.Count == 4)
            {
                Descricao = segmentos[3];
            }
        }

        protected override string ConsolidateActivity()
        {
            string diaMes = Dia.Day.ToString("00") + "/" + Dia.Month.ToString("00");
            return diaMes + "\t" + Assunto + "\t" + DeQuem + "\t" + Descricao;
        }
    }
}

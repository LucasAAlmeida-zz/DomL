using DomL.Business.Utils.Enums;
using System.Collections.Generic;
using System.IO;

namespace DomL.Business.Activities.SingleDayActivities
{
    public class Work : SingleDayActivity
    {
        readonly static Category categoria = Category.Work;

        public void Parse(IReadOnlyList<string> segmentos)
        {
            //WORK; (Descrição) O que aconteceu
            
            Categoria = categoria;
            Descricao = segmentos[1];
        }

        protected override void ParseAtividadeVelha(string[] segmentos)
        {
            Descricao = segmentos[1];
        }

        protected override string ConsolidateActivity()
        {
            string diaMes = Dia.Day.ToString("00") + "/" + Dia.Month.ToString("00");
            return diaMes + "\t" + Descricao;
        }
    }
}

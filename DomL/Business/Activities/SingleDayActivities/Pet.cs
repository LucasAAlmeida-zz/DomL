using DomL.Business.Utils.Enums;
using System.Collections.Generic;
using System.IO;

namespace DomL.Business.Activities.SingleDayActivities
{
    public class Pet : SingleDayActivity
    {
        readonly static Category categoria = Category.Pet;
        
        public void Parse(IReadOnlyList<string> segmentos)
        {
            //PET; (Assunto) Qual Pet; (Descricao) O que Aconteceu
            //PET; (Descricao) O que Aconteceu

            Categoria = categoria;
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

        protected override void ParseAtividadeVelha(string[] segmentos)
        {
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

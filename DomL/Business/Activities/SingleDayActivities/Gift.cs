using DomL.Business.Utils.Enums;
using System.Collections.Generic;
using System.IO;

namespace DomL.Business.Activities.SingleDayActivities
{
    public class Gift : SingleDayActivity
    {
        readonly static Category categoria = Category.Gift;

        public void Parse(IReadOnlyList<string> segmentos)
        {
            //GIFT; (Assunto) O que ganhei; (DeQuem) De quem ganhei o presente
            //GIFT; (Assunto) O que ganhei; (DeQuem) De quem ganhei o presente; (Descrição) o que aconteceu

            Categoria = categoria;
            Assunto = segmentos[1];
            DeQuem = segmentos[2];
            if (segmentos.Count == 4)
            {
                Descricao = segmentos[3];
            }
        }

        protected override void ParseAtividadeVelha(string[] segmentos)
        {
            Assunto = segmentos[1];
            DeQuem = segmentos[2];
            Descricao = segmentos[3];
        }

        protected override void WriteAtividadeConsolidada(StreamWriter file, string dia)
        {
            file.WriteLine(dia + "\t" + Assunto + "\t" + DeQuem + "\t" + Descricao);
        }
    }
}

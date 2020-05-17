using DomL.Business.Utils.Enums;
using System.Collections.Generic;
using System.IO;

namespace DomL.Business.Activities.SingleDayActivities
{
    public class Purchase : SingleDayActivity
    {
        readonly static Category categoria = Category.Purchase;

        public void Parse(IReadOnlyList<string> segmentos)
        {
            //COMPRA; (De Quem) Loja; (Assunto) O que comprei; (Valor) Quanto custou
            //COMPRA; (De Quem) Loja; (Assunto) O que comprei; (Valor) Quanto custou; (Descrição) Misc

            Categoria = categoria;
            DeQuem = segmentos[1];
            Assunto = segmentos[2];
            Valor = segmentos[3];
            if (segmentos.Count == 5)
            {
                Descricao = segmentos[4];
            }
        }

        protected override void ParseAtividadeVelha(string[] segmentos)
        {
            DeQuem = segmentos[1];
            Assunto = segmentos[2];
            Valor = segmentos[3];
            Descricao = segmentos[4];
        }

        protected override void WriteAtividadeConsolidada(StreamWriter file, string dia)
        {
            file.WriteLine(dia + "\t" + DeQuem + "\t" + Assunto + "\t" + Valor + "\t" + Descricao);
        }
    }
}

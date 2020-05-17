using DomL.Business.Utils.Enums;
using System.Collections.Generic;
using System.IO;

namespace DomL.Business.Activities.SingleDayActivities
{
    public class Play : SingleDayActivity
    {
        readonly static Category categoria = Category.Play;
        
        public void Parse(IReadOnlyList<string> segmentos)
        {
            //PLAY; (Assunto) Quem; (Descricao) O que Aconteceu
            //PLAY; (Descricao) O que Aconteceu

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

        protected override void WriteAtividadeConsolidada(StreamWriter file, string dia)
        {
            file.WriteLine(dia + "\t" + Assunto + "\t" + Descricao);
        }

    }
}

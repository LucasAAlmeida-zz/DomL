using DomL.Business.Utils.Enums;
using System.Collections.Generic;
using System.IO;

namespace DomL.Business.Activities.SingleDayActivities
{
    public class Health : SingleDayActivity
    {
        readonly static Category categoria = Category.Health;

        public void Parse(IReadOnlyList<string> segmentos)
        {
            //SAUDE; (Descrição) o que aconteceu
            //SAUDE; (Assunto) Especialidade médica; (Descrição) o que aconteceu

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
            file.WriteLine(dia + "\t" + (!string.IsNullOrWhiteSpace(Assunto) ? Assunto : "-") + "\t" + Descricao);
        }
    }
}

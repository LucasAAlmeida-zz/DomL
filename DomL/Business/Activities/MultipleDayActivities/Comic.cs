using DomL.Business.Utils.DTOs;
using DomL.Business.Utils.Enums;

namespace DomL.Business.Activities.MultipleDayActivities
{
    public class Comic : MultipleDayActivity
    {
        public Comic(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos)
        {
            this.Categoria = Category.Comic;
        }

        // COMIC|MANGA; (De Quem) Autor; (Assunto) Título; (Classificação) Término; (Valor) Nota; (Descrição) O que achei
    }
}

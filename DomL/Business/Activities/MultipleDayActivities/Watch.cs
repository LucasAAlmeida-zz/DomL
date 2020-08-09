using DomL.Business.Utils.DTOs;
using DomL.Business.Utils.Enums;

namespace DomL.Business.Activities.MultipleDayActivities
{
    public class Watch : MultipleDayActivity
    {
        public Watch(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos)
        {
            this.Categoria = Category.Watch;
        }

        // WATCH; (De Quem) Plataforma; (Assunto) Título; (Classificação) Término; (Valor) Nota; (Descrição) O que achei
    }
}

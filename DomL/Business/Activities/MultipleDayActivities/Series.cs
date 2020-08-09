using DomL.Business.Utils.DTOs;
using DomL.Business.Utils.Enums;

namespace DomL.Business.Activities.MultipleDayActivities
{
    public class Series : MultipleDayActivity
    {
        public Series(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos)
        {
            this.Categoria = Category.Series;
        }

        // SERIE; (De Quem) Produtora; (Assunto) Título; (Classificação) Término; (Valor) Nota; (Descrição) O que achei
    }
}

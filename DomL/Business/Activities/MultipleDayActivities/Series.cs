using DomL.Business.Utils.DTOs;
using DomL.Business.Utils.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomL.Business.Activities.MultipleDayActivities
{
    [Table("Series")]
    public class Series : MultipleDayActivity
    {
        public Series(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos)
        {
            this.Categoria = Category.Series;
        }

        // SERIE; (De Quem) Produtora; (Assunto) Título; (Classificação) Término; (Valor) Nota; (Descrição) O que achei
    }
}

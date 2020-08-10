using DomL.Business.Utils.DTOs;
using DomL.Business.Utils.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomL.Business.Activities.MultipleDayActivities
{
    [Table("Watch")]
    public class Watch : MultipleDayActivity
    {
        public Watch(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos)
        {
            this.Categoria = Category.Watch;
        }

        // WATCH; (De Quem) Plataforma; (Assunto) Título; (Classificação) Término; (Valor) Nota; (Descrição) O que achei
    }
}

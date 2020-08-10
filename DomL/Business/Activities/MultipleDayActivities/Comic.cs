using DomL.Business.Utils.DTOs;
using DomL.Business.Utils.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomL.Business.Activities.MultipleDayActivities
{
    [Table("Comic")]
    public class Comic : MultipleDayActivity
    {
        public Comic(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos)
        {
            this.Categoria = Category.Comic;
        }

        // COMIC|MANGA; (De Quem) Autor; (Assunto) Título; (Classificação) Término; (Valor) Nota; (Descrição) O que achei
    }
}

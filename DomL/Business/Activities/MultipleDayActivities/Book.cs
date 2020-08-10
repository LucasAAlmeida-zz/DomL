using DomL.Business.Utils.DTOs;
using DomL.Business.Utils.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomL.Business.Activities.MultipleDayActivities
{
    [Table("Book")]
    public class Book : MultipleDayActivity
    {
        public Book(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos)
        {
            this.Categoria = Category.Book;
        }

        // BOOK; (De Quem) Autor; (Assunto) Título; (Classificação) Término; (Valor) Nota; (Descrição) O que achei
    }
}

using DomL.Business.Utils.DTOs;
using DomL.Business.Utils.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomL.Business.Activities.MultipleDayActivities
{
    [Table("Game")]
    public class Game : MultipleDayActivity
    {
        public Game(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos)
        {
            this.Categoria = Category.Game;
        }

        // JOGO; (De Quem) Plataforma; (Assunto) Título; (Classificação) Término; (Valor) Nota; (Descrição) O que achei
    }
}

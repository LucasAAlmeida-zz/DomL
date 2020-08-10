using DomL.Business.Utils;
using DomL.Business.Utils.DTOs;
using DomL.Business.Utils.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomL.Business.Activities.SingleDayActivities
{
    [Table("Pet")]
    public class Pet : SingleDayActivity
    {
        public Pet(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos)
        {
            this.Categoria = Category.Pet;
        }

        protected override void ParseAtividade(IReadOnlyList<string> segmentos)
        {
            //PET; (Assunto) Qual Pet; (Descricao) O que Aconteceu

            this.Assunto = segmentos[1];
            this.Descricao = segmentos[2];
        }

        protected override string ConsolidateActivity()
        {
            return Util.GetDiaMes(this.Dia) + "\t" + this.Assunto + "\t" + this.Descricao;
        }
    }
}

using DomL.Business.Utils;
using DomL.Business.Utils.DTOs;
using DomL.Business.Utils.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomL.Business.Activities.SingleDayActivities
{
    [Table("Work")]
    public class Work : SingleDayActivity
    {
        public Work(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos)
        {
            this.Categoria = Category.Work;
        }

        protected override void ParseAtividade(IReadOnlyList<string> segmentos)
        {
            //WORK; (Descrição) O que aconteceu

            this.Descricao = segmentos[1];
        }

        protected override string ConsolidateActivity()
        {
            return Util.GetDiaMes(this.Dia) + "\t" + this.Descricao;
        }
    }
}

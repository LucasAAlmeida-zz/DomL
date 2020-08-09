using DomL.Business.Utils;
using DomL.Business.Utils.DTOs;
using DomL.Business.Utils.Enums;
using System.Collections.Generic;

namespace DomL.Business.Activities.SingleDayActivities
{
    public class Doom : SingleDayActivity
    {
        public Doom(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos)
        {
            this.Categoria = Category.Doom;
        }

        protected override void ParseAtividade(IReadOnlyList<string> segmentos)
        {
            //DOOM; (Descrição) O que aconteceu

            this.Descricao = segmentos[1];
        }

        protected override string ConsolidateActivity()
        {
            return Util.GetDiaMes(this.Dia) + "\t" + this.Descricao;
        }

    }
}
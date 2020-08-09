using DomL.Business.Utils;
using DomL.Business.Utils.DTOs;
using DomL.Business.Utils.Enums;
using System.Collections.Generic;

namespace DomL.Business.Activities.SingleDayActivities
{
    public class Play : SingleDayActivity
    {
        public Play(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos)
        {
            this.Categoria = Category.Play;
        }

        protected override void ParseAtividade(IReadOnlyList<string> segmentos)
        {
            //PLAY; (Assunto) Pessoa; (Descricao) O que Aconteceu

            this.Assunto = segmentos[1];
            this.Descricao = segmentos[2];
        }

        protected override string ConsolidateActivity()
        {
            return Util.GetDiaMes(this.Dia) + "\t" + this.Assunto + "\t" + this.Descricao;
        }
    }
}

using DomL.Business.Utils;
using DomL.Business.Utils.DTOs;
using DomL.Business.Utils.Enums;
using System.Collections.Generic;

namespace DomL.Business.Activities.SingleDayActivities
{
    public class Travel : SingleDayActivity
    {
        public string MeioTransporte { get; set; }

        public Travel(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos)
        {
            this.Categoria = Category.Travel;
        }

        protected override void ParseAtividade(IReadOnlyList<string> segmentos)
        {
            //VIAGEM; (Assunto) De onde pra onde; (MeioTransporte)
            //VIAGEM; (Assunto) De onde pra onde; (MeioTransporte); (Descrição) o que aconteceu

            this.Assunto = segmentos[1];
            this.MeioTransporte = segmentos[2];
            if (segmentos.Count == 4)
            {
                this.Descricao = segmentos[3];
            }
        }

        protected override string ConsolidateActivity()
        {
            return Util.GetDiaMes(this.Dia) + "\t" + this.Assunto + "\t" + this.MeioTransporte + "\t" + this.Descricao;
        }
    }
}

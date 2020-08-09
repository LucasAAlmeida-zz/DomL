using DomL.Business.Utils;
using DomL.Business.Utils.DTOs;
using DomL.Business.Utils.Enums;
using System.Collections.Generic;

namespace DomL.Business.Activities.SingleDayActivities
{
    public class Purchase : SingleDayActivity
    {
        public Purchase(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos)
        {
            this.Categoria = Category.Purchase;
        }

        protected override void ParseAtividade(IReadOnlyList<string> segmentos)
        {
            //COMPRA; (De Quem) Loja; (Assunto) O que comprei; (Valor) Quanto custou
            //COMPRA; (De Quem) Loja; (Assunto) O que comprei; (Valor) Quanto custou; (Descrição) Misc

            this.DeQuem = segmentos[1];
            this.Assunto = segmentos[2];
            this.Valor = segmentos[3];
            if (segmentos.Count == 5)
            {
                this.Descricao = segmentos[4];
            }
        }

        protected override string ConsolidateActivity()
        {
            return Util.GetDiaMes(this.Dia) + "\t" + this.DeQuem + "\t" + this.Assunto + "\t" + this.Valor + "\t" + this.Descricao;
        }
    }
}

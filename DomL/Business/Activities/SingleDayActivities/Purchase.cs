using DomL.Business.Utils;
using DomL.Business.Utils.DTOs;
using DomL.Business.Utils.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomL.Business.Activities.SingleDayActivities
{
    [Table("Purchase")]
    public class Purchase : SingleDayActivity
    {
        [Required]
        public string Loja { get; set; }
        [Required]
        public string Valor { get; set; }

        public Purchase(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos)
        {
            this.Categoria = Category.Purchase;
        }

        protected override void ParseAtividade(IReadOnlyList<string> segmentos)
        {
            //COMPRA; (Loja); (Assunto) O que comprei; (Valor) Quanto custou
            //COMPRA; (Loja); (Assunto) O que comprei; (Valor) Quanto custou; (Descrição) Misc

            this.Loja = segmentos[1];
            this.Assunto = segmentos[2];
            this.Valor = segmentos[3];
            if (segmentos.Count == 5)
            {
                this.Descricao = segmentos[4];
            }
        }

        protected override string ConsolidateActivity()
        {
            return Util.GetDiaMes(this.Dia) + "\t" + this.Loja + "\t" + this.Assunto + "\t" + this.Valor + "\t" + this.Descricao;
        }
    }
}

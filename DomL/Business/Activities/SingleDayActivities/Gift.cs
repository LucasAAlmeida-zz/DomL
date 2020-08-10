using DomL.Business.Utils;
using DomL.Business.Utils.DTOs;
using DomL.Business.Utils.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomL.Business.Activities.SingleDayActivities
{
    [Table("Gift")]
    public class Gift : SingleDayActivity
    {
        [Required]
        public string DeQuem { get; set; }

        public Gift(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos)
        {
            this.Categoria = Category.Gift;
        }

        protected override void ParseAtividade(IReadOnlyList<string> segmentos)
        {
            //GIFT; (Assunto) O que ganhei; (DeQuem) De quem ganhei o presente
            //GIFT; (Assunto) O que ganhei; (DeQuem) De quem ganhei o presente; (Descrição) o que aconteceu

            this.Assunto = segmentos[1];
            this.DeQuem = segmentos[2];
            if (segmentos.Count == 4)
            {
                this.Descricao = segmentos[3];
            }
        }

        protected override string ConsolidateActivity()
        {
            return Util.GetDiaMes(this.Dia) + "\t" + this.Assunto + "\t" + this.DeQuem + "\t" + this.Descricao;
        }
    }
}

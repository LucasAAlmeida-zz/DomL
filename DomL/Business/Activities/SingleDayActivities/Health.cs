using DomL.Business.Utils;
using DomL.Business.Utils.DTOs;
using DomL.Business.Utils.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomL.Business.Activities.SingleDayActivities
{
    [Table("Health")]
    public class Health : SingleDayActivity
    {
        public Health(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos)
        {
            this.Categoria = Category.Health;
        }

        protected override void ParseAtividade(IReadOnlyList<string> segmentos)
        {
            //SAUDE; (Descrição) o que aconteceu
            //SAUDE; (Assunto) Especialidade médica; (Descrição) o que aconteceu

            if (segmentos.Count == 2)
            {
                this.Descricao = segmentos[1];
            }
            else
            {
                this.Assunto = segmentos[1];
                this.Descricao = segmentos[2];
            }
        }

        protected override string ConsolidateActivity()
        {
            string assunto = !string.IsNullOrWhiteSpace(this.Assunto) ? this.Assunto : "-";
            return Util.GetDiaMes(this.Dia) + "\t" + assunto + "\t" + this.Descricao;
        }

    }
}

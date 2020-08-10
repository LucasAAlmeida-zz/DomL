using DomL.Business.Utils;
using DomL.Business.Utils.DTOs;
using DomL.Business.Utils.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomL.Business.Activities.SingleDayActivities
{
    [Table("Person")]
    public class Person : SingleDayActivity
    {
        [Required]
        public string Origem { get; set; }

        public Person(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos)
        {
            this.Categoria = Category.Person;
        }

        protected override void ParseAtividade(IReadOnlyList<string> segmentos)
        {
            //PESSOA; (Assunto) Nome da Pessoa; (Origem) De onde conheci (amigo de x, furry, etc); (Descrição) Coisas pra me lembrar

            this.Assunto = segmentos[1];
            this.Origem = segmentos[2];
            this.Descricao = segmentos[3];
        }

        protected override string ConsolidateActivity()
        {
            return Util.GetDiaMes(this.Dia) + "\t" + this.Assunto + "\t" + this.Origem + "\t" + this.Descricao;
        }

    }
}

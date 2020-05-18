using DomL.Business.Utils.DTOs;
using System.Collections.Generic;

namespace DomL.Business.Activities.SingleDayActivities
{
    public class Person : SingleDayActivity
    {
        public Person(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos) { }

        protected override void ParseAtividade(IReadOnlyList<string> segmentos)
        {
            //PESSOA; (Assunto) Nome da Pessoa; (DeQuem) Origem conheci (amigo de x, furry, etc); (Descrição) Coisas pra me lembrar

            Assunto = segmentos[1];
            DeQuem = segmentos[2];
            Descricao = segmentos[3];
        }

        protected override string ConsolidateActivity()
        {
            string diaMes = Dia.Day.ToString("00") + "/" + Dia.Month.ToString("00");
            string deQuem = !string.IsNullOrWhiteSpace(DeQuem) ? DeQuem : "-";
            return diaMes + "\t" + Assunto + "\t" + deQuem + "\t" + Descricao;
        }

    }
}

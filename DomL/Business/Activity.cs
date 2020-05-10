using DomL.Business.Enums;
using System;

namespace DomL.Business
{
    public class Activity
    {
        public DateTime Dia { get; set; }
        public DateTime? DiaFim { get; set; }
        public Category Categoria { get; set; }
        public string Assunto { get; set; }
        public string Descricao { get; set; }
        public Classification Classificacao { get; set; }
        public bool IsInBlocoEspecial { get; set; }
        public string Companhia { get; set; }

        public string DeQuem { get; set; }
        public string MeioTransporte { get; set; }
        public string Valor { get; set; }

        public string FullLine { get; set; }
    }
}

using DomL.Business.Utils.DTOs;
using DomL.Business.Utils.Enums;
using System;
using System.Collections.Generic;

namespace DomL.Business.Activities
{
    public abstract class Activity
    {
        public Activity(ActivityDTO atividadeDTO)
        {
            Dia = atividadeDTO.Dia;
            IsInBlocoEspecial = atividadeDTO.IsInBlocoEspecial;
            FullLine = atividadeDTO.FullLine;
        }

        public DateTime Dia { get; set; }
        public Category Categoria { get; set; }
        public string Assunto { get; set; }
        public string Descricao { get; set; }
        public bool IsInBlocoEspecial { get; set; }
        public string Companhia { get; set; }

        public Classification Classificacao { get; set; }

        public string DeQuem { get; set; }
        public string MeioTransporte { get; set; }
        public string Valor { get; set; }

        public string FullLine { get; set; }

        protected abstract void ParseAtividade(IReadOnlyList<string> segmentos);
        protected abstract string ConsolidateActivity();
    }
}

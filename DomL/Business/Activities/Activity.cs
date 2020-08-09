using DomL.Business.Utils.DTOs;
using DomL.Business.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DomL.Business.Activities
{
    public abstract class Activity
    {
        public Activity(ActivityDTO atividadeDTO)
        {
            this.Dia = atividadeDTO.Dia;
            this.IsInBlocoEspecial = atividadeDTO.IsInBlocoEspecial;
            this.FullLine = atividadeDTO.FullLine;
        }

        public DateTime Dia { get; set; }
        public Category Categoria { get; set; }
        public string Assunto { get; set; }
        public string Descricao { get; set; }
        public bool IsInBlocoEspecial { get; set; }
        public string FullLine { get; set; }

        protected abstract void ParseAtividade(IReadOnlyList<string> segmentos);
        protected abstract string ConsolidateActivity();
    }
}

using DomL.Business.Utils.Enums;
using System;

namespace DomL.Business.Utils.DTOs
{
    public class ActivityDTO
    {
        public DateTime Dia { get; set; }
        public bool IsInBlocoEspecial { get; set; }
        public string FullLine { get; set; }
        public Classification Classificacao { get; set; }

        public bool IsNewActivity { get; set; }
    }
}

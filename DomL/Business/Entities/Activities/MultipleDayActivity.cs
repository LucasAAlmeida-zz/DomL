using DomL.Business.Utils;
using DomL.Business.Utils.DTOs;
using DomL.Business.Utils.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;

namespace DomL.Business.Activities
{
    public abstract class MultipleDayActivity : Activity
    {
        [Required]
        [MaxLength(50)]
        public string DeQuem { get; set; }

        [Required]
        public Classification Classificacao { get; set; }

        public int? Nota { get; set; }

        [NotMapped]
        public DateTime? DiaTermino { get; set; }

        public MultipleDayActivity(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos) { }
        public MultipleDayActivity() { }

        protected override void PopulateActivity(IReadOnlyList<string> segmentos)
        {
            // 4-   (Categoria)  (De Quem)   (Assunto)   (Classificação) C
            // 5-   (Categoria)  (De Quem)   (Assunto)   (Classificação) C  (Descrição)
            // 5-   (Categoria)  (De Quem)   (Assunto)   (Classificação) T  (Nota)
            // 5-   (Categoria)  (De Quem)   (Assunto)   (Classificação) U  (Nota)
            // 6-   (Categoria)  (De Quem)   (Assunto)   (Classificação) T  (Nota)      (Descrição)
            // 6-   (Categoria)  (De Quem)   (Assunto)   (Classificação) U  (Nota)      (Descrição)

            this.DeQuem = segmentos[1];
            this.Subject = segmentos[2];

            string segmentoToLower = segmentos[3].ToLower();
            if (segmentoToLower == "comeco" || segmentoToLower == "começo") {
                this.Classificacao = Classification.Comeco;
            } else if (segmentoToLower == "termino" || segmentoToLower == "término") {
                this.Classificacao = Classification.Termino;
            } else {
                this.Classificacao = Classification.Unica;
            }

            switch (segmentos.Count) {
                case 5:
                    int nota;
                    if (int.TryParse(segmentos[4], out nota)) {
                        this.Nota = nota;
                    } else {
                        this.Description = segmentos[4];
                    }
                    break;
                case 6:
                    this.Nota = int.Parse(segmentos[4]);
                    this.Description = segmentos[5];
                    break;
            }
        }

        protected static void EscreveConsolidadasNoArquivo(string filePath, List<MultipleDayActivity> activities)
        {
            using (var file = new StreamWriter(filePath)) {
                foreach (var activity in activities) {
                    switch (activity.Classificacao) {
                        case Classification.Unica:
                            activity.DiaTermino = activity.Date;
                            break;

                        case Classification.Comeco:
                            var activityTermino = activities.FirstOrDefault(a => a.Classificacao == Classification.Termino && Util.IsEqualTitle(a.Subject, activity.Subject));
                            if (activityTermino != null) {
                                activity.DiaTermino = activityTermino.Date;
                                activity.Nota = activityTermino.Nota;
                                if (string.IsNullOrWhiteSpace(activity.Description)) {
                                    activity.Description = activity.Description + ", " + activityTermino.Description;
                                }
                            }
                            break;

                        case Classification.Termino:
                            //Pra não fazer duas vezes a mesma atividade
                            Activity activityComeco = activities.FirstOrDefault(a => a.Classificacao == Classification.Comeco && Util.IsEqualTitle(a.Subject, activity.Subject));
                            if (activityComeco != null) {
                                continue;
                            }

                            activity.DiaTermino = activity.Date;
                            activity.Date = DateTime.MinValue;
                            break;
                    }

                    var consolidatedBook = activity.ParseConsolidatedToString();
                    file.WriteLine(consolidatedBook);
                }
            }
        }

        public override string ParseToString()
        {
            var nota = this.Nota != null ? this.Nota.ToString() : "-";
            var descricao = !string.IsNullOrWhiteSpace(this.Description) ? this.Description : "-";
            return Util.GetDiaMes(this.Date) + "\t" + this.Classificacao + "\t" + this.DeQuem + "\t" + this.Subject + "\t" + nota + "\t" + descricao;
        }

        public string ParseConsolidatedToString()
        {
            var diaComeco = this.Date != DateTime.MinValue ? Util.GetDiaMes(this.Date) : "??/??";
            var diaTermino = this.DiaTermino.HasValue ? Util.GetDiaMes(this.DiaTermino.Value) : "??/??";
            var nota = this.Nota != null ? this.Nota.ToString() : "-";
            var descricao = !string.IsNullOrWhiteSpace(this.Description) ? this.Description : "-";
            return diaComeco + "\t" + diaTermino + "\t" + this.DeQuem + "\t" + this.Subject + "\t" + nota + "\t" + descricao;
        }
    }
}

using DomL.Business.Utils;
using DomL.Business.Utils.DTOs;
using DomL.Business.Utils.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;

namespace DomL.Business.Activities
{
    public abstract class MultipleDayActivity : Activity
    {
        [Required]
        [MaxLength(50)]
        public string DeQuem { get; set; }

        [Required]
        public Classification Classificacao { get; set; }

        public int Nota { get; set; }

        [NotMapped]
        public DateTime? DiaTermino { get; set; }

        public MultipleDayActivity(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos) { }

        protected override void PopulateActivity(IReadOnlyList<string> segmentos)
        {
            // (Categoria); (De Quem); (Assunto); (Nota)
            // (Categoria); (De Quem); (Assunto); (Classificação) Começo
            // (Categoria); (De Quem); (Assunto); (Nota); (Descrição)
            // (Categoria); (De Quem); (Assunto); (Classificação) Termino; (Nota)
            // (Categoria); (De Quem); (Assunto); (Classificação) Termino; (Nota); (Descrição)

            this.DeQuem = segmentos[1];
            this.Subject = segmentos[2];
            string segmentoToLower = segmentos[3].ToLower();
            string classificacao = "unica";
            switch (segmentos.Count) {
                case 4:
                    if (segmentoToLower == "comeco" || segmentoToLower == "começo") {
                        classificacao = segmentoToLower;
                    } else {
                        this.Nota = int.Parse(segmentos[3]);
                    }
                    break;
                case 5:
                    if (segmentoToLower == "termino" || segmentoToLower == "término") {
                        classificacao = segmentoToLower;
                        this.Nota = int.Parse(segmentos[4]);
                    } else {
                        this.Nota = int.Parse(segmentos[3]);
                        this.Description = segmentos[4];
                    }
                    break;
                case 6:
                    classificacao = segmentos[3].ToLower();
                    this.Nota = int.Parse(segmentos[4]);
                    this.Description = segmentos[5];
                    break;
                default:
                    throw new Exception("what");
            }

            switch (classificacao) {
                case "comeco":
                case "começo":
                    this.Classificacao = Classification.Comeco;
                    break;
                case "termino":
                case "término":
                    this.Classificacao = Classification.Termino;
                    break;
                case "unica":
                    this.Classificacao = Classification.Unica;
                    break;
                default:
                    throw new Exception("what");
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
            return Util.GetDiaMes(this.Date) + "\t" + this.Classificacao + "\t" + this.DeQuem + "\t" + this.Subject + "\t" + this.Nota + "\t" + this.Description;
        }

        public string ParseConsolidatedToString()
        {
            var diaComeco = this.Date != DateTime.MinValue ? Util.GetDiaMes(this.Date) : "??/??";
            var diaTermino = this.DiaTermino.HasValue ? Util.GetDiaMes(this.DiaTermino.Value) : "??/??";
            return diaComeco + "\t" + diaTermino + "\t" + this.DeQuem + "\t" + this.Subject + "\t" + this.Nota + "\t" + this.Description;
        }
    }
}

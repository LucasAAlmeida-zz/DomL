using DomL.Business.Utils;
using DomL.Business.Utils.DTOs;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace DomL.Business.Activities.SpecialActivities
{
    public class Event : SpecialActivity
    {
        public Event(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos) { }

        public bool shouldSave = false;

        protected override void ParseAtividade(IReadOnlyList<string> segmentos)
        {
            this.Descricao = segmentos[0];
            this.FullLine = this.Descricao;

            if (this.Descricao.StartsWith("*")) {
                this.Descricao = this.Descricao.Substring(1);
                this.shouldSave = true;
            } else if (this.Descricao.StartsWith("<")) {
                this.IsInBlocoEspecial = true;
            } else if (this.Descricao.StartsWith("---")) {
                this.IsInBlocoEspecial = false;
            }
        }

        protected override void ParseAtividadeVelha(string[] segmentos)
        {
            this.Descricao = segmentos[1];
            this.FullLine = this.Descricao;
        }

        public new static void Consolidate(List<Activity> newEventActivities, string fileDir, int year)
        {
            string filePath = fileDir + "Events.txt";
            var atividadesVelhas = GetAtividadesVelhas(filePath, year);
            atividadesVelhas.AddRange(Util.GetAtividadesToAdd(newEventActivities, atividadesVelhas));
            var allAtividadesCategoria = atividadesVelhas;
            EscreverNoArquivo(filePath, allAtividadesCategoria);
        }

        public static List<Activity> GetAtividadesVelhas(string filePath, int year)
        {
            var atividadesVelhas = new List<Activity>();

            if (File.Exists(filePath)) {
                using (var reader = new StreamReader(filePath)) {
                    string line;
                    while ((line = reader.ReadLine()) != null) {
                        var segmentos = Regex.Split(line, "\t");

                        ActivityDTO atividadeVelhaDTO = Util.GetAtividadeVelha(segmentos[0], year);

                        atividadesVelhas.Add(new Event(atividadeVelhaDTO, segmentos));
                    }
                }
            }

            return atividadesVelhas;
        }

        private static void EscreverNoArquivo(string filePath, List<Activity> allAtividadesCategoria)
        {
            using (var file = new StreamWriter(filePath)) {
                foreach (Event atividade in allAtividadesCategoria) {
                    if (atividade.IsInBlocoEspecial && !atividade.FullLine.StartsWith("<")) {
                        continue;
                    }

                    var consolidatedActivity = atividade.ConsolidateActivity();
                    file.WriteLine(consolidatedActivity);
                }
            }
        }

        protected override string ConsolidateActivity()
        {
            return Util.GetDiaMes(this.Dia) + "\t" + this.FullLine;
        }
    }
}

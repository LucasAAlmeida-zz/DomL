using DomL.Business.Utils;
using DomL.Business.Utils.DTOs;
using DomL.Business.Utils.Enums;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace DomL.Business.Activities.SpecialActivities
{
    public class Event : SpecialActivity
    {
        public Event(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos) { }

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

            if (File.Exists(filePath))
            {
                using (var reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var segmentos = Regex.Split(line, "\t");

                        ActivityDTO atividadeVelhaDTO = Util.GetAtividadeVelha(segmentos[0], year, Category.Event);

                        atividadesVelhas.Add(new Event(atividadeVelhaDTO, segmentos));
                    }
                }
            }

            return atividadesVelhas;
        }

        private static void EscreverNoArquivo(string filePath, List<Activity> allAtividadesCategoria)
        {
            using (var file = new StreamWriter(filePath))
            {
                foreach (Event atividade in allAtividadesCategoria)
                {
                    string dia = atividade.Dia.Day.ToString("00") + "/" + atividade.Dia.Month.ToString("00");

                    if (atividade.IsInBlocoEspecial && !atividade.FullLine.StartsWith("<"))
                    {
                        continue;
                    }

                    var consolidatedActivity = atividade.ConsolidateActivity();
                    file.WriteLine(consolidatedActivity);
                }
            }
        }

        protected override void ParseAtividade(IReadOnlyList<string> segmentos)
        {
            Descricao = segmentos[0];

            if (Descricao.StartsWith("*"))
            {
                Descricao = Descricao.Substring(1);
                FullLine = Descricao;
            }
            else if (Descricao.StartsWith("<"))
            {
                IsInBlocoEspecial = true;
            }
            else if (Descricao.StartsWith("---"))
            {
                IsInBlocoEspecial = false;
            }
        }

        protected override string ConsolidateActivity()
        {
            return Dia + "\t" + FullLine;
        }
    }
}

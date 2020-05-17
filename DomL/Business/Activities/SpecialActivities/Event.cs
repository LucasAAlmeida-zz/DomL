using DomL.Business.Utils;
using DomL.Business.Utils.DTOs;
using DomL.Business.Utils.Enums;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DomL.Business.Activities.SpecialActivities
{
    public class Event : SpecialActivity
    {
        public static void Consolidate(ConsolidateDTO consolidateDTO)
        {
            var filePath = consolidateDTO.fileDir + "Event.txt";
            var atividadesVelhas = GetAtividadesVelhas(filePath, consolidateDTO.year);

            var atividadesNovas = consolidateDTO.allNewAtividades.Where(ad => ad.IsInBlocoEspecial).ToList();
            atividadesVelhas.AddRange(Util.GetAtividadesToAdd(atividadesNovas, atividadesVelhas));

            var allAtividadesCategoria = atividadesVelhas;
            EscreverNoArquivo(filePath, allAtividadesCategoria);
        }

        private static List<Activity> GetAtividadesVelhas(string filePath, int year)
        {
            var atividadesVelhas = new List<Activity>();

            if (File.Exists(filePath))
            {
                using (var reader = new StreamReader(filePath))
                {
                    string line = reader.ReadLine();
                    while (line == "")
                    {
                        line = reader.ReadLine();
                    }
                    string diaMesStr = "";
                    while (line != null)
                    {
                        var segmentos = Regex.Split(line, "\t");
                        if (!string.IsNullOrWhiteSpace(segmentos[0]))
                        {
                            diaMesStr = segmentos[0];
                        }

                        Activity atividadeVelha = Util.GetAtividadeVelha(diaMesStr, year, Category.Undefined);
                        atividadeVelha.FullLine = segmentos[1];
                        atividadeVelha.IsInBlocoEspecial = true;

                        line = reader.ReadLine();
                        while (line == "")
                        {
                            line = reader.ReadLine();
                        }
                        atividadesVelhas.Add(atividadeVelha);
                    }
                }
            }

            return atividadesVelhas;
        }

        private static void EscreverNoArquivo(string filePath, List<Activity> allAtividadesCategoria)
        {
            using (var file = new StreamWriter(filePath))
            {
                int dia = allAtividadesCategoria.First().Dia.Day;
                foreach (Activity atividade in allAtividadesCategoria)
                {
                    string diaStr = atividade.Dia.Day.ToString("00") + "/" + atividade.Dia.Month.ToString("00");
                    if (atividade.FullLine.StartsWith("<"))
                    {
                        if (!atividade.FullLine.StartsWith("<END>") && !atividade.FullLine.StartsWith("<FIM>"))
                        {
                            // Tag de começo de bloco especial
                            dia = atividade.Dia.Day;
                            file.WriteLine(diaStr + "\t" + atividade.FullLine);
                        }
                        else
                        {
                            // Tag de fim de bloco especial
                            file.WriteLine("\t" + atividade.FullLine);
                            file.WriteLine("");
                        }
                    }
                    else
                    {
                        if (atividade.Dia.Day != dia)
                        {
                            // atividade novo dia dentro de bloco especial
                            dia = atividade.Dia.Day;
                            file.WriteLine(diaStr + "\t" + atividade.FullLine);
                        }
                        else
                        {
                            // atividade mesmo dia dentro de bloco especial
                            file.WriteLine("\t" + atividade.FullLine);
                        }
                    }
                }
            }
        }
    }
}

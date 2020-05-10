using DomL.Business.DTOs;
using DomL.Business.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DomL.Business
{
    class Event
    {
        readonly static Category categoria = Category.Event;

        public static void Consolidate(ConsolidateDTO consolidateDTO)
        {
            var filePath = consolidateDTO.fileDir + categoria.ToString() + ".txt";
            var atividadesVelhas = GetAtividadesVelhas(filePath, consolidateDTO.year);

            var atividadesNovas = consolidateDTO.allNewAtividades.Where(ad => ad.Categoria == categoria).ToList();
            atividadesVelhas.AddRange(Utils.GetAtividadesToAdd(atividadesNovas, atividadesVelhas));

            var allAtividadesCategoria = atividadesVelhas;
            EscreverNoArquivo(filePath, allAtividadesCategoria);

            consolidateDTO.allAtividades.AddRange(allAtividadesCategoria);
        }

        private static List<Activity> GetAtividadesVelhas(string filePath, int year)
        {
            var atividadesVelhas = new List<Activity>();

            if (File.Exists(filePath))
            {
                using (var reader = new StreamReader(filePath))
                {
                    bool isBlocoEspecial = false;

                    string line = reader.ReadLine();
                    while (line == "")
                    {
                        line = reader.ReadLine();
                    }
                    string diaMesStr = "";
                    while (line != null)
                    {
                        //line = line.Replace("\t", ";");
                        var segmentos = Regex.Split(line, "\t");
                        if (!string.IsNullOrWhiteSpace(segmentos[0]))
                        {
                            diaMesStr = segmentos[0];
                        }

                        Activity atividadeVelha = Utils.GetAtividadeVelha(diaMesStr, year, categoria);
                        atividadeVelha.FullLine = segmentos[1];
                        atividadeVelha.IsInBlocoEspecial = isBlocoEspecial;
                        if (atividadeVelha.FullLine.StartsWith("<"))
                        {
                            atividadeVelha.IsInBlocoEspecial = true;
                            isBlocoEspecial = !isBlocoEspecial;
                        }

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
                    if (atividade.IsInBlocoEspecial)
                    {
                        if (atividade.FullLine.StartsWith("<"))
                        {
                            if (!atividade.FullLine.StartsWith("<END>") && !atividade.FullLine.StartsWith("<FIM>"))
                            {
                                // Tag de começo de bloco especial
                                dia = atividade.Dia.Day;
                                file.WriteLine("");
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
                    else
                    {
                        // atividade fora de bloco especial
                        file.WriteLine(diaStr + "\t" + atividade.FullLine);
                    }
                }
            }
        }

    }
}

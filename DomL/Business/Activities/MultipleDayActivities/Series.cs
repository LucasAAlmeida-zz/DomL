using DomL.Business.Utils;
using DomL.Business.Utils.DTOs;
using DomL.Business.Utils.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DomL.Business.Activities.MultipleDayActivities
{
    public class Series : MultipleDayActivity
    {
        readonly static Category categoria = Category.Series;

        public static void Parse(Activity atividade, IReadOnlyList<string> segmentos)
        {
            // SERIE; (Assunto) Título; (Valor) Nota
            // SERIE; (Assunto) Título; (Classificação) Começo
            // SERIE; (Assunto) Título; (Valor) Nota; (Descrição) O que achei
            // SERIE; (Assunto) Título; (Classificação) Término; (Valor) Nota
            // SERIE; (Assunto) Título; (Classificação) Término; (Valor) Nota; (Descrição) O que achei

            atividade.Categoria = categoria;
            atividade.Assunto = segmentos[1];
            string segmentoToLower = segmentos[2].ToLower();
            string classificacao = "unica";
            switch (segmentos.Count)
            {
                case 3:
                    if (segmentoToLower == "comeco" || segmentoToLower == "começo" || segmentoToLower == "1")
                    {
                        classificacao = segmentoToLower;
                    }
                    else
                    {
                        atividade.Valor = segmentos[2];
                    }
                    break;
                case 4:
                    if (segmentoToLower == "termino" || segmentoToLower == "término" || segmentoToLower == "2")
                    {
                        classificacao = segmentoToLower;
                        atividade.Valor = segmentos[3];
                    }
                    else
                    {
                        atividade.Valor = segmentos[2];
                        atividade.Descricao = segmentos[3];
                    }
                    break;
                case 5:
                    classificacao = segmentos[2].ToLower();
                    atividade.Valor = segmentos[3];
                    atividade.Descricao = segmentos[4];
                    break;
                default:
                    throw new Exception("what");
            }

            switch (classificacao)
            {
                case "comeco": case "começo": case "1": atividade.Classificacao = Classification.Comeco; break;
                case "termino": case "término": case "2": atividade.Classificacao = Classification.Termino; break;
                case "unica": atividade.Classificacao = Classification.Unica; break;
                default: throw new Exception("what");
            }

            if (atividade.Classificacao != Classification.Comeco && atividade.Valor != "-" && !int.TryParse(atividade.Valor, out _))
            {
                throw new Exception("what");
            }
        }

        public static void Consolidate(ConsolidateDTO consolidateDTO)
        {
            var filePath = consolidateDTO.fileDir + categoria.ToString() + ".txt";
            var atividadesVelhas = GetAtividadesVelhas(filePath, consolidateDTO.year);

            var atividadesNovas = consolidateDTO.allNewAtividades.Where(ad => ad.Categoria == categoria).ToList();
            atividadesVelhas.AddRange(Util.GetAtividadesToAdd(atividadesNovas, atividadesVelhas));

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
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        line = line.Replace("\t", ";");
                        var segmentos = Regex.Split(line, ";");

                        Activity atividadeVelha;
                        int dia;
                        int mes;

                        if (int.TryParse(segmentos[0].Substring(0, 2), out dia))
                        {
                            // se tiver data de inicio
                            mes = int.Parse(segmentos[0].Substring(3, 2));

                            atividadeVelha = new Activity
                            {
                                Categoria = categoria,
                                Dia = new DateTime(year, mes, dia),
                                Classificacao = Classification.Comeco,
                                Assunto = segmentos[2]
                            };

                            atividadesVelhas.Add(atividadeVelha);
                        }

                        if (int.TryParse(segmentos[1].Substring(0, 2), out dia))
                        {
                            mes = int.Parse(segmentos[1].Substring(3, 2));

                            atividadeVelha = new Activity
                            {
                                Categoria = categoria,
                                Dia = new DateTime(year, mes, dia),
                                Classificacao = Classification.Termino,
                                Assunto = segmentos[2],
                                Valor = segmentos[3],
                                Descricao = segmentos[4]
                            };

                            atividadesVelhas.Add(atividadeVelha);
                        }
                    }
                }
            }

            return atividadesVelhas;
        }

        private static void EscreverNoArquivo(string filePath, List<Activity> allAtividadesCategoria)
        {
            using (var file = new StreamWriter(filePath))
            {
                foreach (Activity atividade in allAtividadesCategoria)
                {
                    string dataInicio = "??/??";
                    string dataTermino = "??/??";
                    string valor = atividade.Valor;
                    string descricao = atividade.Descricao;

                    switch (atividade.Classificacao)
                    {
                        case Classification.Unica:
                            dataInicio = atividade.Dia.Day.ToString("00") + "/" + atividade.Dia.Month.ToString("00");
                            dataTermino = dataInicio;
                            break;

                        case Classification.Comeco:
                            dataInicio = atividade.Dia.Day.ToString("00") + "/" + atividade.Dia.Month.ToString("00");

                            Activity atividadeTermino = allAtividadesCategoria.FirstOrDefault(a => a.Classificacao == Classification.Termino && Util.IsEqualTitle(a.Assunto, atividade.Assunto));
                            if (atividadeTermino != null)
                            {
                                dataTermino = atividadeTermino.Dia.Day.ToString("00") + "/" + atividadeTermino.Dia.Month.ToString("00");
                                valor = atividadeTermino.Valor;
                                descricao = atividadeTermino.Descricao;
                            }
                            break;

                        case Classification.Termino:
                            //Pra não fazer duas vezes a mesma atividade
                            Activity atividadeComeco = allAtividadesCategoria.FirstOrDefault(a => a.Classificacao == Classification.Comeco && Util.IsEqualTitle(a.Assunto, atividade.Assunto));
                            if (atividadeComeco != null)
                            {
                                continue;
                            }

                            dataTermino = atividade.Dia.Day.ToString("00") + "/" + atividade.Dia.Month.ToString("00");
                            break;

                        case Classification.Indefinido:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    valor = string.IsNullOrWhiteSpace(valor) ? "-" : valor;
                    file.WriteLine(dataInicio + "\t" + dataTermino + "\t" + atividade.Assunto + "\t" + valor + "\t" + descricao);
                }
            }
        }
    }
}

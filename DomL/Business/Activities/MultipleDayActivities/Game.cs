﻿using DomL.Business.Utils;
using DomL.Business.Utils.DTOs;
using DomL.Business.Utils.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DomL.Business.Activities.MultipleDayActivities
{
    public class Game : MultipleDayActivity
    {
        readonly static Category categoria = Category.Game;

        public void Parse(IReadOnlyList<string> segmentos)
        {
            // JOGO; (De Quem) Plataforma; (Assunto) Título; (Valor) Nota
            // JOGO; (De Quem) Plataforma; (Assunto) Título; (Classificação) Começo
            // JOGO; (De Quem) Plataforma; (Assunto) Título; (Valor) Nota; (Descrição) O que achei
            // JOGO; (De Quem) Plataforma; (Assunto) Título; (Classificação) Término; (Valor) Nota
            // JOGO; (De Quem) Plataforma; (Assunto) Título; (Classificação) Término; (Valor) Nota; (Descrição) O que achei

            Categoria = categoria;
            DeQuem = segmentos[1];
            Assunto = segmentos[2];
            string segmentoToLower = segmentos[3].ToLower();
            string classificacao = "unica";
            switch (segmentos.Count)
            {
                case 4:
                    if (segmentoToLower == "comeco" || segmentoToLower == "começo")
                    {
                        classificacao = segmentoToLower;
                    }
                    else
                    {
                        Valor = segmentos[3];
                    }
                    break;
                case 5:
                    if (segmentoToLower == "termino" || segmentoToLower == "término")
                    {
                        classificacao = segmentoToLower;
                        Valor = segmentos[4];
                    }
                    else
                    {
                        Valor = segmentos[3];
                        Descricao = segmentos[4];
                    }
                    break;
                case 6:
                    classificacao = segmentos[3].ToLower();
                    Valor = segmentos[4];
                    Descricao = segmentos[5];
                    break;
                default:
                    throw new Exception("what");
            }

            switch (classificacao)
            {
                case "comeco": case "começo": Classificacao = Classification.Comeco; break;
                case "termino": case "término": Classificacao = Classification.Termino; break;
                case "unica": Classificacao = Classification.Unica; break;
                default: throw new Exception("what");
            }

            if (Classificacao != Classification.Comeco && Valor != "-" && !int.TryParse(Valor, out _))
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
                                DeQuem = segmentos[2],
                                Assunto = segmentos[3]
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
                                DeQuem = segmentos[2],
                                Assunto = segmentos[3],
                                Valor = segmentos[4],
                                Descricao = segmentos[5]
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
                    file.WriteLine(dataInicio + "\t" + dataTermino + "\t" + atividade.DeQuem + "\t" + atividade.Assunto + "\t" + valor + "\t" + descricao);
                }
            }
        }

        protected override void ParseAtividadeVelha(string[] segmentos)
        {
            DeQuem = segmentos[2];
            Assunto = segmentos[3];
            Valor = segmentos[4];
            Descricao = segmentos[5];
        }

        protected override void WriteAtividadeConsolidada(StreamWriter file, string dataInicio, string dataTermino)
        {
            file.WriteLine(dataInicio + "\t" + dataTermino + "\t" + DeQuem + "\t" + Assunto + "\t" + Valor + "\t" + Descricao);
        }
    }
}

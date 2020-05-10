using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DomL.Business.DTOs;
using DomL.Business.Enums;

namespace DomL.Business
{
    public class Game
    {
        readonly static Category categoria = Category.Game;

        public static void Parse(Activity atividade, IReadOnlyList<string> segmentos)
        {
            // JOGO; (De Quem) Plataforma; (Assunto) Título; (Valor) Nota
            // JOGO; (De Quem) Plataforma; (Assunto) Título; (Classificação) Começo
            // JOGO; (De Quem) Plataforma; (Assunto) Título; (Valor) Nota; (Descrição) O que achei
            // JOGO; (De Quem) Plataforma; (Assunto) Título; (Classificação) Término; (Valor) Nota
            // JOGO; (De Quem) Plataforma; (Assunto) Título; (Classificação) Término; (Valor) Nota; (Descrição) O que achei

            atividade.Categoria = categoria;
            atividade.DeQuem = segmentos[1];
            atividade.Assunto = segmentos[2];
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
                        atividade.Valor = segmentos[3];
                    }
                    break;
                case 5:
                    if (segmentoToLower == "termino" || segmentoToLower == "término")
                    {
                        classificacao = segmentoToLower;
                        atividade.Valor = segmentos[4];
                    }
                    else
                    {
                        atividade.Valor = segmentos[3];
                        atividade.Descricao = segmentos[4];
                    }
                    break;
                case 6:
                    classificacao = segmentos[3].ToLower();
                    atividade.Valor = segmentos[4];
                    atividade.Descricao = segmentos[5];
                    break;
                default:
                    throw new Exception("what");
            }

            switch (classificacao)
            {
                case "comeco": case "começo": atividade.Classificacao = Classification.Comeco; break;
                case "termino": case "término": atividade.Classificacao = Classification.Termino; break;
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

                            Activity atividadeTermino = allAtividadesCategoria.FirstOrDefault(a => a.Classificacao == Classification.Termino && Utils.IsEqualTitle(a.Assunto, atividade.Assunto));
                            if (atividadeTermino != null)
                            {
                                dataTermino = atividadeTermino.Dia.Day.ToString("00") + "/" + atividadeTermino.Dia.Month.ToString("00");
                                valor = atividadeTermino.Valor;
                                descricao = atividadeTermino.Descricao;
                            }
                            break;

                        case Classification.Termino:
                            //Pra não fazer duas vezes a mesma atividade
                            Activity atividadeComeco = allAtividadesCategoria.FirstOrDefault(a => a.Classificacao == Classification.Comeco && Utils.IsEqualTitle(a.Assunto, atividade.Assunto));
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
    }
}

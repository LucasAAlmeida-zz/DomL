using DomL.Business.Activities.MultipleDayActivities;
using DomL.Business.Utils;
using DomL.Business.Utils.DTOs;
using DomL.Business.Utils.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DomL.Business.Activities
{
    public abstract class MultipleDayActivity : Activity
    {
        public MultipleDayActivity(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO)
        {
            Classificacao = atividadeDTO.Classificacao;
            if (atividadeDTO.IsNewActivity)
            {
                ParseAtividade(segmentos);
            }
            else
            {
                ParseAtividadeVelha(segmentos);
            }
        }

        public DateTime DiaTermino { get; set; }

        public static List<Activity> Consolidate(Category category, List<Activity> newCategoryActivities, string fileDir, int ano)
        {
            var filePath = fileDir + category.ToString() + ".txt";
            var atividadesVelhas = GetAtividadesVelhas(filePath, ano, category);
            atividadesVelhas.AddRange(Util.GetAtividadesToAdd(newCategoryActivities, atividadesVelhas));

            var allCategoryActivities = atividadesVelhas;
            EscreverNoArquivo(filePath, allCategoryActivities, category);

            return allCategoryActivities;
        }

        private static List<Activity> GetAtividadesVelhas(string filePath, int year, Category category)
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

                        ActivityDTO atividadeVelhaComecoDTO = Util.GetAtividadeVelha(segmentos[0], year, category, Classification.Comeco);
                        ActivityDTO atividadeVelhaTerminoDTO = Util.GetAtividadeVelha(segmentos[1], year, category, Classification.Termino);

                        switch (category)
                        {
                            case Category.Book:
                                if (atividadeVelhaComecoDTO != null)
                                {
                                    atividadesVelhas.Add(new Book(atividadeVelhaComecoDTO, segmentos));
                                }
                                if (atividadeVelhaTerminoDTO != null)
                                {
                                    atividadesVelhas.Add(new Book(atividadeVelhaTerminoDTO, segmentos));
                                }
                                break;
                            case Category.Comic:
                                if (atividadeVelhaComecoDTO != null)
                                {
                                    atividadesVelhas.Add(new Comic(atividadeVelhaComecoDTO, segmentos));
                                }
                                if (atividadeVelhaTerminoDTO != null)
                                {
                                    atividadesVelhas.Add(new Comic(atividadeVelhaTerminoDTO, segmentos));
                                }
                                break;
                            case Category.Game:
                                if (atividadeVelhaComecoDTO != null)
                                {
                                    atividadesVelhas.Add(new Game(atividadeVelhaComecoDTO, segmentos));
                                }
                                if (atividadeVelhaTerminoDTO != null)
                                {
                                    atividadesVelhas.Add(new Game(atividadeVelhaTerminoDTO, segmentos));
                                }
                                break;
                            case Category.Movie:
                                if (atividadeVelhaComecoDTO != null)
                                {
                                    atividadesVelhas.Add(new Movie(atividadeVelhaComecoDTO, segmentos));
                                }
                                if (atividadeVelhaTerminoDTO != null)
                                {
                                    atividadesVelhas.Add(new Movie(atividadeVelhaTerminoDTO, segmentos));
                                }
                                break;
                            case Category.Series:
                                if (atividadeVelhaComecoDTO != null)
                                {
                                    atividadesVelhas.Add(new Series(atividadeVelhaComecoDTO, segmentos));
                                }
                                if (atividadeVelhaTerminoDTO != null)
                                {
                                    atividadesVelhas.Add(new Series(atividadeVelhaTerminoDTO, segmentos));
                                }
                                break;
                            case Category.Watch:
                                if (atividadeVelhaComecoDTO != null)
                                {
                                    atividadesVelhas.Add(new Watch(atividadeVelhaComecoDTO, segmentos));
                                }
                                if (atividadeVelhaTerminoDTO != null)
                                {
                                    atividadesVelhas.Add(new Watch(atividadeVelhaTerminoDTO, segmentos));
                                }
                                break;
                            default:
                                throw new Exception("what");
                        }
                    }
                }
            }

            return atividadesVelhas;
        }

        private static void EscreverNoArquivo(string filePath, List<Activity> allCategoryActivities, Category category)
        {
            using (var file = new StreamWriter(filePath))
            {
                foreach (MultipleDayActivity activity in allCategoryActivities)
                {
                    switch (activity.Classificacao)
                    {
                        case Classification.Unica:
                            activity.DiaTermino = activity.Dia;
                            break;

                        case Classification.Comeco:
                            Activity atividadeTermino = allCategoryActivities.FirstOrDefault(a => a.Classificacao == Classification.Termino && Util.IsEqualTitle(a.Assunto, activity.Assunto));
                            if (atividadeTermino != null)
                            {
                                activity.DiaTermino = atividadeTermino.Dia;
                                activity.Valor = atividadeTermino.Valor;
                                activity.Descricao = string.IsNullOrWhiteSpace(activity.Descricao) ? atividadeTermino.Descricao : activity.Descricao + ", " + atividadeTermino.Descricao;
                            }
                            break;

                        case Classification.Termino:
                            activity.DiaTermino = activity.Dia;
                            activity.Dia = DateTime.MinValue;

                            //Pra não fazer duas vezes a mesma atividade
                            Activity atividadeComeco = allCategoryActivities.FirstOrDefault(a => a.Classificacao == Classification.Comeco && Util.IsEqualTitle(a.Assunto, activity.Assunto));
                            if (atividadeComeco != null)
                            {
                                continue;
                            }
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    string consolidatedActivity = activity.ConsolidateActivity();
                    file.WriteLine(consolidatedActivity);
                }
            }
        }

        protected abstract void ParseAtividadeVelha(string[] segmentos);
    }
}

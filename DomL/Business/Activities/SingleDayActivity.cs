using DomL.Business.Activities.SingleDayActivities;
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
    public abstract class SingleDayActivity : Activity
    {
        public SingleDayActivity(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO)
        {
            this.ParseAtividade(segmentos);
        }

        public static List<Activity> Consolidate(Category category, List<Activity> newCategoryActivities, string fileDir, int ano)
        {
            var filePath = fileDir + category.ToString() + ".txt";
            var atividadesVelhas = GetAtividadesVelhas(filePath, ano, category);
            atividadesVelhas.AddRange(Util.GetAtividadesToAdd(newCategoryActivities, atividadesVelhas));
            
            var allCategoryActivities = atividadesVelhas; 
            EscreverNoArquivo(filePath, allCategoryActivities);

            return allCategoryActivities;
        }

        public static int Count(Category category, List<Activity> activities)
        {
            return activities.Count(a => a.Categoria == category);
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
                        line = line.Replace("\t", ";");
                        var segmentos = Regex.Split(line, ";");

                        ActivityDTO atividadeVelhaDTO = Util.GetAtividadeVelha(segmentos[0], year);

                        switch (category)
                        {
                            case Category.Auto:     atividadesVelhas.Add(new Auto(atividadeVelhaDTO, segmentos));       break;
                            case Category.Doom:     atividadesVelhas.Add(new Doom(atividadeVelhaDTO, segmentos));       break;
                            case Category.Gift:     atividadesVelhas.Add(new Gift(atividadeVelhaDTO, segmentos));       break;
                            case Category.Health:   atividadesVelhas.Add(new Health(atividadeVelhaDTO, segmentos));     break;
                            case Category.Movie:    atividadesVelhas.Add(new Movie(atividadeVelhaDTO, segmentos));      break;
                            case Category.Person:   atividadesVelhas.Add(new Person(atividadeVelhaDTO, segmentos));     break;
                            case Category.Pet:      atividadesVelhas.Add(new Pet(atividadeVelhaDTO, segmentos));        break;
                            case Category.Play:     atividadesVelhas.Add(new Play(atividadeVelhaDTO, segmentos));       break;
                            case Category.Purchase: atividadesVelhas.Add(new Purchase(atividadeVelhaDTO, segmentos));   break;
                            case Category.Travel:   atividadesVelhas.Add(new Travel(atividadeVelhaDTO, segmentos));     break;
                            case Category.Work:     atividadesVelhas.Add(new Work(atividadeVelhaDTO, segmentos));       break;
                            default:                throw new Exception("what");
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
                foreach (SingleDayActivity atividade in allAtividadesCategoria)
                {
                    string consolidatedActivity = atividade.ConsolidateActivity();
                    file.WriteLine(consolidatedActivity);
                }
            }
        }
    }
}

﻿using DomL.Business.Utils;
using DomL.Business.Utils.DTOs;
using DomL.Business.Utils.Enums;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DomL.Business.Activities.SingleDayActivities
{
    public class Person : SingleDayActivity
    {
        readonly static Category categoria = Category.Person;

        public static void Parse(Activity atividade, IReadOnlyList<string> segmentos)
        {
            //PESSOA; (Assunto) Nome da Pessoa; (Descrição) Coisas pra me lembrar
            //PESSOA; (Assunto) Nome da Pessoa; (DeQuem) Origem conheci (amigo de x, furry, etc); (Descrição) Coisas pra me lembrar

            atividade.Categoria = categoria;
            atividade.Assunto = segmentos[1];
            if (segmentos.Count == 3)
            {
                atividade.Descricao = segmentos[2];
            }
            else
            {
                atividade.DeQuem = segmentos[2];
                atividade.Descricao = segmentos[3];
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

                        Activity atividadeVelha = Util.GetAtividadeVelha(segmentos[0], year, categoria);

                        ParseAtividadeVelha(atividadeVelha, segmentos);

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
                foreach (Activity atividade in allAtividadesCategoria)
                {
                    string dia = atividade.Dia.Day.ToString("00") + "/" + atividade.Dia.Month.ToString("00");
                    WriteAtividadesConsolidadas(file, dia, atividade);
                }
            }
        }

        private static void ParseAtividadeVelha(Activity atividadeVelha, string[] segmentos)
        {
            atividadeVelha.Assunto = segmentos[1];
            atividadeVelha.DeQuem = segmentos[2];
            atividadeVelha.Descricao = segmentos[3];
        }

        private static void WriteAtividadesConsolidadas(StreamWriter file, string dia, Activity atividade)
        {
            file.WriteLine(dia + "\t" + atividade.Assunto + "\t" + (!string.IsNullOrWhiteSpace(atividade.DeQuem) ? atividade.DeQuem : "") + "\t" + atividade.Descricao);
        }
    }
}
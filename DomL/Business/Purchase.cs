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
    public class Purchase
    {
        readonly static Category categoria = Category.Purchase;

        public static void Parse(Activity atividade, IReadOnlyList<string> segmentos)
        {
            //COMPRA; (De Quem) Loja; (Assunto) O que comprei; (Valor) Quanto custou
            //COMPRA; (De Quem) Loja; (Assunto) O que comprei; (Valor) Quanto custou; (Descrição) Misc

            atividade.Categoria = Category.Purchase;
            atividade.DeQuem = segmentos[1];
            atividade.Assunto = segmentos[2];
            atividade.Valor = segmentos[3];
            if (segmentos.Count == 5)
            {
                atividade.Descricao = segmentos[4];
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

                        Activity atividadeVelha = Utils.GetAtividadeVelha(segmentos[0], year, categoria);

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
            atividadeVelha.DeQuem = segmentos[1];
            atividadeVelha.Assunto = segmentos[2];
            atividadeVelha.Valor = segmentos[3];
            atividadeVelha.Descricao = segmentos[4];
        }

        private static void WriteAtividadesConsolidadas(StreamWriter file, string dia, Activity atividade)
        {
            file.WriteLine(dia + "\t" + atividade.DeQuem + "\t" + atividade.Assunto + "\t" + atividade.Valor + "\t" + atividade.Descricao);
        }
    }
}

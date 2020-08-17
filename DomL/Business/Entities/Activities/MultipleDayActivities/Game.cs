using DomL.Business.Utils;
using DomL.Business.Utils.DTOs;
using DomL.Business.Utils.Enums;
using DomL.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DomL.Business.Activities.MultipleDayActivities
{
    [Table("Game")]
    public class Game : MultipleDayActivity
    {
        public Game(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos) { }
        public Game() { }

        // JOGO; (De Quem) Plataforma; (Assunto) Título; (Classificação) Término; (Valor) Nota; (Descrição) O que achei

        public override void Save()
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                if (unitOfWork.GameRepo.Exists(b => b.Date == this.Date && b.Subject == this.Subject)) {
                    return;
                }

                unitOfWork.GameRepo.Add(this);
                unitOfWork.Complete();
            }
        }

        public static IEnumerable<Game> GetAllFromMes(int mes, int ano)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                return unitOfWork.GameRepo.Find(b => b.Date.Month == mes && b.Date.Year == ano);
            }
        }

        public static void ConsolidateYear(string fileDir, int year)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                var allGames = unitOfWork.GameRepo.Find(b => b.Date.Year == year).ToList();
                EscreveConsolidadasNoArquivo(fileDir + "Game" + year + ".txt", allGames.Cast<MultipleDayActivity>().ToList());
            }
        }

        public static void ConsolidateAll(string fileDir)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                var allGames = unitOfWork.GameRepo.GetAll().ToList();
                EscreveConsolidadasNoArquivo(fileDir + "Game.txt", allGames.Cast<MultipleDayActivity>().ToList());
            }
        }

        public static int CountBegunYear(int ano)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                return unitOfWork.GameRepo
                    .Find(g =>
                        (g.Classificacao == Classification.Comeco || g.Classificacao == Classification.Unica)
                        && g.Date.Year == ano)
                    .Count();
            }
        }

        public static int CountEndedYear(int ano)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                return unitOfWork.GameRepo
                    .Find(g =>
                        (g.Classificacao == Classification.Termino || g.Classificacao == Classification.Unica)
                        && g.Date.Year == ano)
                    .Count();
            }
        }

        public static void FullRestoreFromFile(string fileDir)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                var allGames = GetGamesFromFile(fileDir + "Game.txt");
                unitOfWork.GameRepo.AddRange(allGames);
                unitOfWork.Complete();
            }
        }

        private static List<Game> GetGamesFromFile(string filePath)
        {
            if (!File.Exists(filePath)) {
                return null;
            }

            var games = new List<Game>();
            using (var reader = new StreamReader(filePath)) {

                string line = "";
                try {
                    while ((line = reader.ReadLine()) != null) {
                        if (string.IsNullOrWhiteSpace(line)) {
                            continue;
                        }
                        var segmentos = Regex.Split(line, "\t");

                        // DataInicio; DataFim; (De Quem); (Assunto); (Nota); (Descrição)

                        int? nota = segmentos[4] != "-" ? int.Parse(segmentos[4]) : (int?)null;
                        string descricao = segmentos[5] != "-" ? segmentos[5] : null;

                        if (segmentos[0] == segmentos[1]) {
                            var game = new Game() {
                                Date = DateTime.ParseExact(segmentos[0], "dd/MM/yy", null),
                                Classificacao = Classification.Unica,
                                DeQuem = segmentos[2],
                                Subject = segmentos[3],
                                Nota = nota,
                                Description = descricao,

                                DayOrder = 0,
                            };
                            games.Add(game);
                            continue;
                        }

                        if (!segmentos[0].StartsWith("??/??")) {
                            var game = new Game() {
                                Date = DateTime.ParseExact(segmentos[0], "dd/MM/yy", null),
                                Classificacao = Classification.Comeco,
                                DeQuem = segmentos[2],
                                Subject = segmentos[3],
                                Nota = nota,
                                Description = segmentos[1].StartsWith("??/??") ? descricao : null,

                                DayOrder = 0,
                            };
                            games.Add(game);
                        }

                        if (!segmentos[1].StartsWith("??/??")) {
                            var game = new Game() {
                                Date = DateTime.ParseExact(segmentos[1], "dd/MM/yy", null),
                                Classificacao = Classification.Termino,
                                DeQuem = segmentos[2],
                                Subject = segmentos[3],
                                Nota = nota,
                                Description = descricao,

                                DayOrder = 0,
                            };
                            games.Add(game);
                        }
                    }
                } catch (Exception e) {
                    var msg = "Deu ruim na linha " + line;
                    throw new ParseException(msg, e);
                }
            }
            return games;
        }
    }
}

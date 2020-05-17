using DomL.Business.Activities;
using DomL.Business.Activities.MultipleDayActivities;
using DomL.Business.Activities.SingleDayActivities;
using DomL.Business.Activities.SpecialActivities;
using DomL.Business.Utils.DTOs;
using DomL.Business.Utils.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace DomL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private List<Activity> _atividades;
        private List<Activity> _atividadesFull;

        const string BASE_DIR_PATH = "C:\\Users\\User\\Desktop\\DomL\\";

        public MainWindow()
        {
            InitializeComponent();

            MesTb.Text = (DateTime.Now.Month - 1).ToString();
            AnoTb.Text = DateTime.Now.Year.ToString();
        }

        private void CategorizarButton_Click(object sender, RoutedEventArgs e)
        {
            _atividades = new List<Activity>();

            try
            {
                ClassificaAtividades();
                EscreverAtividadesDoMesEmArquivo();
                ConsolidaAtividadesImportantesEmArquivo();
                GeraEstatisticas();
            }
            catch (Exception exception)
            {
                MessageLabel2.Content = exception.Message;
                Console.Write(exception);
            }
        }

        #region Classifica
        private void ClassificaAtividades()
        {
            var atividadesDiaString = Regex.Split(AtividadesTextBox.Text, "\r\n");
            var atividadeString = "";
            var isBlocoEspecial = false;

            var diaDT = new DateTime();
            try
            {
                for (var linha = 0; linha < atividadesDiaString.Length; linha++)
                {
                    atividadeString = atividadesDiaString[linha];

                    if (string.IsNullOrWhiteSpace(atividadeString))
                    {
                        continue;
                    }

                    if (IsNewDay(atividadeString, out int dia))
                    {
                        diaDT = new DateTime(int.Parse(AnoTb.Text), int.Parse(MesTb.Text), dia);
                        continue;
                    }

                    var atividade = new Activity
                    {
                        Dia = diaDT,
                        Categoria = Category.Undefined,
                        FullLine = atividadeString,
                        IsInBlocoEspecial = isBlocoEspecial
                    };

                    var segmentos = Regex.Split(atividadeString, "; ");
                    string categoria = segmentos[0];

                    var isImportante = true;

                    #region switch de categoria
                    switch (categoria)
                    {
                        case "AUTO":
                        case "CARRO":
                            Auto auto = (Auto)atividade;
                            auto.Parse(segmentos);
                            _atividades.Add(auto);
                            break;

                        case "DOOM":
                        case "DESGRACA":
                        case "DESGRAÇA":
                            Doom doom = (Doom)atividade;
                            doom.Parse(segmentos);
                            _atividades.Add(doom);
                            break;

                        case "GIFT":
                        case "PRESENTE":
                            Gift gift = (Gift)atividade;
                            gift.Parse(segmentos);
                            _atividades.Add(gift);
                            break;
                        
                        case "SAUDE":
                        case "HEALTH":
                            Health health = (Health)atividade;
                            health.Parse(segmentos);
                            _atividades.Add(health);
                            break;

                        case "PESSOA":
                            Person person = (Person)atividade;
                            person.Parse(segmentos);
                            _atividades.Add(person);
                            break;
                        
                        case "PET":
                        case "ANIMAL":
                            Pet pet = (Pet)atividade;
                            pet.Parse(segmentos);
                            _atividades.Add(pet);
                            break;

                        case "PLAY":
                            Play play = (Play)atividade;
                            play.Parse(segmentos);
                            _atividades.Add(play);
                            break;

                        case "COMPRA":
                            Purchase purchase = (Purchase)atividade;
                            purchase.Parse(segmentos);
                            _atividades.Add(purchase);
                            break;

                        case "VIAGEM":
                        case "TRIP":
                            Travel travel = (Travel)atividade;
                            travel.Parse(segmentos);
                            _atividades.Add(travel);
                            break;

                        case "WORK":
                            Work work = (Work)atividade;
                            work.Parse(segmentos);
                            _atividades.Add(work);
                            break;

                        // -----

                        case "LIVRO":
                        case "BOOK":
                            Book book = (Book)atividade;
                            book.Parse(segmentos);
                            _atividades.Add(book);
                            break;

                        case "COMIC":
                        case "MANGA":
                            Comic comic = (Comic)atividade;
                            comic.Parse(segmentos);
                            _atividades.Add(comic);
                            break;
                        
                        case "JOGO":
                        case "GAME":
                            Game game = (Game)atividade;
                            game.Parse(segmentos);
                            _atividades.Add(game);
                            break;

                        case "FILME":
                        case "MOVIE":
                        case "CINEMA":
                            Movie movie = (Movie)atividade;
                            movie.Parse(segmentos);
                            _atividades.Add(movie);
                            break;

                        case "SERIE":
                        case "DESENHO":
                        case "ANIME":
                        case "CARTOON":
                            Series series = (Series)atividade;
                            series.Parse(segmentos);
                            _atividades.Add(series);
                            break;

                        case "WATCH":
                            Watch watch = (Watch)atividade;
                            watch.Parse(segmentos);
                            _atividades.Add(watch);
                            break;

                        default:
                            atividade.Descricao = segmentos[0];
                            if (atividade.Descricao.StartsWith("*"))
                            {
                                atividade.Descricao = atividade.Descricao.Substring(1);
                                atividade.FullLine = atividade.Descricao;
                            }
                            else if (atividade.Descricao.StartsWith("<"))
                            {
                                atividade.IsInBlocoEspecial = true;
                                isBlocoEspecial = !isBlocoEspecial;
                            }
                            else
                            {
                                if (atividade.Descricao.StartsWith("---"))
                                {
                                    atividade.IsInBlocoEspecial = false;
                                }
                                isImportante = false;
                            }
                            break;
                    }
                    #endregion

                    if (isImportante || atividade.IsInBlocoEspecial)
                    {
                        _atividades.Add(atividade);
                    }
                }
            }
            catch (Exception e)
            {
                MessageLabel.Content = "Deu ruim no dia " + diaDT.Day + ", atividade: " + atividadeString;
                throw e;
            }
        }

        private static bool IsNewDay(string linha, out int dia)
        {
            int indexPrimeiroEspaco = linha.IndexOf(" ", StringComparison.Ordinal);
            string firstWord = (indexPrimeiroEspaco != -1) ? linha.Substring(0, indexPrimeiroEspaco) : linha;
            return int.TryParse(firstWord, out dia) && (linha.Contains(" - ") || linha.Contains(" – "));
        }
        #endregion

        #region Escrever Mes
        private void EscreverAtividadesDoMesEmArquivo()
        {
            string mes = _atividades.First().Dia.Month.ToString("00");
            string filePath = BASE_DIR_PATH + "AtividadesMes\\AtividadesMes" + mes + ".txt";
            var fi = new FileInfo(filePath);
            if (fi.Directory != null && !fi.Directory.Exists && fi.DirectoryName != null)
            {
                Directory.CreateDirectory(fi.DirectoryName);
            }

            using (var file = new StreamWriter(filePath))
            {
                var jaPulouLinha = true;
                int dia = _atividades.First().Dia.Day;
                foreach (Activity atividade in _atividades)
                {
                    string diaStr = atividade.Dia.Day.ToString("00");
                    string mesStr = atividade.Dia.Month.ToString("00");
                    string diaSemana = atividade.Dia.DayOfWeek.ToString().Substring(0, 3);
                    string descricao = atividade.FullLine;

                    if (atividade.IsInBlocoEspecial)
                    {
                        if (atividade.FullLine.StartsWith("<"))
                        {
                            if (!atividade.FullLine.StartsWith("<END>") && !atividade.FullLine.StartsWith("<FIM>"))
                            {
                                // Tag de começo de bloco especial
                                file.WriteLine("");
                                file.WriteLine(diaStr + "/" + mesStr + "\t" + diaSemana + "\t" + descricao);
                            }
                            else
                            {
                                // Tag de fim de bloco especial
                                file.WriteLine("\t\t" + descricao);
                                file.WriteLine("");
                            }
                        }
                        else
                        {
                            if (atividade.Dia.Day != dia)
                            {
                                // atividade novo dia dentro de bloco especial
                                dia = atividade.Dia.Day;
                                file.WriteLine(diaStr + "/" + mesStr + "\t" + diaSemana + "\t" + descricao);
                            }
                            else
                            {
                                // atividade mesmo dia dentro de bloco especial
                                file.WriteLine("\t\t" + descricao);
                            }
                        }
                    }
                    else
                    {
                        // atividade fora de bloco especial
                        if (atividade.Dia.Day != dia)
                        {
                            jaPulouLinha = false;
                            dia = atividade.Dia.Day;
                        }

                        if ((atividade.Dia.DayOfWeek == DayOfWeek.Monday || atividade.Dia.DayOfWeek == DayOfWeek.Saturday) && !jaPulouLinha)
                        {
                            file.WriteLine("");
                            jaPulouLinha = true;
                        }

                        file.WriteLine(diaStr + "/" + mesStr + "\t" + diaSemana + "\t" + descricao);
                    }

                }
            }
            MessageLabel.Content = "Mês consolidado com sucesso";
        }
        #endregion

        #region Consolida Atividades
        private void ConsolidaAtividadesImportantesEmArquivo()
        {
            _atividadesFull = new List<Activity>();

            string fileDir = BASE_DIR_PATH + "AtividadesConsolidadas\\";
            var fi = new FileInfo(fileDir);
            if (fi.Directory != null && !fi.Directory.Exists && fi.DirectoryName != null)
            {
                Directory.CreateDirectory(fi.DirectoryName);
            }

            var consolidateDTO = new ConsolidateDTO(
                _atividades,
                fileDir,
                int.Parse(AnoTb.Text),
                _atividadesFull
            );

            int ano = int.Parse(AnoTb.Text);

            ConsolidateSingleDayActivityCategory(Category.Auto, fileDir, ano);
            ConsolidateSingleDayActivityCategory(Category.Doom, fileDir, ano);
            ConsolidateSingleDayActivityCategory(Category.Gift, fileDir, ano);
            ConsolidateSingleDayActivityCategory(Category.Health, fileDir, ano);
            ConsolidateSingleDayActivityCategory(Category.Person, fileDir, ano);
            ConsolidateSingleDayActivityCategory(Category.Pet, fileDir, ano);
            ConsolidateSingleDayActivityCategory(Category.Play, fileDir, ano);
            ConsolidateSingleDayActivityCategory(Category.Purchase, fileDir, ano);
            ConsolidateSingleDayActivityCategory(Category.Travel, fileDir, ano);
            ConsolidateSingleDayActivityCategory(Category.Work, fileDir, ano);

            ConsolidateMultipleDayActivityCategory(Category.Game, fileDir, ano);
            ConsolidateMultipleDayActivityCategory(Category.Watch, fileDir, ano);
            ConsolidateMultipleDayActivityCategory(Category.Movie, fileDir, ano);
            ConsolidateMultipleDayActivityCategory(Category.Series, fileDir, ano);
            ConsolidateMultipleDayActivityCategory(Category.Book, fileDir, ano);
            ConsolidateMultipleDayActivityCategory(Category.Comic, fileDir, ano);
            ConsolidateMultipleDayActivityCategory(Category.Course, fileDir, ano);

            Event.Consolidate(consolidateDTO);
            Undefined.Consolidate(consolidateDTO);

            MessageLabel.Content = "Atividades Importantes Consolidadas";
            MessageLabel2.Content = "";
        }

        private void ConsolidateSingleDayActivityCategory(Category category, string fileDir, int ano)
        {
            var newCategoryActivities = _atividades.Where(a => a.Categoria == category).ToList();
            var allCategoryActivities = SingleDayActivity.Consolidate(category, newCategoryActivities, fileDir, ano);
            _atividadesFull.AddRange(allCategoryActivities);
        }

        private void ConsolidateMultipleDayActivityCategory(Category category, string fileDir, int ano)
        {
            var newCategoryActivities = _atividades.Where(a => a.Categoria == category).ToList();
            var allCategoryActivities = MultipleDayActivity.Consolidate(category, newCategoryActivities, fileDir, ano);
            _atividadesFull.AddRange(allCategoryActivities);
        }
        #endregion

        #region Gera Estatisticas
        private void GeraEstatisticas()
        {
            string filePath = BASE_DIR_PATH + "ResumoAno.txt";
            using (var file = new StreamWriter(filePath))
            {
                // ReSharper disable once JoinDeclarationAndInitializer
                int numero;

                numero = _atividadesFull.Count(af => af.Categoria == Category.Game && af.Classificacao == Classification.Comeco);
                file.WriteLine("Jogos começados:\t" + numero);

                numero = _atividadesFull.Count(af => af.Categoria == Category.Game && (af.Classificacao == Classification.Termino || af.Classificacao == Classification.Unica));
                file.WriteLine("Jogos terminados:\t" + numero);

                numero = _atividadesFull.Count(af => af.Categoria == Category.Movie && (af.Classificacao == Classification.Termino || af.Classificacao == Classification.Unica));
                file.WriteLine("Filmes assistidos:\t" + numero);

                numero = _atividadesFull.Count(af => af.Categoria == Category.Series && (af.Classificacao == Classification.Termino || af.Classificacao == Classification.Unica));
                file.WriteLine("Temporadas de séries assistidas:\t" + numero);

                numero = _atividadesFull.Count(af => af.Categoria == Category.Book && (af.Classificacao == Classification.Termino || af.Classificacao == Classification.Unica));
                file.WriteLine("Livros lidos:\t" + numero);

                numero = _atividadesFull.Count(af => af.Categoria == Category.Comic && (af.Classificacao == Classification.Termino || af.Classificacao == Classification.Unica));
                file.WriteLine("K Páginas de comics lidos:\t" + numero);

                numero = _atividadesFull.Count(af => af.Categoria == Category.Travel);
                file.WriteLine("Viagens feitas:\t" + numero);

                numero = _atividadesFull.Count(af => af.Categoria == Category.Person);
                file.WriteLine("Pessoas novas conhecidas:\t" + numero);

                numero = _atividadesFull.Count(af => af.Categoria == Category.Purchase);
                file.WriteLine("Compras notáveis:\t" + numero);
            }
        }
        #endregion
    }
}

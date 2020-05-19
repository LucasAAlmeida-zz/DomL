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

                    var atividadeDTO = new ActivityDTO
                    {
                        Dia = diaDT,
                        FullLine = atividadeString,
                        IsInBlocoEspecial = isBlocoEspecial,
                        IsNewActivity = true
                    };

                    var segmentos = Regex.Split(atividadeString, "; ");
                    string categoria = segmentos[0];

                    #region switch de categoria
                    switch (categoria)
                    {
                        case "AUTO":
                        case "CARRO":
                            _atividades.Add(new Auto(atividadeDTO, segmentos));
                            break;

                        case "DOOM":
                        case "DESGRACA":
                        case "DESGRAÇA":
                            _atividades.Add(new Doom(atividadeDTO, segmentos));
                            break;

                        case "GIFT":
                        case "PRESENTE":
                            _atividades.Add(new Gift(atividadeDTO, segmentos));
                            break;
                        
                        case "SAUDE":
                        case "HEALTH":
                            _atividades.Add(new Health(atividadeDTO, segmentos));
                            break;

                        case "PESSOA":
                            _atividades.Add(new Person(atividadeDTO, segmentos));
                            break;
                        
                        case "PET":
                        case "ANIMAL":
                            _atividades.Add(new Pet(atividadeDTO, segmentos));
                            break;

                        case "PLAY":
                            _atividades.Add(new Play(atividadeDTO, segmentos));
                            break;

                        case "COMPRA":
                            _atividades.Add(new Purchase(atividadeDTO, segmentos));
                            break;

                        case "VIAGEM":
                        case "TRIP":
                            _atividades.Add(new Travel(atividadeDTO, segmentos));
                            break;

                        case "WORK":
                            _atividades.Add(new Work(atividadeDTO, segmentos));
                            break;

                        // -----

                        case "LIVRO":
                        case "BOOK":
                            _atividades.Add(new Book(atividadeDTO, segmentos));
                            break;

                        case "COMIC":
                        case "MANGA":
                            _atividades.Add(new Comic(atividadeDTO, segmentos));
                            break;
                        
                        case "JOGO":
                        case "GAME":
                            _atividades.Add(new Game(atividadeDTO, segmentos));
                            break;

                        case "FILME":
                        case "MOVIE":
                        case "CINEMA":
                            _atividades.Add(new Movie(atividadeDTO, segmentos));
                            break;

                        case "SERIE":
                        case "DESENHO":
                        case "ANIME":
                        case "CARTOON":
                            _atividades.Add(new Series(atividadeDTO, segmentos));
                            break;

                        case "WATCH":
                            _atividades.Add(new Watch(atividadeDTO, segmentos));
                            break;

                        default:
                            if (segmentos[0].StartsWith("<"))
                            {
                                isBlocoEspecial = !isBlocoEspecial;
                            }

                            Event evento = new Event(atividadeDTO, segmentos);
                            if (evento.IsInBlocoEspecial || evento.shouldSave)
                            {
                                _atividades.Add(evento);
                            }
                            break;
                    }
                    #endregion
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
                int dia = 0;
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

            int year = int.Parse(AnoTb.Text);

            ConsolidateSingleDayActivityCategory(Category.Auto, fileDir, year);
            ConsolidateSingleDayActivityCategory(Category.Doom, fileDir, year);
            ConsolidateSingleDayActivityCategory(Category.Gift, fileDir, year);
            ConsolidateSingleDayActivityCategory(Category.Health, fileDir, year);
            ConsolidateSingleDayActivityCategory(Category.Person, fileDir, year);
            ConsolidateSingleDayActivityCategory(Category.Pet, fileDir, year);
            ConsolidateSingleDayActivityCategory(Category.Play, fileDir, year);
            ConsolidateSingleDayActivityCategory(Category.Purchase, fileDir, year);
            ConsolidateSingleDayActivityCategory(Category.Travel, fileDir, year);
            ConsolidateSingleDayActivityCategory(Category.Work, fileDir, year);

            ConsolidateMultipleDayActivityCategory(Category.Game, fileDir, year);
            ConsolidateMultipleDayActivityCategory(Category.Watch, fileDir, year);
            ConsolidateMultipleDayActivityCategory(Category.Movie, fileDir, year);
            ConsolidateMultipleDayActivityCategory(Category.Series, fileDir, year);
            ConsolidateMultipleDayActivityCategory(Category.Book, fileDir, year);
            ConsolidateMultipleDayActivityCategory(Category.Comic, fileDir, year);
            ConsolidateMultipleDayActivityCategory(Category.Course, fileDir, year);

            ConsolidateSpecialActivityCategory(fileDir, year);

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

        private void ConsolidateSpecialActivityCategory(string fileDir, int ano)
        {
            var newCategoryActivities = _atividades.Where(a => a.Categoria == Category.Event || a.IsInBlocoEspecial).ToList();
            SpecialActivity.Consolidate(newCategoryActivities, fileDir, ano);
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

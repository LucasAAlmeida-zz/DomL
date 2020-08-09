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
            this.InitializeComponent();

            this.MesTb.Text = (DateTime.Now.Month - 1).ToString();
            this.AnoTb.Text = DateTime.Now.Year.ToString();
        }

        private void CategorizarButton_Click(object sender, RoutedEventArgs e)
        {
            this._atividades = new List<Activity>();

            try
            {
                this.ClassificaAtividades();
                this.EscreverAtividadesDoMesEmArquivo();
                this.ConsolidaAtividadesImportantesEmArquivo();
                this.GeraEstatisticas();
            }
            catch (Exception exception)
            {
                this.MessageLabel2.Content = exception.Message;
                Console.Write(exception);
            }
        }

        #region Classifica
        private void ClassificaAtividades()
        {
            var atividadesDiaString = Regex.Split(this.AtividadesTextBox.Text, "\r\n");
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
                        diaDT = new DateTime(int.Parse(this.AnoTb.Text), int.Parse(this.MesTb.Text), dia);
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
                            this._atividades.Add(new Auto(atividadeDTO, segmentos));
                            break;

                        case "DOOM":
                        case "DESGRACA":
                        case "DESGRAÇA":
                            this._atividades.Add(new Doom(atividadeDTO, segmentos));
                            break;

                        case "GIFT":
                        case "PRESENTE":
                            this._atividades.Add(new Gift(atividadeDTO, segmentos));
                            break;
                        
                        case "SAUDE":
                        case "HEALTH":
                            this._atividades.Add(new Health(atividadeDTO, segmentos));
                            break;

                        case "PESSOA":
                            this._atividades.Add(new Person(atividadeDTO, segmentos));
                            break;
                        
                        case "PET":
                        case "ANIMAL":
                            this._atividades.Add(new Pet(atividadeDTO, segmentos));
                            break;

                        case "PLAY":
                            this._atividades.Add(new Play(atividadeDTO, segmentos));
                            break;

                        case "COMPRA":
                            this._atividades.Add(new Purchase(atividadeDTO, segmentos));
                            break;

                        case "VIAGEM":
                        case "TRIP":
                            this._atividades.Add(new Travel(atividadeDTO, segmentos));
                            break;

                        case "WORK":
                            this._atividades.Add(new Work(atividadeDTO, segmentos));
                            break;

                        // -----

                        case "LIVRO":
                        case "BOOK":
                            this._atividades.Add(new Book(atividadeDTO, segmentos));
                            break;

                        case "COMIC":
                        case "MANGA":
                            this._atividades.Add(new Comic(atividadeDTO, segmentos));
                            break;
                        
                        case "JOGO":
                        case "GAME":
                            this._atividades.Add(new Game(atividadeDTO, segmentos));
                            break;

                        case "FILME":
                        case "MOVIE":
                        case "CINEMA":
                            this._atividades.Add(new Movie(atividadeDTO, segmentos));
                            break;

                        case "SERIE":
                        case "DESENHO":
                        case "ANIME":
                        case "CARTOON":
                            this._atividades.Add(new Series(atividadeDTO, segmentos));
                            break;

                        case "WATCH":
                            this._atividades.Add(new Watch(atividadeDTO, segmentos));
                            break;

                        default:
                            if (segmentos[0].StartsWith("<"))
                            {
                                isBlocoEspecial = !isBlocoEspecial;
                            }

                            Event evento = new Event(atividadeDTO, segmentos);
                            if (evento.IsInBlocoEspecial || evento.shouldSave)
                            {
                                this._atividades.Add(evento);
                            }
                            break;
                    }
                    #endregion
                }
            }
            catch (Exception e)
            {
                this.MessageLabel.Content = "Deu ruim no dia " + diaDT.Day + ", atividade: " + atividadeString;
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
            string mes = this._atividades.First().Dia.Month.ToString("00");
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
                foreach (Activity atividade in this._atividades)
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
            this.MessageLabel.Content = "Mês consolidado com sucesso";
        }
        #endregion

        #region Consolida Atividades
        private void ConsolidaAtividadesImportantesEmArquivo()
        {
            this._atividadesFull = new List<Activity>();

            string fileDir = BASE_DIR_PATH + "AtividadesConsolidadas\\";
            var fi = new FileInfo(fileDir);
            if (fi.Directory != null && !fi.Directory.Exists && fi.DirectoryName != null)
            {
                Directory.CreateDirectory(fi.DirectoryName);
            }

            int year = int.Parse(this.AnoTb.Text);

            this.ConsolidateSingleDayActivityCategory(Category.Auto, fileDir, year);
            this.ConsolidateSingleDayActivityCategory(Category.Doom, fileDir, year);
            this.ConsolidateSingleDayActivityCategory(Category.Gift, fileDir, year);
            this.ConsolidateSingleDayActivityCategory(Category.Health, fileDir, year);
            this.ConsolidateSingleDayActivityCategory(Category.Person, fileDir, year);
            this.ConsolidateSingleDayActivityCategory(Category.Pet, fileDir, year);
            this.ConsolidateSingleDayActivityCategory(Category.Play, fileDir, year);
            this.ConsolidateSingleDayActivityCategory(Category.Purchase, fileDir, year);
            this.ConsolidateSingleDayActivityCategory(Category.Travel, fileDir, year);
            this.ConsolidateSingleDayActivityCategory(Category.Work, fileDir, year);

            this.ConsolidateMultipleDayActivityCategory(Category.Game, fileDir, year);
            this.ConsolidateMultipleDayActivityCategory(Category.Watch, fileDir, year);
            this.ConsolidateMultipleDayActivityCategory(Category.Movie, fileDir, year);
            this.ConsolidateMultipleDayActivityCategory(Category.Series, fileDir, year);
            this.ConsolidateMultipleDayActivityCategory(Category.Book, fileDir, year);
            this.ConsolidateMultipleDayActivityCategory(Category.Comic, fileDir, year);
            this.ConsolidateMultipleDayActivityCategory(Category.Course, fileDir, year);

            this.ConsolidateSpecialActivityCategory(fileDir, year);

            this.MessageLabel.Content = "Atividades Importantes Consolidadas";
            this.MessageLabel2.Content = "";
        }

        private void ConsolidateSingleDayActivityCategory(Category category, string fileDir, int ano)
        {
            var newCategoryActivities = this._atividades.Where(a => a.Categoria == category).ToList();
            var allCategoryActivities = SingleDayActivity.Consolidate(category, newCategoryActivities, fileDir, ano);
            this._atividadesFull.AddRange(allCategoryActivities);
        }

        private void ConsolidateMultipleDayActivityCategory(Category category, string fileDir, int ano)
        {
            var newCategoryActivities = this._atividades.Where(a => a.Categoria == category).ToList();
            var allCategoryActivities = MultipleDayActivity.Consolidate(category, newCategoryActivities, fileDir, ano);
            this._atividadesFull.AddRange(allCategoryActivities);
        }

        private void ConsolidateSpecialActivityCategory(string fileDir, int ano)
        {
            var newCategoryActivities = this._atividades.Where(a => a.Categoria == Category.Event || a.IsInBlocoEspecial).ToList();
            SpecialActivity.Consolidate(newCategoryActivities, fileDir, ano);
        }
        #endregion

        #region Gera Estatisticas
        private void GeraEstatisticas()
        {
            string filePath = BASE_DIR_PATH + "ResumoAno.txt";
            using (var file = new StreamWriter(filePath))
            {
                int numero;

                numero = MultipleDayActivity.CountBegun(Category.Game, this._atividadesFull);
                file.WriteLine("Jogos começados:\t" + numero);

                numero = MultipleDayActivity.CountEnded(Category.Game, this._atividadesFull);
                file.WriteLine("Jogos terminados:\t" + numero);

                numero = MultipleDayActivity.CountEnded(Category.Series, this._atividadesFull);
                file.WriteLine("Temporadas de séries assistidas:\t" + numero);

                numero = MultipleDayActivity.CountEnded(Category.Book, this._atividadesFull);
                file.WriteLine("Livros lidos:\t" + numero);

                numero = MultipleDayActivity.CountEnded(Category.Comic, this._atividadesFull);
                file.WriteLine("K Páginas de comics lidos:\t" + numero);

                numero = SingleDayActivity.Count(Category.Movie, this._atividadesFull);
                file.WriteLine("Filmes assistidos:\t" + numero);

                numero = SingleDayActivity.Count(Category.Movie, this._atividadesFull);
                file.WriteLine("Viagens feitas:\t" + numero);

                numero = SingleDayActivity.Count(Category.Movie, this._atividadesFull);
                file.WriteLine("Pessoas novas conhecidas:\t" + numero);

                numero = SingleDayActivity.Count(Category.Movie, this._atividadesFull);
                file.WriteLine("Compras notáveis:\t" + numero);
            }
        }
        #endregion
    }
}

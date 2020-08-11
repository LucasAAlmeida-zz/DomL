using DomL.Business;
using DomL.Business.Activities;
using DomL.Business.Activities.MultipleDayActivities;
using DomL.Business.Activities.SingleDayActivities;
using DomL.Business.Utils.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;

namespace DomL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        const string BASE_DIR_PATH = "C:\\Users\\User\\Desktop\\DomL\\";

        public MainWindow()
        {
            this.InitializeComponent();

            this.MesTb.Text = (DateTime.Now.Month - 1).ToString();
            this.AnoTb.Text = DateTime.Now.Year.ToString();
        }

        private void CategorizarButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                this.ClassificaAtividades();
                this.EscreverAtividadesDoMesEmArquivo();
                this.EscreverAtividadesConsolidadasEmArquivo();
                this.EscreveResumoAnoEmArquivo();
            } catch (Exception exception) {
                this.MessageLabel2.Content = exception.Message;
                Console.Write(exception);
            }
        }

        #region Classifica
        private void ClassificaAtividades()
        {
            var atividadesDiaString = Regex.Split(this.AtividadesTextBox.Text, "\r\n");
            var atividadeString = "";
            int? activityBlockId = null;
            var ordem = 0;

            var diaDT = new DateTime();
            try {
                for (var linha = 0; linha < atividadesDiaString.Length; linha++) {
                    atividadeString = atividadesDiaString[linha];

                    if (string.IsNullOrWhiteSpace(atividadeString)
                        || atividadeString.StartsWith("---"))
                    {
                        continue;
                    }

                    if (IsNewDay(atividadeString, out int dia)) {
                        diaDT = new DateTime(int.Parse(this.AnoTb.Text), int.Parse(this.MesTb.Text), dia);
                        ordem = 0;
                        continue;
                    }

                    ordem++;

                    var atividadeDTO = new ActivityDTO {
                        DayOrder = ordem,
                        Dia = diaDT,
                        ActivityBlockId = activityBlockId,
                    };

                    if (atividadeString.StartsWith("<")) {
                        if (atividadeString != "<END>") {
                            var activityBlockName = atividadeString.Substring(1, atividadeString.Length-2);
                            var activityBlock = new ActivityBlock(activityBlockName, int.Parse(this.AnoTb.Text));
                            activityBlockId = activityBlock.Save();
                            atividadeDTO.ActivityBlockId = activityBlockId;
                        } else {
                            activityBlockId = null;
                        }
                    }

                    var segmentos = Regex.Split(atividadeString, "; ");
                    string categoria = segmentos[0];

                    #region switch de categoria
                    switch (categoria) {
                        case "AUTO":
                        case "CARRO":
                            new Auto(atividadeDTO, segmentos).Save();
                            break;

                        case "DOOM":
                        case "DESGRACA":
                        case "DESGRAÇA":
                            new Doom(atividadeDTO, segmentos).Save();
                            break;

                        case "GIFT":
                        case "PRESENTE":
                            new Gift(atividadeDTO, segmentos).Save();
                            break;

                        case "SAUDE":
                        case "HEALTH":
                            new Health(atividadeDTO, segmentos).Save();
                            break;

                        case "FILME":
                        case "MOVIE":
                        case "CINEMA":
                            new Movie(atividadeDTO, segmentos).Save();
                            break;

                        case "PESSOA":
                            new Person(atividadeDTO, segmentos).Save();
                            break;

                        case "PET":
                        case "ANIMAL":
                            new Pet(atividadeDTO, segmentos).Save();
                            break;

                        case "PLAY":
                            new Play(atividadeDTO, segmentos).Save();
                            break;

                        case "COMPRA":
                            new Purchase(atividadeDTO, segmentos).Save();
                            break;

                        case "VIAGEM":
                        case "TRIP":
                            new Travel(atividadeDTO, segmentos).Save();
                            break;

                        case "WORK":
                            new Work(atividadeDTO, segmentos).Save();
                            break;

                        // -----

                        case "LIVRO":
                        case "BOOK":
                            new Book(atividadeDTO, segmentos).Save();
                            break;

                        case "COMIC":
                        case "MANGA":
                            new Comic(atividadeDTO, segmentos).Save();
                            break;

                        case "JOGO":
                        case "GAME":
                            new Game(atividadeDTO, segmentos).Save();
                            break;

                        case "SERIE":
                        case "DESENHO":
                        case "ANIME":
                        case "CARTOON":
                            new Series(atividadeDTO, segmentos).Save();
                            break;

                        case "WATCH":
                            new Watch(atividadeDTO, segmentos).Save();
                            break;

                        default:
                            new Event(atividadeDTO, segmentos).Save();
                            break;
                    }
                    #endregion
                }
            } catch (Exception e) {
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
            var mes = this.MesTb.Text;
            var ano = this.AnoTb.Text;
            string filePath = BASE_DIR_PATH + "AtividadesMes\\AtividadesMes" + mes + ".txt";
            var fi = new FileInfo(filePath);
            if (fi.Directory != null && !fi.Directory.Exists && fi.DirectoryName != null) {
                Directory.CreateDirectory(fi.DirectoryName);
            }

            var atividades = GetAllAtividadesMesAno(int.Parse(mes), int.Parse(ano));
            using (var file = new StreamWriter(filePath)) {
                foreach (Activity atividade in atividades) {
                    file.WriteLine(atividade.ParseToString());
                }
            }
            this.MessageLabel.Content = "Mês consolidado com sucesso";
        }

        private List<Activity> GetAllAtividadesMesAno(int mes, int ano)
        {
            var atividades = new List<Activity>();

            atividades.AddRange(Book.GetAllFromMes(mes, ano));
            atividades.AddRange(Comic.GetAllFromMes(mes, ano));
            atividades.AddRange(Game.GetAllFromMes(mes, ano));
            atividades.AddRange(Series.GetAllFromMes(mes, ano));
            atividades.AddRange(Watch.GetAllFromMes(mes, ano));
            
            atividades.AddRange(Auto.GetAllFromMes(mes, ano));
            atividades.AddRange(Doom.GetAllFromMes(mes, ano));
            atividades.AddRange(Gift.GetAllFromMes(mes, ano));
            atividades.AddRange(Health.GetAllFromMes(mes, ano));
            atividades.AddRange(Movie.GetAllFromMes(mes, ano));
            atividades.AddRange(Person.GetAllFromMes(mes, ano));
            atividades.AddRange(Pet.GetAllFromMes(mes, ano));
            atividades.AddRange(Play.GetAllFromMes(mes, ano));
            atividades.AddRange(Purchase.GetAllFromMes(mes, ano));
            atividades.AddRange(Travel.GetAllFromMes(mes, ano));
            atividades.AddRange(Work.GetAllFromMes(mes, ano));

            atividades.AddRange(Event.GetImportantFromMesAno(mes, ano));

            return atividades;
        }
        #endregion

        #region Consolida Atividades
        private void EscreverAtividadesConsolidadasEmArquivo()
        {
            string fileDir = BASE_DIR_PATH + "AtividadesConsolidadas\\";
            var fi = new FileInfo(fileDir);
            if (fi.Directory != null && !fi.Directory.Exists && fi.DirectoryName != null) {
                Directory.CreateDirectory(fi.DirectoryName);
            }

            int year = int.Parse(this.AnoTb.Text);

            Book.Consolidate(fileDir, year);
            Comic.Consolidate(fileDir, year);
            Game.Consolidate(fileDir, year);
            Series.Consolidate(fileDir, year);
            Watch.Consolidate(fileDir, year);
            
            Auto.Consolidate(fileDir, year);
            Doom.Consolidate(fileDir, year);
            Gift.Consolidate(fileDir, year);
            Health.Consolidate(fileDir, year);
            Movie.Consolidate(fileDir, year);
            Person.Consolidate(fileDir, year);
            Pet.Consolidate(fileDir, year);
            Play.Consolidate(fileDir, year);
            Purchase.Consolidate(fileDir, year);
            Travel.Consolidate(fileDir, year);
            Work.Consolidate(fileDir, year);

            Event.Consolidate(fileDir, year);

            ActivityBlock.Consolidate(fileDir, year);

            this.MessageLabel.Content = "Atividades Importantes Consolidadas";
            this.MessageLabel2.Content = "";
        }
        #endregion

        #region Escreve Resumo Ano
        private void EscreveResumoAnoEmArquivo()
        {
            var ano = int.Parse(this.AnoTb.Text);

            string filePath = BASE_DIR_PATH + "ResumoAno.txt";
            using (var file = new StreamWriter(filePath)) {
                int numero;
                
                numero = Game.CountBegunYear(ano);
                file.WriteLine("Jogos começados:\t" + numero);

                numero = Game.CountEndedYear(ano);
                file.WriteLine("Jogos terminados:\t" + numero);

                numero = Series.CountEndedYear(ano);
                file.WriteLine("Temporadas de séries assistidas:\t" + numero);

                numero = Book.CountEndedYear(ano);
                file.WriteLine("Livros lidos:\t" + numero);

                numero = Comic.CountEndedYear(ano);
                file.WriteLine("K Páginas de comics lidos:\t" + numero);

                numero = Movie.CountYear(ano);
                file.WriteLine("Filmes assistidos:\t" + numero);

                numero = Travel.CountYear(ano);
                file.WriteLine("Viagens feitas:\t" + numero);

                numero = Person.CountYear(ano);
                file.WriteLine("Pessoas novas conhecidas:\t" + numero);

                numero = Purchase.CountYear(ano);
                file.WriteLine("Compras notáveis:\t" + numero);
            }
        }
        #endregion
    }
}

using DomL.Business;
using DomL.Business.DTOs;
using DomL.Business.Enums;
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
                        case "WORK":
                            Work.Parse(atividade, segmentos);
                            break;

                        case "DOOM":
                        case "DESGRACA":
                        case "DESGRAÇA":
                            Doom.Parse(atividade, segmentos);
                            break;

                        case "SAUDE":
                        case "HEALTH":
                            Health.Parse(atividade, segmentos);
                            break;

                        case "VIAGEM":
                        case "TRIP":
                            Travel.Parse(atividade, segmentos);
                            break;

                        case "AUTO":
                        case "CARRO":
                            Auto.Parse(atividade, segmentos);
                            break;

                        case "GIFT":
                        case "PRESENTE":
                            Gift.Parse(atividade, segmentos);
                            break;

                        case "PESSOA":
                            Person.Parse(atividade, segmentos);
                            break;

                        case "PET":
                        case "ANIMAL":
                            Pet.Parse(atividade, segmentos);
                            break;

                        case "COMPRA":
                            Purchase.Parse(atividade, segmentos);
                            break;

                        case "JOGO":
                        case "GAME":
                            Game.Parse(atividade, segmentos);
                            break;

                        case "WATCH":
                            Watch.Parse(atividade, segmentos);
                            break;

                        case "FILME":
                        case "MOVIE":
                        case "CINEMA":
                            Movie.Parse(atividade, segmentos);
                            break;

                        case "SERIE":
                        case "DESENHO":
                        case "ANIME":
                        case "CARTOON":
                            Series.Parse(atividade, segmentos);
                            break;

                        case "LIVRO":
                        case "BOOK":
                            Book.Parse(atividade, segmentos);
                            break;

                        case "COMIC":
                        case "MANGA":
                            Comic.Parse(atividade, segmentos);
                            break;

                        case "PLAY":
                            Play.Parse(atividade, segmentos);
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

            Work.Consolidate(consolidateDTO);
            Doom.Consolidate(consolidateDTO);
            Health.Consolidate(consolidateDTO);
            Travel.Consolidate(consolidateDTO);
            Gift.Consolidate(consolidateDTO);
            Auto.Consolidate(consolidateDTO);
            Pet.Consolidate(consolidateDTO);
            Play.Consolidate(consolidateDTO);
            Person.Consolidate(consolidateDTO);
            Purchase.Consolidate(consolidateDTO);
            Game.Consolidate(consolidateDTO);
            Watch.Consolidate(consolidateDTO);
            Movie.Consolidate(consolidateDTO);
            Series.Consolidate(consolidateDTO);
            Book.Consolidate(consolidateDTO);
            Comic.Consolidate(consolidateDTO);
            Event.Consolidate(consolidateDTO);
            Undefined.Consolidate(consolidateDTO);

            MessageLabel.Content = "Atividades Importantes Consolidadas";
            MessageLabel2.Content = "";
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

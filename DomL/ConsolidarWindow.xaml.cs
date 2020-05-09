using DomL.Business;
using DomL.Business.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace DomL
{
    /// <summary>
    /// Interaction logic for ConsolidarWindow.xaml
    /// </summary>
    public partial class ConsolidarWindow
    {
        private List<Activity> _atividadesDia;

        public ConsolidarWindow()
        {
            InitializeComponent();

            AnoTB.Text = DateTime.Now.Year.ToString();
        }

        private void ConsolidaAtividadesButton_Click(object sender, RoutedEventArgs e)
        {
            var atividadesDiaString = Regex.Split(AtividadesTextBox.Text, "\r\n");
            _atividadesDia = new List<Activity>();

            Activity atividade = null;
            try {
                for (var linha = 0; linha < atividadesDiaString.Length; linha++) {
                    string atividadeString = atividadesDiaString[linha];

                    if (string.IsNullOrWhiteSpace(atividadeString)) {
                        continue;
                    }

                    if (atividadeString.Length < 5 || atividadeString.IndexOf('/') != 2) {
                        //É titulo de semana/fds/ferias
                        continue;
                    }

                    atividade = new Activity {
                        FullLine = atividadeString,
                        //Importante = true,
                        Ordem = linha
                    };

                    var segmentos = Regex.Split(atividadeString, "; ");

                    string diaMes = segmentos[0];
                    int dia = int.Parse(diaMes.Substring(0, 2));
                    int mes = int.Parse(diaMes.Substring(3, 2));
                    atividade.Dia = new DateTime(int.Parse(AnoTB.Text), mes, dia);

                    //Indice do primeiro segmento sem contar a data/dataFim/diaSemana
                    var indexInicial = 2;
                    if (segmentos[0].Length > 5) {
                        indexInicial = 1;
                        dia = int.Parse(diaMes.Substring(6, 2));
                        mes = int.Parse(diaMes.Substring(9, 2));
                        atividade.DiaFim = new DateTime(int.Parse(AnoTB.Text), mes, dia);
                    }

                    string categoria = segmentos[indexInicial];

                    #region switch de categoria
                    switch (categoria) {
                        case "WORK":
                        case "SS":
                            atividade.Categoria = Category.Trabalho;
                            ParseWorkAndDoomFromStringToAtividade(atividade, segmentos, indexInicial);
                            break;

                        case "DOOM":
                        case "DESGRAÇA":
                        case "DESGRACA":
                            atividade.Categoria = Category.Desgraca;
                            ParseWorkAndDoomFromStringToAtividade(atividade, segmentos, indexInicial);
                            break;

                        case "SAUDE":
                        case "SAÚDE":
                        case "HEALTH":
                            atividade.Categoria = Category.Saude;
                            ParseSaudeFromStringToAtividade(atividade, segmentos, indexInicial);
                            break;

                        case "VIAGEM":
                        case "TRIP":
                            atividade.Categoria = Category.Viagem;
                            ParseViagemFromStringToAtividade(atividade, segmentos, indexInicial);
                            break;

                        case "AUTO":
                        case "CARRO":
                            atividade.Categoria = Category.Automovel;
                            ParseAutoAndPetAndPlayFromStringToAtividade(atividade, segmentos, indexInicial);
                            break;

                        case "GIFT":
                        case "PRESENTE":
                            atividade.Categoria = Category.Presente;
                            ParseGiftFromStringToAtividade(atividade, segmentos, indexInicial);
                            break;

                        case "PESSOA":
                            atividade.Categoria = Category.Pessoa;
                            ParsePessoaFromStringToAtividade(atividade, segmentos, indexInicial);
                            break;

                        case "PET":
                        case "ANIMAL":
                            atividade.Categoria = Category.Animal;
                            ParseAutoAndPetAndPlayFromStringToAtividade(atividade, segmentos, indexInicial);
                            break;

                        case "COMPRA":
                            atividade.Categoria = Category.Compra;
                            ParseCompraFromStringToAtividade(atividade, segmentos, indexInicial);
                            break;

                        case "JOGO":
                        case "GAME":
                            atividade.Categoria = Category.Jogo;
                            ParseJogoAndWatchAndFilmeAndSerieAndLivroAndComicFromStringToAtividade(atividade, segmentos, indexInicial);
                            break;

                        case "WATCH":
                            atividade.Categoria = Category.Watch;
                            ParseJogoAndWatchAndFilmeAndSerieAndLivroAndComicFromStringToAtividade(atividade, segmentos, indexInicial);
                            break;

                        case "FILME":
                        case "MOVIE":
                        case "CINEMA":
                            atividade.Categoria = Category.Filme;
                            ParseJogoAndWatchAndFilmeAndSerieAndLivroAndComicFromStringToAtividade(atividade, segmentos, indexInicial);
                            break;

                        case "SERIE":
                        case "SÉRIE":
                        case "DESENHO":
                        case "ANIME":
                        case "CARTOON":
                            atividade.Categoria = Category.Serie;
                            ParseJogoAndWatchAndFilmeAndSerieAndLivroAndComicFromStringToAtividade(atividade, segmentos, indexInicial);
                            break;

                        case "LIVRO":
                        case "BOOK":
                            atividade.Categoria = Category.Livro;
                            ParseJogoAndWatchAndFilmeAndSerieAndLivroAndComicFromStringToAtividade(atividade, segmentos, indexInicial);
                            break;

                        case "COMIC":
                        case "MANGA":
                        case "MANGÁ":
                            atividade.Categoria = Category.Comic;
                            ParseJogoAndWatchAndFilmeAndSerieAndLivroAndComicFromStringToAtividade(atividade, segmentos, indexInicial);
                            break;

                        case "PLAY":
                            atividade.Categoria = Category.Play;
                            ParseAutoAndPetAndPlayFromStringToAtividade(atividade, segmentos, indexInicial);
                            break;

                        default:
                            //atividade.Importante = false;
                            atividade.Categoria = Category.Indefinido;
                            atividade.Descricao = segmentos[indexInicial];
                            break;
                    }
                    #endregion

                    if (!string.IsNullOrWhiteSpace(atividade.Descricao)) {

                        while (atividade.Descricao.EndsWith("(+)")) {
                            linha++;

                            atividade.Descricao = atividade.Descricao.Substring(0, atividade.Descricao.Length - 3);
                            atividade.Descricao += "\n\t" + atividadesDiaString[linha].Substring(2);
                        }

                        //TODO Tem que tirar a companhia da descrição
                    }

                    _atividadesDia.Add(atividade);
                }

                ConsolidaAtividadesImportantes();
                SelecionaAcontecimentosImportantes();
            } catch {
                MessageAtividadesLabel.Content = "Ultima linha lida: " + atividade?.FullLine;
            }

        }

        private void ConsolidaAtividadesImportantes()
        {
            using (var file = new StreamWriter("C:\\Users\\LUCASAUGUSTO\\Desktop\\AtividadesImportantesAno.txt", false)) {
                #region Trabalho
                file.WriteLine("===============================");
                file.WriteLine("Trabalho");
                file.Write("\n");
                ConsolidaTrabalhoEDoom(file, _atividadesDia.Where(ad => ad.Categoria == Category.Trabalho).ToList());
                file.WriteLine("===============================");
                #endregion

                #region Desgraca
                file.WriteLine("===============================");
                file.WriteLine("Desgraça");
                file.Write("\n");
                ConsolidaTrabalhoEDoom(file, _atividadesDia.Where(ad => ad.Categoria == Category.Desgraca).ToList());
                file.WriteLine("===============================");
                #endregion

                #region Saude
                file.WriteLine("===============================");
                file.WriteLine("Saúde");
                file.Write("\n");
                ConsolidaSaude(file, _atividadesDia.Where(ad => ad.Categoria == Category.Saude).ToList());
                file.WriteLine("===============================");
                #endregion

                #region Viagem
                file.WriteLine("===============================");
                file.WriteLine("Viagem");
                file.Write("\n");
                ConsolidaViagem(file, _atividadesDia.Where(ad => ad.Categoria == Category.Viagem).ToList());
                file.WriteLine("===============================");
                #endregion

                #region Gift
                file.WriteLine("===============================");
                file.WriteLine("Gift");
                file.Write("\n");
                ConsolidaPresente(file, _atividadesDia.Where(ad => ad.Categoria == Category.Presente).ToList());
                file.WriteLine("===============================");
                #endregion

                #region Auto
                file.WriteLine("===============================");
                file.WriteLine("Auto");
                file.Write("\n");
                ConsolidaAutoEPetEPlay(file, _atividadesDia.Where(ad => ad.Categoria == Category.Automovel).ToList());
                file.WriteLine("===============================");
                #endregion

                #region Pet
                file.WriteLine("===============================");
                file.WriteLine("Pet");
                file.Write("\n");
                ConsolidaAutoEPetEPlay(file, _atividadesDia.Where(ad => ad.Categoria == Category.Animal).ToList());
                file.WriteLine("===============================");
                #endregion

                #region Play
                file.WriteLine("===============================");
                file.WriteLine("Play");
                file.Write("\n");
                ConsolidaAutoEPetEPlay(file, _atividadesDia.Where(ad => ad.Categoria == Category.Play).ToList());
                file.WriteLine("===============================");
                #endregion

                #region Pessoa
                file.WriteLine("===============================");
                file.WriteLine("Pessoa");
                file.Write("\n");
                ConsolidaPessoa(file, _atividadesDia.Where(ad => ad.Categoria == Category.Pessoa).ToList());
                file.WriteLine("===============================");
                #endregion

                #region Compra
                file.WriteLine("===============================");
                file.WriteLine("Compra");
                file.Write("\n");
                ConsolidaCompra(file, _atividadesDia.Where(ad => ad.Categoria == Category.Compra).ToList());
                file.WriteLine("===============================");
                #endregion

                #region Jogo
                file.WriteLine("===============================");
                file.WriteLine("Jogo");
                file.Write("\n");
                ConsolidaJogoEWatchEFilmeESerieELivroEComic(file, _atividadesDia.Where(ad => ad.Categoria == Category.Jogo).ToList());
                file.WriteLine("===============================");
                #endregion

                #region Watch
                file.WriteLine("===============================");
                file.WriteLine("Watch");
                file.Write("\n");
                ConsolidaJogoEWatchEFilmeESerieELivroEComic(file, _atividadesDia.Where(ad => ad.Categoria == Category.Watch).ToList());
                file.WriteLine("===============================");
                #endregion

                #region Filme
                file.WriteLine("===============================");
                file.WriteLine("Filme");
                file.Write("\n");
                ConsolidaJogoEWatchEFilmeESerieELivroEComic(file, _atividadesDia.Where(ad => ad.Categoria == Category.Filme).ToList());
                file.WriteLine("===============================");
                #endregion

                #region Serie
                file.WriteLine("===============================");
                file.WriteLine("Serie");
                file.Write("\n");
                ConsolidaJogoEWatchEFilmeESerieELivroEComic(file, _atividadesDia.Where(ad => ad.Categoria == Category.Serie).ToList());
                file.WriteLine("===============================");
                #endregion

                #region Livro
                file.WriteLine("===============================");
                file.WriteLine("Livro");
                file.Write("\n");
                ConsolidaJogoEWatchEFilmeESerieELivroEComic(file, _atividadesDia.Where(ad => ad.Categoria == Category.Livro).ToList());
                file.WriteLine("===============================");
                #endregion

                #region Comic
                file.WriteLine("===============================");
                file.WriteLine("Comic");
                file.Write("\n");
                ConsolidaJogoEWatchEFilmeESerieELivroEComic(file, _atividadesDia.Where(ad => ad.Categoria == Category.Comic).ToList());
                file.WriteLine("===============================");
                #endregion
            }

            MessageAtividadesLabel.Content = "Atividades Importantes Consolidadas";
        }

        private void SelecionaAcontecimentosImportantes()
        {
            AcontecimentosImportantesGrid.Children.Clear();

            var linha = 0;
            foreach (Activity atividadeDia in _atividadesDia.Where(ad => ad.Categoria == Category.Indefinido)) {
                AcontecimentosImportantesGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                SetGridPositionDiaLabel(atividadeDia, linha);
                SetGridPositionOrdemLabel(atividadeDia, linha);
                SetGridPositionTextBox(atividadeDia, linha);
                SetGridPositionCheckBox(linha, atividadeDia);

                linha++;
            }
        }

        private void SetGridPositionCheckBox(int linha, Activity atividadeDia)
        {
            var atividadeOrdinariaCB = new CheckBox();
            Grid.SetRow(atividadeOrdinariaCB, linha);
            Grid.SetColumn(atividadeOrdinariaCB, 3);

            //if (atividadeDia.Importante) {
            //    atividadeOrdinariaCB.IsChecked = true;
            //    atividadeOrdinariaCB.IsEnabled = false;
            //}

            AcontecimentosImportantesGrid.Children.Add(atividadeOrdinariaCB);
        }

        private void SetGridPositionTextBox(Activity atividadeDia, int linha)
        {
            var atividadeOrdinariaTB = new TextBox {
                Text = atividadeDia.Descricao,
                AcceptsReturn = true
            };
            Grid.SetRow(atividadeOrdinariaTB, linha);
            Grid.SetColumn(atividadeOrdinariaTB, 2);
            AcontecimentosImportantesGrid.Children.Add(atividadeOrdinariaTB);
        }

        private void SetGridPositionOrdemLabel(Activity atividadeDia, int linha)
        {
            var atividadeOrdinariaOrdemL = new Label {
                Content = atividadeDia.Ordem
            };
            Grid.SetRow(atividadeOrdinariaOrdemL, linha);
            Grid.SetColumn(atividadeOrdinariaOrdemL, 1);
            AcontecimentosImportantesGrid.Children.Add(atividadeOrdinariaOrdemL);
        }

        private void SetGridPositionDiaLabel(Activity atividadeDia, int linha)
        {
            var atividadeOrdinariaDiaL = new Label {
                Content = atividadeDia.Dia.Day.ToString("00") + "/" + atividadeDia.Dia.Month.ToString("00")
            };
            Grid.SetRow(atividadeOrdinariaDiaL, linha);
            Grid.SetColumn(atividadeOrdinariaDiaL, 0);
            AcontecimentosImportantesGrid.Children.Add(atividadeOrdinariaDiaL);
        }

        private void ConsolidaAcontecimentosButton_Click(object sender, RoutedEventArgs e)
        {
            var acontecimentosImportantesGridChildren = AcontecimentosImportantesGrid.Children.Cast<UIElement>().ToList();

            for (var rowIndex = 0; rowIndex < AcontecimentosImportantesGrid.RowDefinitions.Count; rowIndex++) {
                UIElement checkBoxElement = acontecimentosImportantesGridChildren.FirstOrDefault(aigc => Grid.GetRow(aigc) == rowIndex && Grid.GetColumn(aigc) == 3);

                if (checkBoxElement == null) {
                    continue;
                }

                var activityCB = checkBoxElement as CheckBox;
                Debug.Assert(activityCB != null, "activityCB != null");
                Debug.Assert(activityCB.IsChecked != null, "activityCB.IsChecked != null");

                if (!activityCB.IsChecked.Value) {
                    continue;
                }

                var activityOrderL = acontecimentosImportantesGridChildren.First(aigc => Grid.GetRow(aigc) == rowIndex && Grid.GetColumn(aigc) == 1) as Label;
                Debug.Assert(activityOrderL != null, "activityOrderL != null");
                int activityOrder = int.Parse(activityOrderL.Content.ToString());
                Activity activity = _atividadesDia.First(a => a.Ordem == activityOrder);
                //activity.Importante = true;

                var activityDescriptionL = acontecimentosImportantesGridChildren.First(l => Grid.GetRow(l) == rowIndex && Grid.GetColumn(l) == 2) as TextBox;
                activity.Descricao = activityDescriptionL?.Text;
            }

            //var acontecimentosImportantes = _atividadesDia.Where(ad => ad.Importante && ad.Categoria == Category.Indefinido).ToList();
            //ConsolidaAcontecimentosImportantes(acontecimentosImportantes);
        }

        private void ConsolidaAcontecimentosImportantes(IEnumerable<Activity> acontecimentosImportantes)
        {
            using (var file = new StreamWriter("C:\\Users\\LUCASAUGUSTO\\Desktop\\AcontecimentosImportantesAno.txt", false)) {
                foreach (Activity acontecimento in acontecimentosImportantes) {
                    string dia = acontecimento.Dia.Day.ToString("00") + "/" + acontecimento.Dia.Month.ToString("00");
                    string diaFim = (acontecimento.DiaFim != null) ? "~" + acontecimento.DiaFim.Value.Day.ToString("00") + "/" + acontecimento.DiaFim.Value.Month.ToString("00") : "";
                    file.WriteLine(dia + diaFim + "\t"
                        + acontecimento.Descricao);
                }
            }
            MessageAcontecimentosLabel.Content = "Acontecimentos Importantes Consolidadas";
        }

        #region Parse de Atividades
        private void ParseWorkAndDoomFromStringToAtividade(Activity atividade, string[] segmentos, int indexInicial)
        {
            //WORK; (Descrição) O que aconteceu
            //DOOM; (Descrição) O que aconteceu

            atividade.Descricao = segmentos[indexInicial + 1];
        }
        private void ParseSaudeFromStringToAtividade(Activity atividade, string[] segmentos, int indexInicial)
        {
            //SAUDE; (Descrição) o que aconteceu
            //SAUDE; (Assunto) Especialidade médica; (Descrição) o que aconteceu

            if (segmentos.Length == indexInicial + 2) {
                atividade.Descricao = segmentos[indexInicial + 1];
            } else {
                atividade.Assunto = segmentos[indexInicial + 1];
                atividade.Descricao = segmentos[indexInicial + 2];
            }
        }
        private void ParseViagemFromStringToAtividade(Activity atividade, string[] segmentos, int indexInicial)
        {
            //VIAGEM; (Assunto) De onde pra onde; (MeioTransporte) Meio de transporte
            //VIAGEM; (Assunto) De onde pra onde; (MeioTransporte) Meio de transporte; (Descrição) o que aconteceu

            atividade.Assunto = segmentos[indexInicial + 1];
            atividade.MeioTransporte = segmentos[indexInicial + 2];
            if (segmentos.Length == indexInicial + 4) {
                atividade.Descricao = segmentos[indexInicial + 3];
            }
        }
        private void ParseGiftFromStringToAtividade(Activity atividade, string[] segmentos, int indexInicial)
        {
            //GIFT; (Assunto) O que ganhei; (DeQuem) De quem ganhei o presente
            //GIFT; (Assunto) O que ganhei; (DeQuem) De quem ganhei o presente; (Descrição) o que aconteceu

            atividade.Assunto = segmentos[indexInicial + 1];
            atividade.DeQuem = segmentos[indexInicial + 2];
            if (segmentos.Length == indexInicial + 4) {
                atividade.Descricao = segmentos[indexInicial + 3];
            }
        }
        private void ParseAutoAndPetAndPlayFromStringToAtividade(Activity atividade, string[] segmentos, int indexInicial)
        {
            //AUTO; (Assunto) Qual automovel; (Descricao) O que Aconteceu
            //PET; (Assunto) Qual pet; (Descricao) O que aconteceu
            //PLAY; (Assunto) Nome da pessoa; (Descricao) Impressões

            atividade.Assunto = segmentos[indexInicial + 1];
            atividade.Descricao = segmentos[indexInicial + 2];
        }
        private void ParsePessoaFromStringToAtividade(Activity atividade, string[] segmentos, int indexInicial)
        {
            //PESSOA; (Assunto) Nome da Pessoa; (Descrição) Coisas pra me lembrar
            //PESSOA; (Assunto) Nome da Pessoa; (DeQuem) Origem conheci (amigo de x, furry, etc); (Descrição) Coisas pra me lembrar

            atividade.Assunto = segmentos[indexInicial + 1];
            if (segmentos.Length == indexInicial + 3) {
                atividade.Descricao = segmentos[indexInicial + 2];
            } else {
                atividade.DeQuem = segmentos[indexInicial + 2];
                atividade.Descricao = segmentos[indexInicial + 3];
            }
        }
        private void ParseCompraFromStringToAtividade(Activity atividade, string[] segmentos, int indexInicial)
        {
            //COMPRA; (Assunto) O que comprei; (Valor) Quanto custou

            atividade.Assunto = segmentos[indexInicial + 1];
            if (segmentos.Length == indexInicial + 3) {
                atividade.Valor = int.Parse(segmentos[indexInicial + 2]);
            }
        }
        private void ParseJogoAndWatchAndFilmeAndSerieAndLivroAndComicFromStringToAtividade(Activity atividade, string[] segmentos, int indexInicial)
        {
            //JOGO; (Assunto) Título;
            //JOGO; (Assunto) Título; (Descrição) O que achei
            //JOGO; (Assunto) Título; (Classificação) Começo, término ou recomeço;
            //JOGO; (Assunto) Título; (Classificação) Começo, término ou recomeço; (Descrição) O que achei

            atividade.Assunto = segmentos[indexInicial + 1];

            string classificacao = null;
            if (segmentos.Length == indexInicial + 3) {
                if (segmentos[indexInicial + 2] == "Comeco" || segmentos[indexInicial + 2] == "Começo" || segmentos[indexInicial + 2] == "1"
                    || segmentos[indexInicial + 2] == "Termino" || segmentos[indexInicial + 2] == "Término" || segmentos[indexInicial + 2] == "2"
                    || segmentos[indexInicial + 2] == "Recomeco" || segmentos[indexInicial + 2] == "Recomeço" || segmentos[indexInicial + 2] == "3") {
                    classificacao = segmentos[indexInicial + 2];
                } else {
                    atividade.Descricao = segmentos[indexInicial + 2];
                }

            } else if (segmentos.Length == indexInicial + 4) {
                classificacao = segmentos[indexInicial + 2];
                atividade.Descricao = segmentos[indexInicial + 3];
            }

            if (classificacao != null) {
                switch (classificacao) {
                    case "Comeco":
                    case "Começo":
                    case "1":
                        atividade.Classificacao = Classification.Comeco;
                        break;
                    case "Termino":
                    case "Término":
                    case "2":
                        atividade.Classificacao = Classification.Termino;
                        break;
                }
            } else {
                atividade.Classificacao = Classification.Unica;
            }
        }
        #endregion
        #region Consolida Atividades
        private static void ConsolidaTrabalhoEDoom(TextWriter file, IEnumerable<Activity> atividades)
        {
            foreach (Activity atividade in atividades) {
                string dia = atividade.Dia.Day.ToString("00") + "/" + atividade.Dia.Month.ToString("00");
                string diaFim = (atividade.DiaFim != null) ? "~" + atividade.DiaFim.Value.Day.ToString("00") + "/" + atividade.DiaFim.Value.Month.ToString("00") : "";
                file.WriteLine(dia + diaFim + "\t"
                    + atividade.Descricao);
            }
        }
        private static void ConsolidaSaude(TextWriter file, IEnumerable<Activity> atividades)
        {
            foreach (Activity atividade in atividades) {
                string dia = atividade.Dia.Day.ToString("00") + "/" + atividade.Dia.Month.ToString("00");
                string diaFim = (atividade.DiaFim != null) ? "~" + atividade.DiaFim.Value.Day.ToString("00") + "/" + atividade.DiaFim.Value.Month.ToString("00") : "";
                file.WriteLine(dia + diaFim + "\t"
                    + (!string.IsNullOrWhiteSpace(atividade.Assunto) ? atividade.Assunto : "") + "\t" + atividade.Descricao);
            }
        }
        private static void ConsolidaViagem(TextWriter file, IEnumerable<Activity> atividades)
        {
            foreach (Activity atividade in atividades) {
                string dia = atividade.Dia.Day.ToString("00") + "/" + atividade.Dia.Month.ToString("00");
                string diaFim = (atividade.DiaFim != null) ? "~" + atividade.DiaFim.Value.Day.ToString("00") + "/" + atividade.DiaFim.Value.Month.ToString("00") : "";
                file.WriteLine(dia + diaFim + "\t"
                    + atividade.Assunto + "\t" + atividade.MeioTransporte + "\t" + atividade.Descricao);
            }
        }
        private static void ConsolidaPresente(TextWriter file, IEnumerable<Activity> atividades)
        {
            foreach (Activity atividade in atividades) {
                string dia = atividade.Dia.Day.ToString("00") + "/" + atividade.Dia.Month.ToString("00");
                string diaFim = (atividade.DiaFim != null) ? "~" + atividade.DiaFim.Value.Day.ToString("00") + "/" + atividade.DiaFim.Value.Month.ToString("00") : "";
                file.WriteLine(dia + diaFim + "\t"
                    + atividade.Assunto + "\t" + atividade.DeQuem + "\t" + atividade.Descricao);
            }
        }
        private static void ConsolidaAutoEPetEPlay(TextWriter file, IEnumerable<Activity> atividades)
        {
            foreach (Activity atividade in atividades) {
                string dia = atividade.Dia.Day.ToString("00") + "/" + atividade.Dia.Month.ToString("00");
                string diaFim = (atividade.DiaFim != null) ? "~" + atividade.DiaFim.Value.Day.ToString("00") + "/" + atividade.DiaFim.Value.Month.ToString("00") : "";
                file.WriteLine(dia + diaFim + "\t"
                    + atividade.Assunto + "\t" + atividade.Descricao);
            }
        }
        private static void ConsolidaPessoa(TextWriter file, IEnumerable<Activity> atividades)
        {
            foreach (Activity atividade in atividades) {
                string dia = atividade.Dia.Day.ToString("00") + "/" + atividade.Dia.Month.ToString("00");
                string diaFim = (atividade.DiaFim != null) ? "~" + atividade.DiaFim.Value.Day.ToString("00") + "/" + atividade.DiaFim.Value.Month.ToString("00") : "";
                file.WriteLine(dia + diaFim + "\t"
                    + atividade.Assunto + "\t" + (!string.IsNullOrWhiteSpace(atividade.DeQuem) ? atividade.DeQuem : "") + "\t" + atividade.Descricao);
            }
        }
        private static void ConsolidaCompra(TextWriter file, IEnumerable<Activity> atividades)
        {
            foreach (Activity atividade in atividades) {
                string dia = atividade.Dia.Day.ToString("00") + "/" + atividade.Dia.Month.ToString("00");
                string diaFim = (atividade.DiaFim != null) ? "~" + atividade.DiaFim.Value.Day.ToString("00") + "/" + atividade.DiaFim.Value.Month.ToString("00") : "";
                file.WriteLine(dia + diaFim + "\t"
                    + atividade.Assunto + "\t" + atividade.Valor);
            }
        }

        private static void ConsolidaJogoEWatchEFilmeESerieELivroEComic(TextWriter file, ICollection<Activity> atividades)
        {
            for (var index1 = 0; index1 < atividades.Count; index1++) {
                string dataInicio = null;
                string dataTermino = null;

                Activity atividade = atividades.ElementAt(index1);
                switch (atividade.Classificacao) {
                    case Classification.Comeco:
                        dataInicio = atividade.Dia.Day.ToString("00") + "/" + atividade.Dia.Month.ToString("00");

                        Activity atividadeTermino = atividades
                            .FirstOrDefault(a =>
                                string.Equals(a.Assunto, atividade.Assunto, StringComparison.CurrentCultureIgnoreCase)
                                && a.Classificacao == Classification.Termino
                            );
                        if (atividadeTermino != null) {
                            dataTermino = atividadeTermino.Dia.Day.ToString("00") + "/" + atividadeTermino.Dia.Month.ToString("00");
                            atividades.Remove(atividadeTermino);
                        }
                        break;

                    case Classification.Termino:
                        dataTermino = atividade.Dia.Day.ToString("00") + "/" + atividade.Dia.Month.ToString("00");
                        break;

                    case Classification.Unica:
                        dataInicio = atividade.Dia.Day.ToString("00") + "/" + atividade.Dia.Month.ToString("00");
                        file.WriteLine(dataInicio + "\t" + atividade.Assunto + "\t" + atividade.Descricao);
                        continue;

                    case Classification.Indefinido:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                dataInicio = (dataInicio != null) ? dataInicio + "~" : "?~";
                dataTermino = dataTermino ?? "?";
                file.WriteLine(dataInicio + dataTermino + "\t" + atividade.Assunto + "\t" + atividade.Descricao);
            }
        }
        #endregion
    }
}

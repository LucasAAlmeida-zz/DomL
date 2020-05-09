using DomL.Business;
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
    /// Interaction logic for ClassificarWindow.xaml
    /// </summary>
    public partial class ClassificarWindow
    {
        private List<Activity> _atividades;
        private List<Activity> _atividadesFull;

        const string DIR_PATH = "C:\\Users\\User\\Desktop\\DomL\\";

        public ClassificarWindow()
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
            } catch (Exception exception) {
                MessageLabel2.Content = exception.Message;
                Console.Write(exception);
            }
        }

        #region Classificar
        private void ClassificaAtividades()
        {
            var atividadesDiaString = Regex.Split(AtividadesTextBox.Text, "\r\n");
            var atividadeString = "";
            var isBlocoEspecial = false;

            var diaDT = new DateTime();
            try {
                for (var linha = 0; linha < atividadesDiaString.Length; linha++) {
                    atividadeString = atividadesDiaString[linha];

                    if (string.IsNullOrWhiteSpace(atividadeString)) {
                        continue;
                    }

                    int dia;
                    if (IsNewDay(atividadeString, out dia)) {
                        diaDT = new DateTime(int.Parse(AnoTb.Text), int.Parse(MesTb.Text), dia);
                        continue;
                    }

                    var atividade = new Activity {
                        Dia = diaDT,
                        Ordem = linha,
                        Categoria = Category.Indefinido,
                        FullLine = atividadeString,
                        IsInBlocoEspecial = isBlocoEspecial
                    };

                    var segmentos = Regex.Split(atividadeString, "; ");
                    string categoria = segmentos[0];

                    var isImportante = true;

                    #region switch de categoria
                    switch (categoria) {
                        case "WORK":
                        case "SS":
                            atividade.Categoria = Category.Trabalho;
                            ParseWorkAndDoomFromStringToAtividade(atividade, segmentos);
                            break;

                        case "DOOM":
                        case "DESGRAÇA":
                        case "DESGRACA":
                            atividade.Categoria = Category.Desgraca;
                            ParseWorkAndDoomFromStringToAtividade(atividade, segmentos);
                            break;

                        case "SAUDE":
                        case "SAÚDE":
                        case "HEALTH":
                            atividade.Categoria = Category.Saude;
                            ParseSaudeFromStringToAtividade(atividade, segmentos);
                            break;

                        case "VIAGEM":
                        case "TRIP":
                            atividade.Categoria = Category.Viagem;
                            ParseViagemFromStringToAtividade(atividade, segmentos);
                            break;

                        case "AUTO":
                        case "CARRO":
                            atividade.Categoria = Category.Automovel;
                            ParseAutoAndPetAndPlayFromStringToAtividade(atividade, segmentos);
                            break;

                        case "GIFT":
                        case "PRESENTE":
                            atividade.Categoria = Category.Presente;
                            ParseGiftFromStringToAtividade(atividade, segmentos);
                            break;

                        case "PESSOA":
                            atividade.Categoria = Category.Pessoa;
                            ParsePessoaFromStringToAtividade(atividade, segmentos);
                            break;

                        case "PET":
                        case "ANIMAL":
                            atividade.Categoria = Category.Animal;
                            ParseAutoAndPetAndPlayFromStringToAtividade(atividade, segmentos);
                            break;

                        case "COMPRA":
                            atividade.Categoria = Category.Compra;
                            ParseCompraFromStringToAtividade(atividade, segmentos);
                            break;

                        case "JOGO":
                        case "GAME":
                            atividade.Categoria = Category.Jogo;
                            ParseJogoAndWatchAndFilmeAndSerieAndLivroAndComicFromStringToAtividade(atividade, segmentos);
                            break;

                        case "WATCH":
                            atividade.Categoria = Category.Watch;
                            ParseJogoAndWatchAndFilmeAndSerieAndLivroAndComicFromStringToAtividade(atividade, segmentos);
                            break;

                        case "FILME":
                        case "MOVIE":
                        case "CINEMA":
                            atividade.Categoria = Category.Filme;
                            ParseJogoAndWatchAndFilmeAndSerieAndLivroAndComicFromStringToAtividade(atividade, segmentos);
                            break;

                        case "SERIE":
                        case "SÉRIE":
                        case "DESENHO":
                        case "ANIME":
                        case "CARTOON":
                            atividade.Categoria = Category.Serie;
                            ParseJogoAndWatchAndFilmeAndSerieAndLivroAndComicFromStringToAtividade(atividade, segmentos);
                            break;

                        case "LIVRO":
                        case "BOOK":
                            atividade.Categoria = Category.Livro;
                            ParseJogoAndWatchAndFilmeAndSerieAndLivroAndComicFromStringToAtividade(atividade, segmentos);
                            break;

                        case "COMIC":
                        case "MANGA":
                        case "MANGÁ":
                            atividade.Categoria = Category.Comic;
                            ParseJogoAndWatchAndFilmeAndSerieAndLivroAndComicFromStringToAtividade(atividade, segmentos);
                            break;

                        case "PLAY":
                            atividade.Categoria = Category.Play;
                            ParseAutoAndPetAndPlayFromStringToAtividade(atividade, segmentos);
                            break;

                        default:
                            atividade.Descricao = segmentos[0];
                            if (atividade.Descricao.StartsWith("*")) {
                                atividade.Descricao = atividade.Descricao.Substring(1);
                                atividade.FullLine = atividade.Descricao;
                            } else if (atividade.Descricao.StartsWith("<")) {
                                atividade.IsInBlocoEspecial = true;
                                isBlocoEspecial = !isBlocoEspecial;
                            } else {
                                if (atividade.Descricao.StartsWith("---")) {
                                    atividade.IsInBlocoEspecial = false;
                                }
                                isImportante = false;
                            }
                            break;
                    }
                    #endregion

                    if (isImportante || atividade.IsInBlocoEspecial) {
                        _atividades.Add(atividade);
                    }
                }
            } catch (Exception e) {
                MessageLabel.Content = "Deu ruim no dia " + diaDT.Day + ", atividade: " + atividadeString;
                throw e;
            }
        }

        #region Parse de Atividades
        private static void ParseWorkAndDoomFromStringToAtividade(Activity atividade, IReadOnlyList<string> segmentos)
        {
            //WORK; (Descrição) O que aconteceu
            //DOOM; (Descrição) O que aconteceu

            atividade.Descricao = segmentos[1];
        }
        private static void ParseSaudeFromStringToAtividade(Activity atividade, IReadOnlyList<string> segmentos)
        {
            //SAUDE; (Descrição) o que aconteceu
            //SAUDE; (Assunto) Especialidade médica; (Descrição) o que aconteceu

            if (segmentos.Count == 2) {
                atividade.Descricao = segmentos[1];
            } else {
                atividade.Assunto = segmentos[1];
                atividade.Descricao = segmentos[2];
            }
        }
        private static void ParseViagemFromStringToAtividade(Activity atividade, IReadOnlyList<string> segmentos)
        {
            //VIAGEM; (Assunto) De onde pra onde; (MeioTransporte) Meio de transporte
            //VIAGEM; (Assunto) De onde pra onde; (MeioTransporte) Meio de transporte; (Descrição) o que aconteceu

            atividade.Assunto = segmentos[1];
            atividade.MeioTransporte = segmentos[2];
            if (segmentos.Count == 4) {
                atividade.Descricao = segmentos[3];
            }
        }
        private static void ParseGiftFromStringToAtividade(Activity atividade, IReadOnlyList<string> segmentos)
        {
            //GIFT; (Assunto) O que ganhei; (DeQuem) De quem ganhei o presente
            //GIFT; (Assunto) O que ganhei; (DeQuem) De quem ganhei o presente; (Descrição) o que aconteceu

            atividade.Assunto = segmentos[1];
            atividade.DeQuem = segmentos[2];
            if (segmentos.Count == 4) {
                atividade.Descricao = segmentos[3];
            }
        }
        private static void ParseAutoAndPetAndPlayFromStringToAtividade(Activity atividade, IReadOnlyList<string> segmentos)
        {
            //AUTO; (Assunto) Qual automovel/Pet; (Descricao) O que Aconteceu
            //AUTO; (Descricao) O que Aconteceu

            if (segmentos.Count == 2) {
                atividade.Descricao = segmentos[1];
            } else {
                atividade.Assunto = segmentos[1];
                atividade.Descricao = segmentos[2];
            }
        }
        private static void ParsePessoaFromStringToAtividade(Activity atividade, IReadOnlyList<string> segmentos)
        {
            //PESSOA; (Assunto) Nome da Pessoa; (Descrição) Coisas pra me lembrar
            //PESSOA; (Assunto) Nome da Pessoa; (DeQuem) Origem conheci (amigo de x, furry, etc); (Descrição) Coisas pra me lembrar

            atividade.Assunto = segmentos[1];
            if (segmentos.Count == 3) {
                atividade.Descricao = segmentos[2];
            } else {
                atividade.DeQuem = segmentos[2];
                atividade.Descricao = segmentos[3];
            }
        }
        private static void ParseCompraFromStringToAtividade(Activity atividade, IReadOnlyList<string> segmentos)
        {
            //COMPRA; (De Quem) Loja; (Assunto) O que comprei; (Valor) Quanto custou
            //COMPRA; (De Quem) Loja; (Assunto) O que comprei; (Valor) Quanto custou; (Descrição) Misc

            atividade.DeQuem = segmentos[1];
            atividade.Assunto = segmentos[2];
            atividade.Valor = int.Parse(segmentos[3]);
            if (segmentos.Count == 5) {
                atividade.Descricao = segmentos[4];
            }
        }
        private static void ParseJogoAndWatchAndFilmeAndSerieAndLivroAndComicFromStringToAtividade(Activity atividade, IReadOnlyList<string> segmentos)
        {
            // 3 - JOGO; (Assunto) Título; (Valor) Nota
            // 3 - JOGO; (Assunto) Título; (Classificação) Começo
            // 4 - JOGO; (Assunto) Título; (Valor) Nota; (Descrição) O que achei
            // 4 - JOGO; (Assunto) Título; (Classificação) Término; (Valor) Nota
            // 5 - JOGO; (Assunto) Título; (Classificação) Término; (Valor) Nota; (Descrição) O que achei

            atividade.Assunto = segmentos[1];
            string segmentoToLower = segmentos[2].ToLower();
            string classificacao = null;
            switch (segmentos.Count)
            {
                case 3:
                    if (segmentoToLower == "comeco" || segmentoToLower == "começo" || segmentoToLower == "1") {
                        classificacao = segmentoToLower;
                    } else {
                        atividade.Valor = int.Parse(segmentos[2]);
                    }
                    break;
                case 4:
                    if (segmentoToLower == "termino" || segmentoToLower == "término" || segmentoToLower == "2") {
                        classificacao = segmentoToLower;
                        atividade.Valor = int.Parse(segmentos[3]);
                    } else {
                        atividade.Valor = int.Parse(segmentos[2]);
                        atividade.Descricao = segmentos[3];
                    }
                    break;
                case 5:
                    classificacao = segmentos[2].ToLower();
                    atividade.Valor = int.Parse(segmentos[3]);
                    atividade.Descricao = segmentos[4];
                    break;
                default:
                    throw new Exception("what");
            }

            if (classificacao == null) {
                atividade.Classificacao = Classification.Unica;
                return;
            }

            switch (classificacao) {
                case "comeco": case "começo": case "1": atividade.Classificacao = Classification.Comeco; break;
                case "termino": case "término": case "2": atividade.Classificacao = Classification.Termino; break;
                default: throw new Exception("what");
            }
        }
        #endregion
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
            string filePath = DIR_PATH + "AtividadesMes\\AtividadesMes" + mes + ".txt";
            var fi = new FileInfo(filePath);
            if (fi.Directory != null && !fi.Directory.Exists && fi.DirectoryName != null) {
                Directory.CreateDirectory(fi.DirectoryName);
            }

            using (var file = new StreamWriter(filePath)) {
                var jaPulouLinha = true;
                int dia = _atividades.First().Dia.Day;
                foreach (Activity atividade in _atividades) {
                    string diaStr = atividade.Dia.Day.ToString("00");
                    string mesStr = atividade.Dia.Month.ToString("00");
                    string diaSemana = atividade.Dia.DayOfWeek.ToString().Substring(0, 3);
                    string descricao = atividade.FullLine;

                    if (atividade.IsInBlocoEspecial) {
                        if (atividade.FullLine.StartsWith("<")) {
                            if (!atividade.FullLine.StartsWith("<END>") && !atividade.FullLine.StartsWith("<FIM>")) {
                                // Tag de começo de bloco especial
                                file.WriteLine("");
                                file.WriteLine(diaStr + "/" + mesStr + "\t" + diaSemana + "\t" + descricao);
                            } else {
                                // Tag de fim de bloco especial
                                file.WriteLine("\t\t" + descricao);
                                file.WriteLine("");
                            }
                        } else {
                            if (atividade.Dia.Day != dia) {
                                // atividade novo dia dentro de bloco especial
                                dia = atividade.Dia.Day;
                                file.WriteLine(diaStr + "/" + mesStr + "\t" + diaSemana + "\t" + descricao);
                            } else {
                                // atividade mesmo dia dentro de bloco especial
                                file.WriteLine("\t\t" + descricao);
                            }
                        }
                    } else {
                        // atividade fora de bloco especial
                        if (atividade.Dia.Day != dia) {
                            jaPulouLinha = false;
                            dia = atividade.Dia.Day;
                        }

                        if ((atividade.Dia.DayOfWeek == DayOfWeek.Monday || atividade.Dia.DayOfWeek == DayOfWeek.Saturday) && !jaPulouLinha) {
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

        #region Consolidar
        private void ConsolidaAtividadesImportantesEmArquivo()
        {
            _atividadesFull = new List<Activity>();

            string filePath = DIR_PATH + "AtividadesConsolidadas\\";
            var fi = new FileInfo(filePath);
            if (fi.Directory != null && !fi.Directory.Exists && fi.DirectoryName != null) {
                Directory.CreateDirectory(fi.DirectoryName);
            }

            Category categoria;

            categoria = Category.Trabalho;
            var atividadesCategoria = _atividades.Where(ad => ad.Categoria == categoria).ToList();
            ConsolidaTrabalhoEDoom(filePath + "Trabalho.txt", atividadesCategoria);
            
            ConsolidaTrabalhoEDoom(filePath + "Desgraca.txt", _atividades.Where(ad => ad.Categoria == Category.Desgraca).ToList());
            ConsolidaSaude(filePath + "Saude.txt", _atividades.Where(ad => ad.Categoria == Category.Saude).ToList());
            ConsolidaViagem(filePath + "Viagem.txt", _atividades.Where(ad => ad.Categoria == Category.Viagem).ToList());
            ConsolidaPresente(filePath + "Presente.txt", _atividades.Where(ad => ad.Categoria == Category.Presente).ToList());
            ConsolidaAutoEPetEPlay(filePath + "Automovel.txt", _atividades.Where(ad => ad.Categoria == Category.Automovel).ToList());
            ConsolidaAutoEPetEPlay(filePath + "Animal.txt", _atividades.Where(ad => ad.Categoria == Category.Animal).ToList());
            ConsolidaAutoEPetEPlay(filePath + "Play.txt", _atividades.Where(ad => ad.Categoria == Category.Play).ToList());

            categoria = Category.Pessoa;
            atividadesCategoria = _atividades.Where(ad => ad.Categoria == categoria).ToList();
            ConsolidaPessoa(filePath + "Pessoa.txt", atividadesCategoria, categoria);

            categoria = Category.Compra;
            atividadesCategoria = _atividades.Where(ad => ad.Categoria == categoria).ToList();
            ConsolidaCompra(filePath + "Compra.txt", atividadesCategoria, categoria);

            categoria = Category.Jogo;
            atividadesCategoria = _atividades.Where(ad => ad.Categoria == categoria).ToList();
            ConsolidaJogoEWatchEFilmeESerieELivroEComic(filePath + "Jogo.txt", atividadesCategoria, categoria);

            categoria = Category.Watch;
            atividadesCategoria = _atividades.Where(ad => ad.Categoria == categoria).ToList();
            ConsolidaJogoEWatchEFilmeESerieELivroEComic(filePath + "Watch.txt", atividadesCategoria, categoria);

            categoria = Category.Filme;
            atividadesCategoria = _atividades.Where(ad => ad.Categoria == categoria).ToList();
            ConsolidaJogoEWatchEFilmeESerieELivroEComic(filePath + "Filme.txt", atividadesCategoria, categoria);

            categoria = Category.Serie;
            atividadesCategoria = _atividades.Where(ad => ad.Categoria == categoria).ToList();
            ConsolidaJogoEWatchEFilmeESerieELivroEComic(filePath + "Serie.txt", atividadesCategoria, categoria);

            categoria = Category.Livro;
            atividadesCategoria = _atividades.Where(ad => ad.Categoria == categoria).ToList();
            ConsolidaJogoEWatchEFilmeESerieELivroEComic(filePath + "Livro.txt", atividadesCategoria, categoria);

            categoria = Category.Comic;
            atividadesCategoria = _atividades.Where(ad => ad.Categoria == categoria).ToList();
            ConsolidaJogoEWatchEFilmeESerieELivroEComic(filePath + "Comic.txt", atividadesCategoria, categoria);

            ConsolidaAcontecimentos(filePath + "Acontecimentos.txt", _atividades.Where(ad => ad.Categoria == Category.Indefinido || ad.IsInBlocoEspecial).ToList());

            MessageLabel.Content = "Atividades Importantes Consolidadas";
            MessageLabel2.Content = "";
        }

        #region Consolida Atividades
        private void ConsolidaTrabalhoEDoom(string filePath, ICollection<Activity> atividadesNovas)
        {
            if (atividadesNovas.Count == 0) {
                return;
            }

            var atividadesVelhas = new List<Activity>();

            if (File.Exists(filePath)) {
                Category categoria = atividadesNovas.First().Categoria;
                using (var reader = new StreamReader(filePath)) {
                    string line;
                    while ((line = reader.ReadLine()) != null) {
                        line = line.Replace("\t", ";");
                        var segmentos = Regex.Split(line, ";");

                        Activity atividadeVelha = GetAtividadeVelha(segmentos[0], categoria);

                        atividadeVelha.Descricao = segmentos[1];

                        atividadesVelhas.Add(atividadeVelha);
                    }
                }
            }

            atividadesVelhas.AddRange(GetAtividadesToAdd(atividadesNovas, atividadesVelhas));

            using (var file = new StreamWriter(filePath)) {
                foreach (Activity atividade in atividadesVelhas) {
                    string dia = atividade.Dia.Day.ToString("00") + "/" + atividade.Dia.Month.ToString("00");
                    file.WriteLine(dia + "\t" + atividade.Descricao);
                }
            }

            _atividadesFull.AddRange(atividadesVelhas);
        }

        private void ConsolidaSaude(string filePath, ICollection<Activity> atividadesNovas)
        {
            if (atividadesNovas.Count == 0) {
                return;
            }

            var atividadesVelhas = new List<Activity>();

            if (File.Exists(filePath)) {
                Category categoria = atividadesNovas.First().Categoria;
                using (var reader = new StreamReader(filePath)) {
                    string line;
                    while ((line = reader.ReadLine()) != null) {
                        line = line.Replace("\t", ";");
                        var segmentos = Regex.Split(line, ";");

                        Activity atividadeVelha = GetAtividadeVelha(segmentos[0], categoria);

                        atividadeVelha.Assunto = segmentos[1];
                        atividadeVelha.Descricao = segmentos[2];

                        atividadesVelhas.Add(atividadeVelha);
                    }
                }
            }

            atividadesVelhas.AddRange(GetAtividadesToAdd(atividadesNovas, atividadesVelhas));

            using (var file = new StreamWriter(filePath)) {
                foreach (Activity atividade in atividadesVelhas) {
                    string dia = atividade.Dia.Day.ToString("00") + "/" + atividade.Dia.Month.ToString("00");
                    file.WriteLine(dia + "\t" + (!string.IsNullOrWhiteSpace(atividade.Assunto) ? atividade.Assunto : "") + "\t" + atividade.Descricao);
                }
            }

            _atividadesFull.AddRange(atividadesVelhas);
        }
        private void ConsolidaViagem(string filePath, ICollection<Activity> atividadesNovas)
        {
            if (atividadesNovas.Count == 0) {
                return;
            }

            var atividadesVelhas = new List<Activity>();

            if (File.Exists(filePath)) {
                Category categoria = atividadesNovas.First().Categoria;
                using (var reader = new StreamReader(filePath)) {
                    string line;
                    while ((line = reader.ReadLine()) != null) {
                        line = line.Replace("\t", ";");
                        var segmentos = Regex.Split(line, ";");

                        Activity atividadeVelha = GetAtividadeVelha(segmentos[0], categoria);

                        atividadeVelha.Assunto = segmentos[1];
                        atividadeVelha.MeioTransporte = segmentos[2];
                        atividadeVelha.Descricao = segmentos[3];

                        atividadesVelhas.Add(atividadeVelha);
                    }
                }
            }

            atividadesVelhas.AddRange(GetAtividadesToAdd(atividadesNovas, atividadesVelhas));

            using (var file = new StreamWriter(filePath)) {
                foreach (Activity atividade in atividadesVelhas) {
                    string dia = atividade.Dia.Day.ToString("00") + "/" + atividade.Dia.Month.ToString("00");
                    file.WriteLine(dia + "\t" + atividade.Assunto + "\t" + atividade.MeioTransporte + "\t" + atividade.Descricao);
                }
            }

            _atividadesFull.AddRange(atividadesVelhas);
        }
        private void ConsolidaPresente(string filePath, ICollection<Activity> atividadesNovas)
        {
            if (atividadesNovas.Count == 0) {
                return;
            }

            var atividadesVelhas = new List<Activity>();

            if (File.Exists(filePath)) {
                Category categoria = atividadesNovas.First().Categoria;
                using (var reader = new StreamReader(filePath)) {
                    string line;
                    while ((line = reader.ReadLine()) != null) {
                        line = line.Replace("\t", ";");
                        var segmentos = Regex.Split(line, ";");

                        Activity atividadeVelha = GetAtividadeVelha(segmentos[0], categoria);

                        atividadeVelha.Assunto = segmentos[1];
                        atividadeVelha.DeQuem = segmentos[2];
                        atividadeVelha.Descricao = segmentos[3];

                        atividadesVelhas.Add(atividadeVelha);
                    }
                }
            }

            atividadesVelhas.AddRange(GetAtividadesToAdd(atividadesNovas, atividadesVelhas));

            using (var file = new StreamWriter(filePath)) {
                foreach (Activity atividade in atividadesVelhas) {
                    string dia = atividade.Dia.Day.ToString("00") + "/" + atividade.Dia.Month.ToString("00");
                    file.WriteLine(dia + "\t" + atividade.Assunto + "\t" + atividade.DeQuem + "\t" + atividade.Descricao);
                }
            }

            _atividadesFull.AddRange(atividadesVelhas);
        }
        private void ConsolidaAutoEPetEPlay(string filePath, ICollection<Activity> atividadesNovas)
        {
            if (atividadesNovas.Count == 0) {
                return;
            }

            var atividadesVelhas = new List<Activity>();

            if (File.Exists(filePath)) {
                Category categoria = atividadesNovas.First().Categoria;
                using (var reader = new StreamReader(filePath)) {
                    string line;
                    while ((line = reader.ReadLine()) != null) {
                        line = line.Replace("\t", ";");
                        var segmentos = Regex.Split(line, ";");

                        Activity atividadeVelha = GetAtividadeVelha(segmentos[0], categoria);

                        atividadeVelha.Assunto = segmentos[1];
                        atividadeVelha.Descricao = segmentos[2];

                        atividadesVelhas.Add(atividadeVelha);
                    }
                }
            }

            atividadesVelhas.AddRange(GetAtividadesToAdd(atividadesNovas, atividadesVelhas));

            using (var file = new StreamWriter(filePath)) {
                foreach (Activity atividade in atividadesVelhas) {
                    string dia = atividade.Dia.Day.ToString("00") + "/" + atividade.Dia.Month.ToString("00");
                    file.WriteLine(dia + "\t" + atividade.Assunto + "\t" + atividade.Descricao);
                }
            }

            _atividadesFull.AddRange(atividadesVelhas);
        }
        private void ConsolidaPessoa(string filePath, ICollection<Activity> atividadesNovas, Category categoria)
        {
            var atividadesVelhas = new List<Activity>();

            if (File.Exists(filePath)) {
                using (var reader = new StreamReader(filePath)) {
                    string line;
                    while ((line = reader.ReadLine()) != null) {
                        line = line.Replace("\t", ";");
                        var segmentos = Regex.Split(line, ";");

                        Activity atividadeVelha = GetAtividadeVelha(segmentos[0], categoria);

                        atividadeVelha.Assunto = segmentos[1];
                        atividadeVelha.DeQuem = segmentos[2];
                        atividadeVelha.Descricao = segmentos[3];

                        atividadesVelhas.Add(atividadeVelha);
                    }
                }
            }

            atividadesVelhas.AddRange(GetAtividadesToAdd(atividadesNovas, atividadesVelhas));
            _atividadesFull.AddRange(atividadesVelhas);
            
            if (atividadesNovas.Count == 0) {
                return;
            }

            using (var file = new StreamWriter(filePath)) {
                foreach (Activity atividade in atividadesVelhas) {
                    string dia = atividade.Dia.Day.ToString("00") + "/" + atividade.Dia.Month.ToString("00");
                    file.WriteLine(dia + "\t" + atividade.Assunto + "\t" + (!string.IsNullOrWhiteSpace(atividade.DeQuem) ? atividade.DeQuem : "") + "\t" + atividade.Descricao);
                }
            }
        }
        private void ConsolidaCompra(string filePath, ICollection<Activity> atividadesNovas, Category categoria)
        {
            var atividadesVelhas = new List<Activity>();

            if (File.Exists(filePath)) {
                using (var reader = new StreamReader(filePath)) {
                    string line;
                    while ((line = reader.ReadLine()) != null) {
                        line = line.Replace("\t", ";");
                        var segmentos = Regex.Split(line, ";");

                        Activity atividadeVelha = GetAtividadeVelha(segmentos[0], categoria);

                        atividadeVelha.DeQuem = segmentos[1];
                        atividadeVelha.Assunto = segmentos[2];
                        atividadeVelha.Valor = int.Parse(segmentos[3]);
                        atividadeVelha.Descricao = segmentos[4];

                        atividadesVelhas.Add(atividadeVelha);
                    }
                }
            }

            atividadesVelhas.AddRange(GetAtividadesToAdd(atividadesNovas, atividadesVelhas));
            _atividadesFull.AddRange(atividadesVelhas);
            
            if (atividadesNovas.Count == 0) {
                return;
            }

            using (var file = new StreamWriter(filePath)) {
                foreach (Activity atividade in atividadesVelhas) {
                    string dia = atividade.Dia.Day.ToString("00") + "/" + atividade.Dia.Month.ToString("00");
                    file.WriteLine(dia + "\t" + atividade.DeQuem + "\t" + atividade.Assunto + "\t" + atividade.Valor + "\t" + atividade.Descricao);
                }
            }
        }
        private void ConsolidaJogoEWatchEFilmeESerieELivroEComic(string filePath, ICollection<Activity> atividadesNovas, Category categoria)
        {
            var atividadesVelhas = new List<Activity>();

            if (File.Exists(filePath)) {
                using (var reader = new StreamReader(filePath)) {
                    string line;
                    while ((line = reader.ReadLine()) != null) {
                        line = line.Replace("\t", ";");
                        var segmentos = Regex.Split(line, ";");

                        Activity atividadeVelha;
                        int dia;
                        int mes;

                        if (segmentos[0].Length == 5) {
                            //se for entrada única
                            dia = int.Parse(segmentos[0].Substring(0, 2));
                            mes = int.Parse(segmentos[0].Substring(3, 2));

                            atividadeVelha = new Activity {
                                Categoria = categoria,
                                Dia = new DateTime(int.Parse(AnoTb.Text), mes, dia),
                                Classificacao = Classification.Unica,
                                Assunto = segmentos[1],
                                Valor = int.Parse(segmentos[2]),
                                Descricao = segmentos[3]
                            };

                            atividadesVelhas.Add(atividadeVelha);
                            continue;
                        }

                        if (int.TryParse(segmentos[0].Substring(0, 2), out dia)) {
                            // se tiver data de inicio
                            mes = int.Parse(segmentos[0].Substring(3, 2));

                            atividadeVelha = new Activity {
                                Categoria = categoria,
                                Dia = new DateTime(int.Parse(AnoTb.Text), mes, dia),
                                Classificacao = Classification.Comeco,
                                Assunto = segmentos[1]
                            };

                            atividadesVelhas.Add(atividadeVelha);

                            if (!int.TryParse(segmentos[0].Substring(6, 2), out dia)) {
                                continue;
                            }

                            //se tiver data de fim também
                            mes = int.Parse(segmentos[0].Substring(9, 2));

                            atividadeVelha = new Activity {
                                Categoria = categoria,
                                Dia = new DateTime(int.Parse(AnoTb.Text), mes, dia),
                                Classificacao = Classification.Termino,
                                Assunto = segmentos[1],
                                Valor = int.Parse(segmentos[2]),
                                Descricao = segmentos[3]
                            };

                            atividadesVelhas.Add(atividadeVelha);
                        } else {
                            // se só tiver data de fim
                            dia = int.Parse(segmentos[0].Substring(3, 2));
                            mes = int.Parse(segmentos[0].Substring(6, 2));

                            atividadeVelha = new Activity {
                                Categoria = categoria,
                                Dia = new DateTime(int.Parse(AnoTb.Text), mes, dia),
                                Classificacao = Classification.Termino,
                                Assunto = segmentos[1],
                                Valor = int.Parse(segmentos[2]),
                                Descricao = segmentos[3]
                            };

                            atividadesVelhas.Add(atividadeVelha);
                        }
                    }
                }
            }

            atividadesVelhas.AddRange(GetAtividadesToAdd(atividadesNovas, atividadesVelhas));
            _atividadesFull.AddRange(atividadesVelhas);

            if (atividadesNovas.Count == 0) {
                return;
            }

            using (var file = new StreamWriter(filePath)) {
                for (var index = 0; index < atividadesVelhas.Count; index++) {
                    string dataInicio = null;
                    string dataTermino = null;

                    Activity atividade = atividadesVelhas.ElementAt(index);
                    switch (atividade.Classificacao) {
                        case Classification.Unica:
                            dataInicio = atividade.Dia.Day.ToString("00") + "/" + atividade.Dia.Month.ToString("00");
                            file.WriteLine(dataInicio + "\t" + atividade.Assunto + "\t" + atividade.Valor + "\t" + atividade.Descricao);
                            continue;

                        case Classification.Comeco:
                            dataInicio = atividade.Dia.Day.ToString("00") + "/" + atividade.Dia.Month.ToString("00");

                            Activity atividadeTermino = atividadesVelhas.FirstOrDefault(a => a.Classificacao == Classification.Termino && IsEqualTitle(a.Assunto, atividade.Assunto));
                            if (atividadeTermino != null) {
                                dataTermino = atividadeTermino.Dia.Day.ToString("00") + "/" + atividadeTermino.Dia.Month.ToString("00");
                                file.WriteLine(dataInicio + "~" + dataTermino + "\t" + atividade.Assunto + "\t" + atividadeTermino.Valor + "\t" + atividadeTermino.Descricao);
                            }
                            else {
                                file.WriteLine(dataInicio + "~??\t" + atividade.Assunto + "\t?\t");
                            }
                            continue;

                        case Classification.Termino:
                            //Pra não fazer duas vezes a mesma atividade
                            Activity atividadeComeco = atividadesVelhas.FirstOrDefault(a => a.Classificacao == Classification.Comeco && IsEqualTitle(a.Assunto, atividade.Assunto));
                            if (atividadeComeco != null) {
                                continue;
                            }

                            dataTermino = atividade.Dia.Day.ToString("00") + "/" + atividade.Dia.Month.ToString("00");
                            file.WriteLine("??~" + dataTermino + "\t" + atividade.Assunto + "\t" + atividade.Valor + "\t" + atividade.Descricao);
                            continue;

                        case Classification.Indefinido:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }

        private void ConsolidaAcontecimentos(string filePath, ICollection<Activity> atividadesNovas)
        {
            if (atividadesNovas.Count == 0) {
                return;
            }

            var atividadesVelhas = new List<Activity>();

            if (File.Exists(filePath)) {
                Category categoria = atividadesNovas.First().Categoria;
                using (var reader = new StreamReader(filePath)) {
                    bool isBlocoEspecial = false;

                    string line = reader.ReadLine();
                    while (line == "") {
                        line = reader.ReadLine();
                    }
                    string diaMesStr = "";
                    while (line != null) {
                        //line = line.Replace("\t", ";");
                        var segmentos = Regex.Split(line, "\t");
                        if (!string.IsNullOrWhiteSpace(segmentos[0])) {
                            diaMesStr = segmentos[0];
                        }

                        Activity atividadeVelha = GetAtividadeVelha(diaMesStr, categoria);
                        atividadeVelha.FullLine = segmentos[1];
                        atividadeVelha.IsInBlocoEspecial = isBlocoEspecial;
                        if (atividadeVelha.FullLine.StartsWith("<")) {
                            atividadeVelha.IsInBlocoEspecial = true;
                            isBlocoEspecial = !isBlocoEspecial;
                        }

                        line = reader.ReadLine();
                        while (line == "") {
                            line = reader.ReadLine();
                        }
                        atividadesVelhas.Add(atividadeVelha);
                    }
                }
            }

            atividadesVelhas.AddRange(GetAtividadesToAdd(atividadesNovas, atividadesVelhas));

            using (var file = new StreamWriter(filePath)) {
                int dia = atividadesVelhas.First().Dia.Day;
                foreach (Activity atividade in atividadesVelhas) {
                    string diaStr = atividade.Dia.Day.ToString("00") + "/" + atividade.Dia.Month.ToString("00");
                    if (atividade.IsInBlocoEspecial) {
                        if (atividade.FullLine.StartsWith("<")) {
                            if (!atividade.FullLine.StartsWith("<END>") && !atividade.FullLine.StartsWith("<FIM>")) {
                                // Tag de começo de bloco especial
                                dia = atividade.Dia.Day;
                                file.WriteLine("");
                                file.WriteLine(diaStr + "\t" + atividade.FullLine);
                            }
                            else {
                                // Tag de fim de bloco especial
                                file.WriteLine("\t" + atividade.FullLine);
                                file.WriteLine("");
                            }
                        } else {
                            if (atividade.Dia.Day != dia) {
                                // atividade novo dia dentro de bloco especial
                                dia = atividade.Dia.Day;
                                file.WriteLine(diaStr + "\t" + atividade.FullLine);
                            } else {
                                // atividade mesmo dia dentro de bloco especial
                                file.WriteLine("\t" + atividade.FullLine);
                            }
                        }
                    } else {
                        // atividade fora de bloco especial
                        file.WriteLine(diaStr + "\t" + atividade.FullLine);
                    }
                }
            }

            //não precisa colocar nas atividades full pq não vai gerar estatísticas em relação à categoria acontecimentos
            //_atividadesFull.AddRange(atividadesVelhas);
        }
        #endregion

        private static bool IsEqualTitle(string titulo1, string titulo2)
        {
            string titulo1Limpo = titulo1.Replace(":", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace(" ", "").ToLower();
            string titulo2Limpo = titulo2.Replace(":", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace(" ", "").ToLower();
            return titulo1Limpo == titulo2Limpo;
        }
        private Activity GetAtividadeVelha(string diaMes, Category categoria)
        {
            int dia = int.Parse(diaMes.Substring(0, 2));
            int mes = int.Parse(diaMes.Substring(3, 2));
            var atividadeVelha = new Activity {
                Categoria = categoria,
                Dia = new DateTime(int.Parse(AnoTb.Text), mes, dia)
            };
            return atividadeVelha;
        }
        private static IEnumerable<Activity> GetAtividadesToAdd(IEnumerable<Activity> atividadesNovas, List<Activity> atividadesVelhas)
        {
            var atividadesToAdd = new List<Activity>();
            foreach (Activity atividade in atividadesNovas) {
                Activity atividadeVelha = atividadesVelhas.FirstOrDefault(av => av.Dia == atividade.Dia);
                if (atividadeVelha == null) { atividadesToAdd.Add(atividade); }
            }
            return atividadesToAdd;
        }
        #endregion

        #region Gera Estatisticas
        private void GeraEstatisticas()
        {
            string filePath = DIR_PATH + "ResumoAno.txt";
            using (var file = new StreamWriter(filePath)) {
                // ReSharper disable once JoinDeclarationAndInitializer
                int numero;

                numero = _atividadesFull.Count(af => af.Categoria == Category.Jogo && af.Classificacao == Classification.Comeco);
                file.WriteLine("Jogos começados:\t" + numero);

                numero = _atividadesFull.Count(af => af.Categoria == Category.Jogo && (af.Classificacao == Classification.Termino || af.Classificacao == Classification.Unica));
                file.WriteLine("Jogos terminados:\t" + numero);

                numero = _atividadesFull.Count(af => af.Categoria == Category.Filme && (af.Classificacao == Classification.Termino || af.Classificacao == Classification.Unica));
                file.WriteLine("Filmes assistidos:\t" + numero);

                numero = _atividadesFull.Count(af => af.Categoria == Category.Serie && (af.Classificacao == Classification.Termino || af.Classificacao == Classification.Unica));
                file.WriteLine("Temporadas de séries assistidas:\t" + numero);

                numero = _atividadesFull.Count(af => af.Categoria == Category.Livro && (af.Classificacao == Classification.Termino || af.Classificacao == Classification.Unica));
                file.WriteLine("Livros lidos:\t" + numero);

                numero = _atividadesFull.Count(af => af.Categoria == Category.Comic && (af.Classificacao == Classification.Termino || af.Classificacao == Classification.Unica));
                file.WriteLine("K Páginas de comics lidos:\t" + numero);

                numero = _atividadesFull.Count(af => af.Categoria == Category.Viagem);
                file.WriteLine("Viagens feitas:\t" + numero);

                numero = _atividadesFull.Count(af => af.Categoria == Category.Pessoa);
                file.WriteLine("Pessoas novas conhecidas:\t" + numero);

                numero = _atividadesFull.Count(af => af.Categoria == Category.Compra);
                file.WriteLine("Compras notáveis:\t" + numero);
            }
        }
        #endregion
    }
}

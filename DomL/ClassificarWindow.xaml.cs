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

        public ClassificarWindow()
        {
            InitializeComponent();

            MesTb.Text = (DateTime.Now.Month - 1).ToString();
            AnoTb.Text = DateTime.Now.Year.ToString();
        }

        private void CategorizarButton_Click(object sender, RoutedEventArgs e)
        {
            _atividades = new List<Activity>();

            try {
                ClassificaAtividades();
                EscreverAtividadesDoMesEmArquivo();
                ConsolidaAtividadesImportantesEmArquivo();
                GeraEstatisticas();
            } catch (Exception ex) {
                MessageLabel.Content = ex.Message;
            }
        }


        #region Classificar
        private void ClassificaAtividades()
        {
            var atividadesDiaString = Regex.Split(AtividadesTextBox.Text, "\r\n");
            var atividadeString = "";
            DateTime diaDT = DateTime.MinValue;
            var isInBlocoEspecial = false;
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

                    if (diaDT == DateTime.MinValue) {
                        throw new Exception("Data inválida");
                    }

                    var atividade = new Activity {
                        Dia = diaDT,
                        Ordem = linha,
                        Categoria = Category.Indefinido,
                        FullLine = atividadeString,
                        IsInBlocoEspecial = isInBlocoEspecial
                    };

                    var segmentos = Regex.Split(atividadeString, ";");
                    for (var index = 0; index < segmentos.Length; index++) {
                        segmentos[index] = segmentos[index].Trim();
                    }
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
                            ParseJogoAndWatchAndFilmeAndSerieAndLivroAndComicAndCursoFromStringToAtividade(atividade, segmentos);
                            break;

                        case "WATCH":
                            atividade.Categoria = Category.Watch;
                            ParseJogoAndWatchAndFilmeAndSerieAndLivroAndComicAndCursoFromStringToAtividade(atividade, segmentos);
                            break;

                        case "FILME":
                        case "MOVIE":
                        case "CINEMA":
                            atividade.Categoria = Category.Filme;
                            ParseJogoAndWatchAndFilmeAndSerieAndLivroAndComicAndCursoFromStringToAtividade(atividade, segmentos);
                            break;

                        case "SERIE":
                        case "SÉRIE":
                        case "DESENHO":
                        case "ANIME":
                        case "CARTOON":
                            atividade.Categoria = Category.Serie;
                            ParseJogoAndWatchAndFilmeAndSerieAndLivroAndComicAndCursoFromStringToAtividade(atividade, segmentos);
                            break;

                        case "LIVRO":
                        case "BOOK":
                            atividade.Categoria = Category.Livro;
                            ParseJogoAndWatchAndFilmeAndSerieAndLivroAndComicAndCursoFromStringToAtividade(atividade, segmentos);
                            break;

                        case "COMIC":
                        case "MANGA":
                        case "MANGÁ":
                            atividade.Categoria = Category.Comic;
                            ParseJogoAndWatchAndFilmeAndSerieAndLivroAndComicAndCursoFromStringToAtividade(atividade, segmentos);
                            break;

                        case "CURSO":
                        case "COURSE":
                            atividade.Categoria = Category.Curso;
                            ParseJogoAndWatchAndFilmeAndSerieAndLivroAndComicAndCursoFromStringToAtividade(atividade, segmentos);
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
                            } else if (atividade.Descricao.StartsWith("<") && atividade.Descricao.EndsWith(">")) {
                                isInBlocoEspecial = !isInBlocoEspecial;
                                atividade.IsInBlocoEspecial = true;
                            } else {
                                isImportante = (isInBlocoEspecial && !atividade.FullLine.Contains("---"));
                            }
                            break;
                    }
                    #endregion

                    if (isImportante) {
                        _atividades.Add(atividade);
                    }
                }
            } catch (Exception ex) {
                throw new Exception("Deu ruim no dia " + diaDT.Day + ", atividade: " + atividadeString + "\n" + ex.Message);
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
            //COMPRA; (Assunto) O que comprei; (Valor) Quanto custou
            //COMPRA; (Assunto) O que comprei; (Valor) Quanto custou; (Descrição) Misc

            atividade.Assunto = segmentos[1];
            atividade.Valor = int.Parse(segmentos[2]);
            if (segmentos.Count == 4) {
                atividade.Descricao = segmentos[3];
            }
        }
        private static void ParseJogoAndWatchAndFilmeAndSerieAndLivroAndComicAndCursoFromStringToAtividade(Activity atividade, IReadOnlyList<string> segmentos)
        {
            // 3 JOGO; (Assunto) Título; (Valor) Nota
            // 4 JOGO; (Assunto) Título; (Valor) Nota; (Descrição) O que achei
            // 3 JOGO; (Assunto) Título; (Classificação) Começo
            // 4 JOGO; (Assunto) Título; (Classificação) Término; (Valor) Nota
            // 5 JOGO; (Assunto) Título; (Classificação) Término; (Valor) Nota; (Descrição) O que achei

            atividade.Assunto = segmentos[1];
            atividade.Classificacao = ObterClassificacaoEntretenimento(segmentos);
            atividade.Valor = ObterNotaEntretenimento(segmentos, atividade.Classificacao);
            atividade.Descricao = ObterDescricaoEntretenimento(segmentos, atividade.Classificacao);
        }

        #endregion
        private static bool IsNewDay(string linha, out int dia)
        {
            int indexPrimeiroEspaco = linha.IndexOf(" ", StringComparison.Ordinal);
            string firstWord = (indexPrimeiroEspaco != -1) ? linha.Substring(0, indexPrimeiroEspaco) : linha;
            return int.TryParse(firstWord, out dia) && linha.Contains(" - ");
        }
        private static Classification ObterClassificacaoEntretenimento(IReadOnlyList<string> segmentos)
        {
            switch (segmentos[2].ToLower()) {
                case "comeco": case "começo": case "1": return Classification.Comeco;
                case "termino": case "término": case "2": return Classification.Termino;
                default: return Classification.Unica;
            }
        }
        private static int ObterNotaEntretenimento(IReadOnlyList<string> segmentos, Classification classificacao)
        {
            var notaString = "0";
            if (classificacao == Classification.Unica) {
                notaString = Regex.Split(segmentos[2], "/")[0];
            } else if (classificacao == Classification.Termino) {
                notaString = Regex.Split(segmentos[3], "/")[0];
            }

            return int.Parse(notaString);
        }
        private static string ObterDescricaoEntretenimento(IReadOnlyList<string> segmentos, Classification classificacao)
        {
            if (segmentos.Count == 5) {
                return segmentos[4];
            }

            if (classificacao == Classification.Unica && segmentos.Count == 4) {
                return segmentos[3];
            }

            return null;
        }
        #endregion

        #region Escrever Mes
        private void EscreverAtividadesDoMesEmArquivo()
        {
            string filePath = "C:\\Users\\LUCASAUGUSTO\\Desktop\\DomL\\AtividadesMes\\AtividadesMes" + int.Parse(MesTb.Text).ToString("00") + ".txt";
            var fi = new FileInfo(filePath);
            if (fi.Directory != null && !fi.Directory.Exists && fi.DirectoryName != null) {
                Directory.CreateDirectory(fi.DirectoryName);
            }

            using (var file = new StreamWriter(filePath)) {
                Activity atividadeAnterior = null;
                foreach (Activity atividade in _atividades) {
                    if (PodePularLinha(atividade, atividadeAnterior)) {
                        file.WriteLine("");
                    }

                    var data = "\t";
                    if (PodeEscreverDia(atividade, atividadeAnterior)) {
                        string diaStr = atividade.Dia.Day.ToString("00");
                        string mesStr = atividade.Dia.Month.ToString("00");
                        string diaSemana = atividade.Dia.DayOfWeek.ToString().Substring(0, 3);

                        data = diaStr + "/" + mesStr + "; " + diaSemana + "; ";
                    }

                    file.WriteLine(data + atividade.FullLine);

                    atividadeAnterior = atividade;
                }
            }
            MessageLabel.Content = "Mês consolidado com sucesso";
        }

        private static bool PodeEscreverDia(Activity atividade, Activity atividadeAnterior)
        {
            if (atividadeAnterior == null) {
                return true;
            }

            if (atividadeAnterior.FullLine == "<END>") {
                return true;
            }

            if (atividade.Dia == atividadeAnterior.Dia && atividade.IsInBlocoEspecial && atividadeAnterior.IsInBlocoEspecial) {
                return false;
            }

            return true;
        }

        private static bool PodePularLinha(Activity atividade, Activity atividadeAnterior)
        {
            if (atividadeAnterior == null) {
                return false;
            }

            if (IsLineStartOfSpecialBlock(atividade.FullLine) || IsLineEndOfSpecialBlock(atividadeAnterior)) {
                return true;
            }

            if (atividadeAnterior.IsInBlocoEspecial && atividade.IsInBlocoEspecial) {
                return false;
            }

            if (atividade.Dia != atividadeAnterior.Dia
                &&
                (atividade.Dia.DayOfWeek == DayOfWeek.Monday || atividade.Dia.DayOfWeek == DayOfWeek.Saturday
                || atividadeAnterior.Dia.DayOfWeek == DayOfWeek.Friday || atividadeAnterior.Dia.DayOfWeek == DayOfWeek.Sunday)
            ) {
                return true;
            }

            return false;
        }

        #endregion

        #region Consolidar
        private void ConsolidaAtividadesImportantesEmArquivo()
        {
            _atividadesFull = new List<Activity>();

            const string filePath = "C:\\Users\\LUCASAUGUSTO\\Desktop\\DomL\\AtividadesConsolidadas\\";
            var fi = new FileInfo(filePath);
            if (fi.Directory != null && !fi.Directory.Exists && fi.DirectoryName != null) {
                Directory.CreateDirectory(fi.DirectoryName);
            }

            ConsolidaTrabalhoEDoom(filePath + "Trabalho.txt", _atividades.Where(a => a.Categoria == Category.Trabalho).ToList(), Category.Trabalho);
            ConsolidaTrabalhoEDoom(filePath + "Desgraca.txt", _atividades.Where(a => a.Categoria == Category.Desgraca).ToList(), Category.Desgraca);
            ConsolidaSaude(filePath + "Saude.txt", _atividades.Where(a => a.Categoria == Category.Saude).ToList(), Category.Saude);
            ConsolidaViagem(filePath + "Viagem.txt", _atividades.Where(a => a.Categoria == Category.Viagem).ToList(), Category.Viagem);
            ConsolidaPresente(filePath + "Presente.txt", _atividades.Where(a => a.Categoria == Category.Presente).ToList(), Category.Presente);
            ConsolidaAutoEPetEPlay(filePath + "Automovel.txt", _atividades.Where(a => a.Categoria == Category.Automovel).ToList(), Category.Automovel);
            ConsolidaAutoEPetEPlay(filePath + "Animal.txt", _atividades.Where(a => a.Categoria == Category.Animal).ToList(), Category.Animal);
            ConsolidaAutoEPetEPlay(filePath + "Play.txt", _atividades.Where(a => a.Categoria == Category.Play).ToList(), Category.Play);
            ConsolidaPessoa(filePath + "Pessoa.txt", _atividades.Where(a => a.Categoria == Category.Pessoa).ToList(), Category.Pessoa);
            ConsolidaCompra(filePath + "Compra.txt", _atividades.Where(a => a.Categoria == Category.Compra).ToList(), Category.Compra);
            ConsolidaJogoEWatchEFilmeESerieELivroEComicECurso(filePath + "Jogo.txt", _atividades.Where(a => a.Categoria == Category.Jogo).ToList(), Category.Jogo);
            ConsolidaJogoEWatchEFilmeESerieELivroEComicECurso(filePath + "Watch.txt", _atividades.Where(a => a.Categoria == Category.Watch).ToList(), Category.Watch);
            ConsolidaJogoEWatchEFilmeESerieELivroEComicECurso(filePath + "Filme.txt", _atividades.Where(a => a.Categoria == Category.Filme).ToList(), Category.Filme);
            ConsolidaJogoEWatchEFilmeESerieELivroEComicECurso(filePath + "Serie.txt", _atividades.Where(a => a.Categoria == Category.Serie).ToList(), Category.Serie);
            ConsolidaJogoEWatchEFilmeESerieELivroEComicECurso(filePath + "Livro.txt", _atividades.Where(a => a.Categoria == Category.Livro).ToList(), Category.Livro);
            ConsolidaJogoEWatchEFilmeESerieELivroEComicECurso(filePath + "Comic.txt", _atividades.Where(a => a.Categoria == Category.Comic).ToList(), Category.Comic);
            ConsolidaJogoEWatchEFilmeESerieELivroEComicECurso(filePath + "Curso.txt", _atividades.Where(a => a.Categoria == Category.Curso).ToList(), Category.Curso);
            ConsolidaAcontecimentos(
                filePath + "Acontecimentos" + int.Parse(MesTb.Text).ToString("00") + ".txt",
                _atividades.Where(a => a.Categoria == Category.Indefinido || a.IsInBlocoEspecial).ToList()
            );

            MessageLabel.Content = "Atividades Importantes Consolidadas";
        }

        #region Consolida Atividades
        private void ConsolidaTrabalhoEDoom(string filePath, ICollection<Activity> atividadesNovas, Category categoria)
        {
            var atividadesVelhas = new List<Activity>();

            if (File.Exists(filePath)) {
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
            _atividadesFull.AddRange(atividadesVelhas);

            if (atividadesNovas.Count == 0) {
                return;
            }

            using (var file = new StreamWriter(filePath)) {
                foreach (Activity atividade in atividadesVelhas) {
                    string dia = atividade.Dia.Day.ToString("00") + "/" + atividade.Dia.Month.ToString("00");
                    file.WriteLine(dia + "\t" + atividade.Descricao);
                }
            }

        }
        private void ConsolidaSaude(string filePath, ICollection<Activity> atividadesNovas, Category categoria)
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
                        atividadeVelha.Descricao = segmentos[2];

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
                    file.WriteLine(dia + "\t" + (!string.IsNullOrWhiteSpace(atividade.Assunto) ? atividade.Assunto : "") + "\t" + atividade.Descricao);
                }
            }
        }
        private void ConsolidaViagem(string filePath, ICollection<Activity> atividadesNovas, Category categoria)
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
                        atividadeVelha.MeioTransporte = segmentos[2];
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
                    file.WriteLine(dia + "\t" + atividade.Assunto + "\t" + atividade.MeioTransporte + "\t" + atividade.Descricao);
                }
            }
        }
        private void ConsolidaPresente(string filePath, ICollection<Activity> atividadesNovas, Category categoria)
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
                    file.WriteLine(dia + "\t" + atividade.Assunto + "\t" + atividade.DeQuem + "\t" + atividade.Descricao);
                }
            }
        }
        private void ConsolidaAutoEPetEPlay(string filePath, ICollection<Activity> atividadesNovas, Category categoria)
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
                        atividadeVelha.Descricao = segmentos[2];

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
                    file.WriteLine(dia + "\t" + atividade.Assunto + "\t" + atividade.Descricao);
                }
            }
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

                        atividadeVelha.Assunto = segmentos[1];
                        atividadeVelha.Valor = int.Parse(segmentos[2]);
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
                    file.WriteLine(dia + "\t" + atividade.Assunto + "\t" + atividade.Valor + "\t" + atividade.Descricao);
                }
            }
        }
        private void ConsolidaJogoEWatchEFilmeESerieELivroEComicECurso(string filePath, ICollection<Activity> atividadesNovas, Category categoria)
        {
            var atividadesVelhas = new List<Activity>();

            if (File.Exists(filePath)) {
                using (var reader = new StreamReader(filePath)) {
                    string line;
                    while ((line = reader.ReadLine()) != null) {
                        line = line.Replace("\t", ";");
                        var segmentos = Regex.Split(line, ";");

                        var atividadeVelha = new Activity {
                            Categoria = categoria,
                            Assunto = segmentos[1],
                            Descricao = (segmentos.Length == 3) ? segmentos[2] : ""
                        };
                        int dia;
                        int mes;

                        if (segmentos[0].Length == 5) {
                            dia = int.Parse(segmentos[0].Substring(0, 2));
                            mes = int.Parse(segmentos[0].Substring(3, 2));
                            atividadeVelha.Dia = new DateTime(int.Parse(AnoTb.Text), mes, dia);
                            atividadeVelha.Classificacao = Classification.Unica;
                        } else if (int.TryParse(segmentos[0].Substring(0, 2), out dia)) {
                            mes = int.Parse(segmentos[0].Substring(3, 2));
                            atividadeVelha.Dia = new DateTime(int.Parse(AnoTb.Text), mes, dia);
                            atividadeVelha.Classificacao = Classification.Comeco;

                            if (int.TryParse(segmentos[0].Substring(6, 2), out dia)) {
                                atividadesVelhas.Add(atividadeVelha);

                                mes = int.Parse(segmentos[0].Substring(9, 2));
                                atividadeVelha = new Activity {
                                    Categoria = categoria,
                                    Dia = new DateTime(int.Parse(AnoTb.Text), mes, dia),
                                    Classificacao = Classification.Termino,
                                    Assunto = segmentos[1],
                                    Descricao = segmentos[2]
                                };
                            }
                        } else {
                            dia = int.Parse(segmentos[0].Substring(3, 2));
                            mes = int.Parse(segmentos[0].Substring(6, 2));
                            atividadeVelha.Dia = new DateTime(int.Parse(AnoTb.Text), mes, dia);
                            atividadeVelha.Classificacao = Classification.Termino;
                        }

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
                for (var index = 0; index < atividadesVelhas.Count; index++) {
                    string dataInicio = null;
                    string dataTermino = null;

                    Activity atividade = atividadesVelhas.ElementAt(index);
                    switch (atividade.Classificacao) {
                        case Classification.Comeco:
                            dataInicio = atividade.Dia.Day.ToString("00") + "/" + atividade.Dia.Month.ToString("00");

                            Activity atividadeTermino = atividadesVelhas.FirstOrDefault(a => a.Classificacao == Classification.Termino && IsEqualTitle(a.Assunto, atividade.Assunto));
                            if (atividadeTermino != null) {
                                dataTermino = atividadeTermino.Dia.Day.ToString("00") + "/" + atividadeTermino.Dia.Month.ToString("00");
                            }
                            break;

                        case Classification.Termino:
                            Activity atividadeComeco = atividadesVelhas.FirstOrDefault(a => a.Classificacao == Classification.Comeco && IsEqualTitle(a.Assunto, atividade.Assunto));
                            if (atividadeComeco != null) {
                                continue;
                            }
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

                    dataInicio = (dataInicio != null) ? dataInicio + "~" : "??~";
                    dataTermino = dataTermino ?? "??";
                    file.WriteLine(dataInicio + dataTermino + "\t" + atividade.Assunto + "\t" + atividade.Descricao);
                }
            }
        }
        private void ConsolidaAcontecimentos(string filePath, ICollection<Activity> atividades)
        {
            if (atividades.Count == 0) {
                return;
            }

            using (var file = new StreamWriter(filePath)) {
                Activity atividadeAnterior = null;
                foreach (Activity atividade in atividades) {

                    if (IsLineStartOfSpecialBlock(atividade.FullLine) || IsLineEndOfSpecialBlock(atividadeAnterior)) {
                        file.WriteLine("");
                    }

                    var dia = "\t\t";
                    if (PodeEscreverDia(atividade, atividadeAnterior)) {
                        dia = atividade.Dia.Day.ToString("00") + "/" + atividade.Dia.Month.ToString("00") + "\t";
                    }

                    file.WriteLine(dia + atividade.FullLine);

                    atividadeAnterior = atividade;
                }
            }
        }

        private static bool IsLineEndOfSpecialBlock(Activity atividade)
        {
            return atividade != null && atividade.FullLine == "<END>";
        }

        private static bool IsLineStartOfSpecialBlock(string line)
        {
            return line.StartsWith("<") && line.EndsWith(">") && line != "<END>";
        }

        #endregion

        private static bool IsEqualTitle(string titulo1, string titulo2)
        {
            string titulo1Limpo = titulo1.Replace(":", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace(" ", "").ToLower();
            string titulo2Limpo = titulo2.Replace(":", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace(" ", "").ToLower();
            return titulo1Limpo == titulo2Limpo;
        }
        private Activity GetAtividadeVelha(string diaMes, Category categoria, bool isInBlocoEspecial = false)
        {
            int dia = int.Parse(diaMes.Substring(0, 2));
            int mes = int.Parse(diaMes.Substring(3, 2));
            var atividadeVelha = new Activity {
                Categoria = categoria,
                Dia = new DateTime(int.Parse(AnoTb.Text), mes, dia),
                IsInBlocoEspecial = isInBlocoEspecial
            };
            return atividadeVelha;
        }
        private static IEnumerable<Activity> GetAtividadesToAdd(IEnumerable<Activity> atividadesNovas, List<Activity> atividadesVelhas)
        {
            var atividadesToAdd = new List<Activity>();
            foreach (Activity atividade in atividadesNovas) {
                Activity atividadeVelha = atividadesVelhas.FirstOrDefault(av => av.Dia == atividade.Dia);
                if (atividadeVelha == null) {
                    atividadesToAdd.Add(atividade);
                }
            }
            return atividadesToAdd;
        }
        #endregion

        #region Gera Estatisticas
        private void GeraEstatisticas()
        {
            const string filePath = "C:\\Users\\LUCASAUGUSTO\\Desktop\\DomL\\ResumoAno.txt";
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

                numero = _atividadesFull.Count(af => af.Categoria == Category.Curso && (af.Classificacao == Classification.Termino || af.Classificacao == Classification.Unica));
                file.WriteLine("Cursos terminados:\t" + numero);

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

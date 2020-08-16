using DomL.Business;
using DomL.Business.Activities;
using DomL.Business.Activities.MultipleDayActivities;
using DomL.Business.Activities.SingleDayActivities;
using DomL.Business.Services;
using DomL.Business.Utils.DTOs;
using DomL.Presentation;
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
        public MainWindow()
        {
            this.InitializeComponent();

            this.MesTb.Text = (DateTime.Now.Month - 1).ToString();
            this.AnoTb.Text = DateTime.Now.Year.ToString();
        }

        private void MenuFileExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuViewRestoreFull_Click(object sender, RoutedEventArgs e)
        {
            var restoreFullWindow = new RestoreFullWindow();
            this.Visibility = Visibility.Hidden;
            restoreFullWindow.Show();
        }

        private void SubmeterButton_Click(object sender, RoutedEventArgs e)
        {
            var atividadesString = this.AtividadesTextBox.Text;
            var mes = int.Parse(this.MesTb.Text);
            var ano = int.Parse(this.AnoTb.Text);

            try {
                DomLServices.ParseAtividadesDoMesEmTextoParaBanco(atividadesString, mes, ano);
                this.MessageLabel.Content = "Atividades passadas para o banco com sucesso";

                DomLServices.EscreverAtividadesDoMesEmArquivo(mes, ano);
                this.MessageLabel.Content = "Atividades do Mes escrito em arquivo com sucesso";

                DomLServices.EscreverAtividadesConsolidadasDoAnoEmArquivo(ano);
                this.MessageLabel.Content = "Atividades consolidadas do ano escritas em arquivo com sucesso";

                DomLServices.EscreveResumoDoAnoEmArquivo(ano);
                this.MessageLabel.Content = "Resumo do ano escrito em arquivo com sucesso";

                DomLServices.EscreverAtividadesConsolidadasOfAllTimeEmArquivo();
                this.MessageLabel.Content = "Todas Atividades consolidadas escritas em arquivo com sucesso";
            } catch (Exception exception) {
                this.MessageLabel2.Content = exception.Message;
                Console.Write(exception);
            }
        }
    }
}

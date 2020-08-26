using DomL.Business.Services;
using DomL.Presentation;
using System;
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

            this.MonthTb.Text = (DateTime.Now.Month - 1).ToString();
            this.YearTb.Text = DateTime.Now.Year.ToString();
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
            this.MessageLabel.Content = "";
            var atividadesString = this.AtividadesTextBox.Text;
            var month = int.Parse(this.MonthTb.Text);
            var year = int.Parse(this.YearTb.Text);

            try {
                DomLServices.SaveFromRawMonthText(atividadesString, month, year);
                this.MessageLabel.Content = "Atividades passadas para o banco com sucesso";

                DomLServices.WriteMonthRecapFile(month, year);
                this.MessageLabel.Content = "Atividades do Mes escrito em arquivo com sucesso";

                DomLServices.WriteYearRecapFiles(year);
                this.MessageLabel.Content = "Atividades consolidadas do ano escritas em arquivo com sucesso";

                DomLServices.WriteRecapFiles();
                this.MessageLabel.Content = "Todas Atividades consolidadas escritas em arquivo com sucesso";
            } catch (Exception exception) {
                this.MessageLabel2.Content = exception.Message;
                Console.Write(exception);
            }
        }
    }
}

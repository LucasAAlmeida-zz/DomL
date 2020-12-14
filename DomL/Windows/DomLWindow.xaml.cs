using DomL.Business.Services;
using DomL.Presentation;
using System;
using System.Windows;

namespace DomL
{
    /// <summary>
    /// Interaction logic for DomLWindow.xaml
    /// </summary>
    public partial class DomLWindow
    {
        public DomLWindow()
        {
            InitializeComponent();

            MonthTb.Text = (DateTime.Now.Month - 1).ToString();
            YearTb.Text = DateTime.Now.Year.ToString();
        }

        private void MenuFileExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuViewBackupFull_Click(object sender, RoutedEventArgs e)
        {
            var backupFullWindow = new BackupWindow();
            Visibility = Visibility.Hidden;
            backupFullWindow.Show();
        }

        private void MenuViewRestoreFull_Click(object sender, RoutedEventArgs e)
        {
            var restoreFullWindow = new RestoreWindow();
            Visibility = Visibility.Hidden;
            restoreFullWindow.Show();
        }

        private void SubmeterButton_Click(object sender, RoutedEventArgs e)
        {
            MessageLabel.Content = "";
            var atividadesString = AtividadesTextBox.Text;
            var month = int.Parse(MonthTb.Text);
            var year = int.Parse(YearTb.Text);

            try {
                DomLServices.SaveFromRawMonthText(atividadesString, month, year);
                MessageLabel.Content = "Atividades passadas para o banco com sucesso";

                DomLServices.WriteMonthRecapFile(month, year);
                MessageLabel.Content = "Atividades do Mes escrito em arquivo com sucesso";

                DomLServices.WriteYearRecapFiles(year);
                MessageLabel.Content = "Atividades consolidadas do ano escritas em arquivo com sucesso";

                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                MessageLabel2.Content = exception.Message;
                Console.Write(exception);
            }
        }
    }
}

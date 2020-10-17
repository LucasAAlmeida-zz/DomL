
using DomL.Business.Entities;
using DomL.Business.Services;
using System;
using System.Windows;

namespace DomL.Presentation
{
    /// <summary>
    /// Interaction logic for RestoreFullWindow.xaml
    /// </summary>
    public partial class BackupWindow : Window
    {
        const string BACKUP_DIR_PATH = "D:\\OneDrive\\Área de Trabalho\\DomL\\Backup\\";

        public BackupWindow()
        {
            InitializeComponent();
        }

        private void AutoRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                AutoService.RestoreFromFile(BACKUP_DIR_PATH);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                this.MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void BookRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                BookService.RestoreFromFile(BACKUP_DIR_PATH);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                this.MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void ComicRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                ComicService.RestoreFromFile(BACKUP_DIR_PATH);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                this.MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void DoomRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                DoomService.RestoreFromFile(BACKUP_DIR_PATH);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                this.MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void EventRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                EventService.RestoreFromFile(BACKUP_DIR_PATH);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                this.MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void GameRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                GameService.RestoreFromFile(BACKUP_DIR_PATH);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                this.MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void GiftRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                GiftService.RestoreFromFile(BACKUP_DIR_PATH);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                this.MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void HealthRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                HealthService.RestoreFromFile(BACKUP_DIR_PATH);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                this.MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void MeetRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                MeetService.RestoreFromFile(BACKUP_DIR_PATH);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                this.MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void MovieRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                MovieService.RestoreFromFile(BACKUP_DIR_PATH);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                this.MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }
        
        private void PetRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                PetService.RestoreFromFile(BACKUP_DIR_PATH);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                this.MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void PlayRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                PlayService.RestoreFromFile(BACKUP_DIR_PATH);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                this.MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void PurchaseRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                PurchaseService.RestoreFromFile(BACKUP_DIR_PATH);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                this.MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void ShowRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                ShowService.RestoreFromFile(BACKUP_DIR_PATH);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                this.MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void TravelRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                TravelService.RestoreFromFile(BACKUP_DIR_PATH);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                this.MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void WorkRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                WorkService.RestoreFromFile(BACKUP_DIR_PATH);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                this.MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }
    }
}

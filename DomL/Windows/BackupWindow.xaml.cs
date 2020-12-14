
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


        private void AutoBackupButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                DomLServices.BackupToFile(BACKUP_DIR_PATH, Category.AUTO_ID);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void BookBackupButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                DomLServices.BackupToFile(BACKUP_DIR_PATH, Category.BOOK_ID);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void ComicBackupButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                DomLServices.BackupToFile(BACKUP_DIR_PATH, Category.COMIC_ID);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void DoomBackupButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                DomLServices.BackupToFile(BACKUP_DIR_PATH, Category.DOOM_ID);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void EventBackupButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                DomLServices.BackupToFile(BACKUP_DIR_PATH, Category.EVENT_ID);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void GameBackupButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                DomLServices.BackupToFile(BACKUP_DIR_PATH, Category.GAME_ID);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void GiftBackupButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                DomLServices.BackupToFile(BACKUP_DIR_PATH, Category.GIFT_ID);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void HealthBackupButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                DomLServices.BackupToFile(BACKUP_DIR_PATH, Category.HEALTH_ID);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void MeetBackupButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                DomLServices.BackupToFile(BACKUP_DIR_PATH, Category.MEET_ID);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void MovieBackupButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                DomLServices.BackupToFile(BACKUP_DIR_PATH, Category.MOVIE_ID);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void PetBackupButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                DomLServices.BackupToFile(BACKUP_DIR_PATH, Category.PET_ID);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void PlayBackupButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                DomLServices.BackupToFile(BACKUP_DIR_PATH, Category.PLAY_ID);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void PurchaseBackupButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                DomLServices.BackupToFile(BACKUP_DIR_PATH, Category.PURCHASE_ID);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void ShowBackupButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                DomLServices.BackupToFile(BACKUP_DIR_PATH, Category.SHOW_ID);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void TravelBackupButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                DomLServices.BackupToFile(BACKUP_DIR_PATH, Category.TRAVEL_ID);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void WorkBackupButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                DomLServices.BackupToFile(BACKUP_DIR_PATH, Category.WORK_ID);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }
    }
}

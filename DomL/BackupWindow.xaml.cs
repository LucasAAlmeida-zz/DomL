
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
                ActivityService.BackupToFile(BACKUP_DIR_PATH, ActivityCategory.AUTO_ID);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                this.MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void BookBackupButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                ActivityService.BackupToFile(BACKUP_DIR_PATH, ActivityCategory.BOOK_ID);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                this.MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void ComicBackupButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                ActivityService.BackupToFile(BACKUP_DIR_PATH, ActivityCategory.COMIC_ID);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                this.MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void DoomBackupButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                ActivityService.BackupToFile(BACKUP_DIR_PATH, ActivityCategory.DOOM_ID);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                this.MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void EventBackupButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                ActivityService.BackupToFile(BACKUP_DIR_PATH, ActivityCategory.EVENT_ID);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                this.MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void GameBackupButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                ActivityService.BackupToFile(BACKUP_DIR_PATH, ActivityCategory.GAME_ID);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                this.MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void GiftBackupButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                ActivityService.BackupToFile(BACKUP_DIR_PATH, ActivityCategory.GIFT_ID);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                this.MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void HealthBackupButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                ActivityService.BackupToFile(BACKUP_DIR_PATH, ActivityCategory.HEALTH_ID);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                this.MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void MeetBackupButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                ActivityService.BackupToFile(BACKUP_DIR_PATH, ActivityCategory.MEET_ID);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                this.MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void MovieBackupButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                ActivityService.BackupToFile(BACKUP_DIR_PATH, ActivityCategory.MOVIE_ID);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                this.MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void PetBackupButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                ActivityService.BackupToFile(BACKUP_DIR_PATH, ActivityCategory.PET_ID);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                this.MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void PlayBackupButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                ActivityService.BackupToFile(BACKUP_DIR_PATH, ActivityCategory.PLAY_ID);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                this.MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void PurchaseBackupButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                ActivityService.BackupToFile(BACKUP_DIR_PATH, ActivityCategory.PURCHASE_ID);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                this.MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void ShowBackupButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                ActivityService.BackupToFile(BACKUP_DIR_PATH, ActivityCategory.SHOW_ID);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                this.MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void TravelBackupButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                ActivityService.BackupToFile(BACKUP_DIR_PATH, ActivityCategory.TRAVEL_ID);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                this.MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void WorkBackupButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                ActivityService.BackupToFile(BACKUP_DIR_PATH, ActivityCategory.WORK_ID);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                this.MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        // ------------------------------
        // -----------RESTORE------------
        // ------------------------------

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

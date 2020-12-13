
using DomL.Business.Entities;
using DomL.Business.Services;
using System;
using System.Windows;

namespace DomL.Presentation
{
    /// <summary>
    /// Interaction logic for RestoreFullWindow.xaml
    /// </summary>
    public partial class RestoreWindow : Window
    {
        const string BACKUP_DIR_PATH = "D:\\OneDrive\\Área de Trabalho\\DomL\\Backup\\";

        public RestoreWindow()
        {
            InitializeComponent();
        }

        private void AutoRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                DomLServices.RestoreFromFile(BACKUP_DIR_PATH, ActivityCategory.AUTO_ID);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                this.MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void BookRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                DomLServices.RestoreFromFile(BACKUP_DIR_PATH, ActivityCategory.BOOK_ID);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                this.MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void ComicRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            DomLServices.RestoreFromFile(BACKUP_DIR_PATH, ActivityCategory.COMIC_ID);
            MessageBox.Show("Funcionou!");
        }

        private void CourseRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            DomLServices.RestoreFromFile(BACKUP_DIR_PATH, ActivityCategory.COURSE_ID);
            MessageBox.Show("Funcionou!");
        }

        private void DoomRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                DomLServices.RestoreFromFile(BACKUP_DIR_PATH, ActivityCategory.DOOM_ID);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                this.MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void EventRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            DomLServices.RestoreFromFile(BACKUP_DIR_PATH, ActivityCategory.EVENT_ID);
            MessageBox.Show("Funcionou!");
        }

        private void GameRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            DomLServices.RestoreFromFile(BACKUP_DIR_PATH, ActivityCategory.GAME_ID);
            MessageBox.Show("Funcionou!");
        }

        private void GiftRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                DomLServices.RestoreFromFile(BACKUP_DIR_PATH, ActivityCategory.GIFT_ID);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                this.MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void HealthRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                DomLServices.RestoreFromFile(BACKUP_DIR_PATH, ActivityCategory.HEALTH_ID);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                this.MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void MeetRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                DomLServices.RestoreFromFile(BACKUP_DIR_PATH, ActivityCategory.MEET_ID);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                this.MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void MovieRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            DomLServices.RestoreFromFile(BACKUP_DIR_PATH, ActivityCategory.MOVIE_ID);
            MessageBox.Show("Funcionou!");
        }
        
        private void PetRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                DomLServices.RestoreFromFile(BACKUP_DIR_PATH, ActivityCategory.PET_ID);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                this.MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void PlayRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                DomLServices.RestoreFromFile(BACKUP_DIR_PATH, ActivityCategory.PLAY_ID);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                this.MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void PurchaseRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                DomLServices.RestoreFromFile(BACKUP_DIR_PATH, ActivityCategory.PURCHASE_ID);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                this.MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void ShowRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                DomLServices.RestoreFromFile(BACKUP_DIR_PATH, ActivityCategory.SHOW_ID);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                this.MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void TravelRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                DomLServices.RestoreFromFile(BACKUP_DIR_PATH, ActivityCategory.TRAVEL_ID);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                this.MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void WorkRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                DomLServices.RestoreFromFile(BACKUP_DIR_PATH, ActivityCategory.WORK_ID);
                MessageBox.Show("Funcionou!");
            } catch (Exception exception) {
                this.MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }
    }
}

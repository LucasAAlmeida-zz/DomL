using DomL.Business.Entities;
using DomL.Business.Services;
using DomL.Business.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DomL.Presentation
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private readonly UnitOfWork UnitOfWork;

        public GameWindow(string[] segments, Activity activity, UnitOfWork unitOfWork)
        {
            InitializeComponent();

            this.UnitOfWork = unitOfWork;

            this.InfoMessage.Content =
                "Date:\t\t" + activity.Date.ToString("dd/MM/yyyy") + "\n" +
                "Category:\t" + activity.Category.Name + "\n" +
                "Status:\t\t" + activity.Status.Name;

            for (int index = 1; index < segments.Length; index++) {
                var segmento = segments[index];
                var dynLabel = new TextBox {
                    Text = segmento,
                    IsReadOnly = true,
                    Margin = new Thickness(5)
                };

                this.SegmentosStack.Children.Add(dynLabel);
            }

            var platformNames = new List<string>(segments);
            platformNames.AddRange(MediaTypeService.GetAll(unitOfWork).Select(u => u.Name).ToList());

            var seriesNames = new List<string>(segments);
            seriesNames.AddRange(SeriesService.GetAll(unitOfWork).Select(u => u.Name).ToList());

            var directorNames = new List<string>(segments);
            directorNames.AddRange(PersonService.GetAll(unitOfWork).Select(u => u.Name).ToList());

            var publisherNames = new List<string>(segments);
            publisherNames.AddRange(CompanyService.GetAll(unitOfWork).Select(u => u.Name).ToList());

            this.TitleCB.ItemsSource = segments;
            this.PlatformCB.ItemsSource = platformNames;
            this.SeriesCB.ItemsSource = seriesNames;
            this.NumberCB.ItemsSource = segments;
            this.DirectorCB.ItemsSource = directorNames;
            this.PublisherCB.ItemsSource = publisherNames;
            this.ScoreCB.ItemsSource = segments;
            this.DescriptionCB.ItemsSource = segments;

            this.TitleCB.SelectedItem = segments[1];
            this.PlatformCB.SelectedItem = segments[2];
            this.SeriesCB.SelectedItem = segments.Length > 3 ? segments[3] : null;
            this.NumberCB.SelectedItem = segments.Length > 4 ? segments[4] : null;
            this.DirectorCB.SelectedItem = segments.Length > 5 ? segments[5] : null;
            this.PublisherCB.SelectedItem = segments.Length > 6 ? segments[6] : null;
            this.ScoreCB.SelectedItem = segments.Length > 7 ? segments[7] : null;
            this.DescriptionCB.SelectedItem = segments.Length > 8 ? segments[8] : null;

            this.TitleCB_LostFocus(null, null);
            this.PlatformCB_LostFocus(null, null);
            this.SeriesCB_LostFocus(null, null);
            this.DirectorCB_LostFocus(null, null);
            this.PublisherCB_LostFocus(null, null);
        }

        private void BtnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void TitleCB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.TitleCB.IsKeyboardFocusWithin) {
                return;
            }

            var title = this.TitleCB.Text;
            var game = GameService.GetGameByTitle(title, this.UnitOfWork);
            Util.ChangeInfoLabel(title, game, this.TitleInfoLb);

            UpdateOptionalComboBoxes(title, this.PlatformCB.Text);
        }

        private void PlatformCB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.PlatformCB.IsKeyboardFocusWithin) {
                return;
            }

            var platformName = this.PlatformCB.Text;
            var platform = MediaTypeService.GetByName(platformName, this.UnitOfWork);
            Util.ChangeInfoLabel(platformName, platform, this.PlatformInfoLb);

            UpdateOptionalComboBoxes(this.TitleCB.Text, platformName);
        }

        private void SeriesCB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.SeriesCB.IsKeyboardFocusWithin) {
                return;
            }

            var seriesName = this.SeriesCB.Text;
            var series = SeriesService.GetByName(seriesName, this.UnitOfWork);
            Util.ChangeInfoLabel(seriesName, series, this.SeriesInfoLb);
        }

        private void DirectorCB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.DirectorCB.IsKeyboardFocusWithin) {
                return;
            }

            var directorName = this.DirectorCB.Text;
            var director = PersonService.GetByName(directorName, this.UnitOfWork);
            Util.ChangeInfoLabel(directorName, director, this.DirectorInfoLb);
        }

        private void PublisherCB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.PublisherCB.IsKeyboardFocusWithin) {
                return;
            }

            var publisherName = this.PublisherCB.Text;
            var publisher = CompanyService.GetByName(publisherName, this.UnitOfWork);
            Util.ChangeInfoLabel(publisherName, publisher, this.PublisherInfoLb);
        }

        private void UpdateOptionalComboBoxes(string title, string platformName)
        {
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(platformName)) {
                return;
            }

            var game = GameService.GetGameByTitleAndPlatformName(title, platformName, this.UnitOfWork);

            if (game == null) {
                return;
            }

            if (game.Series != null) {
                this.SeriesCB.Text = game.Series.Name;
                this.SeriesCB_LostFocus(null, null);
            }

            if (game.Director != null) {
                this.DirectorCB.Text = game.Director.Name;
                this.DirectorCB_LostFocus(null, null);
            }

            if (game.Publisher != null) {
                this.PublisherCB.Text = game.Publisher.Name;
                this.PublisherCB_LostFocus(null, null);
            }

            this.NumberCB.Text = game.NumberInSeries ?? this.NumberCB.Text;
            this.ScoreCB.Text = game.Score ?? this.ScoreCB.Text;
        }

    }
}

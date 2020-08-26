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
    /// Interaction logic for ShowWindow.xaml
    /// </summary>
    public partial class ShowWindow : Window
    {
        private readonly UnitOfWork UnitOfWork;

        public ShowWindow(string[] segments, Activity activity, UnitOfWork unitOfWork)
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

            var directorNames = new List<string>(segments);
            directorNames.AddRange(PersonService.GetAll(unitOfWork).Select(u => u.Name).ToList());

            var seriesNames = new List<string>(segments);
            seriesNames.AddRange(SeriesService.GetAll(unitOfWork).Select(u => u.Name).ToList());

            var mediaTypeNames = new List<string>(segments);
            mediaTypeNames.AddRange(MediaTypeService.GetAll(unitOfWork).Select(u => u.Name).ToList());

            this.SeriesCB.ItemsSource = seriesNames;
            this.SeasonCB.ItemsSource = segments;
            this.DirectorCB.ItemsSource = directorNames;
            this.TypeCB.ItemsSource = mediaTypeNames;
            this.ScoreCB.ItemsSource = segments;
            this.DescriptionCB.ItemsSource = segments;

            // SHOW; Series Name; Season; (Media Type Name); (Director Name); (Score); (Description)
            this.SeriesCB.SelectedItem = segments[1];
            this.SeasonCB.SelectedItem = segments[2];
            this.TypeCB.SelectedItem = segments.Length > 3 ? segments[3] : null;
            this.DirectorCB.SelectedItem = segments.Length > 4 ? segments[4] : null;
            this.ScoreCB.SelectedItem = segments.Length > 5 ? segments[5] : null;
            this.DescriptionCB.SelectedItem = segments.Length > 6 ? segments[6] : null;

            this.SeriesCB_LostFocus(null, null);
            this.DirectorCB_LostFocus(null, null);
            this.TypeCB_LostFocus(null, null);
        }

        private void BtnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void SeriesCB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.SeriesCB.IsKeyboardFocusWithin) {
                return;
            }

            var seriesName = this.SeriesCB.Text;
            var series = SeriesService.GetByName(seriesName, this.UnitOfWork);
            Util.ChangeInfoLabel(seriesName, series, this.SeriesInfoLb);

            UpdateOptionalComboBoxes(seriesName, this.SeasonCB.Text);
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

        private void TypeCB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.TypeCB.IsKeyboardFocusWithin) {
                return;
            }

            var typeName = this.TypeCB.Text;
            var mediaType = MediaTypeService.GetByName(typeName, this.UnitOfWork);
            Util.ChangeInfoLabel(typeName, mediaType, this.TypeInfoLb);
        }

        private void UpdateOptionalComboBoxes(string seriesName, string season)
        {
            if (string.IsNullOrWhiteSpace(seriesName) || string.IsNullOrWhiteSpace(season)) {
                return;
            }

            var showSeason = ShowService.GetShowSeasonBySeriesNameAndSeason(seriesName, season, this.UnitOfWork);

            if (showSeason == null) {
                return;
            }

            if (showSeason.Director != null) {
                this.DirectorCB.Text = showSeason.Director.Name;
                this.DirectorCB_LostFocus(null, null);
            }

            if (showSeason.Type != null) {
                this.TypeCB.Text = showSeason.Type.Name;
                this.TypeCB_LostFocus(null, null);
            }

            this.ScoreCB.Text = showSeason.Score ?? this.ScoreCB.Text;
        }
    }
}

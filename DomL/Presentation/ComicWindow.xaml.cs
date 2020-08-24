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
    /// Interaction logic for ComicWindow.xaml
    /// </summary>
    public partial class ComicWindow : Window
    {
        private readonly UnitOfWork UnitOfWork;

        public ComicWindow(string[] segments, Activity activity, UnitOfWork unitOfWork)
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

            var authorNames = new List<string>(segments);
            authorNames.AddRange(PersonService.GetAll(unitOfWork).Select(u => u.Name).ToList());

            var seriesNames = new List<string>(segments);
            seriesNames.AddRange(SeriesService.GetAll(unitOfWork).Select(u => u.Name).ToList());

            var mediaTypeNames = new List<string>(segments);
            mediaTypeNames.AddRange(MediaTypeService.GetAll(unitOfWork).Select(u => u.Name).ToList());

            this.SeriesCB.ItemsSource = seriesNames;
            this.ChaptersCB.ItemsSource = segments;
            this.AuthorCB.ItemsSource = authorNames;
            this.TypeCB.ItemsSource = mediaTypeNames;
            this.ScoreCB.ItemsSource = segments;
            this.DescriptionCB.ItemsSource = segments;

            this.SeriesCB.SelectedItem = segments[1];
            this.ChaptersCB.SelectedItem = segments[2];
            this.AuthorCB.SelectedItem = segments.Length > 3 ? segments[3] : null;
            this.TypeCB.SelectedItem = segments.Length > 4 ? segments[4] : null;
            this.ScoreCB.SelectedItem = segments.Length > 5 ? segments[5] : null;
            this.DescriptionCB.SelectedItem = segments.Length > 6 ? segments[6] : null;

            this.SeriesCB_LostFocus(null, null);
            this.AuthorCB_LostFocus(null, null);
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

            if (string.IsNullOrWhiteSpace(seriesName)) {
                this.SeriesInfoLb.Content = "";
                return;
            }

            var series = SeriesService.GetByName(seriesName, this.UnitOfWork);
            Util.ChangeInfoLabel(series, this.SeriesInfoLb);
        }

        private void AuthorCB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.AuthorCB.IsKeyboardFocusWithin) {
                return;
            }

            var authorName = this.AuthorCB.Text;
            
            if (string.IsNullOrWhiteSpace(authorName)) {
                this.AuthorInfoLb.Content = "";
                return;
            }

            var author = PersonService.GetByName(authorName, this.UnitOfWork);
            Util.ChangeInfoLabel(author, this.AuthorInfoLb);
        }

        private void TypeCB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.TypeCB.IsKeyboardFocusWithin) {
                return;
            }

            var typeName = this.TypeCB.Text;

            if (string.IsNullOrWhiteSpace(typeName)) {
                this.TypeInfoLb.Content = "";
                return;
            }

            var mediaType = MediaTypeService.GetByName(typeName, this.UnitOfWork);
            Util.ChangeInfoLabel(mediaType, this.TypeInfoLb);
        }
    }
}

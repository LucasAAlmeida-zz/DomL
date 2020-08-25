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
    /// Interaction logic for BookWindow.xaml
    /// </summary>
    public partial class BookWindow : Window
    {
        private readonly UnitOfWork UnitOfWork;

        public BookWindow(string[] segments, Activity activity, UnitOfWork unitOfWork)
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

            this.TitleCB.ItemsSource = segments;
            this.AuthorCB.ItemsSource = authorNames;
            this.SeriesCB.ItemsSource = seriesNames;
            this.NumberCB.ItemsSource = segments;
            this.ScoreCB.ItemsSource = segments;
            this.DescriptionCB.ItemsSource = segments;

            this.TitleCB.SelectedItem = segments[1];
            this.AuthorCB.SelectedItem = segments.Length > 2 ? segments[2] : null;
            this.SeriesCB.SelectedItem = segments.Length > 3 ? segments[3] : null;
            this.NumberCB.SelectedItem = segments.Length > 4 ? segments[4] : null;
            this.ScoreCB.SelectedItem = segments.Length > 5 ? segments[5] : null;
            this.DescriptionCB.SelectedItem = segments.Length > 6 ? segments[6] : null;

            this.TitleCB_LostFocus(null, null);
            this.AuthorCB_LostFocus(null, null);
            this.SeriesCB_LostFocus(null, null);
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
            var book = BookService.GetByTitle(title, this.UnitOfWork);
            Util.ChangeInfoLabel(title, book, this.TitleInfoLb);

            UpdateOptionalComboBoxes(book);
        }

        private void AuthorCB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.AuthorCB.IsKeyboardFocusWithin) {
                return;
            }

            var authorName = this.AuthorCB.Text;
            var author = PersonService.GetByName(authorName, this.UnitOfWork);
            Util.ChangeInfoLabel(authorName, author, this.AuthorInfoLb);
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

        private void UpdateOptionalComboBoxes(Book book)
        {
            if (book == null) {
                return;
            }

            if (book.Author != null) {
                this.AuthorCB.Text = book.Author.Name;
                this.AuthorCB_LostFocus(null, null);
            }

            if (book.Series != null) {
                this.SeriesCB.Text = book.Series.Name;
                this.SeriesCB_LostFocus(null, null);
            }

            this.NumberCB.Text = book.NumberInSeries ?? this.NumberCB.Text;
            this.ScoreCB.Text = book.Score ?? this.ScoreCB.Text;
        }
    }
}

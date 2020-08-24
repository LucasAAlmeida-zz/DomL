using DomL.Business.Entities;
using DomL.Business.Services;
using DomL.Business.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DomL.Presentation
{
    /// <summary>
    /// Interaction logic for BookWindow.xaml
    /// </summary>
    public partial class BookWindow : Window
    {
        private readonly UnitOfWork UnitOfWork;

        public BookWindow(string[] segmentos, Activity activity, UnitOfWork unitOfWork)
        {
            InitializeComponent();
            DataContext = this;

            this.UnitOfWork = unitOfWork;

            this.InfoMessage.Content =
                "Date:\t\t" + activity.Date.ToString("dd/MM/yyyy") + "\n" +
                "Category:\t" + activity.Category.Name + "\n" +
                "Status:\t\t" + activity.Status.Name;

            for (int index = 1; index < segmentos.Length; index++) {
                var segmento = segmentos[index];
                var dynLabel = new TextBox {
                    Text = segmento,
                    IsReadOnly = true,
                    Margin = new Thickness(5)
                };

                this.SegmentosStack.Children.Add(dynLabel);
            }

            var authorNames = new List<string>(segmentos);
            authorNames.AddRange(PersonService.GetAll(unitOfWork).Select(u => u.Name).ToList());

            var seriesNames = new List<string>(segmentos);
            seriesNames.AddRange(SeriesService.GetAll(unitOfWork).Select(u => u.Name).ToList());

            this.TitleCB.ItemsSource = segmentos;
            this.AuthorCB.ItemsSource = authorNames;
            this.SeriesCB.ItemsSource = seriesNames;
            this.NumberCB.ItemsSource = segmentos;
            this.ScoreCB.ItemsSource = segmentos;
            this.DescriptionCB.ItemsSource = segmentos;

            this.TitleCB.SelectedItem = segmentos[1];
            this.AuthorCB.SelectedItem = segmentos.Length > 2 ? segmentos[2] : null;
            this.SeriesCB.SelectedItem = segmentos.Length > 3 ? segmentos[3] : null;
            this.NumberCB.SelectedItem = segmentos.Length > 4 ? segmentos[4] : null;
            this.ScoreCB.SelectedItem = segmentos.Length > 5 ? segmentos[5] : null;
            this.DescriptionCB.SelectedItem = segmentos.Length > 6 ? segmentos[6] : null;

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

            if (book == null) {
                Util.ChangeToNew(this.TitleInfoLb);
                return;
            }
            Util.ChangeToExists(this.TitleInfoLb);

            if (book.Author != null) {
                this.AuthorCB.Text = book.Author.Name;
                this.AuthorCB_LostFocus(null, null);
            }

            if (book.Series != null) {
                this.SeriesCB.Text = book.Series.Name;
                this.SeriesCB_LostFocus(null, null);
            }

            this.NumberCB.Text = book.NumberInSeries ?? this.NumberCB.Text;
        }

        private void AuthorCB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.TitleCB.IsKeyboardFocusWithin) {
                return;
            }

            var authorName = this.AuthorCB.Text;
            var author = PersonService.GetByName(authorName, this.UnitOfWork);

            if (author == null) {
                Util.ChangeToNew(this.AuthorInfoLb);
                return;
            }
            Util.ChangeToExists(this.AuthorInfoLb);
        }

        private void SeriesCB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.TitleCB.IsKeyboardFocusWithin) {
                return;
            }

            var seriesName = this.SeriesCB.Text;
            var series = SeriesService.GetByName(seriesName, this.UnitOfWork);

            if (series == null) {
                Util.ChangeToNew(this.SeriesInfoLb);
                return;
            }
            Util.ChangeToExists(this.SeriesInfoLb);
        }
    }
}

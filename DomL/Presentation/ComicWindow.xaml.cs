using System.Windows;
using System.Windows.Controls;

namespace DomL.Presentation
{
    /// <summary>
    /// Interaction logic for ComicWindow.xaml
    /// </summary>
    public partial class ComicWindow : Window
    {
        public ComicWindow(string[] segmentos)
        {
            InitializeComponent();

            for(int index = 1; index < segmentos.Length; index++) {
                var segmento = segmentos[index];
                var dynLabel = new TextBox {
                    Text = segmento,
                    IsReadOnly = true
                };

                this.SegmentosStack.Children.Add(dynLabel);
            }

            this.SeriesCB.ItemsSource = segmentos;
            this.ChaptersCB.ItemsSource = segmentos;
            this.AuthorCB.ItemsSource = segmentos;
            this.TypeCB.ItemsSource = segmentos;
            this.ScoreCB.ItemsSource = segmentos;
            this.DescriptionCB.ItemsSource = segmentos;

            this.SeriesCB.SelectedItem = segmentos[1];
            this.ChaptersCB.SelectedItem = segmentos[2];

            this.AuthorCB.SelectedItem = segmentos.Length > 3 ? segmentos[3] : null;
            this.TypeCB.SelectedItem = segmentos.Length > 4 ? segmentos[4] : null;
            this.ScoreCB.SelectedItem = segmentos.Length > 5 ? segmentos[5] : null;
            this.DescriptionCB.SelectedItem = segmentos.Length > 6 ? segmentos[6] : null;
        }

        private void BtnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}

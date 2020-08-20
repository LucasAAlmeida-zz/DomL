using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DomL.Presentation
{
    /// <summary>
    /// Interaction logic for BookWindow.xaml
    /// </summary>
    public partial class BookWindow : Window
    {
        public BookWindow(string[] segmentos)
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

            this.TitleCB.ItemsSource = segmentos;
            this.AuthorCB.ItemsSource = segmentos;
            this.SeriesCB.ItemsSource = segmentos;
            this.NumberCB.ItemsSource = segmentos;
            this.ScoreCB.ItemsSource = segmentos;
            this.DescriptionCB.ItemsSource = segmentos;

            this.TitleCB.SelectedItem = segmentos[1];
            this.AuthorCB.SelectedItem = segmentos[2];

            this.SeriesCB.SelectedItem = segmentos.Length > 3 ? segmentos[3] : null;
            this.NumberCB.SelectedItem = segmentos.Length > 4 ? segmentos[4] : null;
            this.ScoreCB.SelectedItem = segmentos.Length > 5 ? segmentos[5] : null;
            this.DescriptionCB.SelectedItem = segmentos.Length > 6 ? segmentos[6] : null;
        }

        private void BtnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}

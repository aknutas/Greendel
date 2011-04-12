using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Collections.ObjectModel;

namespace CloverMobile
{
    public partial class History : PhoneApplicationPage
    {
        public History()
        {

            InitializeComponent();
        }

        private ObservableCollection<PData> _data = new ObservableCollection<PData>()
        {
            new PData() { title = "slice #1", value = 30 },
            new PData() { title = "slice #2", value = 60 },
            new PData() { title = "slice #3", value = 40 },
        };

        public ObservableCollection<PData> Data { get { return _data; } }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = this;
        }

        private void ShowSerialChartButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
    }

    public class PData
    {
        public string title { get; set; }
        public double value { get; set; }
    }
}
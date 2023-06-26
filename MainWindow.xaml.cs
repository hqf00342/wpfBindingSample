using System.Windows;
using wpfRectangleBindingTest.Models;

namespace wpfRectangleBindingTest
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //デモ用に4つのRectangleを設置
            if (this.DataContext is ViewModel vm)
            {
                vm.RectInfoCollection.Add(new RectInfo("1", 10, 10));
                vm.RectInfoCollection.Add(new RectInfo("2", 200, 10));
                vm.RectInfoCollection.Add(new RectInfo("3", 10, 200));
                vm.RectInfoCollection.Add(new RectInfo("4", 200, 200));
            }
        }
    }
}
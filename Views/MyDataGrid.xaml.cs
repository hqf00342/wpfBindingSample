using System.Windows;
using System.Windows.Controls;
using wpfRectangleBindingTest.Models;

namespace wpfRectangleBindingTest
{
    /// <summary>
    /// MyDataGrid.xaml の相互作用ロジック
    /// </summary>
    public partial class MyDataGrid : UserControl
    {
        public MyDataGrid()
        {
            InitializeComponent();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            //削除ボタンクリック
            //本来はコマンド化すべきだが面倒なのでここで削除
            if (this.DataContext is ViewModel vm
                && datagrid.SelectedItem is RectInfo target)
            {
                vm.RectInfoCollection.Remove(target);
            }
        }

        private void datagrid_CurrentCellChanged(object sender, System.EventArgs e)
        {
            //セル編集したら即時反映
            //通常は別行へフォーカスが移らないと反映されない
            datagrid.CommitEdit();
        }
    }
}
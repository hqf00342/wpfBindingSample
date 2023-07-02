using System.Windows;

namespace wpfBindingSample
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Datagrid_CurrentCellChanged(object sender, System.EventArgs e)
        {
            //セル編集したら即時反映
            //通常は別行へフォーカスが移らないと反映されない
            datagrid.CommitEdit();
        }
    }
}
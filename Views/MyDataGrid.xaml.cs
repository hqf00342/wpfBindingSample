using System.Windows.Controls;

namespace wpfBindingSample;

/// <summary>
/// MyDataGrid.xaml の相互作用ロジック
/// </summary>
public partial class MyDataGrid : UserControl
{
    public MyDataGrid()
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
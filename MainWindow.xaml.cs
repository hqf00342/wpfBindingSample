using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;

namespace wpfBindingSample
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// DataGridのセル編集完了イベント
        /// デフォルトではセル編集だけではデータ反映されず、フォーカス移動が必要なため
        /// セル編集後すぐにデータ反映されるようにする。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Datagrid_CurrentCellChanged(object sender, System.EventArgs e)
        {
            datagrid.CommitEdit();
        }

        #region ListBox用イベント

        //ドラッグ開始位置
        //ドラッグしているUIElementのどこをつかんだかを記憶
        private Point _dragOffset;

        //ドラッグ中のRectangle
        // MouseDown(DargStart)で設定、MouseUp(DragEnd)でnullになる。
        private Rectangle? _dragRectangle;

        /// <summary>
        /// ListBox内Rectangleのマウスイベント：Down
        /// </summary>
        private void Rect_mouseDown(object sender, MouseButtonEventArgs e)
        {
            _dragRectangle = sender as Rectangle;
            if (_dragRectangle != null)
            {
                //Rectangleをキャプチャしドラッグ開始
                _dragRectangle.CaptureMouse();

                //Rectangle内のどこをクリックされたか記憶
                _dragOffset = e.GetPosition(_dragRectangle);
                this.Cursor = Cursors.Hand;
            }
        }

        /// <summary>
        /// ListBox内Rectangleのマウスイベント：Move
        /// </summary>
        private void Rect_mouseMove(object sender, MouseEventArgs e)
        {
            if (_dragRectangle != null && listbox.SelectedItem is RectInfo rinfo)
            {
                //RectinfoのX/Yプロパティを変更。
                var pos = e.GetPosition(listbox);
                rinfo.X = (int)(pos.X - _dragOffset.X);
                rinfo.Y = (int)(pos.Y - _dragOffset.Y);
            }
        }

        /// <summary>
        /// ListBox内Rectangleのマウスイベント：Up
        /// </summary>
        private void Rect_mouseUp(object sender, MouseButtonEventArgs e)
        {
            //Dragを開放
            _dragRectangle = null;
            Mouse.Capture(null);
            this.Cursor = Cursors.Arrow;
        }

        #endregion ListBox用イベント
    }
}
using System.Collections.ObjectModel;
using wpfRectangleBindingTest.Models;
using wpfRectangleBindingTest.MvvmUtils;

namespace wpfRectangleBindingTest;

public class ViewModel : BindableBase
{
    /// <summary>
    /// Rectangleの位置情報コレクション
    /// </summary>
    public ObservableCollection<RectInfo> RectInfoCollection { get; set; } = new();

    /// <summary>
    /// 選択中のアイテム
    /// Mainwindow.xamlでMyCanvasにBindingされている
    /// </summary>
    private RectInfo? _SelectedItem;
    public RectInfo? SelectedItem { get => _SelectedItem; set => SetProperty(ref _SelectedItem, value); }
}
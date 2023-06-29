using MvvmUtils;
using System.Collections.ObjectModel;
using wpfRectangleBindingTest.Models;

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

    /// <summary>アイテム削除コマンド</summary>
    public DelegateCommand DeleteItemCommand { get; }

    public ViewModel()
    {
        DeleteItemCommand = new DelegateCommand(DeleteItem);
    }

    /// <summary>
    /// アイテム削除コマンド本体
    /// </summary>
    /// <param name="obj">削除したいRectInfo</param>
    private void DeleteItem(object? obj)
    {
        if (obj is RectInfo item)
        {
            RectInfoCollection.Remove(item);
        }
    }
}
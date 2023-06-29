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
    /// </summary>
    private RectInfo? _SelectedItem;

    public RectInfo? SelectedItem { get => _SelectedItem; set => SetProperty(ref _SelectedItem, value); }

    /// <summary>アイテム削除コマンド</summary>
    private DelegateCommand? _delCmd;
    public DelegateCommand DeleteItemCommand => _delCmd ??= new (o => { if (o is RectInfo item) RectInfoCollection.Remove(item); });
}
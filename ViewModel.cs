using MvvmUtils;
using System.Collections.ObjectModel;

namespace wpfBindingSample;

public class ViewModel : BindableBase
{
    /// <summary>
    /// Rectangleコレクション。
    /// 本来はINotifyPropertyChanged実装すべきだがこのサンプルでは変更されないので
    /// getterのみの通常プロパティとしている</summary>
    public ObservableCollection<RectInfo> RectInfoCollection { get; } = new();

    /// <summary>アイテム削除コマンド</summary>
    private DelegateCommand? _delCmd;

    public DelegateCommand DeleteItemCommand => _delCmd ??= new(o => { if (o is RectInfo item) RectInfoCollection.Remove(item); });

    public ViewModel()
    {
        //デモ用に4つのRectangleを設置
        RectInfoCollection = new()
        {
            new RectInfo("1", 10, 10),
            new RectInfo("2", 200, 10),
            new RectInfo("3", 10, 200),
            new RectInfo("4", 200, 200)
        };
    }
}
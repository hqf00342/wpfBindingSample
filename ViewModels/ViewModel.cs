using MvvmUtils;
using System.Collections.ObjectModel;
using wpfBindingSample.Models;

namespace wpfBindingSample;

public class ViewModel : BindableBase
{
    /// <summary>
    /// Rectangleの位置情報コレクション
    /// </summary>
    //public ObservableCollection<RectInfo> RectInfoCollection { get; set; } = new();

    private ObservableCollection<RectInfo> _rectInfoCollection = null!;
    public ObservableCollection<RectInfo> RectInfoCollection { get => _rectInfoCollection; set => SetProperty(ref _rectInfoCollection, value); }

    /// <summary>アイテム削除コマンド</summary>
    private DelegateCommand? _delCmd;

    public DelegateCommand DeleteItemCommand => _delCmd ??= new(o => { if (o is RectInfo item) RectInfoCollection.Remove(item); });

    public ViewModel()
    {
        //デモ用に4つのRectangleを設置
        RectInfoCollection = new();
        RectInfoCollection.Add(new RectInfo("1", 10, 10));
        RectInfoCollection.Add(new RectInfo("2", 200, 10));
        RectInfoCollection.Add(new RectInfo("3", 10, 200));
        RectInfoCollection.Add(new RectInfo("4", 200, 200));
    }
}
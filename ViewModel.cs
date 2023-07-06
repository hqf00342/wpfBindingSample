﻿using MvvmUtils;
using System.Collections.ObjectModel;

namespace wpfBindingSample;

/// <summary>
/// MainWindow用Viewモデル。
/// モデルのデータはシングルトンのMyDataを参照している。
/// </summary>
public class ViewModel : BindableBase
{
    /// <summary>
    /// Rectangleコレクション。
    /// </summary>
    private ObservableCollection<RectInfo> _rectInfoCollection = null!;
    public ObservableCollection<RectInfo> RectInfoCollection { get => _rectInfoCollection; set => SetProperty(ref _rectInfoCollection, value); }


    /// <summary>アイテム削除コマンド</summary>
    private DelegateCommand? _delCmd;

    public DelegateCommand DeleteItemCommand => _delCmd ??= new(o => { if (o is RectInfo item) MyData.DeleteItem(item); });

    public ViewModel()
    {
        RectInfoCollection = MyData.RectInfos;
    }
}
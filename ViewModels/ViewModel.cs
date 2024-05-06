using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using wpfBindingSample.Models;

namespace wpfBindingSample.ViewModels;

/// <summary>
/// MainWindow用ViewModel
/// </summary>
public partial class ViewModel : ObservableObject
{
    /// <summary>
    /// Rectangleコレクション
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<RectInfo> _rectInfoCollection = null!;

    /// <summary>
    /// アイテム削除コマンド
    /// </summary>
    [RelayCommand]
    private void DeleteItem(RectInfo rinfo) => MyData.DeleteItem(rinfo);

    public ViewModel()
    {
        RectInfoCollection = MyData.RectInfos;
    }
}
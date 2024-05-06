using CommunityToolkit.Mvvm.ComponentModel;

namespace wpfBindingSample.Models;

/// <summary>
/// Datagridに表示する情報
/// </summary>
public partial class RectInfo : ObservableObject
{
    //名前 : Name プロパティ
    [ObservableProperty]
    private string _name = string.Empty;

    //X座標 : X プロパティ
    [ObservableProperty]
    private int _x;

    //Y座標 : Y プロパティ
    [ObservableProperty]
    private int _y;

    public RectInfo(string name, int x, int y)
    {
        Name = name;
        X = x;
        Y = y;
    }
}
using MvvmUtils;

namespace wpfBindingSample.Models;

/// <summary>
/// Datagridに表示する情報
/// </summary>
public class RectInfo : BindableBase
{
    //名前
    private string _name = string.Empty;

    public string Name { get => _name; set => SetProperty(ref _name, value); }

    //X座標
    private int _x;

    public int X { get => _x; set => SetProperty(ref _x, value); }

    //Y座標
    private int _y;

    public int Y { get => _y; set => SetProperty(ref _y, value); }

    public RectInfo(string name, int x, int y)
    {
        Name = name;
        X = x;
        Y = y;
    }
}
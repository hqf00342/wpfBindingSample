using System.Collections.ObjectModel;

namespace wpfBindingSample;

/// <summary>
/// アプリ内のデータを管理するモデル
/// シングルトンのため、static classにしている
/// </summary>
public static class MyData
{
    /// <summary>RectInfoのコレクション</summary>
    public static ObservableCollection<RectInfo> RectInfos { get; set; } = new();

    static MyData()
    {
        RectInfos.Add(new RectInfo("1", 10, 10));
        RectInfos.Add(new RectInfo("2", 200, 10));
        RectInfos.Add(new RectInfo("3", 10, 200));
        RectInfos.Add(new RectInfo("4", 200, 200));
    }

    public static void DeleteItem(RectInfo r)
    {
        RectInfos.Remove(r);
    }
}

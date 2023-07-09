using System.Collections.ObjectModel;

namespace wpfBindingSample;

/// <summary>
/// アプリ内のデータを一元管理するモデルクラス。
/// シングルトン(アプリ内で唯一のデータ)のため、static classとしている。
/// そのためインスタンス生成（new)する必要はなく、直接アクセスする。
/// これらのデータはViewModelから参照され、最終的にGUIに反映される。
/// </summary>
public static class MyData
{
    /// <summary>
    /// RectInfoコレクション
    /// コレクションへの追加削除を伝えるためListではなくObservableCollectionを使う。
    /// このコレクション自体のget/setを伝えるためにはBindableBase派生プロパティにすべきだが
    /// これはstaticで最初の一度しか生成しないためただのプロパティとする。
    /// </summary>
    public static ObservableCollection<RectInfo> RectInfos { get; } = new();

    static MyData()
    {
        RectInfos.Add(new RectInfo("1", 10, 10));
        RectInfos.Add(new RectInfo("2", 200, 10));
        RectInfos.Add(new RectInfo("3", 10, 200));
        RectInfos.Add(new RectInfo("4", 200, 200));
    }

    /// <summary>
    /// アイテムを削除する
    /// コマンド用メソッド。
    /// </summary>
    /// <param name="r">削除したいアイテム</param>
    public static void DeleteItem(RectInfo r)
    {
        RectInfos.Remove(r);
    }
}
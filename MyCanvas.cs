﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace wpfBindingSample;

public class MyCanvas : Canvas
{
    //ドラッグ開始位置
    //ドラッグ対象Rectangle(UIElement)の掴んだ場所を記憶
    private Point _dragOffset;

    //ドラッグ中のRectangle
    // MouseDown(DargStart)で設定、MouseUp(DragEnd)でnullになる。
    private Rectangle? _dragRectangle;

    /// <summary>
    /// SelectedItem 依存関係プロパティ
    /// 選択中のアイテムを表す。
    /// VisualStudioでは「propdp」スニペットで生成
    /// </summary>
    public RectInfo SelectedItem
    {
        get => (RectInfo)GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    public static readonly DependencyProperty SelectedItemProperty =
        DependencyProperty.Register("SelectedItem", typeof(RectInfo), typeof(MyCanvas), new PropertyMetadata(null, SelectedItem_Changed));

    /// <summary>
    /// SelectedItem プロパティ変更時に呼ばれる。
    /// 変更前と変更後の値が分かるのでそこから色を変える
    /// </summary>
    private static void SelectedItem_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        //このメソッドはstaticなので
        //対象のMyCanvasインスタンスをdから探し
        //そのインスタンス内でアイテム変更を処理させる。
        if(d is MyCanvas mycanvas)
        {
            mycanvas.SelectedItemChanged(e.OldValue, e.NewValue);
        }
    }

    /// <summary>
    /// 選択中のアイテム変更処理。背景色を変更
    /// SelectedItemプロパティの変更時に呼び出される。
    /// </summary>
    /// <param name="oldValue">変更前のアイテム</param>
    /// <param name="newValue">変更後のアイテム</param>
    private void SelectedItemChanged(object oldValue, object newValue)
    {
        if (oldValue is RectInfo o)
        {
            //古い選択アイテムは青色に戻す。
            var target = FindRectangle(o);
            if (target != null)
            {
                target.Fill = Brushes.SteelBlue;
            }
        }

        if (newValue is RectInfo n)
        {
            //新しい選択アイテムは赤く塗る。
            var target = FindRectangle(n);
            if (target != null)
            {
                target.Fill = Brushes.Red;
            }
        }
    }

    /// <summary>
    /// Items依存関係プロパティ
    /// Canvas上にあるRectInfo一覧を保持するList。
    /// DataGridやListBoxのItemsSourceプロパティに相当する。
    /// Canvasにない機能なので自前実装する。
    /// このコレクションの変更通知を内部処理し、Canvas上のUIを追加削除、移動させる。
    /// </summary>
    public ObservableCollection<RectInfo> Items
    {
        get => (ObservableCollection<RectInfo>)GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    public static readonly DependencyProperty ItemsProperty =
        DependencyProperty.Register("Items", typeof(ObservableCollection<RectInfo>), typeof(MyCanvas), new PropertyMetadata(null, ItemsChangedCallback));

    /// <summary>
    /// Items依存関係プロパティの変更コールバック
    /// 変更されると呼ばれるのでObservableCollectionの変更通知を登録する。
    /// static関数なので、インスタンス側で登録する。
    /// </summary>
    private static void ItemsChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is MyCanvas mycanvas)
        {
            // Itemsプロパティに変更通知を登録
            mycanvas.Items.CollectionChanged += mycanvas.Items_CollectionChanged;

            //Itemsに初期登録されているデータをCanvasに初期登録する
            if (e.NewValue is IEnumerable<RectInfo> rectInfos)
            {
                foreach (var info in rectInfos)
                {
                    mycanvas.AddRectangle(info);
                    info.PropertyChanged += mycanvas.RectInfo_PropertyChanged;
                }
            }
        }
    }

    /// <summary>
    /// ObservableCollectionの変更通知処理
    /// ItemsにRectInfoが追加、削除されたときに呼ばれる。
    /// CanvasへRectangleを追加/削除している。
    /// </summary>
    /// <param name="sender">ObservableCollection</param>
    /// <param name="e">変更通知情報</param>
    private void Items_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                //新しいRectInfoが追加されたのでCanvasにもRectangleを追加する
                if (e.NewItems != null)
                {
                    foreach (RectInfo info in e.NewItems)
                    {
                        AddRectangle(info);
                        info.PropertyChanged += RectInfo_PropertyChanged;
                    }
                }
                break;

            case NotifyCollectionChangedAction.Remove:
                //削除されたアイテムに対応するRectangleを削除する
                if (e.OldItems != null)
                {
                    foreach (RectInfo info in e.OldItems)
                    {
                        var target = FindRectangle(info);
                        if (target != null)
                        {
                            this.Children.Remove(target);
                            info.PropertyChanged -= RectInfo_PropertyChanged;
                        }
                    }
                }
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// RectInfoからRectangleを探す。
    /// Rectangle.Tagプロパティに埋め込んだRectInfo情報から特定する。
    /// 座標から特定しても良いが小数点以下が一致しない可能性があるので
    /// Tagプロパティに埋め込んでいる情報を使う。
    /// Tagプロパティへの埋め込みはAddRectangle()で生成時に実施している。
    /// </summary>
    /// <param name="info">検索対象</param>
    /// <returns>見つからない場合はnull</returns>
    private Rectangle? FindRectangle(RectInfo info)
    {
        if (info == null) return null;
        return this.Children.OfType<Rectangle>().FirstOrDefault(i => info == (RectInfo)i.Tag);
    }

    /// <summary>
    /// RectInfoのプロパティ変更通知用コールバック
    /// Name, X, Yプロパティの変更を画面に反映させる。
    /// </summary>
    private void RectInfo_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        var info = sender as RectInfo;
        if (info == null) return;

        //操作すべきRectangleを検索
        var target = FindRectangle(info);
        if (target == null) return;

        switch (e.PropertyName)
        {
            case "Name":
                target.ToolTip = info.Name;
                break;

            case "X":
                Canvas.SetLeft(target, info.X);
                break;

            case "Y":
                Canvas.SetTop(target, info.Y);
                break;

            default:
                break;
        }
    }

    internal void AddRectangle(RectInfo rinfo)
    {
        var r = new Rectangle
        {
            Width = 20,
            Height = 20,
            Fill = Brushes.SteelBlue,
            ToolTip = rinfo.Name,
            //RectangleとRectInfoの紐づけにTagを使う
            Tag = rinfo
        };

        //イベント追加
        r.MouseDown += DragStart;
        r.MouseUp += DragEnd;
        r.MouseMove += MoveRectangle;

        //パネルに追加
        this.Children.Add(r);
        Canvas.SetLeft(r, rinfo.X);
        Canvas.SetTop(r, rinfo.Y);
    }

    internal void RemoveRectangle(RectInfo rinfo)
    {
        var r = FindRectangle(rinfo);
        if (r != null)
        {
            this.Children.Remove(r);
            r.MouseDown -= DragStart;
            r.MouseUp -= DragEnd;
            r.MouseMove -= MoveRectangle;
            r.Tag = null;
        }
    }

    private void DragStart(object o, MouseEventArgs e)
    {
        //対象のRectangle
        _dragRectangle = (Rectangle)o;

        //Rectangleをキャプチャしドラッグ開始
        _dragRectangle.CaptureMouse();

        //Rectangle内のどこをクリックされたか記憶
        _dragOffset = e.GetPosition(_dragRectangle);

        //選択されたアイテムを通知する
        this.SelectedItem = (RectInfo)_dragRectangle.Tag;
    }

    private void DragEnd(object sender, MouseButtonEventArgs e)
    {
        _dragRectangle = null;
        if (sender is Rectangle r)
            r.ReleaseMouseCapture();
    }

    private void MoveRectangle(object o, MouseEventArgs e)
    {
        var rect = (Rectangle)o;
        if (rect == _dragRectangle)
        {
            var pos = e.GetPosition(this);
            if (rect.Tag is RectInfo info)
            {
                //RectinfoのX/Yプロパティを変更。
                //変更がItemsに伝わり画面上も動く
                info.X = (int)(pos.X - _dragOffset.X);
                info.Y = (int)(pos.Y - _dragOffset.Y);
            }
        }
    }
}
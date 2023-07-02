using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using wpfBindingSample.Models;

namespace wpfBindingSample;

/// <summary>
/// MyCanvas.xaml の相互作用ロジック
/// </summary>
public partial class MyCanvas : UserControl
{
    public MyCanvas()
    {
        InitializeComponent();
    }

    //ドラッグ用：ドラッグ開始位置
    private Point _dragOffset;

    //ドラッグ用：ドラッグ中のRectangle。ドラッグ中でない場合はnull
    private Rectangle? _dragRectangle;

    //選択されているアイテム
    public RectInfo SelectedItem
    {
        get { return (RectInfo)GetValue(SelectedItemProperty); }
        set { SetValue(SelectedItemProperty, value); }
    }

    public static readonly DependencyProperty SelectedItemProperty =
        DependencyProperty.Register("SelectedItem", typeof(RectInfo), typeof(MyCanvas), new PropertyMetadata(null, SelectedItem_Changed));

    private static void SelectedItem_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        (d as MyCanvas)?.SelectedItemChanged(e.OldValue, e.NewValue);
    }

    private void SelectedItemChanged(object oldValue, object newValue)
    {
        if (oldValue is RectInfo o)
        {
            //選択アイテム変更。以前選択されていたアイテム
            var target = FindRectangle(o);
            if (target != null)
            {
                target.Fill = Brushes.SteelBlue;
            }
        }
        if (newValue is RectInfo n)
        {
            //選択アイテム変更。今回選択されたアイテム
            var target = FindRectangle(n);
            if (target != null)
            {
                target.Fill = Brushes.Red;
            }
        }
    }

    /// <summary>
    /// Items依存関係プロパティ
    /// コレクション系UIElementのItemsSource等に相当するもの。
    /// Canvasベースにしたので自前実装
    /// このコレクションの変更通知を内部処理し、Canvas上にピンを追加削除、移動させる。
    /// </summary>
    public ObservableCollection<RectInfo> Items
    {
        get { return (ObservableCollection<RectInfo>)GetValue(ItemsProperty); }
        set { SetValue(ItemsProperty, value); }
    }

    public static readonly DependencyProperty ItemsProperty =
        DependencyProperty.Register("Items", typeof(ObservableCollection<RectInfo>), typeof(MyCanvas), new PropertyMetadata(null, ItemsChangedCallback));

    /// <summary>
    /// Items依存関係プロパティの変更コールバック
    /// インスタンスがBindingされると呼ばれるのでObservableCollectionの変更通知を登録する。
    /// static関数なので、インスタンス側で登録する。
    /// </summary>
    private static void ItemsChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is MyCanvas canvas)
        {
            // Itemsプロパティに変更通知を登録
            canvas.Items.CollectionChanged += canvas.Items_CollectionChanged;

            //ItemsのデータをCanvasに初期登録する
            if(e.NewValue is IEnumerable<RectInfo> infos)
            {
                foreach (var info in infos)
                {
                    canvas.AddRectangle(info);
                    info.PropertyChanged += canvas.RectInfo_PropertyChanged;
                }
            }
        }
    }

    
    /// <summary>
    /// ObservableCollectionの変更コールバック
    /// 要素の追加、削除、全削除に対しThumbをCanvasへ追加削除している。
    /// このMyCanvasのメイン
    /// </summary>
    /// <param name="sender">ObservableCollection</param>
    /// <param name="e">変更通知情報</param>
    private void Items_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
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
                if (e.OldItems != null)
                {
                    foreach (RectInfo info in e.OldItems)
                    {
                        var target = FindRectangle(info);
                        if (target != null)
                        {
                            canvas2.Children.Remove(target);
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
    /// </summary>
    /// <param name="info">検索対象</param>
    /// <returns>見つからない場合はnull</returns>
    private Rectangle? FindRectangle(RectInfo info)
    {
        if (info == null) return null;
        return canvas2.Children.OfType<Rectangle>().FirstOrDefault(i => info == (RectInfo)i.Tag);
    }

    /// <summary>
    /// コレクション内のRectInfoのプロパティ変更通知用コールバック
    /// 変更されたプロパティを画面に反映させる。
    /// </summary>
    private void RectInfo_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        //senderはデータ
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

    internal void AddRectangle(RectInfo info)
    {
        var r = new Rectangle
        {
            Width = 20,
            Height = 20,
            Fill = Brushes.SteelBlue,
            ToolTip = info.Name,
            //RectangleとRectInfoの紐づけにTagを使う
            //Canvasを使わずItemsControlを使えば不要
            Tag = info
        };

        //イベント追加
        r.MouseDown += DragStart;
        r.MouseUp += DragEnd;
        r.MouseMove += MoveRectangle;

        //パネルに追加
        canvas2.Children.Add(r);
        Canvas.SetLeft(r, info.X);
        Canvas.SetTop(r, info.Y);

        //info.PropertyChanged += RectInfo_PropertyChanged;

    }

    internal void RemoveRectangle(RectInfo info)
    {
        var r = FindRectangle(info);
        if (r != null)
        {
            canvas2.Children.Remove(r);
            r.MouseDown -= DragStart;
            r.MouseUp -= DragEnd;
            r.MouseMove -= MoveRectangle;
            r.Tag = null;
        }
    }

    private void DragEnd(object sender, MouseButtonEventArgs e)
    {
        _dragRectangle = null;
        if(sender is Rectangle r)
            r.ReleaseMouseCapture();
    }


    private void MoveRectangle(object o, MouseEventArgs e)
    {
        var rect = (Rectangle)o;
        if (rect == _dragRectangle)
        {
            var pos = e.GetPosition(canvas2);
            if (rect.Tag is RectInfo info)
            {
                //RectinfoのX/Yを変更すると画面上も動く
                info.X = (int)(pos.X - _dragOffset.X);
                info.Y = (int)(pos.Y - _dragOffset.Y);
            }
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
}
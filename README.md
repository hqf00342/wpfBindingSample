# wpf ドラッグ移動サンプル

MVVM Bindingサンプル。  
モデルデータとUI（Datagrid, Canvas, ListBox)を連動させます。

- WPF / .NET 6.0 
- CommunityToolkit.Mvvmを利用(v1.3以降)。

![sample](img/a.gif) 

UI上でデータを変更するとMyDataクラスにあるデータが変更されます。
それが別のUIにも反映されます。


## 更新

### ver 1.3

- CommunityToolkit.Mvvmを適用

### ver 1.2

- MyCanvasをUserControlではなくCanvas派生に変更
- モデル、ViewModelをフォルダに移動
- ListBoxの選択状態表示をCanvasに合わせるようXAML調整

### ver 1.1（公開時）

- フォルダをやめて平坦に。
- 削除ボタンをコマンド化。
- ファイル数削減。
- DataGridをMainWindow内に直接配置。これにより`SelectedItem`プロパティをDataGridとCanvas間で直接Bindingするように変更、ViewModel管理をやめた。
- データ管理(Model)をMyDataクラスに分離

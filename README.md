# wpf ドラッグ移動サンプル

MVVMを利用しているケースでのCanvas上のUIElementをドラッグ移動させるサンプル。


- WPF / .NET 6.0 
- MVVMライブラリは利用せず、MvvmUtilsフォルダに単純なものを用意。
- `RectInfo`というデータクラスでRectangleを管理


![sample](/img/a.gif) 

### 更新

サンプルなのでより平易になるようにファイル構成などを変更。

- フォルダをやめて平坦に。
- 削除ボタンをコマンド化。
- ファイル数削減。
- DataGridをMainWindow内に直接配置。これにより`SelectedItem`プロパティをDataGridとCanvas間で直接Bindingするように変更、ViewModel管理をやめた。

# wpf ドラッグ移動サンプル

MVVMを利用しているケースでのCanvas上のUIElementをドラッグ移動させるサンプル。


- .NET 6.0 
- WPF
- MVVMライブラリは利用せず。INotifyPropertyChangedを単純実装。
- 見やすくするために、わざと左右パネルをUserControl化しファイル分離。
- `RectInfo`というデータクラスでRectangleを管理

![sample](/img/a.gif) 


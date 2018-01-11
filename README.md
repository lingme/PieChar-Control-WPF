# PieChar Control WPF
---
WPF实现的饼图控件，并不是很完善，最高支持7个数例

#### View代码

```C
<pie:PieCharControl Grid.Row="0" Grid.RowSpan="2" PieItemSources="{Binding PieCharList,Mode=TwoWay}" TitleName="饼图 Demo" AggregateName="集装箱总数"></pie:PieCharControl>
```
#### 依赖属性说明

> **PieItemSources**
>饼图数据集，包含以下对象：

```C {.line-numbers}
private string _typeName;
/// <summary>
/// 类型名称
/// </summary>
public string TypeName {
    get { return _typeName; }
    set {
        if (_typeName != value) 
        {
            _typeName = value;
            OnPropertyChanged("TypeName");
        }
    }
}

private int _typeNumber;
/// <summary>
/// 类型数量
/// </summary>
public int TypeNumber {
    get { return _typeNumber; }
    set {
        if (_typeNumber != value) 
        {
            _typeNumber = value;
            OnPropertyChanged("TypeNumber");
        }
    }
}
```

> **TitleName**
>标题栏名称


> **AggregateName**
>总和名称

![A](https://github.com/lingme/Picture_Bucket/raw/master/PieChar_Control_WPF_img/index_1.jpg)



 

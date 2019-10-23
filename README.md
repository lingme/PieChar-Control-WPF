# PieChar Control WPF
---
WPF implementation of the pie chart control, is not very perfect, the highest support for 7 examples

#### View代码

```C
<pie:PieCharControl Grid.Row="0" Grid.RowSpan="2" PieItemSources="{Binding PieCharList,Mode=TwoWay}" TitleName="饼图 Demo" AggregateName="集装箱总数"></pie:PieCharControl>
```
#### Dependent

> **PieItemSources**
>Object

```C {.line-numbers}
private string _typeName;
/// <summary>
/// Name
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
/// Type count
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



 

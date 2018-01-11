using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Prism.Mvvm;
using Prism.Commands;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace PieChart
{
    [TemplatePart(Name = Parid_timePoint)]
    [TemplatePart(Name = Parid_titleText)]
    [TemplatePart(Name = Parid_legendGrid)]
    [TemplatePart(Name = Parid_leftText)]
    [TemplatePart(Name = Parid_rightText)]
    public class PieCharControl : Control
    {
        private Grid _timePoint;                                //饼图容器
        private WrapPanel _legendGrid;                    //图例容器
        private TextBlock _titleText;                          //标题
        private TextBox _leftText;                             //类型
        private TextBox _rightText;                           //数量

        private const string Parid_timePoint = "Z_Parid_timePoint";
        private const string Parid_titleText = "Z_Parid_titleText";
        private const string Parid_legendGrid = "Z_Parid_legendGrid";
        private const string Parid_leftText = "Z_Parid_leftText";
        private const string Parid_rightText = "Z__Parid_rightText";

        public static readonly DependencyProperty PieItemSourcesProperty = DependencyProperty.Register(
            "PieItemSources",
            typeof(ObservableCollection<PieCharItem>),
            typeof(PieCharControl),
            new PropertyMetadata(null, OnPieItemSourcesChanged, CoerceTMethod));

        public static readonly DependencyProperty TitleNameProperty = DependencyProperty.Register(
            "TitleName",
            typeof(string),
            typeof(PieCharControl),
            new PropertyMetadata("", OnTitleNameChanged, CoerceTMethod));

        public static readonly DependencyProperty AggregateNameProperty = DependencyProperty.Register(
            "AggregateName",
            typeof(string),
            typeof(PieCharControl),
            new PropertyMetadata("", OnAggregateNameChanged, CoerceTMethod));

        /// <summary>
        /// 饼图数据源
        /// </summary>
        public ObservableCollection<PieCharItem> PieItemSources
        {
            get { return (ObservableCollection<PieCharItem>)GetValue(PieItemSourcesProperty); }
            set { SetValue(PieItemSourcesProperty, value); }
        }

        /// <summary>
        /// 标题文字
        /// </summary>
        public string TitleName
        {
            get { return (string)GetValue(TitleNameProperty); }
            set { SetValue(TitleNameProperty, value); }
        }

        /// <summary>
        /// 总计名称
        /// </summary>
        public string AggregateName
        {
            get { return (string)GetValue(AggregateNameProperty); }
            set { SetValue(AggregateNameProperty, value); }
        }

        /// <summary>
        /// 参与依赖属性的基类对象
        /// </summary>
        private static PieCharControl _masterPieChar;

        private List<SolidColorBrush> _colorBrushList;
        /// <summary>
        /// 饼图颜色
        /// </summary>
        private List<SolidColorBrush> ColorBrushList
        {
            get
            {
                if (_colorBrushList == null || _colorBrushList.Count == 0)
                {
                    _colorBrushList = new List<SolidColorBrush>()
                    {
                        new SolidColorBrush((Color)ColorConverter.ConvertFromString("#85ea85")),
                        new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e97473")),
                        new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6392db")),
                        new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8f98b5")),
                        new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e6cf0d")),
                        new SolidColorBrush((Color)ColorConverter.ConvertFromString("#99b54e"))
                    };
                }
                return _colorBrushList;
            }
        }

        /// <summary>
        /// 饼图属性集合更改事件
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnPieItemSourcesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PieCharControl pieControl = (PieCharControl)d;
            _masterPieChar = pieControl;
            if (pieControl.PieItemSources != null && pieControl.PieItemSources.Count() > 0)
            {
                pieControl.PieItemSources.CollectionChanged += PieItemSources_CollectionChanged;
                foreach (var item in pieControl.PieItemSources)
                {
                    item.PropertyChanged += Obj_PropertyChanged;
                }
                pieControl.ConstDataPie();
            }
        }


        /// <summary>
        /// PieItemSources的集合变更事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void PieItemSources_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (PieCharItem item in e.NewItems)
                {
                    item.PropertyChanged += Obj_PropertyChanged;
                }
            }
            _masterPieChar.ConstDataPie();
        }

        /// <summary>
        /// PieItemSources成员属性变更事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Obj_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _masterPieChar.ConstDataPie();
        }

        /// <summary>
        /// 标题更改事件
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnTitleNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PieCharControl pieControl = (PieCharControl)d;
            if (pieControl._titleText != null)
            {
                pieControl._titleText.Text = pieControl.TitleName;
            }
        }

        /// <summary>
        /// 强制装换
        /// </summary>
        /// <param name="d"></param>
        /// <param name="baseValue"></param>
        /// <returns></returns>
        private static object CoerceTMethod(DependencyObject d, object baseValue)
        {
            return baseValue;
        }

        /// <summary>
        /// 总计名称更改事件
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnAggregateNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PieCharControl pieControl = (PieCharControl)d;
            if (pieControl.PieItemSources != null && pieControl.PieItemSources.Count() > 0)
            {
                pieControl.ConstDataPie();
            }
        }

        /// <summary>
        /// 总计名称强制装换
        /// </summary>
        /// <param name="d"></param>
        /// <param name="baseValue"></param>
        /// <returns></returns>
        private static object CoerceAggregateName(DependencyObject d, object baseValue)
        {
            return baseValue;
        }

        /// <summary>
        /// 获取模板项
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _timePoint = GetTemplateChild(Parid_timePoint) as Grid;
            _titleText = GetTemplateChild(Parid_titleText) as TextBlock;
            _legendGrid = GetTemplateChild(Parid_legendGrid) as WrapPanel;
            _leftText = GetTemplateChild(Parid_leftText) as TextBox;
            _rightText = GetTemplateChild(Parid_rightText) as TextBox;
        }

        /// <summary>
        /// 构造函数初始化
        /// </summary>
        public PieCharControl()
        {
            SizeChanged += delegate { ConstDataPie(); };
        }

        /// <summary>
        /// 数据动态生成饼图
        /// </summary>
        public void ConstDataPie()
        {
            if (_timePoint != null)
            {
                SetDefaultChild();
                List<SectorPart> sectorParts = new List<SectorPart>();                                                         //扇形集合
                int allNumber = PieItemSources == null ? 0 : PieItemSources.Sum(x => x.TypeNumber);       //总数
                if (allNumber > 0)
                {
                    _leftText.Text += AggregateName + "：\r\n";
                    _rightText.Text += allNumber.ToString() + "\r\n";
                    foreach (var item in PieItemSources)
                    {
                        int colorCount = sectorParts.Count();
                        double Z_angle = (double)item.TypeNumber / allNumber * 360;
                        sectorParts.Add(new SectorPart(Z_angle >= 360 ? 359.99 : Z_angle, ColorBrushList[colorCount]));

                        AddIconFill(ColorBrushList[colorCount], allNumber, item);

                        _leftText.Text += item.TypeName + "：\r\n";
                        _rightText.Text += item.TypeNumber.ToString() + "\r\n";
                    }
                }
                foreach (var shape in GetEllipsePieChartShapes(GetPieCenterPoint(), GetPieWidth(), GetPieWidth(), 0, sectorParts))
                {
                    _timePoint.Children.Add(shape);
                }
            }
        }

        /// <summary>
        /// 重置绘制区域
        /// </summary>
        private void SetDefaultChild()
        {
            _timePoint.Children.Clear();
            _legendGrid.Children.Clear();
            _leftText.Text = _rightText.Text = string.Empty;
            _titleText.Text = TitleName;
        }

        /// <summary>
        /// 添加图例
        /// </summary>
        /// <param name="color"></param>
        /// <param name="constString"></param>
        private void AddIconFill(Brush color ,  int allNumber , PieCharItem item)
        {
            _legendGrid.Children.Add(new Ellipse()
            {
                Fill = color,
                Width = 15,
                Height = 15,
                Margin = new Thickness(10,0,0,0),
                HorizontalAlignment = HorizontalAlignment.Center
            });
            _legendGrid.Children.Add(new TextBox()
            {
                Text = $"{item.TypeName}({(Math.Round((double)item.TypeNumber / allNumber * 100, 1)).ToString("F1")}%)",
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")),
                Width = 90,
                Height = 20,
                FontSize = 11,
                IsReadOnly = true,
                BorderThickness = new Thickness(0),
                HorizontalContentAlignment = HorizontalAlignment.Left,
                VerticalContentAlignment = VerticalAlignment.Center,
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#333333")),
            });
        }

        /// <summary>
        /// 获取饼图中心点坐标
        /// </summary>
        /// <returns></returns>
        public Point GetPieCenterPoint()
        {
            return new Point(_timePoint.ActualWidth / 2, _timePoint.ActualHeight / 2);
        }

        /// <summary>
        /// 获取饼图半径（以容器最小边为半径周长，减20，上下左右留下边距）
        /// </summary>
        /// <returns></returns>
        public double GetPieWidth()
        {
            double Z_width = _timePoint.ActualWidth > _timePoint.ActualHeight ? (_timePoint.ActualHeight / 2 - 20) : (_timePoint.ActualWidth / 2 - 20);
            return Z_width < 0 ? 10 : Z_width;
        }

        /// <summary>
        /// 将继承控件的原始控件的样式覆盖
        /// </summary>
        static PieCharControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PieCharControl), new FrameworkPropertyMetadata(typeof(PieCharControl)));
        }

        /// <summary>
        /// 获取圆上指定角度的点坐标
        /// </summary>
        /// <param name="center">中心坐标</param>
        /// <param name="radiusX">椭圆X轴（当X=Y为正圆）</param>
        /// <param name="radiusY">椭圆Y轴（当X=Y为正圆）</param>
        /// <param name="angle">角度,从0到360度，以正北方向为0度，顺时针旋转角度增加</param>
        /// <returns>在椭圆上旋转角度后的坐标</returns>
        public static Point GetEllipsePointAnti(Point center, double radiusX, double radiusY, double angle)
        {
            Vector circleZeroAnglePoint = new Vector(0, radiusX);                                              // 半径为X半轴的圆圆心平移到原点后0度所对应的向量
            Matrix rotateMatrix = new Matrix();                                                                          // 旋转角度所对应的矩阵
            rotateMatrix.Rotate(180 + angle);
            Vector circlePoint = circleZeroAnglePoint * rotateMatrix;                                          // 圆旋转角度后的坐标
            Point ellpseOrigin = new Point(circlePoint.X, circlePoint.Y * radiusY / radiusX);          // 将圆拉伸椭圆后的坐标

            return (Vector)ellpseOrigin + center;                                                                         // 将坐标平移至实际坐标
        }

        /// <summary>
        /// 获取圆形状的饼图的形状列表
        /// </summary>
        /// <param name="center">椭圆两个焦点的中点</param>
        /// <param name="radiusX">椭圆的长轴</param>
        /// <param name="radiusY">椭圆的短轴</param>
        /// <param name="offsetAngle">偏移角度,即第一个扇形开始的角度</param>
        /// <param name="sectorParts">扇形列表,扇形列表的SpanAngle之和应为360度</param>
        /// <param name="ringParts">环列表</param>
        /// <returns>构成饼图的形状列表</returns>
        public static IEnumerable<Shape> GetEllipsePieChartShapes(Point center, double radiusX, double radiusY, double offsetAngle, IEnumerable<SectorPart> sectorParts)
        {
            var shapes = new List<Shape>();
            foreach (var sectorPart in sectorParts)
            {
                Point firstPoint = GetEllipsePointAnti(center, radiusX, radiusY, offsetAngle);             // 扇形顺时针方向在椭圆上的第一个点
                offsetAngle += sectorPart.SpanAngle;

                Point secondPoint = GetEllipsePointAnti(center, radiusX, radiusY, offsetAngle);        // 扇形顺时针方向在椭圆上的第二个点
                PathFigure sectorFigure = new PathFigure { StartPoint = center };

                sectorFigure.Segments.Add(new LineSegment(firstPoint, false));                               // 添加中点到第一个点的弦

                // 添加第一个点和第二个点之间的弧
                sectorFigure.Segments.Add(
                    new ArcSegment
                    {
                        Point = secondPoint,
                        Size = new Size(radiusX,radiusY),
                        RotationAngle = 0,
                        IsLargeArc = sectorPart.PIsLargeArc,
                        SweepDirection = SweepDirection.Clockwise,
                        IsStroked = false
                    });
                PathGeometry sectorGeometry = new PathGeometry();
                sectorGeometry.Figures.Add(sectorFigure);
                Path sectorPath = new Path { Data = sectorGeometry, Fill = sectorPart.FillBrush };
                shapes.Add(sectorPath);
            }
            return shapes;
        }
    }
}
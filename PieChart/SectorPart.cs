using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PieChart
{
    public class SectorPart
    {
        /// <summary>
        /// 初始化构造
        /// </summary>
        /// <param name="spanAngle">跨越角度</param>
        /// <param name="fillBrush">填充画刷</param>
        public SectorPart(double spanAngle, Brush fillBrush)
        {
            this.SpanAngle = spanAngle;
            this.FillBrush = fillBrush;
        }

        /// <summary>
        /// 跨越角度，取值范围为0到360
        /// </summary>
        public double SpanAngle { get; set; }

        /// <summary>
        /// 填充画刷（指定颜色或者渐变）
        /// </summary>
        public Brush FillBrush { get; set; }

        /// <summary>
        /// 角度是否超过180度，因为超过180度需要绘制大弧扫掠，小于180度，绘制小弧扫掠
        /// </summary>
        public bool PIsLargeArc
        {
            get
            {
                return SpanAngle > 180 ? true : false;
            }
        }
    }
}

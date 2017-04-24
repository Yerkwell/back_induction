using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WPF_RK_2
{
    public class ArrowLine : Shape   //Стрелка
    {
        public double X1
        {
            get { return (double)this.GetValue(X1Property); }
            set { this.SetValue(X1Property, value); }
        }
        public static readonly DependencyProperty X1Property = System.Windows.DependencyProperty.Register(
            "X1",
            typeof(double),
            typeof(ArrowLine),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public double Y1
        {
            get { return (double)this.GetValue(Y1Property); }
            set { this.SetValue(Y1Property, value); }
        }
        public static readonly DependencyProperty Y1Property = System.Windows.DependencyProperty.Register(
            "Y1",
            typeof(double),
            typeof(ArrowLine),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public double X2
        {
            get { return (double)this.GetValue(X2Property); }
            set { this.SetValue(X2Property, value); }
        }

        public static readonly DependencyProperty X2Property = System.Windows.DependencyProperty.Register(
            "X2",
            typeof(double),
            typeof(ArrowLine),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public double Y2
        {
            get { return (double)this.GetValue(Y2Property); }
            set { this.SetValue(Y2Property, value); }
        }

        public static readonly DependencyProperty Y2Property = System.Windows.DependencyProperty.Register(
            "Y2",
            typeof(double),
            typeof(ArrowLine),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public double offset //Смещение наконечника относительно конца линии
        {
            get;
            set;
        }

        public double Length //Длина линии
        {
            get
            {
                return Math.Sqrt(Math.Pow(this.X2 - this.X1, 2) 
                    + Math.Pow(this.Y2 - this.Y1, 2));
            }
        }
        protected override Geometry DefiningGeometry //Отрисовка
        {
            get
            {
                double d = Length; // длина отрезка
                double ratio = (d - offset) / d;
                //Координаты конца наконечника стрелки
                double X3 = X1 + (X2 - X1) * ratio;
                double Y3 = Y1 + (Y2 - Y1) * ratio;
                //Координаты вектора
                double X = this.X2 - this.X1;
                double Y = this.Y2 - this.Y1;
                //Координаты точки, удалённой от конца наконечника стрелки к началу отрезка на 10px
                double X4 = X3 - (X / d) * 10;
                double Y4 = Y3 - (Y / d) * 10;
                //Координаты вектора перпендикуляра
                double Xp = this.Y2 - this.Y1;
                double Yp = this.X1 - this.X2;
                //Координаты перпендикуляров, удалённых от точки X4;Y4 на 5px в разные стороны
                double X5 = X4 + (Xp / d) * 5;
                double Y5 = Y4 + (Yp / d) * 5;
                double X6 = X4 - (Xp / d) * 5;
                double Y6 = Y4 - (Yp / d) * 5;
                //Сбор стрелки из линий
                GeometryGroup geometryGroup = new GeometryGroup();
                LineGeometry lineGeometry = new LineGeometry(new Point(this.X1, this.Y1), new Point(this.X2, this.Y2));
                LineGeometry arrowPart1Geometry = new LineGeometry(new Point(X3, Y3), new Point(X5, Y5));
                LineGeometry arrowPart2Geometry = new LineGeometry(new Point(X3, Y3), new Point(X6, Y6));
                geometryGroup.Children.Add(lineGeometry);
                geometryGroup.Children.Add(arrowPart1Geometry);
                geometryGroup.Children.Add(arrowPart2Geometry);
                return geometryGroup;
            }
        }
    }
}

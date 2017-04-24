using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WPF_RK_2
{
    public partial class MainWindow : Window
    {
        Tree tree;
        public bool solved;
        public MainWindow()
        {
            InitializeComponent();
            tree = new Tree();
        }
        void paintGraph()   //Нарисовать весь граф
        {
            canvas1.Children.Clear();   //Очистиить поле
            var center_x = canvas1.Width / 2;   //Центр - координата корня
            var x_delta = canvas1.Width / Math.Pow(2, tree.height + 1); //Расстояние между узлами на последнем уровне
            drawNode(tree.root, x_delta, (int)center_x, 50, tree.height);
        }
        void drawNode(Node node, double x_delta, int x, int y, int layer)  //Нарисовать узел дерева
        {
            var ellipse = new Ellipse() { Height = 10, Width = 10, Stroke = Brushes.Black, Fill = Brushes.Cyan}; //Второй игрок - циановым
            if (node.A)
                ellipse.Fill = Brushes.Red;      //Первый игрок - красным
            int y_delta = 30;               //Расстояние между слоями  дерева
            Canvas.SetLeft(ellipse, x - ellipse.Width / 2);
            Canvas.SetTop(ellipse, y - ellipse.Height / 2);
            Canvas.SetZIndex(ellipse, 3);
            canvas1.Children.Add(ellipse);        //Задали положение узла и разместили на холсте
            var label = new Label() { Content = node.data, FontSize = 14, HorizontalAlignment = System.Windows.HorizontalAlignment.Center, Background = Brushes.White };
            canvas1.Children.Add(label);
            label.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity)); //Пересчёт размеров текста
            ellipse.Tag = label;   //Привязываем текст к узлу
            Canvas.SetLeft(label, x - label.DesiredSize.Width / 2);
            Canvas.SetTop(label, y + 10);
            if (node.terminal)         //Если лист - отобразить текст (значения выигрышей)
            {
                ellipse.MouseEnter += ellipse_MouseEnter;    //Действие при наведении мыши
                ellipse.MouseLeave += ellipse_MouseLeave;    //Действие при убирании мыши
                Canvas.SetZIndex(label, 1);
            }
            else
            {
                ellipse.MouseEnter += label_Show;    //Действие при наведении мыши
                ellipse.MouseLeave += label_Hide;    //Действие при убирании мыши
                label.Background = Brushes.Beige;
                Canvas.SetZIndex(label, 4);
                label.Visibility = System.Windows.Visibility.Hidden;
            }
            if (node.left != null) //Если есть левый потомок, рисуем его тоже (рекурсивно)
            {
                drawNode(node.left, x_delta, (int)(x - (x_delta * Math.Pow(2, layer - 1))), y + y_delta, layer - 1);
                var x2 = x - (x_delta * Math.Pow(2, layer - 1));
                var y2 = y + y_delta;
                var line = new ArrowLine() { Stroke = Brushes.Black, offset = ellipse.Width / 2.0 }; //Стрелка
                canvas1.Children.Add(line);
                Canvas.SetZIndex(line, 1);
                line.X1 = x;
                line.Y1 = y;
                line.X2 = x2;
                line.Y2 = y2;
            }
            if (node.right != null) //Если есть правый потомок, рисуем его тоже (рекурсивно)
            {
                drawNode(node.right, x_delta, (int)(x + (x_delta * Math.Pow(2, layer - 1))), y + y_delta, layer - 1);
                var x2 = x + (x_delta * Math.Pow(2, layer - 1));
                var y2 = y + y_delta;
                var line = new ArrowLine() { Stroke = Brushes.Black, offset = ellipse.Width / 2.0 }; //Стрелка
                canvas1.Children.Add(line);
                Canvas.SetZIndex(line, 2);
                line.X1 = x;
                line.Y1 = y;
                line.X2 = x2;
                line.Y2 = y2;
            }
            node.ellipse = ellipse; //Привязываем нарисованный эллипс к узлу
        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)      //Изменение размера холста при изменении размера экрана
        {
            var border = (Border)canvas1.Parent;
            canvas1.Height = border.ActualHeight;
            canvas1.Width = border.ActualWidth;
        }
        void ellipse_MouseEnter(object sender, MouseEventArgs e) //При наведении мыши на эллипс
        {
            var label = (Label)((sender as Ellipse).Tag);
            Canvas.SetZIndex(label, 4);
            label.Background = Brushes.Beige;  //Выделить текст
        }
        private void ellipse_MouseLeave(object sender, MouseEventArgs e) //При убирании мыши
        {
            var label = (Label)((sender as Ellipse).Tag);
            Canvas.SetZIndex(label, 1);
            label.Background = Brushes.White;  //Отменить выделение текста
        }
        void label_Show(object sender, MouseEventArgs e) //При наведении мыши на эллипс
        {
            if (solved)
            {
                var label = (Label)((sender as Ellipse).Tag);
                label.Visibility = System.Windows.Visibility.Visible;
            }
        }
        private void label_Hide(object sender, MouseEventArgs e) //При убирании мыши
        {
            var label = (Label)((sender as Ellipse).Tag);
            label.Visibility = System.Windows.Visibility.Hidden;
        }
        private void Gen_Button_Click(object sender, RoutedEventArgs e) //Кнопка Generate
        {
            res1.Content = "";
            tree.randInit(int.Parse(nodeCount.Text)); //Заполнить дерево
            solved = false;
            paintGraph(); //И отрисовать
        }
        private void Solve_Button_Click(object sender, RoutedEventArgs e) //Кнопка Solve
        {
            Pair result = tree.root.evaluate(); //Обсчитать дерево
            res1.Content = result;  //Вывести результат
            var current = tree.root;
            while (!current.terminal) //Нарисовать на дереве путь
            {
                var x1 = Canvas.GetLeft(current.pathFrom.ellipse) + current.pathFrom.ellipse.Width / 2;// x - (x_delta * Math.Pow(2, layer - 1));
                var y1 = Canvas.GetTop(current.pathFrom.ellipse) + current.pathFrom.ellipse.Height / 2;// y + y_delta;
                var x2 = Canvas.GetLeft(current.ellipse) + current.ellipse.Width / 2;// x - (x_delta * Math.Pow(2, layer - 1));
                var y2 = Canvas.GetTop(current.ellipse) + current.ellipse.Height / 2;// y + y_delta;
                var line = new Line() { Stroke = Brushes.Blue, StrokeThickness = 3 };
                canvas1.Children.Add(line);
                Canvas.SetZIndex(line, 1);
                line.X1 = x1;
                line.Y1 = y1;
                line.X2 = x2;
                line.Y2 = y2;
                current = current.pathFrom;
            }
            solved = true;
        }
        private static bool isTextAllowed(string text) //Проверка, число ли введено в поле ввода
        {
            int data;
            return int.TryParse(text, out data);
        }
        private void previewTextInput(object sender, TextCompositionEventArgs e) //При вводе в поле ввода
        {
            e.Handled = !isTextAllowed(e.Text); //Проверка на число
        }
    }
    public struct Pair  //пара чисел
    {
        public int first;
        public int second;
        public override String ToString()
        {
            return string.Format("({0};{1})", first, second);
        }
    }
}

using System.Windows.Shapes;
using System.Windows.Controls;

namespace WPF_RK_2
{
    public class Node
    {
        public Node parent;
        public Node left;
        public Node right;
        public Node pathFrom;   //В какую сторону пройти к оптимальной стратегии
        public Pair data;       //Значения выигрышей
        public bool terminal;   //Лист
        public bool A;          //Чей ход (true - А, false - B)
        public Ellipse ellipse; //Соответствующий круг на холсте
        public Pair evaluate()  //Расчитать цену под-игры
        {
            if (terminal)       //Для листа она известна
                return data;
            if (left == null)   //Если слева ничего нет - совпадает с выигрышем правого ребёнка
            {
                right.evaluate();
                data = right.data;
                pathFrom = right;
                ((Label)(ellipse.Tag)).Content = data;
                return data;
            }
            if (!left.terminal)
                left.evaluate();
            if (right == null)  //Если справа ничего нет - совпадает с выигрышем левого ребёнка
            {
                data = left.data;
                pathFrom = left;
                ((Label)(ellipse.Tag)).Content = data;
                return data;
            }
            if (!right.terminal)
                right.evaluate();
            if (A)              //Если А, выбираем потомка, для которого выигрыш первого игрока больше
            {
                if (right.data.first > left.data.first)
                {
                    data = right.data;
                    pathFrom = right;
                }
                else
                {
                    data = left.data;
                    pathFrom = left;
                }
            }
            else                //Иначе выбираем потомка, для которого выигрыш второго игрока больше
            {
                if (right.data.second > left.data.second)
                {
                    data = right.data;
                    pathFrom = right;
                }
                else
                {
                    data = left.data;
                    pathFrom = left;
                }
            }
            ((Label)(ellipse.Tag)).Content = data;
            return data;
        }
        public void addChild(Pair nodeData, bool right) //Добавить потомка (направо либо налево)
        {
            var child = new Node() { parent = this, data = nodeData, terminal = true, A = !A };
            terminal = false;   //Это больше не лист
            if (right)
                this.right = child;
            else
                this.left = child;
        }
    }
}

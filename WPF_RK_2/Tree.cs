using System;

namespace WPF_RK_2
{
    public class Tree
    {
        public Node root;
        public int height;
        public void randInit(int nodeCount)     //Заполнение дерева
        {
            root = new Node() { parent = null, A = true, terminal = true };
            Random rnd = new Random();
            for (int i = 0; i < nodeCount - 1; i++)
            {
                Pair data = new Pair() { first = rnd.Next(21) - 10, second = rnd.Next(21) - 10 }; //Выигрыш для данной вершины (имеет значение только для листьев)
                var current = root;
                bool placeNotFound = true;
                bool right = true;
                int h = 0;          //Текущая высота дерева
                while (placeNotFound)       //Пока не найдём, куда вставить новую вершину
                {
                    placeNotFound = false;
                    right = (rnd.Next(2) == 0) ? false : true;  //Случайным образом перемещаемся влево или вправо
                    if (right && (current.right != null))
                    {
                        current = current.right;
                        placeNotFound = true;
                    }
                    else if (!right && (current.left != null))
                    {
                        current = current.left;
                        placeNotFound = true;
                    }
                    h++;
                }
                //Нашли свободное место
                current.addChild(data, right);  //Добавляем вершину
                if (h > height)     //Если высота дерева увеличилась, записываем её
                    height = h;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVLdict
{
    class Constants
    {
        public const int MaxTreeDisplay = 128;
        public const int MaxTreeDisplayPower = 7;
    }
    class Dictionary
    {
        public Node Root { get; private set; }

        public void Add(string word)
        {
            Node newNode = new Node(word);
            AddToTree(Root, newNode);
        }

        private void AddToTree(Node parent, Node newNode)
        {
            if (Root == null)
            {
                Root = newNode;
                return;
            }

            if (newNode.Word.CompareTo(parent.Word) < 0)
            {
                if (parent.Left == null)
                {
                    parent.Left = newNode;
                    newNode.Parent = parent;
                }
                else
                {
                    AddToTree(parent.Left, newNode);
                }
            }
            else
            {
                if (parent.Right == null)
                {
                    parent.Right = newNode;
                    newNode.Parent = parent;
                }
                else
                {
                    AddToTree(parent.Right, newNode);
                }
            }
        }

        public void PrintTree(Node root)
        {
            string[] Tree = new string[Constants.MaxTreeDisplay+1];
            int level = MapTree(root, Tree, 1);

            Console.WriteLine(level);

            int power = Constants.MaxTreeDisplayPower;
            while ((level >> power) == 0) --power;

            Console.WriteLine(power);
            level = ((level >> power) << power);
            int spaces = level / 2;

            for (int i = 1; i < level; i *= 2)
            {
                for (int k = 0; k < (level / i) / 2; k++) { Console.Write(" "); }

                for (int j = i; j < i * 2; j++)
                {
                    for (int k = 0; k < level / i / 2; k++) { Console.Write("  "); }

                    if (Tree[j] == null) Tree[j] = "x";
                    Console.Write(Tree[j] + " ");
                }
                spaces /= 2;
                Console.WriteLine();
            }
        }

        private int MapTree(Node n, string[] visualArray, int level)
        {
            if (n == null) return 0;

            int maxlevel = 1;

            visualArray[level] = n.Word;
            level *= 2;
            maxlevel = Math.Max(level, MapTree(n.Left, visualArray, level));
            maxlevel = Math.Max(level, MapTree(n.Right, visualArray, level + 1));

            return maxlevel;
        }
    }
}

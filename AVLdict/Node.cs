using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVLdict
{
    class Node
    {
        public Node Parent { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }
        public string Word { get; set; }
        public int Weight { get; set; }

        public Node()
        {
            Parent = null;
            Left = null;
            Right = null;
            Word = "";
        }

        public Node(string word)
        {
            Parent = null;
            Left = null;
            Right = null;
            Word = word;
        }

        public Node(Node n)
        {
            Parent = n.Parent;
            Left = n.Left;
            Right = n.Right;
            Word = n.Word;
        }

    }
}

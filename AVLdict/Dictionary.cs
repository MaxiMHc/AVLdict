using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVLdict
{
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


    }
}

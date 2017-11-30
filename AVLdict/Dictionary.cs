//#define VERBOSE
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

        public bool Remove(string word)
        {
            Node NodeToRemove = Find(word);
            Node NodeInPlace;

            // not found
            if (NodeToRemove == null)
            {
                return false;
            }
            // deleting node with less than 2 children
            if(NodeToRemove.Left == null || NodeToRemove.Right == null)
            {
                // root cases
                if (NodeToRemove.Parent == null)
                {
                    if(NodeToRemove.Left == null && NodeToRemove.Right == null)
                    {
                        Root = null;
                        return true;
                    }
                    if (NodeToRemove.Left == null)
                    {
                        Root = NodeToRemove.Right;
                        NodeToRemove.Right.Parent = null;
                    }
                    else
                    {
                        Root = NodeToRemove.Left;
                        NodeToRemove.Left.Parent = null;
                    }
                    return true;
                }
                // non-root case; no action needed
                //NodeInPlace = NodeToRemove;
            }
            // 2 children
            else
            {
                // find replacement; RLLLL... or LRRRR...
                NodeInPlace = NodeToRemove;
                if (NodeToRemove.Right != null)
                {
                    NodeToRemove = NodeToRemove.Right;
                    while (NodeToRemove.Left != null)
                    {
                        NodeToRemove = NodeToRemove.Left;
                    }
                }
                else
                {
                    NodeToRemove = NodeToRemove.Left;
                    while (NodeToRemove.Right != null)
                    {
                        NodeToRemove = NodeToRemove.Right;
                    }
                }
                NodeInPlace.Word = NodeToRemove.Word;
            }

            // replace
            if(NodeToRemove.Right != null)
            {
                NodeInPlace = NodeToRemove.Right;
            }
            else
            {
                NodeInPlace = NodeToRemove.Left;
            }
            if (NodeInPlace != null)
            {
                NodeInPlace.Parent = NodeToRemove.Parent;
            }
            // if node removed is root
            if(NodeToRemove.Parent == null)
            {
                Root = NodeInPlace;
            }
            // if it is not root
            else
            {
                if (NodeToRemove == NodeToRemove.Parent.Left)
                {
                    NodeToRemove.Parent.Left = NodeInPlace;
                }
                else
                {
                    NodeToRemove.Parent.Right = NodeInPlace;
                }
            }

            return true;
        }

        public bool IsInTree(string word)
        {
            Node n = Find(word);
            if (n == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private Node Find(string word)
        {
            return FindInTree(Root, word);
        }

        private Node FindInTree(Node n, string word)
        {
            if (n == null) return null;

            if (n.Word.CompareTo(word) == 0)
            {
                return n;
            }

            if (n.Word.CompareTo(word) > 0)
            {
                return FindInTree(n.Left, word);
            }
            else
            {
                return FindInTree(n.Right, word);
            }
        }

        public int CountSubstr(String substr)
        {
            return CountSubstringsInTree(substr);
        }

        private int CountSubstringsInTree(String substr) //FindSubstringInTree is modified FindInTree. Counts nodes that begin with substr
        {
            int SubstringsFoundSoFar = 0;
            Node currentNode = Root;
            bool currentNodePrecedesSubstr = currentNode.Word.CompareTo(substr) < 0;//true if current node's word is before substr in the alphabetical order
            bool currentNodeContainsSubstr = currentNode.Word.Contains(substr);
            
            
            //find most optimal place to start
            while (currentNodePrecedesSubstr)
            {
                currentNodePrecedesSubstr = currentNode.Word.CompareTo(substr) < 0;
                currentNodeContainsSubstr = currentNode.Word.Contains(substr);
                #if VERBOSE 
                Console.WriteLine("going right");
                #endif
                currentNode = currentNode.Right; //if the current node's word is before the substring in the alphabetical order, we got to the node whose word is closer to substr
            }
            //after the loop has finished, we're definitely on a node whose word is after the substring in the alphabetical order

            if (currentNode.Parent!=null && currentNode.Parent.Word.Contains(substr)) SubstringsFoundSoFar++;


            void FindSubstringInTree(Node n) { //my function now TriHard. searches for substring from the optimal node to start and stops when it definitely wont encounter the substring anymore
                String CurrentNodePrefixCutout;
                if (n == null) return;
                if (currentNode.Word.Length >= substr.Length) currentNode.Word.Substring(0, substr.Length - 2);
                if (currentNode.Word.Contains(substr)) SubstringsFoundSoFar++;
                if (currentNode.Word.CompareTo(substr) >= 0) return; //you definitely wont find the substring past this point, so we end it here

                FindSubstringInTree(currentNode.Left);
                FindSubstringInTree(currentNode.Right);
            }

            FindSubstringInTree(currentNode);
            return SubstringsFoundSoFar+1;
        }

        public void PrintTree(Node root)
        {
            string[] Tree = new string[Constants.MaxTreeDisplay+1];
            int level = MapTree(root, Tree, 1);

            //Console.WriteLine(level);

            int power = Constants.MaxTreeDisplayPower;
            while ((level >> power) == 0) --power;

            //Console.WriteLine(power);
            level = ((level >> power) << power);
            int spaces = level / 2;

            for (int i = 1; i < level; i *= 2)
            {
                for (int k = 0; k < (level / i / 2) - 1; k++) { Console.Write(" "); }

                for (int j = i; j < i * 2; j++)
                {
                    if (Tree[j] == null) Tree[j] = "x";
                    Console.Write(Tree[j] + " ");

                    for (int k = 0; k < (level / i / 2) - 1; k++) { Console.Write("  "); }
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
            maxlevel = Math.Max(level, maxlevel);
            maxlevel = Math.Max(maxlevel, MapTree(n.Left, visualArray, level));
            maxlevel = Math.Max(maxlevel, MapTree(n.Right, visualArray, level + 1));

            return maxlevel;
        }
    }
}

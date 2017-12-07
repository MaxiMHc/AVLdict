#define VERBOSE
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
        public const bool WeTestingNow = false;
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
                    CalculateWeights(newNode, 1);
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
                    CalculateWeights(newNode, 1);
                }
                else
                {
                    AddToTree(parent.Right, newNode);
                }
            }
        }

        private void CalculateWeights(Node n, int change)
        {
            while (n != Root)
            {
                if (n.Word == "")   // swap case
                {
                    if (n.Parent.Right == null)
                        n.Word = ((char)(n.Parent.Word.LastOrDefault() + 1)).ToString();
                    else
                        n.Word = ((char)(n.Parent.Word.LastOrDefault() - 1)).ToString();
                }
                if (n.Word.CompareTo(n.Parent.Word) > 0)
                {
                    n = n.Parent;
                    n.Weight -= change;
                }
                else
                {
                    n = n.Parent;
                    n.Weight += change;
                }
                if (change == 1 && n.Weight == 0)
                {
                    break;
                }
                if (change == -1 && (n.Weight == -1 || n.Weight == 1))
                {
                    break;
                }
                if (n.Weight == 2)
                {
                    if (n.Left.Weight == 1)
                    {

                        
#if VERBOSE
                        Console.WriteLine("RR Rotation: Parent \"" + n.Word + "\", Child \"" + n.Left.Word + "\"");
#endif

                        if(Constants.WeTestingNow)swapChildParent(n, true); //doing RR
                    }
                    else
                    {
#if VERBOSE
                        Console.WriteLine("LR Rotation: Parent \"" + n.Word + "\", Child \"" + n.Left.Word + "\"");
#endif
                    }
                    break;
                }
                if (n.Weight == -2)
                {
                    if (n.Right.Weight == -1)
                    {
#if VERBOSE
                        Console.WriteLine("LL Rotation: Parent \"" + n.Word + "\", Child \"" + n.Right.Word + "\"");
#endif
                        if(Constants.WeTestingNow)swapChildParent(n, false); //doing LL
                    }
                    else
                    {
#if VERBOSE
                        Console.WriteLine("RL Rotation: Parent \"" + n.Word + "\", Child \"" + n.Right.Word + "\"");
#endif
                    }
                    break;
                }
            }
        }

        public bool Remove(string word)
        {
            Node NodeToRemove = Find(word);
            Node NodeInPlace = null;
            //bool has2Children = false;

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
                NodeToRemove.Word = "";
            }

            // replace
            if (NodeToRemove.Right != null)
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
            
            CalculateWeights(NodeToRemove, -1);

            return true;
        }


        /*Temporarily public for testing*/
        public void addChild(Node destinationParent, Node newChild) //it's different from the Add() function in the way that it tries to add a new child directly to a node instead to a tree
        {
            if (newChild.Word.CompareTo(destinationParent.Word) > 0)
            {
                if (destinationParent.Right != null)
                    throw new System.ArgumentException("addChild(): can't add new child to node as parent.Right is already taken. We probably don't need to implement this");
                destinationParent.Right = newChild;
            }
            else
            {
                if (destinationParent.Left != null)
                    throw new System.ArgumentException("addChild(): can't add new child to node as parent.Left is already taken. We probably don't need to implement this");
                destinationParent.Left = newChild;

            }
        }

        /*Temporarily public for testing*/
        public bool transferChild(bool giveRight, Node source, Node target) //give child from one node to another (if the target node doesn't have 2 children)
        {
            if (target.Right != null && target.Left != null)
            {
                throw new System.ArgumentException("target parent has 2 children already. cant give child from " + source.Word);
                return false;
            }

            if (giveRight) //if you want to give the right child from the source node (sourceParent)
            {

                if (target.Right.Word.CompareTo(source.Right.Word) < 0) //if target's right is bigger than the source's right
                {
                    target.Left = source.Right;
                    source.Right = null;
                    return true;
                }
                else
                {
                    target.Right = source.Right;
                    source.Right = null;
                    return true;
                }
            }
            else //!(giveRight)
            {
                if (target.Right.Word.CompareTo(source.Left.Word) < 0)
                {
                    target.Left = source.Left;
                    source.Left = null;
                    return true;
                }
                else
                {
                    target.Right = source.Left;
                    source.Left = null;
                    return true;
                }
            }
        }

        public bool swapChildParent(Node node, bool leftChildBecomeParent) //makes it so one of the children of a selected node, becomes the parent of that node
        {                                         //the swapping is done in a way so the BST structure is still correct after the swap

            bool leftExists = (node.Left != null);
            bool rightExists = (node.Right != null);
            int maxWeightOfChildren; //max(left.Left.Left and left.Right.Right) 
            Node left = node.Left;
            Node right = node.Right;
            Node parent = node.Parent;
            
            if (leftExists && rightExists) //need additional logic here
            {
                //throw new System.ArgumentException("target parent has 2 children already. cant have 3 children"); // exception PogChamp
                //wow this exception is 
            }

            // 1. take shortest subtree
            Node shortestSubtree=null; //is never null after below statements (CountWeights handles proper if statements so that left/right.weight is always >2)
            if (left.Weight > 1)
            {
                shortestSubtree = left.Right;
                left.Right = null;
            }

            else if (left.Weight < -1)
            {
                shortestSubtree = left.Left;
                left.Left = null;
            }

            // 2.add it to the nonswapped (node.Right in this case)
            if(shortestSubtree!=null)addChild(node.Right, shortestSubtree);





            if (leftChildBecomeParent == true)
            {
                
                addChild(left, node);
                if (node != Root)
                {
                    left.Parent = node.Parent;
                    node.Parent.Left = left;
                }
                node.Parent = left;
                node.Left = null;
                if (node == Root) { Root = left; }

                node.Weight = RecalculateWeights(node);
                left.Weight = RecalculateWeights(left);
               
            }

            else
            {
                addChild(right, node);
                if (node != Root)
                {
                    right.Parent = node.Parent;
                    node.Parent.Right = right;
                }
                node.Parent = right;
                node.Right = null;
                if (node == Root) { Root = right; }

                node.Weight = RecalculateWeights(node);
                right.Weight = RecalculateWeights(right);


            }

#if VERBOSESWAP
            //************SWAPCHILD DEBUG*****************
            bool newLeftNull = (left.Left == null);
            bool newRightNull = (left.Right == null);

            bool oldLeftNull = (node.Left == null);
            bool oldRightNull = (node.Right == null);

            Console.WriteLine(" Swapchild debug:");
            Console.WriteLine(" Initial parent node:" + node.Word);
            Console.WriteLine(" New parent:");
            Console.Write(" L: ");
            if (!newLeftNull)
                Console.Write(" " +left.Left.Word);
            else Console.Write("    null");

            Console.Write(" R: ");
            if (!newRightNull)
                Console.Write(left.Right.Word+"\n");
            else Console.Write("null \n");

            Console.WriteLine(" Old parent:");
            Console.Write(" L: ");
            if (!oldLeftNull)
                Console.Write(node.Left.Word);
            else Console.Write("null ");

            Console.Write(" R: ");
            if (!oldRightNull)
                Console.Write(node.Right.Word);
            else Console.Write("null\n");

            Console.WriteLine(" root: " + Root.Word);
            Console.WriteLine("\n");

#endif


            return true;
        }


        private int RecalculateWeights(Node n)
        {
            int newLeftWeight = 0;
            int newRightWeight = 0;
            int total;
            if (n.Left != null)
                newLeftWeight = n.Left.Weight;
            if (n.Right != null)
                newRightWeight = n.Right.Weight;

            total = newLeftWeight + newRightWeight;
#if VERBOSE
            Console.WriteLine(newLeftWeight + " + " + newRightWeight + " = " + total);
#endif
            return total;
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
                if(currentNode.Right!=null)
                    currentNode = currentNode.Right; //if the current node's word is before the substring in the alphabetical order, we got to the node whose word is closer to substr
            }
            //after the loop has finished, we're definitely on a node whose word is after the substring in the alphabetical order

            //if (currentNode.Parent!=null && currentNode.Parent.Word.Contains(substr)) SubstringsFoundSoFar++;


            void FindSubstringInTree(Node n) { //my function now TriHard. searches for substring from the optimal node to start and stops when it definitely wont encounter the substring anymore
                String CurrentNodePrefixCutout;
                if (n == null) return;
                if (n.Word.Length >= substr.Length)
                {
                    CurrentNodePrefixCutout = n.Word.Substring(0, substr.Length);
                    if (CurrentNodePrefixCutout == null) return;
#if VERBOSE
                    Console.WriteLine(CurrentNodePrefixCutout);
                    Console.WriteLine(substr);
#endif
                    if (CurrentNodePrefixCutout == substr) { SubstringsFoundSoFar++; }
                    if (n.Word.CompareTo(substr) > 0) return; //you definitely wont find the substring past this point, so we end it here
                }

                FindSubstringInTree(n.Left);
                FindSubstringInTree(n.Right);
            }

            FindSubstringInTree(currentNode);
            return SubstringsFoundSoFar;
        }

        public void PrintTree(Node root)
        {
            string[] Tree = new string[Constants.MaxTreeDisplay+1];
            int level = MapTree(root, Tree, 1);

            if (level == 0) return;

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
                    if (Tree[j] == null) Tree[j] = "*";
                    Console.Write(Tree[j] + " ");
#if VERBOSE
                    if (Tree[j] != "*")
                        Console.Write("(" + Find(Tree[j]).Weight + ")" + " ");
                    else
                        Console.Write("(-) ");
#endif

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

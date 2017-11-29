using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVLdict
{
    class Program
    {
        static void Main(string[] args)
        {
            //String[] tab = System.IO.File.ReadAllLines("in1.txt");
            Dictionary dict = new Dictionary();
            dict.Add("2");
            dict.Add("1");
            dict.Add("4");
            dict.Add("3");
            dict.Add("5");
            dict.Add("6");
            dict.Add("7");
            dict.PrintTree(dict.Root);
        }
    }
}
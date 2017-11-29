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
            Dictionary dict = new Dictionary();

            void doOperation(String inputline)
            {
                char getCommand(String input) //getCommand(String) takes a string and returns the first letter of said string + checking if that command exists
                {
                    char firstLetter = input[0];
                    if ((firstLetter == 'W') || (firstLetter == 'U') || (firstLetter == 'S') || (firstLetter == 'L')) //the possible commands are W U S L which are always on the first letter of the input
                        return firstLetter;
                    else return '0'; //if the user tries to run a command that doesnt exist return char '0'
                }
                String snipCommand(String input)
                {
                    return input.Substring(0, inputline.Length);
                }

                //inputline is something like "S word" (so [command] [word])
                char command = getCommand(inputline); //this is "S", the [command]
                String argument = snipCommand(inputline); //this is "word", the [word]

                if (command=='W')
                {
                    dict.Add(argument);
                }
                else if (command=='U')
                {
                    /*use binary tree search to find nodes that have the
                     * same contents as argument (Node.content==argument)
                    then delete those nodes
                    */
                }
                else if(command=='S')
                {
                    /*find nodes that have the same contents as "argument" and
                     delete those nodes*/
                }
                else if(command=='L')
                {
                    /*count nodes that start with argument and return them
                     eg. argument="ko" for dict[] = "kot", "kot", "kat" returns 2*/
                }



            }


            String[] words_unprocessed = System.IO.File.ReadAllLines("in1.txt"); //TODO: uncomment once initial input is done

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
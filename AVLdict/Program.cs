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
            char getCommand(String inputline) //getCommand(String) takes a string and returns the first letter of said string + checking if that command exists
            { 
                char firstLetter = inputline[0];
                if ((firstLetter == 'W') || (firstLetter == 'U') || (firstLetter == 'S') || (firstLetter == 'L')) //the possible commands are W U S L which are always on the first letter of the input
                    return firstLetter;
                else return '0'; //if the user tries to run a command that doesnt exist return char '0'
            }
            String snipCommand(String inputline)
            {
                return inputline.Substring(0, inputline.Length);
            }



            //String[] tab = System.IO.File.ReadAllLines("in1.txt"); //TODO: uncomment once initial input is done

            String command = Console.ReadLine();
            Console.WriteLine(snipCommand(command));

        }
    }
}
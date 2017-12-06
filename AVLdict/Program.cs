#define VERBOSE
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

            void resolveStrings(String[] raw_strings)
            {
                char getCommand(String input) //gets the first letter of a string (the command). Given "D elephant" -> returns "D"
                {
                    char firstLetter = input[0];
                    if ((firstLetter == 'W') || (firstLetter == 'U') || (firstLetter == 'S') || (firstLetter == 'L')) //the possible commands are W U S L which are always on the first letter of the input
                        return firstLetter;
                    else return '0'; //if the user tries to run a command that doesnt exist return char '0'
                }

                String snipCommand(String input) //returns the input string with the command cut off like so: "A elephant" -> "elephant"
                {
                    return input.Substring(2, input.Length-2);
                }

#if VERBOSE
                Console.WriteLine(raw_strings[0]);
#endif
                int numberOfStrings = int.Parse(raw_strings[0]);

                for (int i = 1; i <= numberOfStrings; i++) //we start from 1 because there's a number on raw_strings[0], and here we want only words
                {
                    String inputline = raw_strings[i];
                    char command = getCommand(inputline); //this is "A", the [command]
                    String argument = snipCommand(inputline); //this is "elephant", the [word]

#if VERBOSE
                    Console.WriteLine(argument); //debug
#endif



                    /*-------COMMANDS (CALLS TO DICT ARE MADE HERE)-----------------------------------------------------------------------*/

                    if (command == 'W') //the add command
                    {
                        dict.Add(argument);
                    }
                    else if (command == 'U') //the delete command
                    {
                        /*use binary tree search to find nodes that have the
                         * same contents as argument (Node.content==argument)
                        then delete those nodes
                        */
                        dict.Remove(argument);
                    }
                    else if (command == 'S') //the find duplicate strings command
                    {
                        /*find nodes that have the same contents as "argument" and
                        if any found print TAK if not print NIE*/

                        if (dict.IsInTree(argument)) Console.WriteLine("TAK");
                        else Console.WriteLine("NIE");
                    }
                    else if (command == 'L') //the find substring in string command 
                    {
                        /*count nodes that start with argument and return them
                         eg. argument="ko" for dict[] = "kot", "kot", "kat" returns 2*/
                        Console.WriteLine(argument);
                        Console.WriteLine(dict.CountSubstr(argument));
                    }

                }

            }


            String startingDirectory = System.IO.Directory.GetCurrentDirectory(); //get dir the program starts in (usually Debug)
            String testDirectory = System.IO.Directory.GetParent(startingDirectory).Parent.FullName; //take its grandparent (AVLdict/AVLdict)
            testDirectory += "\\tests\\"; //add path to tests folder to it
            //Console.WriteLine(testDirectory);

            /*available tests:
            ll.txt
            rr.txt
            lr.txt
            rl.txt

            Just put one of those in the ReadAllLines(testDirectory+HEREFILENAME) line below, you don't have to specify the directory.
            PDF thing: http://p.wi.pb.edu.pl/sites/default/files/krzysztof-ostrowski/files/drzewa_bst_avl.pdf
            */

            Console.Write("File name: ");
            string consoleinput = Console.ReadLine();

            String[] words_unprocessed;
            try
            {
                words_unprocessed = System.IO.File.ReadAllLines(testDirectory + consoleinput + ".txt");
                resolveStrings(words_unprocessed); //all the magic happens here
            }
            catch (System.IO.FileNotFoundException)
            {
                Console.WriteLine("Nie ma takiego pliku.");
            }

#if VERBOSE
            dict.PrintTree(dict.Root);
            
            string[] commands = new string[2];
            commands[0] = "1";
            while (true)
            {
                Console.Write("Command: ");
                consoleinput = Console.ReadLine();
                commands[1] = consoleinput;
                resolveStrings(commands);
                Console.WriteLine("\n");
                dict.PrintTree(dict.Root);
                Console.WriteLine("\n");
            }
#endif

            /*
            dict.Add("4");
            dict.Add("2");
            dict.Add("7");
            dict.Add("1");
            dict.Add("3");
            dict.Add("5");
            dict.Add("9");
            dict.Add("6");
            //Console.WriteLine(dict.CountSubstr("ma"));
            dict.PrintTree(dict.Root);
            dict.Remove("7");
            dict.PrintTree(dict.Root);
            //dict.Remove("3");
            dict.PrintTree(dict.Root);
            */

            //Console.WriteLine(dict.CountSubstr("ma"));

            //dict.Add("1");
            //dict.Add("11");
            //dict.Add("23");
            //dict.Add("4");
            //dict.Add("5");
            //dict.Add("423");
            //dict.Add("6");
            //dict.Add("12");
        }
    }
}
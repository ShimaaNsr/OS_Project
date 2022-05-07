using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINI_FILE_SYSTEM
{
    internal class parser
    {
        public static void ChackInput(string input)
        {
            string[] input_list = input.Split(' ');
            List<string> ls = new List<string>();
            for (int i = 0; i < input_list.Length; i++)
            {
                if (input_list[i] is not " ")
                {
                    ls.Add(input_list[i]);
                }
            }
            string[] arr = ls.ToArray();
            arr[0] = arr[0].ToLower();
            int cont = arr.Length;
            if (IsArgument(arr[0]) == false)
            {
                Console.WriteLine("ERROR!!!  " + arr[0] + " This command is not fuond in  the commands.");
            }
            else
            {
                if (cont <= 1)
                {
                    CallCommand(arr[0]);
                }
                else if(cont == 2)
                {
                    CallCommand(arr[0], arr[1]);
                }
                else
                {
                    CallCommand(arr[0], arr[1],arr[2]);
                }
            }
        }
        public static bool IsArgument(string arg)
        {
            string[] command = { "cls", "del", "md", "rd", "cd", "help", "dir", "quit", "copy", "rename", "type", "import", "export" };
            for (int i = 0; i < command.Length; i++)
            {
                if (command[i] == arg)
                {
                    return true;
                }
            }
            return false;
        }
        public static void CallCommand(string command = " ", string arg = " ", string arg2 = " ")
        {
            if (command == "help")
            {
                if (arg != " ")
                {
                    Command.Help(arg);
                }
                else
                {
                    Command.Help();
                }
            }
            else if (command == "quit")
            {
                Command.Quit(arg);
            }
            else if (command == "cls")
            {
                Command.Clear(arg);
            }
            else if (command == "md")
            {
                Command.CreateDirectory(arg);
            }
            else if (command == "rd")
            {
                Command.rd(arg);
            }
            else if (command == "cd")
            {
                Command.cd(arg);
            }
            else if (command == "dir")
            {
                Command.Dir();
            }
            else if (command == "rename")
            {
                Command.RENAME(arg, arg2);
            }
            else if (command == "import")
            {
                Command.IMPORT(arg);
            }
        }

    }
}

using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//important
using MINI_FILE_SYSTEM;

namespace MINI_FILE_SYSTEM
{

    public static class Program
    {
        public static Directory current;
        public static string currentPath;
        private static void Main(string[] args)
        {
            Virtual_DISK.InitalizeFile("disk");
            currentPath = new string(current.FileName);
            currentPath = currentPath.Trim(new char[] { '\0', ' ' });
            while (true)
            {
                Console.Write(currentPath + "\\" + ">");
                string input = Console.ReadLine();
                if (input != "")
                {
                    parser.ChackInput(input);
                }
                else
                {
                    continue;
                }

            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINI_FILE_SYSTEM
{
    public static class Command
    {
        public static void Help(string input = "")
        {
            bool F = false;
            string[] commands = { "cd", "help", "dir", "quit", "copy", "cls", "del", "md", "rd", "rename", "type", "import", "export" };
            foreach (string i in commands)
            {
                if (input.ToLower() == i)
                {
                    F = true;
                    break;
                }
            }
            if (input == "")
            {
                Console.WriteLine("cd       - Change the current default directory to .");
                Console.WriteLine("           If the argument is not present, report the current directory.");
                Console.WriteLine("           If the directory does not exist an appropriate error should be reported.");
                Console.WriteLine("cls      - Clear the screen.");
                Console.WriteLine("dir      - List the contents of directory .");
                Console.WriteLine("quit     - Quit the shell.");
                Console.WriteLine("copy     - Copies one or more files to another location");
                Console.WriteLine("del      - Deletes one or more files.");
                Console.WriteLine("help     - Provides Help information for commands.");
                Console.WriteLine("md       - Creates a directory.");
                Console.WriteLine("rd       - Removes a directory.");
                Console.WriteLine("rename   - Renames a file.");
                Console.WriteLine("type     - Displays the contents of a text file.");
                Console.WriteLine("import   – import text file(s) from your computer");
                Console.WriteLine("export   – export text file(s) to your computer");
            }
            else if (input != "" && F)
            {
                if (input == "cd")
                {
                    Console.WriteLine("Change the current default directory to.");
                }
                else if (input == "cls")
                {
                    Console.WriteLine("Clear the screen.");
                }
                else if (input == "dir")
                {
                    Console.WriteLine("List the contents of directory.");
                }
                else if (input == "quit")
                {
                    Console.WriteLine("Quit the shell.");
                }
                else if (input == "copy")
                {
                    Console.WriteLine("Copies one or more files to another location.");
                }
                else if (input == "del")
                {
                    Console.WriteLine("Deletes one or more files.");
                }
                else if (input == "help")
                {
                    Console.WriteLine("Provides Help information for commandes.");
                }
                else if (input == "md")
                {
                    Console.WriteLine("Creates a directory.");
                }
                else if (input == "rd")
                {
                    Console.WriteLine("Removes a directory.");
                }
                else if (input == "rename")
                {
                    Console.WriteLine("Renames a file.");
                }
                else if (input == "type")
                {
                    Console.WriteLine("Displays the contents of a text file.");
                }
                else if (input == "import")
                {
                    Console.WriteLine("import text file(s) from your inputputer ");
                }
                else if (input == "export")
                {
                    Console.WriteLine("export text file(s) to your inputputer ");
                }
            }
            else if (F == false)
            {
                Console.WriteLine("ERROR!!!: " + input + " This inputmand is not supported by the project.");
            }
        }
        public static void Clear(string name = " ")
        {
            if (name == " ")
            {
                Console.Clear();
            }
            else
            {
                Console.WriteLine("ERROR!!! cls command syntax is");
                Console.WriteLine("cls \n function: Clear the screen.");
            }
        }
        public static void Quit(string name = " ")
        {
            if (name == " ")
            {
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Error: quit command syntax is ");
                Console.WriteLine("quit \n function: Quit the shell.");
            }
        }
        public static void rd(string name)
        {
            int index = Program.current.SearchDirectory(name);
            if (index != -1)
            {
                int firstCluster = Program.current.DirectoryTable[index].FirstCluster;
                Directory d1 = new Directory(name, 0x10, firstCluster, Program.current);
                d1.DeleteDirectory();
                Program.currentPath = new string(Program.current.FileName).Trim();
            }
            else
            {
                Console.WriteLine("The system cannot find the path specified.");
            }
        }
        public static void cd(string name)
        {
            int index = Program.current.SearchDirectory(name);

            if (index != -1)
            {
                int firstCluster = Program.current.DirectoryTable[index].FirstCluster;
                Directory d1 = new Directory(name, 0x10, firstCluster, Program.current);
                Program.currentPath = new string(Program.current.FileName).Trim() + "\\" + new string(d1.FileName).Trim();
                Program.current.ReadDirectory();
            }
            else
            {
                Console.WriteLine("The system cannot find the path specified.");
            }
        }
        public static void CreateDirectory(string name = " ")
        {
            if (name != " ")
            {
                if (Program.current.SearchDirectory(name) == -1)
                {
                    if (Fat.GetAvilableBlock() != -1)
                    {
                        DirectoryEntry d = new DirectoryEntry(name, 0x10, 0);
                        Program.current.DirectoryTable.Add(d);
                        Program.current.WriteDirectory();
                        if (Program.current.parent != null)
                        {
                            Program.current.parent.UpdateContent(Program.current.GetDirectoryEntry());
                            Program.current.parent.WriteDirectory();
                        }
                        Fat.WriteFAT();
                    }
                    else
                    {
                        Console.WriteLine("Error : sorry the disk is full!");
                    }
                }
                else
                {
                    Console.WriteLine("Error : this directory \" " + name + "\" is already exists!");
                }
            }
            else
            {
                Console.WriteLine("Error: md command syntax is \n md [directory]\n[directory] can be a new directory name or fullpath of a new directory\nCreates a directory.");
            }
        }
        public static void Dir()
        {
            int file_count = 0;
            int dir_count = 0;
            int files_size = 0;
            Console.WriteLine(" Directory of " + Program.currentPath);
            Console.WriteLine();
            int start = 1;
            if (Program.current.parent != null)
            {
                start = 2;
                Console.WriteLine("\t<DIR>    ");
                dir_count++;
                
            }
            for (int i = start; i < Program.current.DirectoryTable.Count; i++)
            {
                if (Program.current.DirectoryTable[i].FileAttr == 0x0)
                {
                    Console.WriteLine("          " + Program.current.DirectoryTable[i].FileSize + new string(Program.current.DirectoryTable[i].FileName));
                    file_count++;
                    files_size += Program.current.DirectoryTable[i].FileSize;
                }
                else if (Program.current.DirectoryTable[i].FileAttr == 0x10)
                {
                    Console.WriteLine("      <DIR>    " + new string(Program.current.DirectoryTable[i].FileName));
                    dir_count++;
                }
            }
            Console.WriteLine("              " + file_count + " File(s)" + files_size + "bytes");
            Console.WriteLine("              " + dir_count + " Dir(s)" + Virtual_DISK.GetFreeSpace() + "bytes free");
        }
        public static void EXPORT(string source, string destination)
        {
            int index = Program.current.SearchDirectory(source);
            if (index != -1)
            {
                if (System.IO.Directory.Exists(destination))
                {
                    int f_c = Program.current.DirectoryTable[index].FirstCluster;
                    int si = Program.current.DirectoryTable[index].FileSize;
                    string content = null;
                    FileEntry f = new FileEntry(source.ToCharArray(), 0, f_c, si, content, Program.current);
                    f.Read();
                    StreamWriter s = new StreamWriter(destination + "\\" + source);
                    s.Write(f.file_content);
                    s.Flush();
                    s.Close();
                }
                else
                {
                    Console.WriteLine("system cannot find the file specified");
                }
            }
            else
            {
                Console.WriteLine("File Not exist");
            }
        }
        public static void IMPORT(string file_path)
        {
            if (File.Exists(file_path))
            {
                string content = File.ReadAllText(file_path);
                int size = content.Length;
                int name_start = file_path.LastIndexOf("\\");
                string name;
                name = file_path.Substring(name_start + 1);
                int index = Program.current.SearchDirectory(name);
                if (index == -1)
                {
                    int f_c;
                    if (size > 0)
                        f_c = Fat.GetAvilableBlock();
                    else
                        f_c = 0;
                    FileEntry f = new FileEntry(name.ToCharArray(), 0, 0, size, content, Program.current);
                    f.Write();
                    DirectoryEntry d = new DirectoryEntry(name.ToCharArray(), 0, 0, size);
                    Program.current.DirectoryTable.Add(d);
                    Program.current.WriteDirectory();
                }
                else
                    Console.WriteLine("already exist in root");

            }
            else
            {
                Console.WriteLine("file not exist ");
            }
        }
        public static void TYPE(string name)
        {
            int index = Program.current.SearchDirectory(name);
            if (index != -1)
            {
                int f_c = Program.current.DirectoryTable[index].FirstCluster;
                int size = Program.current.DirectoryTable[index].FileSize;
                string content = null;
                FileEntry f = new FileEntry(name.ToCharArray(), 0, f_c, size, content, Program.current);
                f.Read();
                Console.WriteLine(f.file_content);
            }
            else
            {
                Console.WriteLine("system can't find file");
            }
        }
        public static void RENAME(string old_name, string new_name)
        {
            int index = Program.current.SearchDirectory(old_name);
            if (index != -1)
            {
                int n = Program.current.SearchDirectory(new_name);
                if (n == -1)
                {
                    DirectoryEntry f = Program.current.DirectoryTable[index];
                    f.FileName = new_name.ToCharArray();
                    Program.current.UpdateContent(f);

                }
                else
                {
                    Console.WriteLine("dublicate file name");
                }

            }
            else
            {
                Console.WriteLine("system cannot find the file specified");
            }
        }
    }
}

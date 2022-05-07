using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINI_FILE_SYSTEM
{
    internal class Virtual_DISK
    {
        public static FileStream DISK;
        public static void InitalizeFile(string path)
        {
            if (!File.Exists(path))
            {
                CreateOrOpenFile(path);
                byte[] b = new byte[1024];
                for (int i = 0; i < b.Length; i++)
                {
                    b[i] = 0;
                }
                WriteCluster(b, 0);
                Fat.InitalizeFat();
                Directory root = new Directory("S:", 0x10, 5, null);
                root.WriteDirectory();
                Fat.SetNext(5, -1);
                Program.current = root;
                Fat.WriteFAT();
            }
            else
            {
                CreateOrOpenFile(path);
                Fat.ReadFAT();
                Directory root = new Directory("S:", 0x10, 5, null);
                root.ReadDirectory();
                Program.current = root;
            }

        }
        public static void CreateOrOpenFile(string path)
        {
            DISK = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        }
        public static void WriteCluster(byte[] block, int Index, int x = 0, int count = 1024)
        {
            DISK.Seek(Index * 1024, SeekOrigin.Begin);
            DISK.Write(block, x, count);
            DISK.Flush();
        }
        public static int GetFreeSpace()
        {
            return (1024 * 1024) - (int)DISK.Length;
        }
        public static byte[] ReadCluster(int Index)
        {
            DISK.Seek(Index * 1024, SeekOrigin.Begin);
            byte[] bytes = new byte[1024];
            DISK.Read(bytes, 0, 1024);
            return bytes;
        }
    }
}

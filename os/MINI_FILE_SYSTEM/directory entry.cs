using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINI_FILE_SYSTEM
{
    public class DirectoryEntry
    {
        public char[] FileName = new char[11];
        public byte FileAttr;
        public byte[] FileEmpty = new byte[12];
        public int FirstCluster;
        public int FileSize;
        private char[] vs;
        private int v1;
        private int v2;
        private int size;

        public DirectoryEntry()
        {

        }
        public DirectoryEntry(string FileName, byte FileAttr, int FirstCluster)
        {
            this.FileAttr = FileAttr;
            if (FileAttr == 0x0)
            {
                string[] fileName = FileName.Split('.');
                FileNameExtension(fileName[0].ToCharArray(), fileName[1].ToCharArray());
            }
            else if (FileAttr == 0x10)
            {

                FileNameNoExtention(FileName.ToCharArray());
            }
            this.FirstCluster = FirstCluster;
        }

        public DirectoryEntry(char[] vs, int v1, int v2, int size)
        {
            this.vs = vs;
            this.v1 = v1;
            this.v2 = v2;
            this.size = size;
        }

        public void FileNameExtension(char[] name, char[] extension)
        {
            if (name.Length <= 7 && extension.Length == 3)
            {
                int j = 0;
                for (int i = 0; i < name.Length; i++)
                {
                    j++;
                    this.FileName[i] = name[i];
                }
                j++;
                this.FileName[j] = '.';
                for (int i = 0; i < extension.Length; i++)
                {
                    j++;
                    this.FileName[j] = extension[i];
                }
                for (int i = ++j; i < FileName.Length; i++)
                {
                    this.FileName[i] = ' ';
                }
            }
            else
            {
                for (int i = 0; i < 7; i++)
                {
                    this.FileName[i] = name[i];
                }
                this.FileName[7] = '.';
                for (int i = 0, j = 8; i < 4; i++, j++)
                {
                    this.FileName[j] = extension[i];
                }
            }
        }
        public void FileNameNoExtention(char[] name)
        {
            if (name.Length <= 11)
            {
                int j = 0;
                for (int i = 0; i < name.Length; i++)
                {
                    j++;
                    this.FileName[i] = name[i];
                }
                for (int i = ++j; i < FileName.Length; i++)
                {
                    this.FileName[i] = ' ';
                }
            }
            else
            {
                for (int i = 0; i < 11; i++)
                {
                    this.FileName[i] = name[i];
                }
            }
        }
    }
}

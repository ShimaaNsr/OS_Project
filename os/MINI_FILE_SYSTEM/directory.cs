using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINI_FILE_SYSTEM
{
    public class Directory : DirectoryEntry
    {
        public List<DirectoryEntry> DirectoryTable;
        public Directory parent;
        public Directory(string filename, byte fileattr, int firstCluster, Directory parant) : base(filename, fileattr, firstCluster)
        {
            if (parant != null)
            {
                this.parent = parant;
            }
        }
        public void UpdateContent(DirectoryEntry d)
        {
            int index = SearchDirectory(new string(d.FileName));
            if (index != -1)
            {
                DirectoryTable.RemoveAt(index);
                DirectoryTable.Insert(index, d);
            }
        }
        public DirectoryEntry GetDirectoryEntry()
        {
            DirectoryEntry d = new DirectoryEntry(new string(this.FileName), this.FileAttr, this.FirstCluster);
            return d;
        }
        public void WriteDirectory()
        {
            byte[] dirsorfilesBYTES = new byte[DirectoryTable.Count * 32];
            for (int i = 0; i < DirectoryTable.Count; i++)
            {
                byte[] b = covert_data.DirectoryEntryToBytes(this.DirectoryTable[i]);
                for (int j = i * 32, k = 0; k < b.Length; k++, j++)
                {
                    dirsorfilesBYTES[j] = b[k];
                }
            }
            List<byte[]> bytesls = covert_data.SplitBytesToBlocks(dirsorfilesBYTES);
            int clusterFATIndex;
            if (this.FirstCluster != 0)
            {
                clusterFATIndex = this.FirstCluster;
            }
            else
            {
                clusterFATIndex = Fat.GetAvilableBlock();
                this.FirstCluster = clusterFATIndex;
            }
            int lastCluster = -1;
            for (int i = 0; i < bytesls.Count; i++)
            {
                if (clusterFATIndex != -1)//DISK has space
                {
                    Virtual_DISK.WriteCluster(bytesls[i], clusterFATIndex, 0, bytesls[i].Length);
                    Fat.SetNext(clusterFATIndex, -1);//temp
                    if (lastCluster != -1)//
                    {
                        Fat.SetNext(lastCluster, clusterFATIndex);
                    }

                    lastCluster = clusterFATIndex;
                    clusterFATIndex = Fat.GetAvilableBlock();
                }
            }
            if (this.parent != null)
            {
                this.parent.UpdateContent(this.GetDirectoryEntry());//since fc changes from 0 to the used fc for example
                this.parent.WriteDirectory();
            }
            Fat.WriteFAT();
        }

        public void ReadDirectory()
        {
            if (this.FirstCluster != 0)
            {
                DirectoryTable = new List<DirectoryEntry>();
                int cluster = this.FirstCluster;
                int next = Fat.GetNext(cluster);
                List<byte> ls = new List<byte>();//1024*count/32 bytes
                do
                {
                    ls.AddRange(Virtual_DISK.ReadCluster(cluster));
                    cluster = next;
                    if (cluster != -1)// not last
                    {
                        next = Fat.GetNext(cluster);
                    }
                }
                while (next != -1);
                for (int i = 0; i < ls.Count; i++)
                {
                    byte[] b = new byte[32];
                    for (int k = i * 32, m = 0; m < b.Length && k < ls.Count; m++, k++)
                    {
                        b[m] = ls[k];
                    }
                    if (b[0] == 0)
                    {
                        break;
                    }

                    DirectoryTable.Add(covert_data.BytesToDirectoryEntry(b));
                }
            }
        }
        public void DeleteDirectory()
        {
            if (this.FirstCluster != 0)
            {
                int index = this.FirstCluster;
                int next = Fat.GetNext(index);
                do
                {
                    Fat.SetNext(index, 0);
                    index = next;
                    if (index != -1)
                        next = Fat.GetNext(index);
                }
                while (index != -1);
            }
            if (this.parent != null)
            {
                int index = this.parent.SearchDirectory(new string(this.FileName));
                if (index != -1)
                {
                    this.parent.DirectoryTable.RemoveAt(index);
                    this.parent.WriteDirectory();
                }
            }
            if (Program.current == this)
            {
                if (this.parent != null)
                {
                    Program.current = this.parent;
                    Program.currentPath = Program.currentPath.Substring(0, Program.currentPath.LastIndexOf('\\'));
                    Program.current.ReadDirectory();
                }
            }
            Fat.WriteFAT();
        }
        public int SearchDirectory(string name)
        {
            if (name.Length < 11)
            {
                name += "\0";
                for (int i = name.Length + 1; i < 12; i++)
                {
                    name += " ";
                }
            }
            else
            {
                name = name.Substring(0, 11);
            }
            for (int i = 0; i < DirectoryTable.Count; i++)
            {
                string Name = new(DirectoryTable[i].FileName);
                if (Name == name)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}

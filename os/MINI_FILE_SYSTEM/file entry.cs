using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINI_FILE_SYSTEM
{
    public class FileEntry : DirectoryEntry
    {
        public string file_content;
        public Directory parent;
        private char[] vs;
        private int v;
        private int f_c;
        private int si;
        private string? content;
        private Directory current;

        public FileEntry(string name, byte FileAttr, int FirstCluster, Directory parent) :
        base(name, FileAttr, FirstCluster)
        {
            file_content = string.Empty;
            if (parent != null)
                this.parent = parent;
        }

        public FileEntry(char[] vs, int v, int f_c, int si, string? content, Directory current)
        {
            this.vs = vs;
            this.v = v;
            this.f_c = f_c;
            this.si = si;
            this.content = content;
            this.current = current;
        }

        public DirectoryEntry GetDirectoryEntry()
        {
            DirectoryEntry de = new DirectoryEntry(new string(this.FileName), this.FileAttr, this.FirstCluster);
            return de;
        }
        public void Write()
        {
            byte[] content_byte = covert_data.StringToBytes(file_content);
            List<byte[]> bytesls = covert_data.SplitBytesToBlocks(content_byte);
            int first_cluster;
            if (this.FirstCluster == 0)
            {
                first_cluster = Fat.GetAvilableBlock();
                this.FirstCluster = first_cluster;
            }
            else
            {
                first_cluster = this.FirstCluster;
            }
            int lastCluster = -1;
            for (int i = 0; i < bytesls.Count; i++)
            {
                if (first_cluster != -1)
                {
                    Virtual_DISK.WriteCluster(bytesls[i], first_cluster, 0, bytesls[i].Length);
                    Fat.SetNext(first_cluster, -1);
                    if (lastCluster != -1)
                        Fat.SetNext(lastCluster, first_cluster);
                    lastCluster = first_cluster;
                    first_cluster = Fat.GetAvilableBlock();
                }
            }
        }
        public void Read()
        {
            if (this.FirstCluster != 0)
            {
                file_content = string.Empty;
                int first_cluster = this.FirstCluster;
                int next = Fat.GetNext(first_cluster);
                List<byte> ls = new List<byte>();
                do
                {
                    ls.AddRange(Virtual_DISK.ReadCluster(first_cluster));
                    first_cluster =next;
                    if (first_cluster != -1)
                        next = Fat.GetNext(first_cluster);
                }
                while (next != -1);
                file_content = Convert.ToString(ls.ToArray());
            }
        }
        public void Delete()
        {
            if (this.FirstCluster != 0)
            {
                int first_cluster = this.FirstCluster;
                int next = Fat.GetNext(first_cluster);
                do
                {
                    Fat.SetNext(first_cluster, 0);
                    first_cluster = next;
                    if (first_cluster != -1)
                        next = Fat.GetNext(first_cluster);
                }
                while (first_cluster != -1);
            }

        }
    }
}

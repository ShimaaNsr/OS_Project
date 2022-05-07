using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINI_FILE_SYSTEM
{
    internal class covert_data
    {
        public static byte[] ToBytes(int[] array)
        {
            byte[] Bytes = null;
            Bytes = new byte[array.Length * 4];
            System.Buffer.BlockCopy(array, 0, Bytes, 0, Bytes.Length);
            return Bytes;
        }
        public static byte[] StringToBytes(string s)
        {
            byte[] bytes = new byte[s.Length];
            for (int i = 0; i < s.Length; i++)
            {
                bytes[i] = (byte)s[i];
            }
            return bytes;
        }
        public static string BytesToString(byte[] bytes)
        {
            string s = string.Empty;
            for (int i = 0; i < bytes.Length; i++)
            {
                if ((char)bytes[i] != '\0')
                {
                    s += (char)bytes[i];
                }
                else
                {
                    break;
                }
            }
            return s;
        }
        public static byte[] DirectoryEntryToBytes(DirectoryEntry d)
        {
            byte[] bytes = new byte[32];
            for (int i = 0; i < d.FileName.Length; i++)
            {
                bytes[i] = (byte)d.FileName[i];
            }
            bytes[11] = d.FileAttr;
            int j = 12;
            for (int i = 0; i < d.FileEmpty.Length; i++)
            {
                bytes[j] = d.FileEmpty[i];
                j++;
            }
            byte[] fc = BitConverter.GetBytes(d.FirstCluster);
            for (int i = 0; i < fc.Length; i++)
            {
                bytes[j] = fc[i];
                j++;
            }
            byte[] sz = BitConverter.GetBytes(d.FileSize);
            for (int i = 0; i < sz.Length; i++)
            {
                bytes[j] = sz[i];
                j++;
            }
            return bytes;
        }


        public static DirectoryEntry BytesToDirectoryEntry(byte[] bytes)
        {
            char[] name = new char[11];
            for (int i = 0; i < name.Length; i++)
            {
                name[i] = (char)bytes[i];
            }
            byte attr = bytes[11];
            byte[] empty = new byte[12];
            int j = 12;
            for (int i = 0; i < empty.Length; i++)
            {
                empty[i] = bytes[j];
                j++;
            }
            byte[] fc = new byte[4];
            for (int i = 0; i < fc.Length; i++)
            {
                fc[i] = bytes[j];
                j++;
            }
            int firstcluster = BitConverter.ToInt32(fc, 0);
            byte[] sz = new byte[4];
            for (int i = 0; i < sz.Length; i++)
            {
                sz[i] = bytes[j];
                j++;
            }
            int filesize = BitConverter.ToInt32(sz, 0);
            DirectoryEntry d = new DirectoryEntry(new string(name), attr, firstcluster);
            d.FileEmpty = empty;
            d.FileSize = filesize;
            return d;
        }
        public static byte[] ToBytes(List<DirectoryEntry> array)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                bformatter.Serialize(stream, array);
                byte[] arr = stream.ToArray();
                return arr;
            }
        }
        public static int[] ToInt(byte[] bytes)
        {
            int[] ints = null;
            ints = new int[bytes.Length / sizeof(int)];
            System.Buffer.BlockCopy(bytes, 0, ints, 0, bytes.Length);
            return ints;
        }
        public static List<DirectoryEntry> ToDirectoryEntry(byte[] bytes)
        {
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                _ = stream.Seek(0, SeekOrigin.Begin);
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                List<DirectoryEntry> ls = ((List<object>)bformatter.Deserialize(stream)).Cast<DirectoryEntry>().ToList();
                return ls;
            }
        }
        public static List<byte[]> SplitBytesToBlocks(byte[] bytes)
        {
            List<byte[]> lest = new List<byte[]>();
            int number_block = bytes.Length / 1024;
            int remender = bytes.Length % 1024;
            for (int i = 0; i < number_block; i++)
            {
                byte[] arr = new byte[1024];
                for (int j = i * 1024, k = 0; k < 1024; j++, k++)
                {
                    arr[k] = bytes[j];
                }
                lest.Add(arr);
            }
            if (remender > 0)
            {
                byte[] arr2 = new byte[1024];
                for (int i = number_block * 1024, k = 0; k < remender; i++, k++)
                {
                    arr2[k] = bytes[i];
                }
                lest.Add(arr2);
            }
            return lest;
        }
    }
}

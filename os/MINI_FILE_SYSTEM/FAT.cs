using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINI_FILE_SYSTEM
{
    internal class Fat
    {
        public static int[] FAT = new int[1024];

        public static void InitalizeFat()
        {
            for (int i = 0; i < FAT.Length; i++)
            {
                if (i == 0 || i == 4)
                {
                    FAT[i] = -1;
                }
                else if (i >= 1 && i <= 3)
                {
                    FAT[i] = i + 1;
                }
                else
                {
                    FAT[i] = 0;
                }
            }
        }

        public static void WriteFAT()
        {
            byte[] FATBYTES = covert_data.ToBytes(FAT);
            List<byte[]> lest = covert_data.SplitBytesToBlocks(FATBYTES);

            for (int i = 0; i < lest.Count; i++)
            {
                Virtual_DISK.WriteCluster(lest[i], i + 1, 0, lest[i].Length);
            }
        }
        public static void ReadFAT()
        {
            List<byte> lest = new();
            for (int i = 1; i < 5; i++)
            {
                lest.AddRange(Virtual_DISK.ReadCluster(i));
            }
            FAT = covert_data.ToInt(lest.ToArray());
        }
        public static void PrintFat()
        {
            for (int i = 0; i < FAT.Length; i++)
            {
                Console.WriteLine("FAT TABLE[" + i + "] =  " + FAT[i]);
            }
        }
        public static void SetFAT(int[] arr)
        {
            if (arr.Length <= 1024)
            {
                FAT = arr;
            }
        }
        public static int GetAvilableBlock()
        {
            for (int i = 0; i < FAT.Length; i++)
            {
                if (FAT[i] == 0)
                {
                    return i;
                }
            }
            return -1;
        }
        public static int GetAvilableBlocks()
        {
            int cont = 0;
            for (int i = 0; i < FAT.Length; i++)
            {
                if (FAT[i] == 0)
                {
                    cont++;
                }
            }
            return cont;
        }
        public static void SetNext(int Index, int Next)
        {
            FAT[Index] = Next;
        }
        public static int GetNext(int Index)
        {
            return Index >= 0 && Index < FAT.Length ? FAT[Index] : -1;
        }
    }
}

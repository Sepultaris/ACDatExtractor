using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ACDatExtractor
{
    internal class DatHeader
    {
        public uint FileType { get; private set; }
        public uint BlockSize { get; private set; }
        public uint FileSize { get; private set; }
        public uint  DataSet { get; private set; }
        public uint DataSubset { get; private set; }

        public uint FreeHead { get; private set; }
        public uint FreeTail { get; private set; }
        public uint FreeCount { get; private set; }
        public uint BTree { get; private set; }

        public uint NewLRU { get; private set; }
        public uint OldLRU { get; private set; }
        public bool UseLRU { get; private set; }

        public uint MasterMapID { get; private set; }

        public uint EnginePackVersion { get; private set; }
        public uint GamePackVersion { get; private set; }
        public byte[] VersionMajor { get; private set; } = new byte[16];
        public uint VersionMinor { get; private set; }

        public DatHeader(BinaryReader reader)
        {
            FileType = reader.ReadUInt32();
            BlockSize = reader.ReadUInt32();
            FileSize = reader.ReadUInt32();
            DataSet = reader.ReadUInt32();
            DataSubset = reader.ReadUInt32();

            FreeHead = reader.ReadUInt32();
            FreeTail = reader.ReadUInt32();
            FreeCount = reader.ReadUInt32();
            BTree = reader.ReadUInt32();

            NewLRU = reader.ReadUInt32();
            OldLRU = reader.ReadUInt32();
            UseLRU = (reader.ReadUInt32() == 1);

            MasterMapID = reader.ReadUInt32();

            EnginePackVersion = reader.ReadUInt32();
            GamePackVersion = reader.ReadUInt32();
            VersionMajor = reader.ReadBytes(16);
            VersionMinor = reader.ReadUInt32();
        }

        public void Print()
        {
            Console.WriteLine("FileType: {0}", FileType);
            Console.WriteLine("BlockSize: {0}", BlockSize);
            Console.WriteLine("FileSize: {0}", FileSize);
            Console.WriteLine("DataSet: {0}", DataSet);
            Console.WriteLine("DataSubset: {0}", DataSubset);
            Console.WriteLine("FreeHead: {0}", FreeHead);
            Console.WriteLine("FreeTail: {0}", FreeTail);
            Console.WriteLine("FreeCount: {0}", FreeCount);
            Console.WriteLine("BTree: {0}", BTree);
            Console.WriteLine("NewLRU: {0}", NewLRU);
            Console.WriteLine("OldLRU: {0}", OldLRU);
            Console.WriteLine("UseLRU: {0}", UseLRU);
            Console.WriteLine("MasterMapID: {0}", MasterMapID);
            Console.WriteLine("EnginePackVersion: {0}", EnginePackVersion);
            Console.WriteLine("GamePackVersion: {0}", GamePackVersion);
            Console.WriteLine("VersionMajor: {0}", Encoding.ASCII.GetString(VersionMajor));
            Console.WriteLine("VersionMinor: {0}", VersionMinor);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ACDatExtractor
{
    internal class DatDirectory
    {
        private static readonly int MaxBranches = 0x3E;

        private DatDatabase datDatabase;
        private uint[] branches = new uint[MaxBranches];
        private DatDirectory[] subdirectories = new DatDirectory[MaxBranches - 1];
        private uint entryCount;
        private uint sector;

        public DatDirectory(DatDatabase datFile, uint offset, int level)
        {
            this.datDatabase = datFile;
            this.sector = offset;
            Console.WriteLine(new String(' ', level) + "[" + offset + "]");
            buildTree(level);
        }
 
        public void buildTree(int level)
        {
            uint offset = 0;
            for (int i = 0; i < branches.Length; i++)
            {
                branches[i] = BitConverter.ToUInt32(datDatabase.Read(sector, offset, 4));
                offset += 4;
            }
            entryCount = BitConverter.ToUInt32(datDatabase.Read(sector, offset, 4));
            offset += 4;
            for (uint i = 0; i < entryCount; i++)
            {
                var bitFlags = BitConverter.ToUInt32(datDatabase.Read(sector, offset, 4));
                offset += 4;
                var objectId = BitConverter.ToUInt32(datDatabase.Read(sector, offset, 4));
                offset += 4;
                var fileOffset = BitConverter.ToUInt32(datDatabase.Read(sector, offset, 4));
                offset += 4;
                var fileSize = BitConverter.ToUInt32(datDatabase.Read(sector, offset, 4));
                offset += 4;
                var date = BitConverter.ToUInt32(datDatabase.Read(sector, offset, 4));
                offset += 4;
                var iteration = BitConverter.ToUInt32(datDatabase.Read(sector, offset, 4));
                offset += 4;
                File.WriteAllBytes(objectId.ToString("X8"), datDatabase.Read(fileOffset, 0, (int)fileSize));
                Console.WriteLine(new String(' ', level) + objectId + "\t" + fileSize + "\t" + bitFlags);
            }

            if (branches[0] != 0)
            {
                for (int i = 0; i < entryCount + 1; i++)
                {
                    subdirectories[i] = new DatDirectory(datDatabase, branches[i], level + 1);
                }
            }

        }
    }
}

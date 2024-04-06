using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACDatExtractor
{

    /*
     * A class that encapsulates low level interaction with the .dat file.
     * Provides a Read() method to read from sector aligned offsets.
     */

    internal class DatDatabase
    {
        private readonly int headerOffest = 0x140;

        public FileStream datFileStream;
        public DatHeader header;

        public DatDatabase(FileStream fileStream)
        {
            datFileStream = fileStream;
            datFileStream.Seek(headerOffest, SeekOrigin.Begin);
            using (var reader = new BinaryReader(datFileStream, System.Text.Encoding.Default, true))
            {
                header = new DatHeader(reader);
            }
        }

        public byte[] ReadRaw(uint offset, int size)
        {
            var buffer = new byte[size];
            datFileStream.Seek(offset, SeekOrigin.Begin);
            datFileStream.Read(buffer, 0, size);
            return buffer;
        }

        public byte[] Read(uint sector, uint offset, int size)
        {
            var buffer = new byte[size];
            var bufferOffset = 0;

            while (offset >= header.BlockSize - 4)
            {
                datFileStream.Seek(sector, SeekOrigin.Begin);
                sector = GetNextAddress();
                offset -= (uint)header.BlockSize - 4;
            }

            while (size > 0)
            {
                var bytesInCurrentBlock = (int)(header.BlockSize - 4 - offset);

                datFileStream.Seek(sector + offset, SeekOrigin.Begin);
                sector = GetNextAddress();

                if (size <= bytesInCurrentBlock)
                {
                    datFileStream.Read(buffer, bufferOffset, size);
                    bufferOffset += size;
                    size = 0;
                }
                else
                {
                    datFileStream.Read(buffer, bufferOffset, bytesInCurrentBlock);
                    bufferOffset += bytesInCurrentBlock;
                    size -= bytesInCurrentBlock;
                    offset = 0;
                }
            }
            return buffer;
        }

        private uint GetNextAddress()
        {
            byte[] nextAddressBytes = new byte[4];
            datFileStream.Read(nextAddressBytes, 0, 4);
            return BitConverter.ToUInt32(nextAddressBytes, 0);
        }
    }
}

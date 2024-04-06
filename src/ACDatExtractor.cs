using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ACDatExtractor
{
    public class ACDatExtractor
    {
        public static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: ACDatExtractor <client_portal.dat>");
                return;
            }

            var fileName = args[0];
            var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            var datFile = new DatDatabase(fileStream);
            datFile.header.Print();
            var datDirectory = new DatDirectory(datFile, datFile.header.BTree, 0);
        }
    }
}

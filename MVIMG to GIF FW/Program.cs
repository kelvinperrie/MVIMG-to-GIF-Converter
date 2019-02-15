using NReco.VideoConverter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVIMG_to_GIF_FW
{
    class Program
    {

        static void Main(string[] args)
        {
            var fileName = "test-mvimg1.jpg";

            Console.WriteLine($"Exciting news, I'm about to convert file {fileName}");

            var outputPath = MvimgConverter.DoIt(fileName);

            Console.WriteLine($"More exciting news, I converted file {fileName} to {outputPath}");

            Console.WriteLine("Enjoy your humongous unoptimized GIF");

            Console.ReadKey();
        }
    }
}

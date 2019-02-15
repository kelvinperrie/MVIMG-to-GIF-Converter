using NReco.VideoConverter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVIMG_to_GIF_FW
{
    public static class MvimgConverter
    {
        public static string DoIt(string sourceFile)
        {
            // error handling just slows me down

            string currentDirectory = Path.GetDirectoryName( Path.GetFullPath(sourceFile) );
            string baseFileName = Path.GetFileNameWithoutExtension(sourceFile);

            string outputPath = Path.Combine(currentDirectory, baseFileName + ".gif");
            string mp4OutputPath = Path.Combine(currentDirectory, baseFileName + ".mp4");


            var contentsBytes = File.ReadAllBytes(sourceFile);

            var hexHeader = "00 00 00 18 66 74 79 70 6D 70 34 32";

            var test = StringToByteArray(hexHeader.Replace(" ", ""));

            var foundIndex = SearchBytePattern(test, contentsBytes);

            var newArray = new byte[contentsBytes.Length - foundIndex];

            Array.Copy(contentsBytes, foundIndex, newArray, 0, contentsBytes.Length - foundIndex);

            using (var fileStream = new FileStream(mp4OutputPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                fileStream.Write(newArray, 0, newArray.Length);
            }

            var ffMpeg = new FFMpegConverter();
            ffMpeg.ConvertMedia(mp4OutputPath, outputPath, Format.gif);

            File.Delete(mp4OutputPath);

            return outputPath;
        }

        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        // https://stackoverflow.com/questions/283456/byte-array-pattern-search
        public static int SearchBytePattern(byte[] pattern, byte[] bytes)
        {
            //List<int> positions = new List<int>();
            int patternLength = pattern.Length;
            int totalLength = bytes.Length;
            byte firstMatchByte = pattern[0];
            for (int i = 0; i < totalLength; i++)
            {
                if (firstMatchByte == bytes[i] && totalLength - i >= patternLength)
                {
                    byte[] match = new byte[patternLength];
                    Array.Copy(bytes, i, match, 0, patternLength);
                    if (match.SequenceEqual<byte>(pattern))
                    {
                        return i;
                        //positions.Add(i);
                        //i += patternLength - 1;
                    }
                }
            }
            return -1; // positions;
        }
    }
}

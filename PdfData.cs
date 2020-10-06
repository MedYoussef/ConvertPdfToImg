using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace ConvertPdfToImg
{
    public static class PdfData
    {
        public static string GetStream(string path)
        {
            using (FileStream fs = File.OpenRead(path))
            {
                using (StreamReader reader = new StreamReader(fs, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public static List<string> GetObjectsAsString(string stream)
        {

            List<string> result = new List<string>();
            result = stream.ExtractBetween("obj", "endobj");
            return result;

        }

        //public static List<string> GetDecoderFromObject()

        public static string ExtractDecoderFromObject(string pdfObject)
        {
            string metadata = pdfObject.ExtractBetween("<<", ">>")[0];

            var array = metadata
                .Split('/', StringSplitOptions.RemoveEmptyEntries)
                .Select(x=>x.Trim()).ToArray();

            var i = Array.FindIndex(array, w => w.Equals("Filter"));

            return i >= 0 ? array[i+1] : "NoFilter";
        }


        public static List<string> ExtractBetween(
            this string source,
            string start,
            string end)
        {
            int indexStart = 0;
            int indexEnd = 0;
            List<string> matched = new List<string>();
            bool exit = false;
            while (!exit)
            {
                indexStart = source.IndexOf(start);

                if (indexStart != -1)
                {
                    indexEnd = indexStart + source.Substring(indexStart).IndexOf(end);

                    matched.Add(source.Substring(indexStart + start.Length, indexEnd - indexStart - start.Length).Trim());

                    source = source.Substring(indexEnd + end.Length);
                }
                else
                {
                    exit = true;
                }
            }

            return matched;
        }
    }

}

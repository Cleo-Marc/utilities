using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using PdfGen;

namespace PdfGenTestDriver
{
    internal class Program
    {
        private const string outputFile = @"c:\users\tony\documents\pdfgen\pdfgentest.pdf";
        private const string backgroundFile = @"c:\users\tony\documents\pdfgen\letterhead.png";

        static void Main(string[] args)
        {
            var output = new PdfOutput();
            output.Background = backgroundFile;
            string[] lines = new string[]
            {
                "Line 1",
                "Line 2",
                "Line 3",
                "Line 4"
            };

            output.NewPage();
            output.AddText(lines, 10, 50);
            output.Save(outputFile);
        }
    }
}

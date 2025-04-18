using System;

using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace PdfGen
{
    internal sealed class ExecContext : IDisposable
    {
        private XGraphics graphics;
        private readonly PdfDocument document;

        internal ExecContext()
        {
            document = new PdfDocument();
            document.PageLayout = PdfPageLayout.SinglePage;
        }

        public void Dispose()
        {
            graphics?.Dispose();
            document.Dispose();
        }

        internal string FontName
        {
            get;
            set;
        }

        internal int FontSize
        {
            get; set;
        }

        internal XFontStyleEx FontStyle
        {
            get; set;
        }


        internal PdfDocument Document => document;

        internal Margins Margins
        {
            get; set;
        }

        internal string Background
        {
            get; set;
        }

        internal PdfPage Page
        {
            get
            {
                return document.Pages[document.PageCount - 1];
            }
        }

        internal XGraphics Graphics
        {
            get
            {
                return graphics;
            }

            set
            {
                graphics?.Dispose();
                graphics = value;
            }
        }

        internal XPoint NextLineStart
        {
            get; set;
        }
    }
}

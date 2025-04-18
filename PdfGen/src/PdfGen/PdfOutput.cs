using System;

using PdfSharp.Drawing;

namespace PdfGen
{
    public sealed class PdfOutput : IDisposable
    {
        private const string defaultFontName = "Courier New";
        private const int defaultFontSize = 12;
        private const string defaultFontStyle = "n";

        private readonly ExecContext execContext;

        public PdfOutput()
        {
            execContext = new ExecContext();
            FontName = defaultFontName;
            FontSize = defaultFontSize;
            FontStyle = defaultFontStyle;
            Margins = new int[] { 0, 0, 0, 0 };
        }

        public void Dispose()
        {
            execContext.Dispose();
        }

        public string Background
        {
            get => execContext.Background;
            set => execContext.Background = value;
        }

        public int[] Margins
        {
            get
            {
                Margins margins = execContext.Margins;
                return new int[] { margins.Left, margins.Right, margins.Top, margins.Bottom };
            }

            set
            {
                if (value.Length != 4)
                {
                    throw new ArgumentException("Margins must be an array of 4 integers");
                }

                execContext.Margins = new Margins()
                {
                    Left = value[0],
                    Right = value[1],
                    Top = value[2],
                    Bottom = value[3]
                };
            }
        }

        public string FontName
        {
            get => execContext.FontName;
            set => execContext.FontName = value;
        }

        public int FontSize
        {
            get => execContext.FontSize;
            set => execContext.FontSize = value;
        }

        public string FontStyle
        {
            get
            {
                switch (execContext.FontStyle)
                {
                    case XFontStyleEx.Bold:
                        return "b";
                    case XFontStyleEx.Italic:
                        return "i";
                    case XFontStyleEx.BoldItalic:
                        return "bi";
                    default:
                        return "n";
                }
            }

            set
            {
                switch (value.ToLower())
                {
                    case "n":
                        execContext.FontStyle = XFontStyleEx.Regular;
                        break;
                    case "b":
                        execContext.FontStyle = XFontStyleEx.Bold;
                        break;
                    case "i":
                        execContext.FontStyle |= XFontStyleEx.Italic;
                        break;
                    case "bi":
                        execContext.FontStyle = XFontStyleEx.BoldItalic;
                        break;
                    default:
                        throw new ArgumentException($"Invalid font style '{value}'");
                }
            }
        }

        public void NewPage()
        {
            var page = execContext.Document.AddPage();
            page.Size = PdfSharp.PageSize.A4;
            page.Orientation = PdfSharp.PageOrientation.Portrait;
            var graphics = XGraphics.FromPdfPage(page, XGraphicsUnit.Millimeter);
            execContext.Graphics = graphics;
            var mapper = new CoordinateMapper(execContext);
            DrawBackground(mapper);
        }

        public void AddText(string[] text, int xmm, int ymm)
        {
            var fontSize = XUnit.FromPoint(execContext.FontSize);
            var font = new XFont(execContext.FontName, fontSize.Millimeter, execContext.FontStyle);
            var g = execContext.Graphics;
            var lineStart = new XPoint(xmm, ymm);
            foreach (string line in text)
            {
                XSize extent = g.MeasureString(line == string.Empty ? " " : line, font);
                lineStart = new XPoint(lineStart.X, lineStart.Y + extent.Height);
                g.DrawString(line, font, XBrushes.Black, lineStart);
            }

            execContext.NextLineStart = lineStart;
        }

        public void Save(string filename)
        {
            execContext.Document.Save(filename);
        }

        private void DrawBackground(CoordinateMapper mapper)
        {
            if (execContext.Background != null)
            {
                using (var img = XImage.FromFile(execContext.Background))
                {
                    var rect = mapper.BackgroundRect(img);
                    execContext.Graphics.DrawImage(img, rect);
                }
            }
        }

    }
}

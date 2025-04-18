using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace PdfGen
{
    internal struct CoordinateMapper
    {
        private Margins margins;
        private readonly double pageWidth;
        private readonly double pageHeight;

        internal CoordinateMapper(ExecContext ctx)
        {
            margins = ctx.Margins;
            var page = ctx.Page;
            pageWidth = page.Width.Millimeter;
            pageHeight = page.Height.Millimeter;
        }

        internal XRect BackgroundRect(XImage image)
        {
            double originalWidth = XUnit.FromPoint(image.PointWidth).Millimeter;
            double originalHeight = XUnit.FromPoint(image.PointHeight).Millimeter;
            double scaledWidth = pageWidth - (margins.Left + margins.Right);
            double scaledHeight = originalHeight * scaledWidth / originalWidth;
            return new XRect(margins.Left, margins.Top, scaledWidth, scaledHeight);
        }

        //internal double MapY(double y) => y + margins.Top;

        internal XPoint Map(double xmm, double ymm)
        {
            return new XPoint(xmm + margins.Left, ymm + margins.Top);
        }
    }
}

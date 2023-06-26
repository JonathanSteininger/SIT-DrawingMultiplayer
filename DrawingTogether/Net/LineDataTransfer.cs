using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingTogether.Net
{
    internal class LineDataTransfer : DTO
    {
        public string Type => nameof(LineDataTransfer);

        List<LinePortion> Portions { get; set; }
        public LineDataTransfer(List<LinePortion> portions)
        {
            Portions = portions;
        }
    }
}

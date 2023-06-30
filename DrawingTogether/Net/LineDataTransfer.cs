using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingTogether.Net
{
    public class LineDataTransfer : DTO
    {
        public string Type => nameof(LineDataTransfer);

        public List<LinePortion> Portions { get; set; }
        public LineDataTransfer(List<LinePortion> portions)
        {
            Portions = portions;
        }
    }
}

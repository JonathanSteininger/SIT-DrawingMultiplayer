using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingTogether.Net
{
    public class PlayerName : DTO
    {
        public string Type => nameof(PlayerName);

        public string Name;
        public PlayerName(string name) {
            Name = name;
        }
    }
}

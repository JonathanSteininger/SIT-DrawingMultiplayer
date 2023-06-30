using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingTogether.Net
{
    public enum GameCommand
    {
        Quit
    }
    public class Command : DTO
    {
        public string Type => nameof(Command);

        public GameCommand CommandType;
        public Command(GameCommand commandType)
        {
            CommandType = commandType;
        }
    }
}

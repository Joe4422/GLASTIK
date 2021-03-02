using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLASTIK.GameConsole
{
    public interface IGameConsole
    {
        public enum MessageType
        {
            Status,
            Warning,
            Error,
            Debug,
            Print
        }

        public void LogStatus(string message);

        public void LogWarning(string message);

        public void LogError(string message);

        public void LogDebug(string message, uint level = 0);

        public void PrintLine(string line, MessageType messageType = MessageType.Print);

        public uint DebugLevel { get; set; }

        public void FocusConsole();
    }
}

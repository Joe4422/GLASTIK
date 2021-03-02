using System.Collections.Generic;

namespace GLASTIK.GameConsole
{
    public partial class WebConsole
    {
        protected enum Opcode
        {
            RequestHistory,
            RequestHistory_Ack,
            LogMessage,
            RequestSuggestions,
            RequestSuggestions_Ack,
            RunCommand,
            FocusWindow,
            FocusWindowPacket
        }

        protected class Packet
        {
            public int Opcode { get; set; }
        }

        protected class RequestHistoryPacket : Packet
        {
            public int Count { get; set; }
        }

        protected class RequestHistoryPacket_Ack : Packet
        {
            public IList<string> History { get; set; }

            public RequestHistoryPacket_Ack(IList<string> history)
            {
                History = history;
                Opcode = (int)WebConsole.Opcode.RequestHistory_Ack;
            }
        }

        protected class LogMessagePacket : Packet
        {
            public string Message { get; set; }

            public LogMessagePacket(string message)
            {
                Message = message;
                Opcode = (int)WebConsole.Opcode.LogMessage;
            }
        }

        protected class RequestSuggestionsPacket : Packet
        {
            public string CurrentInput { get; set; }

            public uint CurrentWord { get; set; }
        }

        protected class RequestSuggestionsPacket_Ack : Packet
        {
            public IList<string> Suggestions { get; set; }

            public RequestSuggestionsPacket_Ack(IList<string> suggestions)
            {
                Suggestions = suggestions;
                Opcode = (int)WebConsole.Opcode.RequestSuggestions_Ack;
            }
        }

        protected class RunCommandPacket : Packet
        {
            public string Command { get; set; }
        }

        protected class FocusWindowPacket : Packet
        {
            public FocusWindowPacket()
            {
                Opcode = (int)WebConsole.Opcode.FocusWindow;
            }
        }
    }
}

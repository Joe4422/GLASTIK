using System.Collections.Generic;

namespace GLASCon
{
    public static class Packets
    {
        public enum MessageType
        {
            Status,
            Warning,
            Error,
            Debug,
            Print
        }

        public enum Opcode
        {
            RequestHistory,
            RequestHistory_Ack,
            LogMessage,
            RequestSuggestions,
            RequestSuggestions_Ack,
            RunCommand,
            FocusWindow
        }

        public class Packet
        {
            public int Opcode { get; set; }
        }

        public class RequestHistoryPacket : Packet
        {
            public int Count { get; set; }

            public RequestHistoryPacket(int count)
            {
                Count = count;
                Opcode = (int)Packets.Opcode.RequestHistory;
            }
        }

        public class RequestHistoryPacket_Ack : Packet
        {
            public IList<string> History { get; set; }
        }

        public class LogMessagePacket : Packet
        {
            public string Message { get; set; }
        }

        public class RequestSuggestionsPacket : Packet
        {
            public string CurrentInput { get; set; }

            public uint CurrentWord { get; set; }

            public RequestSuggestionsPacket(string currentInput, uint currentWord)
            {
                CurrentInput = currentInput;
                CurrentWord = currentWord;
                Opcode = (int)Packets.Opcode.RequestSuggestions;
            }
        }

        public class RequestSuggestionsPacket_Ack : Packet
        {
            public IList<string> Suggestions { get; set; }
        }

        public class RunCommandPacket : Packet
        {
            public string Command { get; set; }

            public RunCommandPacket(string command)
            {
                Command = command;
                Opcode = (int)Packets.Opcode.RunCommand;
            }
        }

        public class FocusWindowPacket : Packet
        {
        }
    }
}

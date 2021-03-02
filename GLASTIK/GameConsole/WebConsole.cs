using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GLASTIK.GameConsole
{
    public partial class WebConsole : IGameConsole
    {
        readonly Socket socket;
        readonly IPEndPoint inEp = new(IPAddress.Parse("127.0.0.1"), 5002);
        readonly IPEndPoint outEp = new(IPAddress.Parse("127.0.0.1"), 5001);

        FixedLengthQueue<string> history = new(256);

        public uint DebugLevel { get; set; } = 5;

        public WebConsole()
        {
            socket = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            socket.Bind(inEp);

            WaitForPacketsAsync().ConfigureAwait(false);
        }

        public void FocusConsole()
        {
            SendPacketAsync(JsonConvert.SerializeObject(new FocusWindowPacket())).ConfigureAwait(false);
        }

        public void LogDebug(string message, uint level = 0)
        {
            if (level <= DebugLevel)
            {
                message = $"{new StackTrace().GetFrame(5).GetMethod().DeclaringType.Name}:\t{message}";

                LogAsync(IGameConsole.MessageType.Debug, message).ConfigureAwait(false);
            }
        }

        public void LogError(string message)
        {
            message = $"{new StackTrace().GetFrame(5).GetMethod().DeclaringType.Name}:\t{message}";

            LogAsync(IGameConsole.MessageType.Error, message).ConfigureAwait(false);
        }

        public void LogStatus(string message)
        {
            message = $"{new StackTrace().GetFrame(3).GetMethod().DeclaringType.Name}:\t{message}";

            LogAsync(IGameConsole.MessageType.Status, message).ConfigureAwait(false);
        }

        public void LogWarning(string message)
        {
            message = $"{new StackTrace().GetFrame(5).GetMethod().DeclaringType.Name}:\t{message}";

            LogAsync(IGameConsole.MessageType.Warning, message).ConfigureAwait(false);
        }

        public void PrintLine(string line, IGameConsole.MessageType messageType = IGameConsole.MessageType.Print)
        {
            LogAsync(messageType, line).ConfigureAwait(false);
        }

        protected async Task LogAsync(IGameConsole.MessageType msgType, string message)
        {
            message = (char)(((ushort)msgType) + 1) + message;

            LogMessagePacket pkt = new(message);

            await SendPacketAsync(JsonConvert.SerializeObject(pkt));

            history.Enqueue(message);
        }

        protected async Task WaitForPacketsAsync()
        {

            while (true)
            {        
                byte[] data = new byte[8192];

                try
                {
                    var result = await socket.ReceiveFromAsync(data, SocketFlags.None, inEp);
                }
                catch (Exception)
                {
                    continue;
                }

                string response = await ProcessPacketAsync(Encoding.ASCII.GetString(data).Trim());

                if (response != null)
                {
                    await SendPacketAsync(response);
                }
            }
        }

        protected async Task<string> ProcessPacketAsync(string data)
        {
            string response = null;

            await Task.Run(() =>
            {
                Packet header = JsonConvert.DeserializeObject<Packet>(data);

                if (!Enum.IsDefined(typeof(Opcode), header.Opcode)) return;

                Opcode op = (Opcode)header.Opcode;

                if (op == Opcode.RequestHistory)
                {
                    RequestHistoryPacket packet = JsonConvert.DeserializeObject<RequestHistoryPacket>(data);
                    RequestHistoryPacket_Ack rsPkt = new(history.ToList());

                    response = JsonConvert.SerializeObject(rsPkt);
                }
                else if (op == Opcode.RequestSuggestions)
                {
                    // TODO
                    response = JsonConvert.SerializeObject(new RequestSuggestionsPacket_Ack(new List<string>()));
                }
                else if (op == Opcode.RunCommand)
                {
                    RunCommandPacket packet = JsonConvert.DeserializeObject<RunCommandPacket>(data);

                    PrintLine($"> {packet.Command}");

                    string[] split = packet.Command.ToString().Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();

                    ConsoleCommand command = null;

                    try
                    {
                        command = ConsoleCommands.Commands.First(x => x.Command.ToLower() == split[0].ToLower());
                    }
                    catch (Exception)
                    {
                        PrintLine("Invalid command.");
                        return;
                    }

                    command.Action(split);
                }
            });

            return response;
        }

        protected async Task SendPacketAsync(string json)
        {
            try
            {
                byte[] data = Encoding.ASCII.GetBytes(json);
                await socket.SendToAsync(data, SocketFlags.None, outEp);
            }
            catch (Exception)
            {
                Debug.WriteLine("oof");
            }
        }
    }

    public class FixedLengthQueue<T> : Queue<T>
    {
        protected uint maxLength = 0;
        public uint MaxLength
        {
            get => maxLength;
            set
            {
                maxLength = value;
                if (maxLength > 0) while (Count > MaxLength) Dequeue();
            }
        }

        public FixedLengthQueue(uint maxLength)
        {
            MaxLength = maxLength;
        }

        public new void Enqueue(T item)
        {
            base.Enqueue(item);

            if (MaxLength > 0) while (Count > MaxLength) Dequeue();
        }
    }
}

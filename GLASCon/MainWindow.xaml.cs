using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GLASCon
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ConnectionManager connection = new();

        public MainWindow()
        {
            InitializeComponent();

            connection.PacketReceived += Connection_PacketReceived;

            InputTextBox.KeyDown += InputTextBox_KeyDown;
            
            LogTextBox.Document.Blocks.Clear();

            LogTextBox.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;

            connection.SendPacketAsync(JsonConvert.SerializeObject(new Packets.RequestHistoryPacket(100))).ConfigureAwait(false);

            Activated += MainWindow_Activated;
        }

        private void MainWindow_Activated(object sender, EventArgs e)
        {
            Keyboard.Focus(InputTextBox);
        }

        private async void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await connection.SendPacketAsync(JsonConvert.SerializeObject(new Packets.RunCommandPacket((sender as TextBox).Text)));

                (sender as TextBox).Text = "";
            }
        }

        private void Connection_PacketReceived(string json)
        {
            Debug.WriteLine(json);

            Packets.Packet packet;

            try
            {
                packet = JsonConvert.DeserializeObject<Packets.Packet>(json);
            }
            catch (Exception)
            {
                return;
            }

            Packets.Opcode opcode = (Packets.Opcode)packet.Opcode;

            if (opcode == Packets.Opcode.LogMessage)
            {
                Packets.LogMessagePacket lmp = JsonConvert.DeserializeObject<Packets.LogMessagePacket>(json);
                AddString(lmp.Message);

                LogTextBox.ScrollToEnd();
            }
            else if (opcode == Packets.Opcode.RequestHistory_Ack)
            {
                Packets.RequestHistoryPacket_Ack rhpa = JsonConvert.DeserializeObject<Packets.RequestHistoryPacket_Ack>(json);

                foreach (string s in rhpa.History)
                {
                    AddString(s);
                }

                LogTextBox.ScrollToEnd();
            }
            else if (opcode == Packets.Opcode.FocusWindow)
            {
                Activate();
            }
        }

        private void AddString(string str)
        {
            Brush fontBrush = str[0] switch
            {
                '\u0001' => Brushes.White,
                '\u0002' => Brushes.Yellow,
                '\u0003' => Brushes.Red,
                '\u0004' => Brushes.Green,
                '\u0005' => Brushes.White,
                _ => Brushes.White
            };

            LogTextBox.Document.Blocks.Add(new Paragraph(new Run(str) { Foreground = fontBrush }));
        }
    }
}

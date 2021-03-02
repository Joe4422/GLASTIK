using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Terminal.Gui;

namespace GLASTIK.GameConsole
{
    public class Logger : View
    {
        private enum LogType
        {
            Status,
            Warning,
            Error,
            Debug
        }

        public uint DebugLevel { get; set; } = 5;

        const int messageCount = 256;
        string[] messages = new string[messageCount];
        ushort messageIndex = 0;

        private readonly ColorScheme statusColorScheme = new()
        {
            Normal = new(Color.White)
        };
        private readonly ColorScheme warningColorScheme = new()
        {
            Normal = new(Color.BrightYellow)
        };
        private readonly ColorScheme errorColorScheme = new()
        {
            Normal = new(Color.BrightRed)
        };
        private readonly ColorScheme debugColorScheme = new()
        {
            Normal = new(Color.BrightGreen)
        };

        public override void Redraw(Rect bounds)
        {
            base.Redraw(bounds);

            int ptr = messageIndex - 1;

            for (int i = bounds.Bottom; i > bounds.Top; i--)
            {
                if (messages[ptr] == null) break;

                string msg = messages[ptr];
                
                ColorScheme color = msg[0] switch
                {
                    '\u0001' => statusColorScheme,
                    '\u0002' => warningColorScheme,
                    '\u0003' => errorColorScheme,
                    '\u0004' => debugColorScheme,
                    _ => null
                };

                msg = msg[1..];

                Driver.Move(bounds.Left + 1, i);

                Driver.SetAttribute(color.Normal);
                Driver.AddStr(msg);

                if (ptr == 0) ptr = messageCount - 1;
                else ptr--;
            }
        }

        public void LogStatus(string message)
        {
            Log(LogType.Status, message);
        }

        public void LogWarning(string message)
        {
            Log(LogType.Warning, message);
        }

        public void LogError(string message)
        {
            Log(LogType.Error, message);
        }

        public void LogDebug(uint level, string message)
        {
            if (level <= DebugLevel)
            {
                Log(LogType.Debug, message);
            }
        }

        public void PrintLine(string message)
        {
            messages[messageIndex++] = '\u0001' + message;

            if (messageIndex >= messageCount) messageIndex = 0;

            typeof(Application).GetMethod("TerminalResized", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, null);
        }

        private void Log(LogType logType, string message)
        {
            string caller = new StackTrace().GetFrame(3).GetMethod().DeclaringType.Name;

            char prepend = logType switch
            {
                LogType.Status => '\u0001',
                LogType.Warning => '\u0002',
                LogType.Error => '\u0003',
                LogType.Debug => '\u0004',
                _ => '\u0001'
            };

            messages[messageIndex++] = $"{prepend}{caller}: {message}";

            if (messageIndex >= messageCount) messageIndex = 0;

            try
            {
                typeof(Application).GetMethod("TerminalResized", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, null);
            }
            catch (Exception) { }
        }
    }
}

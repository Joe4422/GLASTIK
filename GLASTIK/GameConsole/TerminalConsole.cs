using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Terminal.Gui;

namespace GLASTIK
{
    namespace GameConsole
    {
        public class TerminalConsole : IGameConsole
        {
            private Logger log;

            private TextField tf;

            private const int previousCommandCount = 64;
            private string[] previousCommands = new string[previousCommandCount];
            private int lastCommandIndicator = 0;
            private int currentLastCommandIndicator = 0;

            public uint DebugLevel { get; set; } = 5;

            public TerminalConsole()
            {
                InitGui();
            }

            private void InitGui()
            {
                Application.Init();
                Toplevel top = Application.Top;
                Colors.Base.Normal = Application.Driver.MakeAttribute(Color.White, Color.Black);

                Window win = new("Console")
                {
                    X = 0,
                    Y = 0,
                    Width = Dim.Fill(),
                    Height = Dim.Fill() - 1
                };
                top.Add(win);

                log = new()
                {
                    X = 0,
                    Y = 0,
                    Width = Dim.Fill(),
                    Height = Dim.Fill()
                };
                win.Add(log);

                tf = new()
                {
                    X = 0,
                    Y = Pos.Bottom(win),
                    Width = Dim.Fill(),
                    Height = 1
                };
                top.Add(tf);

                tf.KeyPress += Tf_KeyPress;

                Thread thread = new(() => Application.Run());
                thread.IsBackground = true;
                thread.Start();
            }

            private void Tf_KeyPress(View.KeyEventEventArgs obj)
            {
                if (obj.KeyEvent.Key == Key.Enter && tf.Text.ToString().Trim() != "")
                {
                    string input = tf.Text.ToString();

                    tf.Text = "";

                    PrintLine("> " + input);

                    string[] split = input.ToString().Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();

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
                    finally
                    {
                        previousCommands[lastCommandIndicator] = input;

                        if (lastCommandIndicator == previousCommandCount)
                        {
                            lastCommandIndicator = 0;
                        }
                        else
                        {
                            lastCommandIndicator++;
                        }

                        currentLastCommandIndicator = lastCommandIndicator;
                    }

                    command.Action(split);
                }
                else if (obj.KeyEvent.Key == Key.CursorUp)
                {
                    int temp = currentLastCommandIndicator - 1;

                    if (temp < 0) temp = previousCommandCount - 1;

                    if (previousCommands[temp] != null)
                    {
                        currentLastCommandIndicator = temp;

                        tf.Text = previousCommands[currentLastCommandIndicator];
                    }

                    tf.CursorPosition = tf.Text.Length;
                }
                else if (obj.KeyEvent.Key == Key.CursorDown)
                {
                    int temp = currentLastCommandIndicator + 1;

                    if (temp >= previousCommandCount) temp = 0;

                    if (previousCommands[temp] != null)
                    {
                        currentLastCommandIndicator = temp;

                        tf.Text = previousCommands[currentLastCommandIndicator];
                    }
                    else if (temp == lastCommandIndicator)
                    {
                        tf.Text = "";
                    }

                    tf.CursorPosition = tf.Text.Length;
                }
            }

            public void FocusConsole()
            {
            }

            public void LogStatus(string message)
            {
                log.LogStatus(message);
            }

            public void LogWarning(string message)
            {
                log.LogWarning(message);
            }

            public void LogError(string message)
            {
                log.LogError(message);
            }

            public void LogDebug(string message, uint level = 0)
            {
                log.LogDebug(level, message);
            }

            public void PrintLine(string line, IGameConsole.MessageType messageType = IGameConsole.MessageType.Print)
            {
                log.PrintLine(line);
            }
        }
    }
}

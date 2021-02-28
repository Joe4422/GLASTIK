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
        public static class GameConsole
        {
            public static Logger Log { get; private set; }

            private static TextField tf;

            private const int previousCommandCount = 64;
            private static string[] previousCommands = new string[previousCommandCount];
            private static int lastCommandIndicator = 0;
            private static int currentLastCommandIndicator = 0;

            static GameConsole()
            {
                InitGui();
            }

            private static void InitGui()
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

                Log = new()
                {
                    X = 0,
                    Y = 0,
                    Width = Dim.Fill(),
                    Height = Dim.Fill()
                };
                win.Add(Log);

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

            private static void Tf_KeyPress(View.KeyEventEventArgs obj)
            {
                if (obj.KeyEvent.Key == Key.Enter && tf.Text.ToString().Trim() != "")
                {
                    string input = tf.Text.ToString();

                    tf.Text = "";

                    Log.PrintLine("> " + input);

                    string[] split = input.ToString().Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();

                    ConsoleCommand command = null;

                    try
                    {
                        command = ConsoleCommands.Commands.First(x => x.Command.ToLower() == split[0].ToLower());
                    }
                    catch (Exception)
                    {
                        Log.PrintLine("Invalid command.");
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
        }
    }
}

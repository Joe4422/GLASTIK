﻿using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GLASTIK.GameConsole
{
    public class ConsoleCommand
    {
        public string Command { get; }

        public Action<string[]> Action { get; }

        public ConsoleCommand(string command, Action<string[]> action)
        {
            Command = command;
            Action = action;
        }
    }

    public static class ConsoleCommands
    {
        [AttributeUsage(AttributeTargets.Method)]
        private class ConsoleCommandAttribute : Attribute
        {
            public string Value { get; }

            public ConsoleCommandAttribute(string value)
            {
                Value = value;
            }
        }

        public static ConsoleCommand[] Commands { get; }

        static ConsoleCommands()
        {
            List<ConsoleCommand> commands = new();

            foreach (var method in typeof(ConsoleCommands).GetMethods(BindingFlags.Static | BindingFlags.NonPublic))
            {
                foreach (var att in method.GetCustomAttributes(false))
                {
                    if (att is ConsoleCommandAttribute cca)
                    {
                        commands.Add(new(cca.Value, (Action<string[]>)Delegate.CreateDelegate(typeof(Action<string[]>), method)));
                    }
                }
            }

            Commands = commands.ToArray();
        }

#pragma warning disable IDE0051 // Remove unused private members
        [ConsoleCommand("map")]
        private static void Command_Map(string[] args)
        {
            if (args.Length != 2)
            {
                GameData.Console.PrintLine("Invalid arguments.");
                return;
            }

            if (!GameData.LevelManager.LoadLevel(args[1]))
            {
                GameData.Console.PrintLine($"Cannot find level {args[1]}.", IGameConsole.MessageType.Warning);
                return;
            }
        }

        [ConsoleCommand("maps")]
        private static void Command_Maps(string[] args)
        {
            if (args.Length != 1)
            {
                GameData.Console.PrintLine("Invalid arguments.");
                return;
            }

            foreach (string level in GameData.LevelManager.Levels)
            {
                GameData.Console.PrintLine($" - {level}");
            }
        }

        [ConsoleCommand("entities")]
        private static void Command_Entities(string[] args)
        {
            if (args.Length != 1)
            {
                GameData.Console.PrintLine("Invalid arguments.");
                return;
            }

            if (GameData.LevelManager.CurrentLevel == null)
            {
                GameData.Console.PrintLine("No level loaded.");
                return;
            }

            int i = 0;
            foreach (BaseEntity entity in GameData.LevelManager.CurrentLevel.EntityManager.Entities)
            {
                GameData.Console.PrintLine($"{i++}:{new string(' ', 6 - i.ToString().Length)}{entity}");
            }
        }

        [ConsoleCommand("entity")]
        private static void Command_Entity(string[] args)
        {
            if (args.Length < 2)
            {
                GameData.Console.PrintLine("Invalid arguments.");
                return;
            }

            if (GameData.LevelManager.CurrentLevel == null)
            {
                GameData.Console.PrintLine("No level loaded.");
                return;
            }

            BaseEntity entity = GetEntityFromString(args[1]);

            if (entity == null)
            {
                GameData.Console.PrintLine($"No such entity with ID or name {args[1]}.", IGameConsole.MessageType.Warning);
                return;
            }

            if (args.Length == 2)
            {
                GameData.Console.PrintLine(entity.ToString());
                return;
            }

            PropertyInfo entProperty = null;

            foreach (var property in entity.GetType().GetProperties())
            {
                foreach (var att in property.GetCustomAttributes(true))
                {
                    if (att is BaseEntity.ConsoleModifiableAttribute)
                    {
                        if (property.Name.ToLower() == args[2].ToLower())
                        {
                            entProperty = property;
                            break;
                        }
                    }
                }

                if (entProperty != null) break;
            }

            if (entProperty == null)
            {
                GameData.Console.PrintLine($"Unknown or un-modifiable property {args[2]}.", IGameConsole.MessageType.Warning);
                return;
            }

            if (args.Length == 3)
            {
                GameData.Console.PrintLine(entProperty.GetValue(entity)?.ToString());
                return;
            }
            else if (args.Length == 4)
            {
                if (IsNumericType(entProperty.PropertyType))
                {
                    object[] parameters = new object[] { args[3], null };
                    if ((bool)entProperty.PropertyType.GetMethod("TryParse", new[] { typeof(string), entProperty.PropertyType.MakeByRefType() }).Invoke(null, parameters) == true)
                    {
                        entProperty.SetValue(entity, Convert.ChangeType(parameters[1], entProperty.PropertyType));
                    }
                    else GameData.Console.PrintLine("Invalid arguments.");
                }
                else if (entProperty.PropertyType == typeof(Point2D))
                {
                    string[] split = args[3].Split(',', StringSplitOptions.RemoveEmptyEntries);

                    if (split.Length != 2)
                    {
                        GameData.Console.PrintLine("Invalid arguments.");
                    }
                    else
                    {
                        if (double.TryParse(split[0], out double x) == false || double.TryParse(split[1], out double y) == false)
                        {
                            GameData.Console.PrintLine("Invalid arguments.");
                        }
                        else
                        {
                            entProperty.SetValue(entity, new Point2D(x, y));
                        }
                    }
                }
                else if (entProperty.PropertyType == typeof(string))
                {
                    entProperty.SetValue(entity, args[3]);
                }
                else if (entProperty.PropertyType == typeof(BaseEntity))
                {
                    if (args[3] == "null")
                    {
                        entProperty.SetValue(entity, null);
                    }
                    else
                    {
                        BaseEntity other = GetEntityFromString(args[3]);

                        if (other == null)
                        {
                            GameData.Console.PrintLine($"Unknown entity {args[3]}.", IGameConsole.MessageType.Warning);
                        }
                        else
                        {
                            entProperty.SetValue(entity, other);
                        }
                    }
                }
            }
        }

        private static bool IsNumericType(Type type)
        {
            return Type.GetTypeCode(type) switch
            {
                TypeCode.Byte or TypeCode.SByte or TypeCode.UInt16 or TypeCode.UInt32 or TypeCode.UInt64 or TypeCode.Int16 or TypeCode.Int32 or TypeCode.Int64 or TypeCode.Decimal or TypeCode.Double or TypeCode.Single => true,
                _ => false,
            };
        }

        private static BaseEntity GetEntityFromString(string entityRef)
        {
            List<BaseEntity> entities = GameData.LevelManager.CurrentLevel.EntityManager.Entities;

            if (ushort.TryParse(entityRef, out ushort result))
            {
                if (result >= entities.Count) return null;
                else return entities[result];
            }

            try
            {
                return entities.First(x => x.Name.ToLower() == entityRef.ToLower());
            }
            catch (Exception)
            {
                return null;
            }
        }

        [ConsoleCommand("cs")]
        private static void Command_Cs(string[] args)
        {
            string cs = string.Join(' ', args[1..]);

            try
            {
                var result = CSharpScript.EvaluateAsync(cs, ScriptOptions.Default.WithImports("GLASTIK").AddReferences(typeof(GameData).Assembly)).Result;
                GameData.Console.PrintLine($"{result}");
            }
            catch (CompilationErrorException e)
            {
                foreach (string s in e.Diagnostics.Select(x => x.ToString()))
                {
                    GameData.Console.PrintLine(s, IGameConsole.MessageType.Error);
                }
            }

        }

#pragma warning restore IDE0051 // Remove unused private members
    }
}

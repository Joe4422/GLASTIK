using GLASTIK.GameConsole;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace GLASTIK
{
    public class Map
    {
        public string Name { get; }
        public string TextureFile { get; }
        public List<Point2D> Spawns { get; }
        public ushort[,] Tiles { get; }

        protected Map(string name, string textureFile, List<Point2D> spawns, ushort[,] tiles)
        {
            Name = name;
            TextureFile = textureFile;
            Spawns = spawns;
            Tiles = tiles;
        }

        public static Map LoadFromXml(string xmlPath)
        {
            if (!File.Exists(xmlPath))
            {
                throw new FileNotFoundException("Could not find map file!", xmlPath);
            }

            GameConsole.GameConsole.Log.LogDebug(1, $"Loading map {xmlPath}.");

            string name;
            uint width, height;
            string textureFile;
            List<Point2D> spawns = new();
            ushort[,] tiles;

            try
            {
                XElement map = XElement.Load(xmlPath);

                name = map.Element("name").Value;

                width = uint.Parse(map.Element("width").Value);
                height = uint.Parse(map.Element("height").Value);

                textureFile = map.Element("texturefile").Value;

                foreach (var spawn in map.Element("spawns").Elements())
                {
                    spawns.Add(new(double.Parse(spawn.Element("x").Value), double.Parse(spawn.Element("y").Value)));
                }

                tiles = ReadTilesFromString(map.Element("tiles").Value, width, height);
            }
            catch (Exception)
            {
                throw new FormatException("Failed to parse map file!");
            }

            return new(name, textureFile, spawns, tiles);
        }

        protected static ushort[,] ReadTilesFromString(string tiles, uint width, uint height)
        {
            string[] words = Regex.Replace(tiles, @"\s+", " ").Split(' ', StringSplitOptions.RemoveEmptyEntries);

            ushort[,] data = new ushort[width, height];

            int x = 0, y = 0;

            foreach (string word in words)
            {
                if (!ushort.TryParse(word, out ushort tile))
                {
                    throw new FormatException($"Bad index value {word}!");
                }

                data[x++, y] = tile;
                if (x == width)
                {
                    x = 0;
                    y++;
                }
            }

            return data;
        }
    }
}

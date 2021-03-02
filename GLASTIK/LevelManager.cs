using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLASTIK
{
    public class LevelManager
    {
        public List<string> Levels { get; } = new();

        public Level CurrentLevel { get; private set; } = null;

        public string MapPath { get; set; }

        public LevelManager(string mapPath)
        {
            MapPath = mapPath;

            if (Directory.Exists(mapPath))
            {
                Levels = Directory.GetFiles(mapPath, "*.xml").ToList();
            }
            else
            {
                throw new DirectoryNotFoundException(mapPath);
            }
        }

        public bool LoadLevel(string path)
        {
            Map map;

            GameData.Console.LogStatus($"Loading level at path {path}.");

            if (Path.HasExtension(path) == false)
            {
                path += ".xml";
            }

            if (Path.IsPathRooted(path) == false)
            {
                path = MapPath + "/" + path;
            }

            try
            {
                map = Map.LoadFromXml(path);
            }
            catch (Exception)
            {
                return false;
            }

            if (CurrentLevel != null) CurrentLevel.Dispose();

            CurrentLevel = new(map);

            GameData.Console.LogStatus("Level load completed.");

            return true;
        }
    }
}

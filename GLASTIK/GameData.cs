using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GLASTIK.GameConsole;

namespace GLASTIK
{
    public static class GameData
    {
        public static GraphicsDevice GraphicsDevice { get; private set; }
        public static IServiceProvider ServiceProvider { get; private set; }
        public static SpriteBatch SpriteBatch { get; private set; }
        public static ContentManager ContentManager { get; private set; }
        public static TextureManager EntitySpriteManager { get; private set; }
        public static LevelManager LevelManager { get; private set; }
        public static IGameConsole Console { get; set; }

        public static void SetGameData(GraphicsDevice graphicsDevice, IServiceProvider serviceProvider, SpriteBatch spriteBatch, ContentManager contentManager)
        {
            GraphicsDevice = graphicsDevice;
            ServiceProvider = serviceProvider;
            SpriteBatch = spriteBatch;
            ContentManager = contentManager;

            EntitySpriteManager = new(ContentManager);

            try
            {
                EntitySpriteManager.PreloadTextures("entities.sps");
            }
            catch (FileNotFoundException e)
            {
                Console.LogError($"Could not find entity sprite file at {e.FileName}!");
            }

            try
            {
                LevelManager = new(".");
            }
            catch (DirectoryNotFoundException e)
            {
                Console.LogError($"Could not find map directory {e.Message}!");
            }
        }
    }
}

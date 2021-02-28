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
    public class GameData
    {
        public GraphicsDevice GraphicsDevice { get; }
        public IServiceProvider ServiceProvider { get; }
        public SpriteBatch SpriteBatch { get; }
        public ContentManager ContentManager { get; }
        public TextureManager EntitySpriteManager { get; }
        public LevelManager LevelManager { get; }

        public GameData(GraphicsDevice graphicsDevice, IServiceProvider serviceProvider, SpriteBatch spriteBatch, ContentManager contentManager)
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
                GameConsole.GameConsole.Log.LogError($"Could not find entity sprite file at {e.FileName}!");
            }

            try
            {
                LevelManager = new(".");
            }
            catch (DirectoryNotFoundException e)
            {
                GameConsole.GameConsole.Log.LogError($"Could not find map directory {e.Message}!");
            }
        }
    }
}

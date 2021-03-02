using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace GLASTIK
{
    public class Level : ITickable, IDisposable
    {
        public EntityManager EntityManager { get; }
        protected TextureManager spriteManager;

        public BasePlayer Player { get; private set; }
        public double Scale { get; set; } = 1.0;

        public Map Map { get; set; }

        public Level(Map map)
        {
            spriteManager = new(new ContentManager(GameData.ServiceProvider, "Content/"));
            EntityManager = new(this);

            Player = new DefaultPlayer(0);

            EntityManager.Register(Player);

            Map = map;
            spriteManager.PreloadTextures(Map.TextureFile);

            Player.Respawn();

            EntityManager.Register(new NPCEntity(16, 128, 128));
        }

        public void Dispose()
        {
            spriteManager.Dispose();
        }

        public void Tick()
        {
            EntityManager.TickEntities();
        }

        public void Draw()
        {
            int playerHeight = 0, playerWidth = 0;
            Texture2D playerSprite = GameData.EntitySpriteManager.GetTexture(Player.SpriteIndex);

            if (playerSprite != null)
            {
                playerHeight = playerSprite.Height;
                playerWidth = playerSprite.Width;
            }

            double cameraX = Player.Camera.Position.X - GameData.GraphicsDevice.Viewport.Width / 2 + playerWidth / 2;
            double cameraY = Player.Camera.Position.Y - GameData.GraphicsDevice.Viewport.Height / 2 + playerHeight / 2;

            GameData.SpriteBatch.Begin();
            for (int x = 0; x < Map.Tiles.GetLength(0); x++)
            {
                for (int y = 0; y < Map.Tiles.GetLength(1); y++)
                {
                    Texture2D tex = spriteManager.GetTexture(Map.Tiles[x, y]);

                    if (tex != null)
                    {
                        GameData.SpriteBatch.Draw(tex, new Rectangle((int)(((x * tex.Width) - cameraX) * Scale), (int)(((y * tex.Height) - cameraY) * Scale), (int)(tex.Width * Scale), (int)(tex.Height * Scale)), Color.White);
                    }
                }
            }

            foreach (BaseEntity entity in EntityManager.Entities)
            {
                if (entity is SpriteEntity sprite)
                {
                    Texture2D tex = GameData.EntitySpriteManager.GetTexture(sprite.SpriteIndex);

                    if (tex != null)
                    {
                        GameData.SpriteBatch.Draw(tex, new Rectangle((int)((entity.Position.X - cameraX) * Scale), (int)((entity.Position.Y - cameraY) * Scale), (int)(tex.Width * Scale), (int)(tex.Height * Scale)), Color.White);
                    }
                }
            }
            GameData.SpriteBatch.End();
        }
    }
}

using GLASTIK.GameConsole;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GLASTIK
{
    public class Glastik : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private BindingManager inputManager;

        public Glastik()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            GameData.Console = new WebConsole();

            GameData.Console.LogStatus("Initialising...");
            inputManager = new();
            BasePlayer.Input = inputManager;
            BaseController.Input = inputManager;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            GameData.Console.LogStatus("Loading game content...");

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            GameData.SetGameData(_graphics.GraphicsDevice, Services, _spriteBatch, Content);

            if (GameData.LevelManager.LoadLevel("testroom.xml") == false)
            {
                GameData.Console.LogError("Could not load level!");
            }

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            inputManager.Tick();
            GameData.LevelManager.CurrentLevel?.Tick();

            if (inputManager.Menu.Pressed) Exit();

            if (inputManager.ShowConsole.Pressed) GameData.Console.FocusConsole();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            GameData.LevelManager.CurrentLevel?.Draw();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}

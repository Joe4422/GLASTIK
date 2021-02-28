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
        private GameData gameData;

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
            GameConsole.GameConsole.Log.LogStatus("Initialising...");
            inputManager = new();
            BasePlayer.Input = inputManager;
            BaseController.Input = inputManager;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            GameConsole.GameConsole.Log.LogStatus("Loading game content...");

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            gameData = new(_graphics.GraphicsDevice, Services, _spriteBatch, Content);
            Level.GameData = gameData;
            ConsoleCommands.GameData = gameData;

            if (gameData.LevelManager.LoadLevel("testroom.xml") == false)
            {
                GameConsole.GameConsole.Log.LogError("Could not load level!");
            }

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            inputManager.Tick();
            gameData.LevelManager.CurrentLevel?.Tick();

            if (inputManager.Menu.Pressed) Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            gameData.LevelManager.CurrentLevel?.Draw();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}

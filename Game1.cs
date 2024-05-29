using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameFinal___Fallout_Shootout
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D backgroundTexture;
        Texture2D floorTexture;
        Texture2D paMinigunTexture;
        Texture2D paMinigunLeftTexture;

        Rectangle window;
        Rectangle backgroundRect;
        Rectangle floorRect;
        Rectangle paMinigunRect;

        SpriteFont overseerFont;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            window = new Rectangle(0, 0, 960, 512);
            _graphics.PreferredBackBufferWidth = window.Width;
            _graphics.PreferredBackBufferHeight = window.Height;
            _graphics.ApplyChanges();

            backgroundRect = new Rectangle(0, 0, 960, 512);
            floorRect = new Rectangle(0, 200, 960, 512);
            paMinigunRect = new Rectangle(0, 200, 200, 200);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            backgroundTexture = Content.Load<Texture2D>("wasteland");
            floorTexture = Content.Load<Texture2D>("final-floor");
            paMinigunTexture = Content.Load<Texture2D>("final-pa-minigun");
            paMinigunLeftTexture = Content.Load<Texture2D>("final-pa-minigun-left");
            overseerFont = Content.Load<SpriteFont>("overseerFont");


            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            _spriteBatch.Draw(backgroundTexture, backgroundRect, Color.White);
            _spriteBatch.Draw(floorTexture, new Rectangle(-200, 250, 960, 612), Color.White);
            _spriteBatch.Draw(floorTexture, new Rectangle(200, 250, 960, 612), Color.White);
            _spriteBatch.Draw(paMinigunTexture, paMinigunRect, Color.White);

            _spriteBatch.DrawString(overseerFont, "Fallout Fightout", new Vector2(10, 10), Color.Black);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
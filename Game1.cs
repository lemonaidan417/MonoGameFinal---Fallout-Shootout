using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameFinal___Fallout_Shootout
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D floorTexture;
        Texture2D paMinigunTexture;
        Texture2D paMinigunLeftTexture;
        Texture2D bulletTexture;

        KeyboardState keyboardState;

        Rectangle window;
        Rectangle floorRect;
        Rectangle paMinigunRect;
        Rectangle bulletRect;

        Vector2 paMinigunSpeed;

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
            window = new Rectangle(0, 0, 750, 750);
            _graphics.PreferredBackBufferWidth = window.Width;
            _graphics.PreferredBackBufferHeight = window.Height;
            _graphics.ApplyChanges();

            floorRect = new Rectangle(0, 0, 750, 750);
            paMinigunRect = new Rectangle(0, 200, 200, 200);
            bulletRect = new Rectangle(500, 500, 10, 10);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            floorTexture = Content.Load<Texture2D>("desert-background");
            paMinigunTexture = Content.Load<Texture2D>("final-pa-minigun");
            paMinigunLeftTexture = Content.Load<Texture2D>("final-pa-minigun-left");
            bulletTexture = Content.Load<Texture2D>("final-bullet");
            overseerFont = Content.Load<SpriteFont>("overseerFont");


            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            keyboardState = Keyboard.GetState();
            paMinigunSpeed = new Vector2();
            if (keyboardState.IsKeyDown(Keys.W))
            {
                paMinigunSpeed.Y -= 2;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            _spriteBatch.Draw(floorTexture, floorRect, Color.White);
            _spriteBatch.Draw(bulletTexture, bulletRect, Color.White);
            _spriteBatch.Draw(paMinigunTexture, new Rectangle(325, 325, 100, 100), Color.White);

            //_spriteBatch.DrawString(overseerFont, "Fallout Shootout", new Vector2(10, 10), Color.Black);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
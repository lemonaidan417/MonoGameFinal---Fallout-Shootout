﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace MonoGameFinal___Fallout_Shootout
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Player player;

        List<Enemy> enemies;

        List<Bullet> bullets;

        Texture2D paMinigunTexture;
        Texture2D paMinigunLeftTexture;
        Rectangle paMinigunRect;

        Rectangle bulletRect;
        Texture2D bulletTexture;
        Vector2 bulletSpeed;

        Rectangle eyeBotRect;
        Texture2D eyeBotTexture;
        Vector2 eyeBotSpeed;

        Rectangle window;
        Texture2D backgroundTexture;
        
        float playerAngle;
        
        SpriteFont overseerFont;

        Texture2D hitBoxTexture;
        Rectangle hitBoxRect;

        MouseState mouseState, prevMouseState;
        KeyboardState keyboardState;

        float ammoCount;
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

            paMinigunRect = new Rectangle(0, 200, 200, 200);
            bulletRect = new Rectangle(100, 100, 5, 5);

            bullets = new List<Bullet>();
            enemies = new List<Enemy>();

            ammoCount = 250;

            base.Initialize();
            player = new Player(paMinigunTexture, 350, 350);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            paMinigunTexture = Content.Load<Texture2D>("final-pa-minigun");
            paMinigunLeftTexture = Content.Load<Texture2D>("final-pa-minigun-left");
            bulletTexture = Content.Load<Texture2D>("final-bullet");
            overseerFont = Content.Load<SpriteFont>("overseerFont");
            backgroundTexture = Content.Load<Texture2D>("desert-background");
            eyeBotTexture = Content.Load<Texture2D>("eyebot-pixilart");


            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            keyboardState = Keyboard.GetState();

            
            prevMouseState = mouseState;
            mouseState = Mouse.GetState();


            if (keyboardState.IsKeyDown(Keys.Space))
            {
                bullets.Add(new Bullet(bulletTexture, paMinigunRect.Center.ToVector2(), mouseState.Position.ToVector2(), 10));
            }

            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Update();
                ammoCount *= bullets.Count / ammoCount;
                if (!window.Intersects(bullets[i].Rect)) // removes bullets after they leave the window
                {
                    bullets.RemoveAt(i);
                    i--;
                }
            }

            enemies.Add(new Enemy(eyeBotTexture, eyeBotRect.X, eyeBotRect.Y, eyeBotSpeed));

            foreach (Enemy enemy in enemies)
            {
            }

            player.HSpeed = 0;
            player.VSpeed = 0;

            if (keyboardState.IsKeyDown(Keys.D) || (keyboardState.IsKeyDown(Keys.Right)))
                player.HSpeed = 2;
            else if (keyboardState.IsKeyDown(Keys.A) || (keyboardState.IsKeyDown(Keys.Left)))
                player.HSpeed = -2;

            if (keyboardState.IsKeyDown(Keys.W) || (keyboardState.IsKeyDown(Keys.Up)))
                player.VSpeed = -2;
            else if (keyboardState.IsKeyDown(Keys.S) || (keyboardState.IsKeyDown(Keys.Down)))
                player.VSpeed = 2;

            if (keyboardState.IsKeyDown(Keys.LeftShift) || (keyboardState.IsKeyDown(Keys.RightShift)))
            {
                player.VSpeed *= 1.8f;
                player.HSpeed *= 1.8f;
            }
            playerAngle = (float)Math.Atan2(player.VSpeed, player.HSpeed);
            player.Update(gameTime);

            Window.Title = ammoCount + "";

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(backgroundTexture, window, Color.White);

            //_spriteBatch.Draw(bulletTexture, bulletRect, Color.White);

            player.Draw(_spriteBatch);
            foreach (Enemy enemy in enemies)
                enemy.Draw(_spriteBatch);

            foreach (Bullet bullet in bullets)
                bullet.Draw(_spriteBatch);

            //_spriteBatch.DrawString(overseerFont, "Fallout Shootout", new Vector2(10, 10), Color.Black);

            _spriteBatch.End();
            base.Draw(gameTime);







































            // Thank you so much for your help Mr Aldworth!
        }
    }
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace MonoGameFinal___Fallout_Shootout
{
    enum Screen
    {
        Intro,
        Main,
        Controls,
        Gameover
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Player player;
        List<Enemy> enemies;
        List<Bullet> bullets;

        Texture2D paMinigunTexture;
        Texture2D bulletTexture;
        Texture2D eyeBotTexture;
        Texture2D vaultDoorTexture;
        Texture2D introBackgroundTexture;
        Texture2D vaultBoyTexture;
        Texture2D rectangleTexture;
        Texture2D youDiedTexture;

        Rectangle window;
        Rectangle rectangleHealthRect;
        Rectangle rectangleAmmoRect;
        Rectangle vaultDoorRect;
        Rectangle vaultBoyRect;
        Rectangle introBackgroundRect;

        float secondsGun, gunCoolDown;
        float secondsEnemy, spawnCoolDown;
        float secondsMoveDelay, moveCoolDown;
        float secondsTextFlash;
        bool start;
        bool reloading;

        SpriteFont overseerFont;
        SpriteFont overseerFontUI;
        SpriteFont terminalFont;

        Random generator;
        Color textColor;
        Screen screen;
        float vaultDoorRotation;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            window = new Rectangle(0, 0, 700, 700);
            _graphics.PreferredBackBufferWidth = window.Width;
            _graphics.PreferredBackBufferHeight = window.Height;
            _graphics.ApplyChanges();

            bullets = new List<Bullet>();
            enemies = new List<Enemy>();
            generator = new Random();

            // Set up variables
            secondsGun = 0f;
            gunCoolDown = 0.05f;
            secondsEnemy = 0f;
            spawnCoolDown = 0.4f;
            secondsMoveDelay = 0f;
            moveCoolDown = 0.5f;
            secondsTextFlash = 0f;
            start = false;
            reloading = false;

            rectangleHealthRect = new Rectangle(0, 10, 190, 40);
            rectangleAmmoRect = new Rectangle(0, 40, 190, 40);
            vaultDoorRect = new Rectangle(502, 463, 675, 725);
            vaultBoyRect = new Rectangle(0, 0, 1200, 1200);
            introBackgroundRect = new Rectangle(-350, -150, window.Width + 650, window.Height + 50);

            textColor = Color.Transparent;
            screen = Screen.Intro;
            vaultDoorRotation = 0f;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            paMinigunTexture = Content.Load<Texture2D>("final-pa-minigun");
            bulletTexture = Content.Load<Texture2D>("final-bullet");
            overseerFont = Content.Load<SpriteFont>("overseerFont");
            overseerFontUI = Content.Load<SpriteFont>("overseerFontUI");
            terminalFont = Content.Load<SpriteFont>("terminalFont");
            eyeBotTexture = Content.Load<Texture2D>("eyebot-pixilart");
            rectangleTexture = Content.Load<Texture2D>("Rectangle");
            vaultBoyTexture = Content.Load<Texture2D>("final-vaultboy");
            vaultDoorTexture = Content.Load<Texture2D>("Vault_65");
            introBackgroundTexture = Content.Load<Texture2D>("final-desertlandscape");
            youDiedTexture = Content.Load<Texture2D>("youdied");

            // Initialize player AFTER loading textures
            player = new Player(paMinigunTexture, (window.Center.X - paMinigunTexture.Width / 2), (window.Center.Y - paMinigunTexture.Height / 2));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Handle different screen states
            switch (screen)
            {
                case Screen.Intro:
                    HandleIntro(gameTime);
                    break;

                case Screen.Main:
                    HandleMainGame(gameTime);
                    break;

                case Screen.Controls:
                    HandleControlsScreen(gameTime);
                    break;

                case Screen.Gameover:
                    HandleGameOverScreen();
                    break;
            }

            base.Update(gameTime);
        }

        private void HandleIntro(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();

            secondsTextFlash += (float)gameTime.ElapsedGameTime.TotalSeconds;
            textColor = secondsTextFlash % 2 < 1 ? Color.White : Color.Transparent;

            if (keyboardState.IsKeyDown(Keys.Enter))
            {
                start = true;
            }
            if (keyboardState.IsKeyDown(Keys.F) && keyboardState.IsKeyDown(Keys.U))
            {
                screen = Screen.Main;
            }
            if (start && vaultDoorRect.Left < window.Right)
            {
                vaultDoorRect.X += 1;
                vaultDoorRotation += 0.01f;
            }
            else if (vaultDoorRect.X >= window.Right)
            {
                screen = Screen.Main;
            }
        }

        private void HandleMainGame(GameTime gameTime, Player player)
        {
            secondsGun += (float)gameTime.ElapsedGameTime.TotalSeconds;
            secondsEnemy += (float)gameTime.ElapsedGameTime.TotalSeconds;
            secondsMoveDelay += (float)gameTime.ElapsedGameTime.TotalSeconds;

            var keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            HandlePlayerShooting(gameTime, keyboardState, mouseState);

            // Spawn enemies
            if (secondsEnemy >= spawnCoolDown)
            {
                SpawnEnemies();
                secondsEnemy = 0;
            }

            // Update bullets and enemies
            UpdateBullets();
            UpdateEnemies(gameTime);

            // Player movement
            player.HandleMovement(keyboardState, window);
        }

        private void HandlePlayerShooting(GameTime gameTime, KeyboardState keyboardState, MouseState mouseState)
        {
            if ((keyboardState.IsKeyDown(Keys.Space) || mouseState.LeftButton == ButtonState.Pressed) && !reloading)
            {
                if (secondsGun >= gunCoolDown && rectangleAmmoRect.Right > 0)
                {
                    bullets.Add(new Bullet(bulletTexture, player._location.Center.ToVector2(), mouseState.Position.ToVector2(), 10));
                    rectangleAmmoRect.X -= 1;
                    secondsGun = 0;
                }
            }

            if (rectangleAmmoRect.Right <= 0 && keyboardState.IsKeyDown(Keys.R))
            {
                reloading = true;
                secondsGun = 0;
            }

            if (reloading && secondsGun >= 2.7)
            {
                rectangleAmmoRect.X = 190;  // Reset ammo
                reloading = false;
            }
        }

        private void UpdateBullets()
        {
            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                bullets[i].Update();
                if (!window.Intersects(bullets[i].Rect))
                {
                    bullets.RemoveAt(i);
                }
            }
        }

        private void UpdateEnemies(GameTime gameTime)
        {
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                enemies[i].Move(player);
                enemies[i].Update(gameTime);

                // Check for collisions between enemies and bullets
                foreach (Bullet bullet in bullets)
                {
                    if (enemies[i].Collide(bullet.Rect))
                    {
                        enemies[i].TakeDamage(1);
                        bullets.Remove(bullet);
                        break;
                    }
                }

                if (!enemies[i].IsAlive())
                {
                    enemies.RemoveAt(i);
                }
            }
        }

        private void SpawnEnemies()
        {
            int randomSide = generator.Next(0, 4);
            switch (randomSide)
            {
                case 0: // Spawn at top
                    enemies.Add(new Enemy(eyeBotTexture, generator.Next(0, window.Width), -100));
                    break;
                case 1: // Spawn at bottom
                    enemies.Add(new Enemy(eyeBotTexture, generator.Next(0, window.Width), window.Bottom + 100));
                    break;
                case 2: // Spawn at left
                    enemies.Add(new Enemy(eyeBotTexture, -100, generator.Next(0, window.Height)));
                    break;
                case 3: // Spawn at right
                    enemies.Add(new Enemy(eyeBotTexture, window.Right + 100, generator.Next(0, window.Height)));
                    break;
            }
        }

        private void HandleControlsScreen(GameTime gameTime)
        {
            textColor = (secondsTextFlash % 2 < 1) ? Color.White : Color.Transparent;

            if (Keyboard.GetState().IsKeyDown(Keys.E))
            {
                screen = Screen.Main;
            }
        }

        private void HandleGameOverScreen()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            switch (screen)
            {
                case Screen.Intro:
                    DrawIntroScreen();
                    break;
                case Screen.Main:
                    DrawMainGame();
                    break;
                case Screen.Controls:
                    DrawControlsScreen();
                    break;
                case Screen.Gameover:
                    DrawGameOverScreen();
                    break;
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawIntroScreen()
        {
            _spriteBatch.Draw(introBackgroundTexture, introBackgroundRect, Color.White);
            _spriteBatch.Draw(vaultDoorTexture, new Vector2(vaultDoorRect.X, vaultDoorRect.Y), null, Color.White, vaultDoorRotation, new Vector2(vaultDoorRect.Width / 2, vaultDoorRect.Height / 2), 1f, SpriteEffects.None, 0f);
            _spriteBatch.DrawString(overseerFont, "Fallout Shootout", new Vector2(window.Center.X / 4 - 15, 18), Color.White);
            _spriteBatch.DrawString(overseerFontUI, "press Enter to start", new Vector2(266, 447), textColor);
        }

        private void DrawMainGame()
        {
            _spriteBatch.Draw(rectangleTexture, new Rectangle(0, 5, 200, 50), Color.White);
            _spriteBatch.Draw(rectangleTexture, new Rectangle(rectangleHealthRect.X, rectangleHealthRect.Y, player.Health, rectangleHealthRect.Height), Color.Green);
            _spriteBatch.Draw(rectangleTexture, rectangleAmmoRect, Color.Goldenrod);

            player.Draw(_spriteBatch);

            foreach (Enemy enemy in enemies)
            {
                enemy.Draw(_spriteBatch);
                // Health bar for enemies
                var healthBar = new Rectangle(enemy._location.Center.X - 15, enemy._location.Y - 10, (int)(30 * ((float)enemy.Health / enemy.MaxHealth)), 5);
                _spriteBatch.Draw(rectangleTexture, healthBar, enemy.Health > enemy.MaxHealth / 2 ? Color.Green : Color.Red);
            }

            foreach (Bullet bullet in bullets)
            {
                bullet.Draw(_spriteBatch);
            }
        }

        private void DrawControlsScreen()
        {
            _spriteBatch.Draw(youDiedTexture, window, Color.Black);
            _spriteBatch.DrawString(terminalFont, "W - Up\nA - Left\nS - Down\nD - Right\nLeft Click - Shoot", new Vector2(0, 0), Color.White);
            _spriteBatch.DrawString(terminalFont, "Press E to return", new Vector2(0, 170), textColor);
        }

        private void DrawGameOverScreen()
        {
            _spriteBatch.Draw(youDiedTexture, window, Color.White);
            _spriteBatch.DrawString(terminalFont, "your bones are scraped clean by the desolate wind,\n       your vault will now surely die, as you have.", new Vector2(60, 450), Color.DarkRed);
            _spriteBatch.DrawString(terminalFont, "press Escape and accept your fate", new Vector2(160, 525), Color.DarkRed);
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;

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
        Rectangle paMinigunRect;

        Texture2D vaultDoorTexture;
        Rectangle vaultDoorRect;

        Texture2D vaultEntryTexture;
        Rectangle vaultEntryRect;

        Texture2D vaultBoyTexture;
        Rectangle vaultBoyRect;

        Texture2D introBackgroundTexture;
        Rectangle introBackgroundRect;

        Rectangle rectangleHealthRect;
        Rectangle rectangleAmmoRect;
        Texture2D rectangleTexture;

        Texture2D youDiedTexture;

        Texture2D bulletTexture;

        Texture2D eyeBotTexture;

        Rectangle window;
        Texture2D backgroundTexture;

        float secondsGun, gunCoolDown;
        float secondsEnemy, spawnCoolDown;
        float secondsMoveDelay, moveCoolDown;
        float secondsTextFlash;

        float numRand;

        bool start;
        bool reloading;

        SpriteFont overseerFont;
        SpriteFont overseerFontUI;
        SpriteFont terminalFont;

        MouseState mouseState, prevMouseState;
        KeyboardState keyboardState, ks1, ks2;

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

            secondsGun = 0f;
            gunCoolDown = 0.05f;
            secondsEnemy = 0f;
            spawnCoolDown = 0.05f;
            secondsMoveDelay = 0f;
            moveCoolDown = 0.5f;

            paMinigunRect = new Rectangle(0, 0, 350, 350);
            rectangleHealthRect = new Rectangle(0, 10, 190, 40);
            rectangleAmmoRect = new Rectangle(0, 40, 190, 40);
            vaultDoorRect = new Rectangle(502, 463, 675, 725);
            vaultBoyRect = new Rectangle(0, 0, 1200, 1200);
            introBackgroundRect = new Rectangle(-350, -150, window.Width + 650, window.Height + 50);

            textColor = Color.Transparent;

            start = false;
            reloading = false;

            bullets = new List<Bullet>();
            enemies = new List<Enemy>();

            generator = new Random();

            screen = Screen.Intro;
            vaultDoorRotation = 0f;

            base.Initialize();
            player = new Player(paMinigunTexture, (window.Center.X - paMinigunRect.X / 2), (window.Center.Y - paMinigunRect.Y / 2));
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            paMinigunTexture = Content.Load<Texture2D>("final-pa-minigun");
            bulletTexture = Content.Load<Texture2D>("final-bullet");
            overseerFont = Content.Load<SpriteFont>("overseerFont");
            overseerFontUI = Content.Load<SpriteFont>("overseerFontUI");
            terminalFont = Content.Load<SpriteFont>("terminalFont");
            backgroundTexture = Content.Load<Texture2D>("desert-background");
            eyeBotTexture = Content.Load<Texture2D>("eyebot-pixilart");
            rectangleTexture = Content.Load<Texture2D>("Rectangle");
            vaultBoyTexture = Content.Load<Texture2D>("final-vaultboy");
            vaultDoorTexture = Content.Load<Texture2D>("Vault_65");
            vaultEntryTexture = Content.Load<Texture2D>("final-vaultentry");
            introBackgroundTexture = Content.Load<Texture2D>("final-desertlandscape");
            youDiedTexture = Content.Load<Texture2D>("youdied");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (screen == Screen.Intro)
            {
                mouseState = Mouse.GetState();
                keyboardState = Keyboard.GetState();
                Window.Title = " ";
                textColor = Color.White;
                secondsTextFlash += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (secondsTextFlash >= 0 && secondsTextFlash < 1)
                {
                    textColor = Color.White;
                }
                else if (secondsTextFlash >= 1 && secondsTextFlash < 2)
                {
                    textColor = Color.Transparent;
                }
                else if (secondsTextFlash >= 2)
                {
                    secondsTextFlash = 0; // Reset the timer
                }

                if (keyboardState.IsKeyDown(Keys.Enter))
                {
                    start = true;
                    secondsTextFlash = 0;
                }
                if (start == true && vaultDoorRect.Left < window.Right)
                {
                    vaultDoorRect.X += 1;
                    vaultDoorRotation += 0.01f; // Rotate the Vault door while opening
                }
                else if (vaultDoorRect.X >= window.Right)
                {
                    screen = Screen.Main;
                    textColor = Color.Transparent;
                }
            }
            else if (screen == Screen.Main)
            {
                secondsGun += (float)gameTime.ElapsedGameTime.TotalSeconds;
                secondsEnemy += (float)gameTime.ElapsedGameTime.TotalSeconds;
                secondsMoveDelay += (float)gameTime.ElapsedGameTime.TotalSeconds;

                keyboardState = Keyboard.GetState();
                ks1 = Keyboard.GetState();

                prevMouseState = mouseState;
                mouseState = Mouse.GetState();
                if ((keyboardState.IsKeyDown(Keys.Space) || mouseState.LeftButton == ButtonState.Pressed) && reloading == false)
                {
                    if (secondsGun >= gunCoolDown && rectangleAmmoRect.Right > 0)
                    {
                        textColor = Color.Transparent;
                        bullets.Add(new Bullet(bulletTexture, player._location.Center.ToVector2(), mouseState.Position.ToVector2(), 10));
                        rectangleAmmoRect.X -= 1;
                        secondsGun = 0;
                    }
                }
                else if (rectangleAmmoRect.Right <= 0)
                {
                    if (keyboardState.IsKeyDown(Keys.R))
                    {
                        rectangleAmmoRect.X = 0;
                    }
                    reloading = true;
                    textColor = Color.Black;
                    if (secondsGun >= 2.7)
                    {
                        rectangleAmmoRect.X = 0;
                        textColor = Color.Transparent;
                        secondsGun = 0;
                        reloading = false;
                    }
                }

                // Update bullets and check for collisions
                for (int i = 0; i < bullets.Count; i++)
                {
                    bullets[i].Update();

                    if (!window.Intersects(bullets[i].Rect)) // Removes bullets after they leave the window
                    {
                        bullets.RemoveAt(i);
                        i--;
                    }
                }

                // Spawn enemies at random intervals
                if (secondsEnemy >= 0.4f)
                {
                    numRand = generator.Next(0, 4);
                    // Top
                    if (numRand == 0)
                    {
                        enemies.Add(new Enemy(eyeBotTexture, generator.Next(0, 700), -100));
                    }
                    // Bottom
                    else if (numRand == 1)
                    {
                        enemies.Add(new Enemy(eyeBotTexture, generator.Next(0, 700), 800));
                    }
                    // Left
                    else if (numRand == 2)
                    {
                        enemies.Add(new Enemy(eyeBotTexture, -100, generator.Next(0, 700)));
                    }
                    // Right
                    else if (numRand == 3)
                    {
                        enemies.Add(new Enemy(eyeBotTexture, 800, generator.Next(0, 700)));
                    }
                    secondsEnemy = 0;
                }

                // Check collisions between bullets and enemies
                // Followed a tutorial and is a little confusing, so I'll try to explain it a bit

                List<Enemy> enemiesToRemove = new List<Enemy>();
                List<Bullet> bulletsToRemove = new List<Bullet>();

                for (int i = bullets.Count - 1; i >= 0; i--)
                {
                    Bullet bullet = bullets[i];
                    bool bulletRemoved = false;

                    foreach (Enemy enemy in enemies)
                    {
                        if (enemy.Collide(bullet.Rect))
                        {
                            enemy.TakeDamage(1); // Reduce enemy health
                            bulletsToRemove.Add(bullet); // Mark bullet for removal

                            if (!enemy.IsAlive())
                            {
                                enemiesToRemove.Add(enemy); // Mark enemy for removal if health <= 0
                            }

                            bulletRemoved = true; // Mark bullet as removed in this iteration
                            break; // No need to check further enemies for this bullet
                        }
                    }

                    // Remove bullet if it collided with any enemy
                    if (bulletRemoved)
                    {
                        bullets.RemoveAt(i);
                    }
                }

                foreach (Enemy enemy in enemies)
                {
                    if (enemy.Collide(player._location))
                    {
                        player.TakeDamage(12);
                    }
                }
                if (!player.IsAlive())
                {
                    screen = Screen.Gameover;
                }
                // Remove enemies and bullets marked for removal
                foreach (Enemy enemyToRemove in enemiesToRemove)
                {
                    enemies.Remove(enemyToRemove);
                }

                foreach (Bullet bulletToRemove in bulletsToRemove)
                {
                    bullets.Remove(bulletToRemove);
                }

                // Update remaining enemies
                foreach (Enemy enemy in enemies)
                {
                    if (secondsMoveDelay >= moveCoolDown)
                    {
                        enemy.Move(player);
                    }

                    enemy.Update();
                }

                if (secondsMoveDelay >= moveCoolDown)
                    secondsMoveDelay = 0;

                // Handle player movement
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
                player.Update(gameTime, window);

                rectangleHealthRect.X -= 0;

                if (rectangleHealthRect.Right < 0)
                {
                    Exit();
                }

                Window.Title = "Objective: Survive";

                base.Update(gameTime);
            }
            else if (screen == Screen.Controls)
            {

            }
            else if (screen == Screen.Gameover)
            {

            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Start drawing
            _spriteBatch.Begin();
            if (screen == Screen.Intro)
            {
                _spriteBatch.Draw(introBackgroundTexture, introBackgroundRect, Color.White);

                // Draw the vault door with rotation
                _spriteBatch.Draw(vaultDoorTexture, new Vector2(vaultDoorRect.X, vaultDoorRect.Y), null, Color.White, vaultDoorRotation, new Vector2(vaultDoorRect.Width / 2, vaultDoorRect.Height / 2), 1f, SpriteEffects.None, 0f);

                _spriteBatch.Draw(vaultEntryTexture, window, Color.White);
                if (start == false)
                {
                    _spriteBatch.Draw(vaultBoyTexture, new Rectangle(-150, 230, vaultBoyRect.Width - 500, vaultBoyRect.Height - 500), Color.White);
                    _spriteBatch.DrawString(overseerFont, "Fallout Shootout", new Vector2(window.Center.X / 4 - 15, 18), Color.White);
                    _spriteBatch.DrawString(overseerFontUI, "press Enter to start", new Vector2(266, 447), textColor);
                }
                else
                {
                    _spriteBatch.Draw(vaultBoyTexture, new Rectangle(-150, 230, vaultBoyRect.Width - 500, vaultBoyRect.Height - 500), Color.Transparent);
                    _spriteBatch.DrawString(overseerFont, "Fallout Shootout", new Vector2(window.Center.X / 4 - 15, 18), Color.White);
                    _spriteBatch.DrawString(overseerFontUI, "press Enter to start", new Vector2(266, 447), Color.Transparent);
                }

            }
            else if (screen == Screen.Main)
            {
                _spriteBatch.Draw(backgroundTexture, window, Color.White);

                // Draw health and ammo bars
                _spriteBatch.Draw(rectangleTexture, new Rectangle(0, 5, 200, 50), Color.White);
                _spriteBatch.Draw(rectangleTexture, new Rectangle(rectangleHealthRect.X, rectangleHealthRect.Y, player.Health, rectangleHealthRect.Height), Color.Green);
                _spriteBatch.Draw(rectangleTexture, new Rectangle(0, 35, 200, 50), Color.White);
                _spriteBatch.Draw(rectangleTexture, rectangleAmmoRect, Color.Goldenrod);

                _spriteBatch.DrawString(overseerFontUI, "Reloading", new Vector2(player._location.X, player._location.Top), textColor);

                // Draw player, enemies, and bullets
                player.Draw(_spriteBatch);
                foreach (Enemy enemy in enemies)
                {
                    enemy.Draw(_spriteBatch);
                    Rectangle healthBarRect = new Rectangle(enemy._location.Center.X - 20, enemy._location.Bottom, enemy._location.Width, 4);
                    float healthPercentage = (float)enemy.Health / enemy.MaxHealth; // Calculate percentage of health remaining


                    // Draw health bar foreground (green or red, indicating remaining health)
                    Rectangle healthBarFillRect = new Rectangle(healthBarRect.Left, healthBarRect.Bottom - 6, (int)(healthBarRect.Width * healthPercentage), healthBarRect.Height);
                    if (healthPercentage <= 1 && healthPercentage > 0.5)
                    {
                        _spriteBatch.Draw(rectangleTexture, healthBarFillRect, Color.Green);
                    }
                    else if (healthPercentage <= 0.5)
                    {
                        _spriteBatch.Draw(rectangleTexture, healthBarFillRect, Color.Red);
                    }

                }

                foreach (Bullet bullet in bullets)
                {
                    bullet.Draw(_spriteBatch);
                }
                // Draw health and ammo bars
                _spriteBatch.Draw(rectangleTexture, new Rectangle(0, 5, 200, 50), Color.White);
                _spriteBatch.Draw(rectangleTexture, new Rectangle(rectangleHealthRect.X, rectangleHealthRect.Y, player.Health, rectangleHealthRect.Height), Color.Green);
                _spriteBatch.Draw(rectangleTexture, new Rectangle(0, 35, 200, 50), Color.White);
                _spriteBatch.Draw(rectangleTexture, rectangleAmmoRect, Color.Goldenrod);
            }
            else if (screen == Screen.Controls)
            {
                // Draw controls screen elements
            }
            else if (screen == Screen.Gameover)
            {
                // Draw game over screen elements
                 
                _spriteBatch.Draw(youDiedTexture, window, Color.White);
                _spriteBatch.DrawString(terminalFont, "your bones are scraped clean by the desolate wind,\n       your vault will now surely die, as you have.", new Vector2(60, 450), Color.DarkRed);
                _spriteBatch.DrawString(terminalFont, "press Escape and accept your fate", new Vector2(160, 525), Color.DarkRed);

            }


            _spriteBatch.End();
            base.Draw(gameTime);

            // Thank you so much for your help Mr Aldworth!

            // And thank you for your encouragement and support, Alanna, Ethan, Geoffrey, and Merrick!
        }
    }
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;

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
        Rectangle paMinigunRect;

        Rectangle rectangleHealthRect;
        Rectangle rectangleAmmoRect;
        Texture2D rectangleTexture;

        Texture2D bulletTexture;

        Texture2D eyeBotTexture;

        Rectangle window;
        Texture2D backgroundTexture;
        
        float secondsGun, gunCoolDown;
        float secondsEnemy, spawnCoolDown;
        float secondsMoveDelay, moveCoolDown;
        float numRand;
        
        SpriteFont overseerFont;
        SpriteFont overseerFontUI;

        MouseState mouseState, prevMouseState;
        KeyboardState keyboardState, ks1, ks2;

        Random generator;

        Color textColor;
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

            secondsGun = 0f;
            gunCoolDown = 0.05f;
            secondsEnemy = 0f;
            spawnCoolDown = 0.5f;
            secondsMoveDelay = 0f;
            moveCoolDown = 0.5f;
            //paMinigunRect = new Rectangle(0, 200, 200, 200);
            //bulletRect = new Rectangle(100, 100, 5, 5);
            paMinigunRect = new Rectangle(0, 0, 350, 350);
            rectangleHealthRect = new Rectangle(0, 10, 190, 40);
            rectangleAmmoRect = new Rectangle(0, 40, 190, 40);

            textColor = Color.Transparent;

            bullets = new List<Bullet>();
            enemies = new List<Enemy>();

            generator = new Random();

            base.Initialize();
            player = new Player(paMinigunTexture, paMinigunRect.X, paMinigunRect.Y);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            paMinigunTexture = Content.Load<Texture2D>("final-pa-minigun");
            bulletTexture = Content.Load<Texture2D>("final-bullet");
            overseerFont = Content.Load<SpriteFont>("overseerFont");
            overseerFontUI = Content.Load<SpriteFont>("overseerFontUI");
            backgroundTexture = Content.Load<Texture2D>("desert-background");
            eyeBotTexture = Content.Load<Texture2D>("eyebot-pixilart");
            rectangleTexture = Content.Load<Texture2D>("Rectangle");


            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            secondsGun += (float)gameTime.ElapsedGameTime.TotalSeconds;
            secondsEnemy += (float)gameTime.ElapsedGameTime.TotalSeconds;
            secondsMoveDelay += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // TODO: Add your update logic here
            keyboardState = Keyboard.GetState();
            ks1 = Keyboard.GetState();

            prevMouseState = mouseState;
            mouseState = Mouse.GetState();

            if (keyboardState.IsKeyDown(Keys.Space))
            {
                if (secondsGun >= gunCoolDown && rectangleAmmoRect.Right > 0)
                {
                    textColor = Color.Transparent;
                    bullets.Add(new Bullet(bulletTexture, player._location.Center.ToVector2(), mouseState.Position.ToVector2(), 10));
                    rectangleAmmoRect.X -= 1;
                    secondsGun = 0;
                }
                else if (rectangleAmmoRect.Right <= 0)
                {
                    textColor = Color.Black;
                    if (secondsGun >= 3 || keyboardState.IsKeyDown(Keys.R))
                    {
                        rectangleAmmoRect.X = 0;
                        textColor = Color.Transparent;
                        secondsGun = 0;
                    }
                }
                
            }
            
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Update();

                if (!window.Intersects(bullets[i].Rect)) // Removes bullets after they leave the window
                {
                    bullets.RemoveAt(i);
                    i--;
                }
            }
            if (secondsEnemy >= 0.3f) // Drawing enemies is broken right now
            {
                numRand = generator.Next(0, 3);
                if (numRand == 0)
                {
                    // Top
                    enemies.Add(new Enemy(eyeBotTexture, generator.Next(0, 100), 0));

                }
                else if (numRand == 1)
                {
                    // Bottom
                    enemies.Add(new Enemy(eyeBotTexture, generator.Next(0, 100), 100));

                }
                else if(numRand == 2)
                {
                    // Right
                    enemies.Add(new Enemy(eyeBotTexture, 100, generator.Next(0, 100)));

                }
                else if (numRand >= 3)
                {
                    // Left
                    enemies.Add(new Enemy(eyeBotTexture, 0, generator.Next(0, 100)));

                }
                secondsEnemy = 0;
            }
            //for (int i = 0; i < bullets.Count; i++)
            //{
            //    bullets[i].Update();
            //}
            List<Enemy> enemiesToRemove = new List<Enemy>();
            List<Bullet> bulletsToRemove = new List<Bullet>();

            // Check collisions and mark enemies and bullets for removal
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

            // Remove enemies marked for removal
            foreach (Enemy enemyToRemove in enemiesToRemove)
            {
                enemies.Remove(enemyToRemove);
            }

            // Remove bullets marked for removal
            foreach (Bullet bulletToRemove in bulletsToRemove)
            {
                bullets.Remove(bulletToRemove);
            }

            // Update remaining enemies
            foreach (Enemy enemy in enemies)
            {
                // Recalculate enemy speed
                if (secondsMoveDelay >= moveCoolDown)
                {
                    enemy.Move(player);
                }

                enemy.Update();
            }
            if (secondsMoveDelay >= moveCoolDown)
                secondsMoveDelay = 0;

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

            player.Update(gameTime);
            rectangleHealthRect.X -= 0;

            if (rectangleHealthRect.Right < 0)
            {
                Exit();
            }
            Window.Title = mouseState.Position + "";

            base.Update(gameTime);
        }
        

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(backgroundTexture, window, Color.White);

            

            _spriteBatch.Draw(rectangleTexture, new Rectangle(0, 5, 200, 50), Color.White);
            _spriteBatch.Draw(rectangleTexture, rectangleHealthRect, Color.DarkRed);
            _spriteBatch.Draw(rectangleTexture, new Rectangle(0, 35, 200, 50), Color.White);
            _spriteBatch.Draw(rectangleTexture, rectangleAmmoRect, Color.Goldenrod);

            _spriteBatch.DrawString(overseerFontUI, "Reloading", new Vector2(player._location.X, player._location.Top), textColor);

            player.Draw(_spriteBatch);
            foreach (Enemy enemy in enemies)
            {
                enemy.Draw(_spriteBatch); Rectangle healthBarRect = new Rectangle(enemy._location.X, enemy._location.Left - 10, enemy._location.Y, 5);
                float healthPercentage = (float)enemy.Health / enemy.MaxHealth; // Calculate percentage of health remaining

                // Draw health bar background (gray or red)
                _spriteBatch.Draw(rectangleTexture, healthBarRect, Color.Gray);

                // Draw health bar foreground (green or red, indicating remaining health)
                Rectangle healthBarFillRect = new Rectangle(healthBarRect.Left, healthBarRect.Top, (int)(healthBarRect.Width * healthPercentage), healthBarRect.Height);
                _spriteBatch.Draw(rectangleTexture, healthBarFillRect, Color.Green);
            }


            foreach (Bullet bullet in bullets)
            {
                bullet.Draw(_spriteBatch);
            }
                


            _spriteBatch.End();
            base.Draw(gameTime);







































            // Thank you so much for your help Mr Aldworth!
        }
    }
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace MonoGameFinal___Fallout_Shootout
{
    class Player
    {
        private Texture2D _texture;
        public Rectangle _location;
        public Vector2 _speed;
        public float _angle;
        private float secondsDamageDelay;
        public int Health { get; set; }
        public int MaxHealth { get; set; }

        public Player(Texture2D texture, int x, int y)
        {
            _texture = texture;
            _location = new Rectangle(x, y, 100, 100);
            _speed = Vector2.Zero;
            _angle = 0f;
            Health = 190;
            MaxHealth = 190;
            secondsDamageDelay = 0f;
        }

        public void TakeDamage(int damage)
        {
            // Apply damage only if enough time has passed
            if (secondsDamageDelay > 0.3f)
            {
                Health -= damage;
                secondsDamageDelay = 0f;
            }

            if (Health < 0)
            {
                Health = 0; // Prevent health from going negative
            }
        }

        public bool IsAlive()
        {
            return Health > 0;
        }

        public float HSpeed
        {
            get { return _speed.X; }
            set { _speed.X = value; }
        }

        public float VSpeed
        {
            get { return _speed.Y; }
            set { _speed.Y = value; }
        }

        private void HandleMovement(Rectangle window)
        {
            // Move player based on current speed
            _location.X += (int)_speed.X;
            _location.Y += (int)_speed.Y;

            // Ensure player stays within window boundaries
            if (_location.Left < window.Left)
                _location.X = window.Left;
            if (_location.Right > window.Right)
                _location.X = window.Right - _location.Width;
            if (_location.Top < window.Top)
                _location.Y = window.Top;
            if (_location.Bottom > window.Bottom)
                _location.Y = window.Bottom - _location.Height;
        }

        public void UndoMove()
        {
            // Undo the last move
            _location.X -= (int)_speed.X;
            _location.Y -= (int)_speed.Y;
        }

        public void Update(GameTime gameTime, Rectangle window)
        {
            // Update position and handle boundaries
            HandleMovement(window);

            // Calculate the player's facing direction based on speed
            if (_speed != Vector2.Zero)
            {
                _angle = (float)Math.Atan2(_speed.Y, _speed.X);
            }

            // Update damage delay timer
            secondsDamageDelay += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public bool Collide(Rectangle item)
        {
            return _location.Intersects(item);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the player, flipping the sprite based on movement angle
            SpriteEffects effects = (_angle >= -2.4 && _angle < -1.6) || (_angle <= 3.15 && _angle >= 1.6) ? SpriteEffects.FlipVertically : SpriteEffects.None;

            // Center player texture while rotating it at the calculated angle
            spriteBatch.Draw(
                _texture,
                new Rectangle(_location.Center, _location.Size),
                null,
                Color.White,
                _angle,
                new Vector2(_texture.Width / 2, _texture.Height / 2),
                effects,
                1f
            );
        }
    }
    class Enemy
    {
        private readonly Texture2D _texture;
        public Rectangle _location;
        public Vector2 _speed;
        public int Health { get; private set; }
        public int MaxHealth { get; private set; }

        private float _hoverOffset;
        private readonly float _hoverSpeed = 2.0f;  // Controls how fast the enemy hovers
        private readonly float _hoverHeight = 5.0f; // Controls how high the hover is
        private float _time;               // Keeps track of time for sine wave

        public Enemy(Texture2D texture, int x, int y)
        {
            _texture = texture ?? throw new ArgumentNullException(nameof(texture));
            _location = new Rectangle(x, y, 30, 60);
            _speed = new Vector2(1.2f, 1.2f);
            Health = 4;
            MaxHealth = 4;
            _time = 0f;  // Initialize the time to 0
        }

        public void TakeDamage(int damage)
        {
            Health = Math.Max(0, Health - damage); // Ensure health doesn't go negative
        }

        public void Move(Player player)
        {
            if (player == null) throw new ArgumentNullException(nameof(player));

            _speed.X = player._location.Center.X > _location.Center.X ? 1.2f : player._location.Center.X < _location.Center.X ? -1.2f : 0;
            _speed.Y = player._location.Center.Y > _location.Center.Y ? 1.2f : player._location.Center.Y < _location.Center.Y ? -1.2f : 0;

            if (player._location.Top == _location.Bottom || player._location.Bottom == _location.Top)
            {
                _speed.Y = 0;
            }

            if (player._location.Left == _location.Right || player._location.Right == _location.Left)
            {
                _speed.X = 0;
            }
        }

        public bool IsAlive() => Health > 0;

        public bool Collide(Rectangle item) => _location.Intersects(item);

        public void Update(GameTime gameTime)
        {
            if (gameTime == null) throw new ArgumentNullException(nameof(gameTime));

            // Update the hover effect
            _time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            _hoverOffset = (float)Math.Sin(_time * _hoverSpeed) * _hoverHeight;

            // Apply the hover offset to the Y-axis
            _location.Y = (int)(_location.Y + _hoverOffset);

            // Move based on speed
            _location.Offset(_speed);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (spriteBatch == null) throw new ArgumentNullException(nameof(spriteBatch));

            spriteBatch.Draw(_texture, _location, Color.White);
        }
    }
}

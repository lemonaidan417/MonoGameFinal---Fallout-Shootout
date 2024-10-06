using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGameFinal___Fallout_Shootout
{
    class Enemy
    {
        private Texture2D _texture;
        public Rectangle _location;
        public Vector2 _speed;
        public int Health { get; set; }
        public int MaxHealth { get; set; }

        private float _hoverOffset;
        private float _hoverSpeed = 2.0f;  // Controls how fast the enemy hovers
        private float _hoverHeight = 5.0f; // Controls how high the hover is
        private float _time;               // Keeps track of time for sine wave

        public Enemy(Texture2D texture, int x, int y)
        {
            _texture = texture;
            _location = new Rectangle(x, y, 30, 60);
            _speed = new Vector2(1.2f, 1.2f);
            Health = 4;
            MaxHealth = 4;
            _time = 0f;  // Initialize the time to 0
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health < 0)
            {
                Health = 0; // Ensure health doesn't go negative
            }
        }

        public void Move(Player player)
        {
            if (player._location.Center.X > _location.Center.X)
            {
                _speed.X = 1.2f;
            }
            else if (player._location.Center.X < _location.Center.X)
            {
                _speed.X = -1.2f;
            }

            if (player._location.Center.Y > _location.Center.Y)
            {
                _speed.Y = 1.2f;
            }
            else if (player._location.Center.Y < _location.Center.Y)
            {
                _speed.Y = -1.2f;
            }

            if (player._location.Top == _location.Bottom || player._location.Bottom == _location.Top)
            {
                _speed.Y = 0;
            }

            if (player._location.Left == _location.Right || player._location.Right == _location.Left)
            {
                _speed.X = 0;
            }
        }

        public bool IsAlive()
        {
            return Health > 0;
        }

        public bool Collide(Rectangle item)
        {
            return _location.Intersects(item);
        }

        public void Update(GameTime gameTime)
        {
            // Update the hover effect
            _time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            _hoverOffset = (float)Math.Sin(_time * _hoverSpeed) * _hoverHeight;

            // Apply the hover offset to the Y-axis
            Vector2 position = new Vector2(_location.X, _location.Y + _hoverOffset);
            _location = new Rectangle(position.ToPoint(), _location.Size);

            // Move based on speed
            _location.Offset(_speed);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _location, Color.White);
        }
    }
}

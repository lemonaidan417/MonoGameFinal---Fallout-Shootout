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

        public Enemy(Texture2D texture, int x, int y)
        {
            _texture = texture;
            _location = new Rectangle(x, y, 40, 50);
            _speed = new Vector2(1.2f, 1.2f);
            Health = 5;
            MaxHealth = 5;
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

            if (player._location.Top == _location.Bottom)
            {
                _speed.Y = 0;
            }
            else if (player._location.Bottom == _location.Top)
            {
                _speed.Y = 0;
            }

            if (player._location.Left == _location.Right)
            {
                _speed.X = 0;
            }
            else if (player._location.Right == _location.Left)
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

        public void Update()
        {
            _location.Offset(_speed);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _location, Color.White);
        }
    }
}
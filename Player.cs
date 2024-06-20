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
        public int Health { get; set; }
        public int MaxHealth { get; set; }

        public Player(Texture2D texture, int x, int y)
        {
            _texture = texture;
            _location = new Rectangle(x, y, 100, 100);
            _speed = Vector2.Zero;
            _angle = 0f;
            Health = 50;
            MaxHealth = 50;
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health < 0)
            {
                Health = 0; // Ensure health doesn't go negative
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

        private void Move()
        {
            _location.X += (int)_speed.X;
            _location.Y += (int)_speed.Y;
        }

        public void UndoMove()
        {
            _location.X -= (int)_speed.X;
            _location.Y -= (int)_speed.Y;
        }

        public void Update(GameTime gameTime)
        {
            Move();
            _angle = (float)Math.Atan2(_speed.Y, _speed.X);
        }

        public bool Collide(Rectangle item)
        {
            return _location.Intersects(item);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
            if ((_angle >= -2.4 && _angle < -1.6) || (_angle <= 3.15 && _angle >= 1.6))
            {
                spriteBatch.Draw(_texture, new Rectangle(_location.Center, _location.Size), null, Color.White, _angle, new Vector2(_texture.Width / 2, _texture.Height / 2), SpriteEffects.FlipVertically, 1f);

            }
            else
            {
                spriteBatch.Draw(_texture, new Rectangle(_location.Center, _location.Size), null, Color.White, _angle, new Vector2(_texture.Width / 2, _texture.Height / 2), SpriteEffects.None, 1f);
            }
        }
    }
}
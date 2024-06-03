using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGameFinal___Fallout_Shootout
{
    class Player
    {
        private Texture2D _texture;
        private Rectangle _location;
        private Vector2 _speed;

        public Player(Texture2D texture, int x, int y)
        {
            _texture = texture;
            _location = new Rectangle(x, y, 100, 100);
            _speed = new Vector2();
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


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _location, Color.White);
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

        public void Update()
        {
            Move();
        }

        public bool Collide(Rectangle item)
        {
            return _location.Intersects(item);
        }

        public Boolean Contains(Rectangle item)
        {
            return _location.Contains(item);
        }

        public void Grow()
        {
            _location.Width += 1;
            _location.Height += 1;
        }
    }
}

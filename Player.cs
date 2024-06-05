﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace MonoGameFinal___Fallout_Shootout
{
    class Player
    {
        private Texture2D _texture;
        private Rectangle _location;
        private Vector2 _speed;
        private float _angle;

        public Player(Texture2D texture, int x, int y)
        {
            _texture = texture;
            _location = new Rectangle(x, y, 100, 100);
            _speed = new Vector2();
            _angle = 0f;
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
            spriteBatch.Draw(_texture, new Rectangle(_location.Center, _location.Size), null, Color.White, _angle, new Vector2(_texture.Width / 2, _texture.Height / 2), SpriteEffects.None, 1f);
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
            if (_speed != Vector2.Zero)
            {
                _speed.Normalize();
            }
            _angle = (float)Math.Atan2(_speed.Y, _speed.X);
            
            
        }

        public bool Collide(Rectangle item)
        {
            return _location.Intersects(item);
        }

        public Boolean Contains(Rectangle item)
        {
            return _location.Contains(item);
        }
    }
}
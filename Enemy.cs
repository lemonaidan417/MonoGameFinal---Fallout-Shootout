﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Reflection.Metadata;

namespace MonoGameFinal___Fallout_Shootout
{
    class Enemy
    {
        public static List<Enemy> enemies = new List<Enemy>();
        private Texture2D _texture;
        public Rectangle _location;
        public Rectangle _rect;
        public Vector2 _speed;


        public Enemy(Texture2D texture, int x, int y)
        {
            _texture = texture;
            _location = new Rectangle(x, y, 200, 200);
            _speed = new Vector2(1.2f, 1.2f);
            _rect = new Rectangle(_location.Location, new Point(_location.X, _location.Y));
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
        }
        public void Update() 
        {
            _location.Offset(_speed);
        }
        public Rectangle Rect
        {
            get { return _rect; }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, new Rectangle(_location.Center, _location.Size), null, Color.White, 0, new Vector2(_texture.Width / 2, _texture.Height / 2), SpriteEffects.None, 1f);
        }
        public bool Collide(Rectangle item)
        {
            return _rect.Intersects(item);
        }

        public Boolean Contains(Rectangle item)
        {
            return _rect.Contains(item);
        }
    }
}

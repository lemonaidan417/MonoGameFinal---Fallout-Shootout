﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Reflection.Metadata;

namespace MonoGameFinal___Fallout_Shootout
{
    internal class Bullet
    {
        private Texture2D _texture;
        private Rectangle _rect;
        private Vector2 _location;
        private Vector2 _direction;
        private float _angle;
        private Vector2 _speed;
        private int _size;

        public Bullet(Texture2D texture, Vector2 location, Vector2 target, int size)
        {
            _size = size;
            _texture = texture;
            _location = location;
            _rect = new Rectangle(location.ToPoint(), new Point(_size, _size));
            _direction = target - location;
            _direction.Normalize();
            _speed = new Vector2(10);
        }

        // Allows read access to the location Rectangle for collision detection
        public Rectangle Rect
        {
            get { return _rect; }
        }

        public void Update()
        {
            _location += _direction * _speed;
            _rect.Location = _location.ToPoint();
            _angle = (float)Math.Atan2(_speed.Y, _speed.X);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, new Rectangle(_rect.Center, _rect.Size), null, Color.White, _angle, new Vector2(_texture.Width / 2, _texture.Height / 2), SpriteEffects.None, 1f);

        }







































        // Thank you so much for your help Mr Aldworth!
    }
}

using Microsoft.Xna.Framework.Graphics;
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
        public Vector2 _speed;


        public Enemy(Texture2D texture, int x, int y)
        {
            _texture = texture;
            _location = new Rectangle(x, y, 200, 200);
            _speed = new Vector2(1.2f, 1.2f);
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
        public void HandleCollisions()
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                for (int j = i + 1; j < enemies.Count; j++)
                {
                    if (enemies[i]._location.Intersects(enemies[j]._location))
                    {
                        ResolveCollision(enemies[i], enemies[j]);
                    }
                }
            }
        }

        public void ResolveCollision(Enemy enemy1, Enemy enemy2)
        {
            // Basic collision resolution by moving enemies apart
            var overlap = GetOverlap(enemy1._location, enemy2._location);

            if (overlap.X > overlap.Y)
            {
                // Vertical collision
                if (enemy1._location.Y < enemy2._location.Y)
                {
                    enemy1._location.Y -= (int)overlap.Y / 2;
                    enemy2._location.Y += (int)overlap.Y / 2;
                }
                else
                {
                    enemy1._location.Y += (int)overlap.Y / 2;
                    enemy2._location.Y -= (int)overlap.Y / 2;
                }
            }
            else
            {
                // Horizontal collision
                if (enemy1._location.X < enemy2._location.X)
                {
                    enemy1._location.X -= (int)overlap.X / 2;
                    enemy2._location.X += (int)overlap.X / 2;
                }
                else
                {
                    enemy1._location.X += (int)overlap.X / 2;
                    enemy2._location.X -= (int)overlap.X / 2;
                }
            }
        }

        public Vector2 GetOverlap(Rectangle rect1, Rectangle rect2)
        {
            float overlapX = Math.Min(rect1.Right, rect2.Right) - Math.Max(rect1.Left, rect2.Left);
            float overlapY = Math.Min(rect1.Bottom, rect2.Bottom) - Math.Max(rect1.Top, rect2.Top);
            return new Vector2(overlapX, overlapY);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, new Rectangle(_location.Center, _location.Size), null, Color.White, 0, new Vector2(_texture.Width / 2, _texture.Height / 2), SpriteEffects.None, 1f);
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

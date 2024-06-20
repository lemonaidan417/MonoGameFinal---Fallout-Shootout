using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameFinal___Fallout_Shootout
{
    public class HealthBar
    {
        private Texture2D texture;
        private Rectangle backgroundRect;
        private Rectangle fillRect;

        public HealthBar(Texture2D texture, Rectangle backgroundRect)
        {
            this.texture = texture;
            this.backgroundRect = backgroundRect;
            this.fillRect = backgroundRect;
        }

        public void Update(float healthPercentage)
        {
            fillRect.Width = (int)(backgroundRect.Width * healthPercentage);
        }

        public void Draw(SpriteBatch spriteBatch, Color fillColor)
        {
            spriteBatch.Draw(texture, backgroundRect, Color.Gray);
            spriteBatch.Draw(texture, fillRect, fillColor);
        }
    }
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3
{
    public class Explosion
    {
        private static Texture2D texture;

        private Vector2 position;

        private Color color;

        private Vector2 scale;

        private readonly Animation animation;

        public Explosion(Vector2 startPosition, Color _color)
        {
            texture ??= TextureManager.explosionTexture;
            animation = new(texture, 11, 15, 0.1f, 7);
            position = startPosition;
            color = _color;
            scale = new(2, 2);
        }

        public void Update(GameTime gameTime)
        {
            animation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch, position, scale, color);
        }
    }
}

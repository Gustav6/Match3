using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3
{
    public class Animation
    {
        private readonly Texture2D texture;

        private readonly List<Rectangle> sourceRects = new();

        private readonly int frames;

        private int frame;

        private readonly float frameTime;

        private float fraimeTimeLeft;

        private bool isActive = true;

        public Animation(Texture2D _texture, int framesX, int framesY, float _frameTime, int row = 1)
        {
            texture = _texture;
            frameTime = _frameTime;
            fraimeTimeLeft = _frameTime;
            frames = framesX;

            var frameWidth = _texture.Width / framesX;
            var frameHeight = _texture.Height / framesY;

            for (int i = 0; i < frames; i++)
            {
                sourceRects.Add(new(i * frameWidth, (row - 1) * frameHeight, frameWidth, frameHeight));
            }
        }

        public void Stop()
        {
            isActive = false;
        }

        public void Start()
        {
            isActive = true;
        }

        public void Update(GameTime gameTime)
        {
            if (!isActive)
            {
                return;
            }

            fraimeTimeLeft -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (fraimeTimeLeft <= 0)
            {
                fraimeTimeLeft += frameTime;
                frame = (frame + 1) % frames;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(texture, position, sourceRects[frame], Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 1);
        }
    }
}

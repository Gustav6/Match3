using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3
{
    public class Gem : Moveable
    {
        public Rectangle boundingBox;

        public Gem(Vector2 startPosition, int gemType)
        {
            position = startPosition;
            texutre = TextureManager.textures[gemType];
            color = Color.White;
            moveSpeed = 100;
            scale = new Vector2(1, 1);

            boundingBox = new Rectangle((int)position.X, (int)position.Y, texutre.Width, texutre.Height);
            SetSourceRectangle(TextureManager.textures[0]);
        }

        public override void Update(GameTime gameTime)
        {
            boundingBox.Location = position.ToPoint();
            base.Update(gameTime);
        }

        public void DrawBoundingBox()
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}

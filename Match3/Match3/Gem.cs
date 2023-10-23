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
        public int gemType;

        public Gem(Vector2 startPosition, int _gemType)
        {
            position = startPosition;
            texutre = TextureManager.textures[_gemType];
            gemType = _gemType;
            color = Color.White;
            moveSpeed = 100;
            velociy.Y = 1;
            scale = new Vector2(1, 1);

            boundingBox = new Rectangle((int)position.X, (int)position.Y, texutre.Width, texutre.Height);
            SetSourceRectangle(TextureManager.textures[0]);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            //boundingBox.Location = position.ToPoint();
        }

        public void DrawBoundingBox(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureManager.boundingBoxTexture, boundingBox, Color.White);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            //DrawBoundingBox(spriteBatch);
        }
    }
}

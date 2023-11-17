using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3
{
    public abstract class InteractableGui : GuiObject
    {
        public Rectangle boundingBox;

        public override void Update(GameTime gameTime)
        {

        }

        public void SetBoundingBoxSizeAndLocation(int width, int height)
        {
            boundingBox = new Rectangle(0, 0, width, height);
            boundingBox.Location = new Vector2(position.X - width / 2, position.Y - height / 2).ToPoint();
        }

        public void DrawBoundingBox(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureManager.boundingBoxTexture, boundingBox, Color.White);
        }
    }
}

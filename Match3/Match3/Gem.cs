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
        public Gem(Vector2 startPosition, Texture2D gemType)
        {
            position = startPosition;
            texutre = gemType;
            color = Color.White;
            scale = new Vector2(1, 1);
            SetSourceRectangle(TextureManager.arrayOfTextures[0]);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}

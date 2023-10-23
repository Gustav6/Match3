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
            scale = new Vector2(1, 1);

            boundingBox = new Rectangle((int)position.X, (int)position.Y, texutre.Width, texutre.Height);
            SetSourceRectangle(TextureManager.textures[0]);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void Dir(String dir)
        {
            if (dir == "Left")
                velociy = new Vector2(-1, 0);
            if (dir == "Right")
                velociy = new Vector2(1, 0);
            if (dir == "Up")
                velociy = new Vector2(0, -1);
            if (dir == "Down")
                velociy = new Vector2(0, 1);
        }

        public void DrawBoundingBox(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureManager.boundingBoxTexture, boundingBox, Color.White);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            DrawBoundingBox(spriteBatch);
        }
    }
}

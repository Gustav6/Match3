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
        public int checkUnder;
        public Point? destination;

        public Gem(Vector2 startPosition, int _gemType)
        {
            position = startPosition;
            texutre = TextureManager.textures[_gemType];
            gemType = _gemType;
            color = Color.White;
            moveSpeed = 100;
            scale = new Vector2(1, 1);

            boundingBox = new Rectangle((int)position.X, (int)position.Y, texutre.Width, texutre.Height);
            SetSource(TextureManager.textures[0]);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            boundingBox.Location = position.ToPoint();
        }

        //public Point? Destination(Point? destination)
        //{
        //    if (destination != null)
        //    {
        //        return destination;
        //    }
        //    return null;
        //}

        public static void SwapSpeed()
        {

        }

        public void Direction(Direction dir)
        {
            if (dir == Match3.Direction.up)
            {
                velociy = new Vector2(0, -1);
            }
            else if(dir == Match3.Direction.down)
            {
                velociy = new Vector2(0, 1);
            }
            else if(dir == Match3.Direction.left)
            {
                velociy = new Vector2(-1, 0);
            }
            else if (dir == Match3.Direction.right)
            {
                velociy = new Vector2(1, 0);
            }
            else if (dir == Match3.Direction.none)
            {
                velociy = Vector2.Zero;
            }
        }

        public void DrawBoundingBox(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureManager.boundingBoxTexture, boundingBox, Color.White * 0.7f);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            //DrawBoundingBox(spriteBatch);
        }
    }
}

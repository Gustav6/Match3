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
        public int gemType;
        public Rectangle boundingBox;
        public Point? destination;

        private float startMoveSpeed = 100;
        private float maxMoveSpeed = 300;

        public Gem(Vector2 startPosition, int _gemType)
        {
            position = startPosition;
            texutre = TextureManager.textures[_gemType];
            gemType = _gemType;
            color = Color.White;
            moveSpeed = startMoveSpeed;
            scale = new Vector2(1, 1);

            boundingBox = new Rectangle((int)position.X, (int)position.Y, texutre.Width, texutre.Height);
            SetSourceRectangle(TextureManager.textures[0]);
            SetOrigin(texutre);
        }

        public override void Update(GameTime gameTime)
        {
            //if (velociy.Y == 1 && moveSpeed < maxMoveSpeed)
            //{
            //    moveSpeed *= 1.25f;
            //}
            //else if (velociy.Y == 1 && moveSpeed > maxMoveSpeed)
            //{
            //    moveSpeed = maxMoveSpeed;
            //}
            //else if (velociy == Vector2.Zero)
            //{
            //    moveSpeed = startMoveSpeed;
            //}

            base.Update(gameTime);
            boundingBox.Location = new Vector2(position.X - texutre.Width / 2, position.Y - texutre.Height / 2).ToPoint();
        }

        public void TypeAndTexture(int _gemType)
        {
            texutre = TextureManager.textures[_gemType];
            gemType = _gemType;
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

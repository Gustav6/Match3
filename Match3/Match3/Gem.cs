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

        public Gem(Vector2 startPosition, int _gemType)
        {
            position = startPosition;
            texutre = TextureManager.textures[_gemType];
            gemType = _gemType;
            color = Color.White;
            moveSpeed = 200;
            scale = new Vector2(1, 1);

            boundingBox = new Rectangle((int)position.X, (int)position.Y, texutre.Width, texutre.Height);
            SetSourceRectangle(TextureManager.textures[0]);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            boundingBox.Location = position.ToPoint();
        }

        public bool CanMoveDown(int x, int y)
        {
            if (Data.tileMap[x, y + 1 + checkUnder].gem != null && Data.tileMap[x, y + 1 + checkUnder].canHaveGem && Data.tileMap[x, y + 1 + checkUnder].gem.velociy != Vector2.Zero)
            {
                //Data.tileMap[MissingGem()[i].X, y].gem.Direction(direction.none);
                checkUnder = 0;
                return false;
            }
            else if (Data.tileMap[x, y].gem.position.Y >= Data.tileMap[x, y + 1 + checkUnder].position.Y)
            {
                checkUnder += 1;
            }

            return true;
        }

        public void Direction(direction dir)
        {
            if (dir == direction.up)
            {
                velociy = new Vector2(0, -1);
            }
            else if(dir == direction.down)
            {
                velociy = new Vector2(0, 1);
            }
            else if(dir == direction.left)
            {
                velociy = new Vector2(-1, 0);
            }
            else if (dir == direction.right)
            {
                velociy = new Vector2(1, 0);
            }
            else if (dir == direction.none)
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

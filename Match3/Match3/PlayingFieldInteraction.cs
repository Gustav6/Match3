using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3
{
    public class PlayingFieldInteraction
    {
        private Point? prevGem;
        private Point? currentGem;

        public void MoveSelectedGem(GameTime gameTime)
        {
            if (InputManager.MouseHasBeenPressed(InputManager.currentMS.LeftButton, InputManager.prevMS.LeftButton))
            {
                GetSelectedGem();
            }

            if (prevGem != null && currentGem != null && prevGem != currentGem)
            {
                bool canSwitch = false;

                int prevPosX = prevGem.Value.X, prevPosY = prevGem.Value.Y;
                int currnetPosX = currentGem.Value.X, currnetPosY = currentGem.Value.Y;

                if (prevPosX + 1 == currnetPosX && prevPosY == currnetPosY || prevPosX - 1 == currnetPosX && prevPosY == currnetPosY)
                {
                    canSwitch = true;
                }
                else if (prevPosX == currnetPosX && prevPosY + 1 == currnetPosY || prevPosX == currnetPosX && prevPosY - 1 == currnetPosY)
                {
                    canSwitch = true;
                }

                if (canSwitch)
                {
                    //GemChange(currentGem.Value, prevGem.Value);

                    int prevGemType = Data.tileMap[prevPosX, prevPosY].gem.gemType;
                    int currentGemType = Data.tileMap[currnetPosX, currnetPosY].gem.gemType;

                    Data.tileMap[prevPosX, prevPosY].gem.TypeAndTexture(currentGemType);
                    Data.tileMap[currnetPosX, currnetPosY].gem.TypeAndTexture(prevGemType);

                    Data.tileMap[currentGem.Value.X, currentGem.Value.Y].gem.scale = new Vector2(1, 1);
                    Data.tileMap[prevGem.Value.X, prevGem.Value.Y].gem.scale = new Vector2(1, 1);

                    PlayingFieldAction.canClear = true;

                    // Check if the new position makes a match if not change them back
                    //if (Data.tileMap[prevPosX, prevPosY].gem != null && Data.tileMap[currnetPosX, currnetPosY].gem != null)
                    //{
                    //    Data.tileMap[prevPosX, prevPosY].gem.texutre = newPrevPos;
                    //    Data.tileMap[currnetPosX, currnetPosY].gem.texutre = newCurrentPos;
                    //}
                }
                else
                {
                    Data.tileMap[currentGem.Value.X, currentGem.Value.Y].gem.scale = new Vector2(1, 1);
                    Data.tileMap[prevGem.Value.X, prevGem.Value.Y].gem.scale = new Vector2(1, 1);
                }

                currentGem = null;
                prevGem = null;
            }
            else if (currentGem != null)
            {
                if (Data.tileMap[currentGem.Value.X, currentGem.Value.Y].isFilled)
                {
                    prevGem = currentGem;
                }
                else
                {
                    Data.tileMap[currentGem.Value.X, currentGem.Value.Y].gem.scale = new Vector2(1, 1);
                    prevGem = null;
                    currentGem = null;
                }
            }
        }

        public void GetSelectedGem()
        {
            for (int x = 0; x < Data.tileMap.GetLength(0); x++)
            {
                for (int y = 0; y < Data.tileMap.GetLength(1); y++)
                {
                    if (Data.tileMap[x, y].gem != null && Data.tileMap[x, y].isFilled)
                    {
                        if (InputManager.GetMouseBounds(true).Intersects(Data.tileMap[x, y].gem.boundingBox))
                        {
                            currentGem = new Point(x, y);

                            Data.tileMap[currentGem.Value.X, currentGem.Value.Y].gem.scale = new Vector2(1.1f, 1.1f);

                            if (prevGem != null && Data.tileMap[currentGem.Value.X, currentGem.Value.Y].gem == Data.tileMap[prevGem.Value.X, prevGem.Value.Y].gem)
                            {
                                Data.tileMap[currentGem.Value.X, currentGem.Value.Y].gem.scale = new Vector2(1, 1);
                                prevGem = null;
                                currentGem = null;
                            }
                        }
                    }
                }
            }
        }

        public static void VisualChange(int x, int y, float newAlpha)
        {
            Data.tileMap[x, y].gem.color = Color.White * newAlpha;
        }

        public static void GemChange(Point? prev, Point? current)
        {
            Data.tileMap[prev.Value.X, prev.Value.Y].isFilled = false;
            Data.tileMap[current.Value.X, current.Value.Y].isFilled = false;

            Data.tileMap[prev.Value.X, prev.Value.Y].gem.destination = Data.tileMap[current.Value.X, current.Value.Y].position;
            Data.tileMap[current.Value.X, current.Value.Y].gem.destination = Data.tileMap[prev.Value.X, prev.Value.Y].position;

            if (prev.Value.Y > current.Value.Y)
            {
                Data.tileMap[prev.Value.X, prev.Value.Y].gem.Direction(Direction.up);
                Data.tileMap[current.Value.X, current.Value.Y].gem.Direction(Direction.down);
            }
            else if (prev.Value.Y < current.Value.Y)
            {
                Data.tileMap[prev.Value.X, prev.Value.Y].gem.Direction(Direction.down);
                Data.tileMap[current.Value.X, current.Value.Y].gem.Direction(Direction.up);
            }
            else if (prev.Value.X > current.Value.X)
            {
                Data.tileMap[prev.Value.X, prev.Value.Y].gem.Direction(Direction.left);
                Data.tileMap[current.Value.X, current.Value.Y].gem.Direction(Direction.right);
            }
            else if (prev.Value.X < current.Value.X)
            {
                Data.tileMap[prev.Value.X, prev.Value.Y].gem.Direction(Direction.right);
                Data.tileMap[current.Value.X, current.Value.Y].gem.Direction(Direction.left);
            }
        }
    }
}

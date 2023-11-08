using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3
{
    public class PlayingFieldInteraction
    {
        private Point? prevGem;
        private Point? currentGem;

        public void MoveSelectedGem()
        {
            // Input
            if (InputManager.MouseHasBeenPressed(InputManager.currentMS.LeftButton, InputManager.prevMS.LeftButton))
            {
                GetSelectedGem();
            }

            if (prevGem != null && currentGem != null && prevGem != currentGem)
            {
                bool canSwitch = false;

                // Positions for the gems
                int prevPosX = prevGem.Value.X;
                int prevPosY = prevGem.Value.Y;
                int currnetPosX = currentGem.Value.X;
                int currnetPosY = currentGem.Value.Y;

                // Check if object can or cant switch
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
                    //GemChange(prevGem, currentGem);
                    
                    // Saved gem positions
                    Texture2D newPrevPos = Data.tileMap[prevPosX, prevPosY].gem.texutre;
                    Texture2D newCurrentPos = Data.tileMap[currnetPosX, currnetPosY].gem.texutre;

                    // Switch the gems positions
                    Data.tileMap[prevPosX, prevPosY].gem.texutre = newCurrentPos;
                    Data.tileMap[currnetPosX, currnetPosY].gem.texutre = newPrevPos;

                    // temp change for debbug
                    VisualChange(currnetPosX, currnetPosY, 1);
                    VisualChange(prevPosX, prevPosY, 1);

                    PlayingFieldAction.ClearMatches();

                    // Check if the new position makes a match if not change them back
                    if (Data.tileMap[prevPosX, prevPosY].gem != null && Data.tileMap[currnetPosX, currnetPosY].gem != null)
                    {
                        Data.tileMap[prevPosX, prevPosY].gem.texutre = newPrevPos;
                        Data.tileMap[currnetPosX, currnetPosY].gem.texutre = newCurrentPos;
                    }
                }
                else
                {
                    // Reset visual selection change if gems dont switch
                    VisualChange(currnetPosX, currnetPosY, 1);
                    VisualChange(prevPosX, prevPosY, 1);
                }

                // Resets variables after change
                currentGem = null;
                prevGem = null;
            }
            else if (currentGem != null)
            {
                prevGem = currentGem;
            }
        }

        public void GetSelectedGem()
        {
            for (int x = 0; x < Data.tileMap.GetLength(0); x++)
            {
                for (int y = 0; y < Data.tileMap.GetLength(1); y++)
                {
                    if (Data.tileMap[x, y].gem != null)
                    {
                        if (InputManager.GetMouseBounds(true).Intersects(Data.tileMap[x, y].gem.boundingBox))
                        {
                            currentGem = new Point(x, y);

                            // Make a visual change when selecting object
                            VisualChange(currentGem.Value.X, currentGem.Value.Y, 0.7f);

                            // Revert change if you press same object and or invalid target
                            if (prevGem != null && Data.tileMap[currentGem.Value.X, currentGem.Value.Y].gem == Data.tileMap[prevGem.Value.X, prevGem.Value.Y].gem)
                            {
                                VisualChange(currentGem.Value.X, currentGem.Value.Y, 1);
                                // Resets variables because there was no change
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

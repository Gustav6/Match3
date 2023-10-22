using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Match3
{
    public static class PlayingField
    {
        private static Point? prevGem;
        private static Point? currentGem;

        public static void Update()
        {
            MoveGem();
            CheckForMatches();
        }

        public static void MoveGem()
        {
            // Input
            if (InputManager.MouseHasBeenPressed(InputManager.currentMS.LeftButton, InputManager.prevMS.LeftButton))
                GetSelectedGem();

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
                    // Saved gem positions
                    Gem newCurrentPos = Data.tileMap[prevPosX, prevPosY].gem;
                    Gem newPrevPos = Data.tileMap[currnetPosX, currnetPosY].gem;

                    // Switch the gems positions
                    Data.tileMap[prevPosX, prevPosY].gem = newPrevPos;
                    Data.tileMap[currnetPosX, currnetPosY].gem = newCurrentPos;

                    // Reset visual selection change after gems w
                    VisualEffect(currnetPosX, currnetPosY, 1);
                    VisualEffect(prevPosX, prevPosY, 1);

                    // Check if the new position makes a match if not change them back
                    List<Point> tempCheck = new List<Point>();

                    tempCheck.AddRange(CheckVertical());
                    tempCheck.AddRange(CheckHorizontal());

                    if (tempCheck.Count == 0)
                    {
                        Data.tileMap[prevPosX, prevPosY].gem = newCurrentPos;
                        Data.tileMap[currnetPosX, currnetPosY].gem = newPrevPos;
                    }

                    tempCheck.Clear();
                }
                else
                {
                    // Reset visual selection change if gems dont switch
                    VisualEffect(currnetPosX, currnetPosY, 1);
                    VisualEffect(prevPosX, prevPosY, 1);
                }

                // Resets variables after change
                currentGem = null;
                prevGem = null;
            }
            else if (currentGem != null)
                prevGem = currentGem;
        }

        public static void GetSelectedGem()
        {
            int mouseX = (int)(InputManager.currentMS.X / Data.tileSize - 1);
            int mouseY = (int)(InputManager.currentMS.Y / Data.tileSize - 1);

            if (0 <= mouseX && mouseX < Data.tileMap.GetLength(0))
                if (0 <= mouseY && mouseY < Data.tileMap.GetLength(1))
                    if (Data.tileMap[mouseX, mouseY].gem != null)
                    {
                        currentGem = new Point(mouseX, mouseY);

                        // Make a visual change when selecting object
                        VisualEffect(currentGem.Value.X, currentGem.Value.Y, 0.7f);

                        // Revert change if you press same object and or invalid target
                        if (prevGem != null && Data.tileMap[currentGem.Value.X, currentGem.Value.Y].gem == Data.tileMap[prevGem.Value.X, prevGem.Value.Y].gem)
                        {
                            VisualEffect(currentGem.Value.X, currentGem.Value.Y, 1);
                            // Resets variables because there was no change
                            prevGem = null;
                            currentGem = null;
                        }
                    }
        }

        public static void VisualEffect(int positionX, int positionY, float newAlpha)
        {
            if (Data.tileMap[positionX, positionY].gem != null)
                Data.tileMap[positionX, positionY].gem.color = Color.White * newAlpha;
        }

        public static void CheckForMatches()
        {
            List<Point> temp = new List<Point>();
            temp.AddRange(CheckVertical());
            temp.AddRange(CheckHorizontal());

            if (temp.Count != 0)
                RemoveGems(temp);
        }

        public static List<Point> CheckVertical()
        {
            Texture2D currentGem;
            List<Point> tempVertical = new List<Point>();

            for (int x = 0; x < Data.tileMap.GetLength(0); x++)
                for (int y = 0; y < Data.tileMap.GetLength(1); y++)
                    if (Data.tileMap[x, y].gem != null)
                    {
                        currentGem = Data.tileMap[x, y].gem.texutre;

                        if (Data.InBounds(x, y - 1) && Data.tileMap[x, y - 1].gem != null && currentGem == Data.tileMap[x, y - 1].gem.texutre)
                        {
                            tempVertical.Add(new Point(x, y));
                        }
                        else if (tempVertical.Count >= 3)
                        {
                            return tempVertical;
                        }
                        else
                        {
                            tempVertical.Clear();
                            tempVertical.Add(new Point(x, y));
                        }
                    }

            return new List<Point>();
        }

        public static List<Point> CheckHorizontal()
        {
            Texture2D currentGem;
            List<Point> tempHotizontal = new List<Point>();

            for (int y = 0; y < Data.tileMap.GetLength(1); y++)
                for (int x = 0; x < Data.tileMap.GetLength(0); x++)
                    if (Data.tileMap[x, y].gem != null)
                    {
                        currentGem = Data.tileMap[x, y].gem.texutre;

                        if (Data.InBounds(x - 1, y) && Data.tileMap[x - 1, y].gem != null && currentGem == Data.tileMap[x - 1, y].gem.texutre)
                        {
                            tempHotizontal.Add(new Point(x, y));
                        }
                        else if (tempHotizontal.Count >= 3)
                        {
                            return tempHotizontal;
                        }
                        else
                        {
                            tempHotizontal.Clear();
                            tempHotizontal.Add(new Point(x, y));
                        }
                    }

            return new List<Point>();
        }

        private static void RemoveGems(List<Point> gemPos)
        {
            for (int i = 0; i < gemPos.Count; i++)
                Data.tileMap[gemPos[i].X, gemPos[i].Y].gem = null;
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Match3
{
    public static class PlayingField
    {
        private static Point? prevGem;
        private static Point? currentGem;
        public static int gemsLeft = 10;

        public static void Update()
        {
            ChangeGemAfterMove();
            MoveGem();
        }

        public static void MoveGem()
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
                    canSwitch = true;
                else if (prevPosX == currnetPosX && prevPosY + 1 == currnetPosY || prevPosX == currnetPosX && prevPosY - 1 == currnetPosY)
                    canSwitch = true;

                if (canSwitch)
                {
                    // Saved gem positions
                    Texture2D newPrevPos = Data.tileMap[prevPosX, prevPosY].gem.texutre;
                    Texture2D newCurrentPos = Data.tileMap[currnetPosX, currnetPosY].gem.texutre;

                    // Switch the gems positions
                    Data.tileMap[prevPosX, prevPosY].gem.texutre = newCurrentPos;
                    Data.tileMap[currnetPosX, currnetPosY].gem.texutre = newPrevPos;
                        
                    // Reset visual selection change after gems w
                    VisualChange(currnetPosX, currnetPosY, 1);
                    VisualChange(prevPosX, prevPosY, 1);

                    CheckForMatches();

                    // Check if the new position makes a match if not change them back
                    //List<Point> tempCheck = new List<Point>();

                    //tempCheck.AddRange(CheckVertical());
                    //tempCheck.AddRange(CheckHorizontal());

                    //if (tempCheck.Count == 0)
                    //{
                    //    Data.tileMap[prevPosX, prevPosY].gem.texutre = newCurrentPos;
                    //    Data.tileMap[currnetPosX, currnetPosY].gem.texutre = newPrevPos;
                    //}

                    //tempCheck.Clear();
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

        public static void GetSelectedGem()
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

        public static void VisualChange(int positionX, int positionY, float newAlpha)
        {
            Data.tileMap[positionX, positionY].gem.color = Color.White * newAlpha;
        }

        public static void ChangeGemAfterMove()
        {
            for (int i = 0; i < Data.gameObjects.Count; i++)
            {
                if (Data.gameObjects[i] is Gem g)
                {
                    g.Direction(direction.down);
                }
            }

            for (int i = 0; i < Data.gameObjects.Count; i++)
            {
                if (Data.gameObjects[i] is Gem g)
                {
                    if ((int)(g.position.Y / Data.tileSize) == Data.tileMap.GetLength(1) - 1 || Data.tileMap[(int)(g.position.X / Data.tileSize), (int)(g.position.Y / Data.tileSize) + 1].gem != null)
                    {
                        // Make position more accurate on the tile, and add check if gem can keep moving
                        Tile temp = new Tile() { gem = new Gem(new Vector2((int)g.position.X, (int)g.position.Y), g.gemType), canHaveGem = true };

                        Data.tileMap[(int)(g.position.X / Data.tileSize), (int)(g.position.Y / Data.tileSize)] = temp;
                        Data.gameObjects.RemoveAt(i);
                        CheckForMatches();
                    }
                }
            }
        }

        public static void MoveGemsDown()
        {
            List<Point> emptyRow = RowWithMissingGem();

            if (emptyRow.Count != 0)
            {
                for (int i = 0; i < emptyRow.Count; i++)
                {
                    for (int y = Data.tileMap.GetLength(1) - 1; y >= 0; y--)
                    {
                        if (Data.tileMap[emptyRow[i].X, y].gem != null && y < emptyRow[i].Y)
                        {
                            Data.gameObjects.Add(new Gem(new Vector2(emptyRow[i].X, y) * Data.tileSize, Data.tileMap[emptyRow[i].X, y].gem.gemType));
                            Data.tileMap[emptyRow[i].X, y].gem = null;

                            //Data.tileMap[MissingGem()[i].X, y].gem.color = Color.Red;
                            //Data.tileMap[MissingGem()[i].X, y].gem.Direction(direction.down);
                        }
                    }
                }
            }
        }

        public static List<Point> RowWithMissingGem()
        {
            List<Point> emptyPos = new List<Point>();

            for (int x = 0; x < Data.tileMap.GetLength(0); x++)
            {
                for (int y = Data.tileMap.GetLength(1) - 1; y >= 0; y--)
                {
                    if (Data.tileMap[x, y].canHaveGem && Data.tileMap[x, y].gem == null)
                    {
                        emptyPos.Add(new Point(x, y));
                    }
                }
            }

            return emptyPos;
        }

        public static void SpawnNewGems()
        {
            // Spawn gems at rows where there was a clear
            List<Point> emptyRow = RowWithMissingGem();

            if (gemsLeft > 0 && emptyRow.Count != 0)
            {
                for (int i = 0; i < emptyRow.Count; i++)
                {
                    Data.gameObjects.Add(new Gem(new Vector2(emptyRow[i].X * Data.tileSize, -Data.tileSize), 0));
                }
                gemsLeft--;
            }
        }

        public static void CheckForMatches()
        {
            List<Point[]> verticalMatches = CheckVertical();
            List<Point[]> horizontalMatches = CheckHorizontal();

            foreach (Point[] points in horizontalMatches)
            {
                foreach (Point point in points)
                {
                    Data.tileMap[point.X, point.Y].gem = null;

                    // Debug
                    //if (Data.tileMap[point.X, point.Y].gem != null)
                    //{
                    //    Data.tileMap[point.X, point.Y].gem.color = Color.Red;
                    //}
                }
            }
            foreach (Point[] points in verticalMatches)
            {
                foreach (Point point in points)
                {
                    Data.tileMap[point.X, point.Y].gem = null;

                    // Debug
                    //if (Data.tileMap[point.X, point.Y].gem != null)
                    //{
                    //    Data.tileMap[point.X, point.Y].gem.color = Color.Red;
                    //}
                }
            }

            SpawnNewGems();
            MoveGemsDown();
        }

        public static List<Point[]> CheckVertical()
        {
            Gem currentGem = null;
            List<Point[]> totalMatches = new List<Point[]>();
            List<Point> tempMatches = new List<Point>();

            for (int x = 0; x < Data.tileMap.GetLength(0); x++)
            {
                if (tempMatches.Count >= 3)
                {
                    totalMatches.Add(tempMatches.ToArray());
                }

                tempMatches.Clear();

                for (int y = 0; y < Data.tileMap.GetLength(0); y++)
                {
                    if (currentGem != null && Data.tileMap[x, y].gem != null && currentGem.texutre == Data.tileMap[x, y].gem.texutre && currentGem.position.X == Data.tileMap[x, y].gem.position.X)
                    {
                        tempMatches.Add(new Point(x, y));
                    }
                    else if (tempMatches.Count >= 3)
                    {
                        totalMatches.Add(tempMatches.ToArray());
                        tempMatches.Clear();
                    }
                    else
                    {
                        tempMatches.Clear();
                        if (Data.tileMap[x, y].gem != null)
                        {
                            currentGem = Data.tileMap[x, y].gem;
                        }
                        else if (!Data.tileMap[x, y].canHaveGem)
                        {
                            currentGem = null;
                        }
                        tempMatches.Add(new Point(x, y));
                    }
                }
            }

            return totalMatches;
        }

        public static List<Point[]> CheckHorizontal()
        {
            Gem currentGem = null;
            List<Point[]> totalMatches = new List<Point[]>();
            List<Point> tempMatches = new List<Point>();

            for (int y = 0; y < Data.tileMap.GetLength(0); y++)
            {
                if (tempMatches.Count >= 3)
                {
                    totalMatches.Add(tempMatches.ToArray());
                }

                tempMatches.Clear();

                for (int x = 0; x < Data.tileMap.GetLength(0); x++)
                {
                    if (currentGem != null && Data.tileMap[x, y].gem != null && currentGem.texutre == Data.tileMap[x, y].gem.texutre && currentGem.position.Y == Data.tileMap[x, y].gem.position.Y)
                    {
                        tempMatches.Add(new Point(x, y));
                    }
                    else if (tempMatches.Count >= 3)
                    {
                        totalMatches.Add(tempMatches.ToArray());
                        tempMatches.Clear();
                    }
                    else
                    {
                        tempMatches.Clear();
                        if (Data.tileMap[x, y].gem != null)
                        {
                            currentGem = Data.tileMap[x, y].gem;
                        }
                        else if (!Data.tileMap[x, y].canHaveGem)
                        {
                            currentGem = null;
                        }
                        tempMatches.Add(new Point(x, y));
                    }
                }
            }

            return totalMatches;
        }
    }
}

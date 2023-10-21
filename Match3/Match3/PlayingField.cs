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

            if (prevGem != null && prevGem != currentGem)
            {
                bool canSwitch = false;

                int tempPrevPosX = prevGem.Value.X;
                int tempPrevPosY = prevGem.Value.Y;
                int tempCurrnetPosX = currentGem.Value.X;
                int tempCurrnetPosY = currentGem.Value.Y;

                List<Point> list = new List<Point>();

                // Check if object can or cant switch
                // Need to add a check if move makes a match 

                if (tempPrevPosX + 1 == tempCurrnetPosX && tempPrevPosY == tempCurrnetPosY || tempPrevPosX - 1 == tempCurrnetPosX && tempPrevPosY == tempCurrnetPosY)
                {
                    canSwitch = true;
                }
                else if (tempPrevPosX == tempCurrnetPosX && tempPrevPosY + 1 == tempCurrnetPosY || tempPrevPosX == tempCurrnetPosX && tempPrevPosY - 1 == tempCurrnetPosY)
                {
                    canSwitch = true;
                }

                if (canSwitch)
                {
                    Gem tempPrevGem = Data.tileMap[tempPrevPosX, tempPrevPosY].gem;
                    Gem tempCurrentGem = Data.tileMap[tempCurrnetPosX, tempCurrnetPosY].gem;

                    Data.tileMap[tempPrevPosX, tempPrevPosY].gem = tempCurrentGem;
                    Data.tileMap[tempCurrnetPosX, tempCurrnetPosY].gem = tempPrevGem;

                    // Reset visual selection change after gems w
                    VisualEffect(tempCurrnetPosX, tempCurrnetPosY, 1);
                    VisualEffect(tempPrevPosX, tempPrevPosY, 1);

                    list.AddRange(CheckVertical());
                    list.AddRange(CheckHorizontal());
                    if (list.Count == 0)
                    {
                        Data.tileMap[tempPrevPosX, tempPrevPosY].gem = tempPrevGem;
                        Data.tileMap[tempCurrnetPosX, tempCurrnetPosY].gem = tempCurrentGem;
                    }
                    list.Clear();
                }
                else
                {
                    // Reset visual selection change if gems dont switch
                    VisualEffect(tempCurrnetPosX, tempCurrnetPosY, 1);
                    VisualEffect(tempPrevPosX, tempPrevPosY, 1);
                }

                // Resets variables
                currentGem = null;
                prevGem = null;
            }
            else if (currentGem != null)
                prevGem = currentGem;
        }

        public static void GetSelectedGem()
        {
            int mouseX = InputManager.currentMS.X / Data.tileSize;
            int mouseY = InputManager.currentMS.Y / Data.tileSize;

            if (0 <= mouseX && mouseX < Data.tileMap.GetLength(0))
                if (0 <= mouseY && mouseY < Data.tileMap.GetLength(1))
                        if (Data.tileMap[mouseX, mouseY].canHaveGem)
                        {
                            currentGem = new Point(mouseX, mouseY);

                            // Make a visual change when selecting object
                            VisualEffect(currentGem.Value.X, currentGem.Value.Y, 0.8f);

                            // Revert change if you press same object and or invalid target
                            if (prevGem != null && Data.tileMap[currentGem.Value.X, currentGem.Value.Y].gem == Data.tileMap[prevGem.Value.X, prevGem.Value.Y].gem)
                                VisualEffect(currentGem.Value.X, currentGem.Value.Y, 1);
                        }
        }

        public static void VisualEffect(int positionX, int positionY, float newAlpha)
        {
            if (Data.tileMap[positionX, positionY].gem != null)
                Data.tileMap[positionX, positionY].gem.color = Color.White * newAlpha;
        }

        public static void CheckForMatches()
        {
            List<Point> points = new List<Point>();
            points.AddRange(CheckVertical());
            points.AddRange(CheckHorizontal());

            RemoveGems(points);
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

                        if (InBounds(x, y - 1) && Data.tileMap[x, y - 1].gem != null && currentGem == Data.tileMap[x, y - 1].gem.texutre)
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

                        if (InBounds(x - 1, y) && Data.tileMap[x - 1, y].gem != null && currentGem == Data.tileMap[x - 1, y].gem.texutre)
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

        private static bool InBounds(int x, int y)
        {
            return 0 <= y && y < Data.tileMap.GetLength(1) && 0 <= x && x < Data.tileMap.GetLength(0);
        }

        public static void CompletedPlayingField()
        {
            CreateTempMap();
            SpawnGems();
        }

        private static void SpawnGems()
        {
            for (int x = 0; x < Data.tileMap.GetLength(0); x++)
                for (int y = 0; y < Data.tileMap.GetLength(1); y++)
                    if (Data.tileMap[x, y].canHaveGem)
                    {
                        Data.tileMap[x, y] = new Tile() { gem = new Gem(new Vector2(x * Data.tileSize, y * Data.tileSize), Data.Random(0, TextureManager.textures.Length)), canHaveGem = true, removed = false};

                        if (InBounds(x, y - 2) && Data.tileMap[x, y - 2].gem != null)
                            if (Data.tileMap[x, y].gem.texutre == Data.tileMap[x, y - 2].gem.texutre)
                            {
                                Data.tileMap[x, y].gem = null;
                                y--;
                            }

                        if (InBounds(x - 2, y) && Data.tileMap[x - 2, y].gem != null)
                            if (Data.tileMap[x, y].gem.texutre == Data.tileMap[x - 2, y].gem.texutre)
                            {
                                Data.tileMap[x, y].gem = null;
                                y--;
                            }
                    }
        }

        private static void CreateTempMap()
        {
            // Create a map that shows where gems can be (0 = cant, 1 = can)
            int[,] tempMap = new int[,]
            {
                {0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0},
                {0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                {0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0},
                {0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0}
            };

            LoadTileMap(tempMap);
        }

        private static void LoadTileMap(int[,] tempMap)
        {
            // Set project wide tilemap to temp with the inverse of the temp map
            Data.tileMap = new Tile[tempMap.GetLength(0), tempMap.GetLength(1)];

            for (int y = 0; y < Data.tileMap.GetLength(1); y++)
                for (int x = 0; x < Data.tileMap.GetLength(0); x++)
                {
                    int number = tempMap[y, x];

                    if (number == 1)
                        Data.tileMap[x, y].canHaveGem = true;
                }
        }
    }
}

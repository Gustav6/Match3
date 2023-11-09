using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3
{
    public class PlayingFieldAction
    {
        private static bool[,] isFilled = new bool[Data.tileMap.GetLength(0), Data.tileMap.GetLength(1)];
        private static int gemsLeft = 50;
        //private static Point? gemDestination;

        //private static int amountOfGemsMoving;
        //private static int amountOfGemsNotMoving;
        //private static List<Point> oldPosition = new();
        //private static List<Point> newPosition = new();

        public static void TempMove()
        {
            for (int x = 0; x < Data.tileMap.GetLength(0); x++)
            {
                for (int y = 0; y < Data.tileMap.GetLength(1); y++)
                {
                    if (Data.tileMap[x, y].gem != null)
                    {
                        isFilled[x, y] = true;
                    }
                }
            }
        }

        public static void DeclearDestination()
        {
            bool gemHasDestination = true;

            for (int x = 0; x < Data.tileMap.GetLength(0); x++)
            {
                for (int y = Data.tileMap.GetLength(1) - 1; y >= 0; y--)
                {
                    if (Data.tileMap[x, y].gem != null && !Data.tileMap[x, y].isFilled)
                    {
                        if (Data.tileMap[x, y].gem.destination == null)
                        {
                            gemHasDestination = false;
                            break;
                        }
                    }
                }

                if (!gemHasDestination)
                {
                    Point? des = null;
                    bool hasFound = false;

                    for (int y = Data.tileMap.GetLength(1) - 1; y >= 0; y--)
                    {
                        if (!Data.tileMap[x, y].isFilled && Data.tileMap[x, y].canHaveGem)
                        {
                            if (des == null && !hasFound)
                            {
                                des = Data.tileMap[x, y].position;
                                Data.tileMap[x, y].isFilled = true;
                            }
                        }

                        if (!Data.tileMap[x, y].isFilled && Data.tileMap[x, y].gem != null && des != null)
                        {
                            if (Data.tileMap[x, y].gem.destination == null)
                            {
                                Data.tileMap[x, y].gem.destination = des;
                                hasFound = true;
                                des = null;
                            }
                        }

                        if (hasFound)
                        {
                            y = Data.tileMap.GetLength(1) - 1;
                            hasFound = false;
                        }
                        else if (!hasFound && y == 0)
                        {
                            break;
                        }
                    }
                }
            }
        }

        public static void CheckIfGemCanMove()
        {
            for (int x = 0; x < Data.tileMap.GetLength(0); x++)
            {
                // int y = Data.tileMap.GetLength(1) - 1; y >= 0; y--
                for (int y = Data.tileMap.GetLength(1) - 1; y >= 0; y--)
                {
                    if (Data.tileMap[x, y].gem != null)
                    {
                        if (Data.tileMap[x, y].gem.velociy != Vector2.Zero)
                        {
                            if (Data.tileMap[x, y].gem.destination != null)
                            {
                                if (Data.tileMap[x, y].gem.position.Y >= Data.tileMap[x, y].gem.destination.Value.ToVector2().Y * Data.tileSize)
                                {
                                    // Add to tileMap 
                                    Data.tileMap[x, y].gem.Direction(Direction.none);
                                    Data.tileMap[x, y].gem.position = Data.tileMap[x, y].gem.destination.Value.ToVector2() * Data.tileSize;
                                    Data.tileMap[x, y].gem.destination = null;
                                    //Data.tileMap[(int)(Data.tileMap[x, y].gem.position.X / 64), (int)(Data.tileMap[x, y].gem.position.Y / 64)].gem = Data.tileMap[x, y].gem;
                                    //Data.tileMap[x, y].gem = null;
                                    //y = 0;
                                    ClearMatches();
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void ChangeGem(int x, int y)
        {
            //Gem temp = new Gem(new(x * Data.tileSize, y * Data.tileSize), Data.tileMap[x, y].gem.gemType);
            Gem temp = new Gem(new(x * Data.tileSize, Data.tileMap[x, (int)(Data.tileMap[x, y].gem.position.Y / 64)].position.Y * Data.tileSize), Data.tileMap[x, y].gem.gemType);
            Data.tileMap[x, (int)(Data.tileMap[x, y].gem.position.Y / 64)].gem = temp;
            Data.tileMap[x, y].gem = null;
            ClearMatches();
        }

        public static void MoveGemsDown()
        {
            List<Point> canMove = WhichGemCanMove();

            for (int i = 0; i < canMove.Count; i++)
            {
                for (int y = Data.tileMap.GetLength(1) - 1; y >= 0; y--)
                {
                    if (Data.tileMap[canMove[i].X, y].gem != null && y < canMove[i].Y)
                    {
                        Data.tileMap[canMove[i].X, y].isFilled = false;
                        Data.tileMap[canMove[i].X, y].gem.Direction(Direction.down);
                    }
                }
            }

            DeclearDestination();
        }

        private static List<Point> WhichGemCanMove()
        {
            List<Point> emptyPos = new();

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
            List<Point> emptyRow = WhichGemCanMove();

            if (gemsLeft > 0 && emptyRow.Count != 0)
            {
                for (int i = 0; i < emptyRow.Count; i++)
                {
                    Data.gameObjects.Add(new Gem(new Vector2(emptyRow[i].X * Data.tileSize, -Data.tileSize), 0));
                }
                gemsLeft--;
            }
        }

        public static void ClearMatches()
        {
            List<Point[]> verticalMatches = CheckVertical(), horizontalMatches = CheckHorizontal();

            foreach (Point[] points in horizontalMatches)
            {
                foreach (Point point in points)
                {
                    Data.tileMap[point.X, point.Y].gem = null;
                    Data.tileMap[point.X, point.Y].isFilled = false;
                }
            }
            foreach (Point[] points in verticalMatches)
            {
                foreach (Point point in points)
                {
                    Data.tileMap[point.X, point.Y].gem = null;
                    Data.tileMap[point.X, point.Y].isFilled = false;
                }
            }

            //PlayingFieldActions.SpawnNewGems();
            //PlayingFieldAction.MoveGemsDown();
            if (verticalMatches.Count != 0 || horizontalMatches.Count != 0)
            {
                MoveGemsDown();
            }
        }

        public static List<Point[]> CheckVertical()
        {
            Gem currentGem = null;
            List<Point[]> totalMatches = new();
            List<Point> tempMatches = new();

            for (int x = 0; x < Data.tileMap.GetLength(0); x++)
            {
                if (tempMatches.Count >= 3)
                {
                    totalMatches.Add(tempMatches.ToArray());
                }

                tempMatches.Clear();

                for (int y = 0; y < Data.tileMap.GetLength(0); y++)
                {
                    if (currentGem != null && Data.tileMap[x, y].gem != null && currentGem.texutre == Data.tileMap[x, y].gem.texutre && currentGem.position.X == Data.tileMap[x, y].gem.position.X && Data.tileMap[x, y].isFilled)
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
            List<Point[]> totalMatches = new();
            List<Point> tempMatches = new();

            for (int y = 0; y < Data.tileMap.GetLength(0); y++)
            {
                if (tempMatches.Count >= 3)
                {
                    totalMatches.Add(tempMatches.ToArray());
                }

                tempMatches.Clear();

                for (int x = 0; x < Data.tileMap.GetLength(0); x++)
                {
                    if (currentGem != null && Data.tileMap[x, y].gem != null && currentGem.texutre == Data.tileMap[x, y].gem.texutre && currentGem.position.Y == Data.tileMap[x, y].gem.position.Y && Data.tileMap[x, y].isFilled)
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

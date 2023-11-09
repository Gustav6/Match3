﻿using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3
{
    public class PlayingFieldAction
    {
        private static bool[,] isFilled = new bool[Data.tileMap.GetLength(0), Data.tileMap.GetLength(1)];
        private static int gemsLeft = 50;
        private static Point? gemDestination;

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
                    for (int y = Data.tileMap.GetLength(1) - 1; y >= 0; y--)
                    {
                        if (!Data.tileMap[x, y].isFilled && Data.tileMap[x, y].canHaveGem && Data.tileMap[x, y].gem == null)
                        {
                            if (des == null)
                            {
                                des = Data.tileMap[x, y].position;
                                Data.tileMap[x, y].isFilled = true;
                            }
                        }

                        if (!Data.tileMap[x, y].isFilled && Data.tileMap[x, y].gem != null && des != null)
                        {
                            //if (Data.tileMap[x, y].gem.destination != null)
                            {
                                Data.tileMap[x, y].gem.destination = des;
                                des = null;
                            }
                        }

                        if (y == 0)
                        {
                            for (int i = 0; i < Data.tileMap.GetLength(1); i++)
                            {
                                if (Data.tileMap[x, i].gem != null && Data.tileMap[x, i].gem.velociy != Vector2.Zero && Data.tileMap[x, i].gem.destination == null)
                                {
                                    y = Data.tileMap.GetLength(1) - 1;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void CheckIfGemCanMove()
        {
            for (int x = 0; x < Data.tileMap.GetLength(0); x++)
            {
                for (int y = Data.tileMap.GetLength(1) - 1; y >= 0; y--)
                {
                    if (Data.tileMap[x, y].gem != null)
                    {
                        if (Data.tileMap[x, y].gem.velociy != Vector2.Zero)
                        {
                            //if (Data.tileMap[x, y].gem.position.Y >= (Data.tileMap.GetLength(1) - 1) * Data.tileSize)
                            //{
                            //    ChangeGem(x, y);
                            //}
                            //else if (Data.InBounds(x, y + 1) && Data.tileMap[x, (int)(Data.tileMap[x, y].gem.position.Y / 64) + 1].isFilled)
                            //{
                            //    ChangeGem(x, y);
                            //}
                            if (Data.tileMap[x, y].gem.destination != null)
                            {
                                if (Data.tileMap[x, y].gem.position.Y >= Data.tileMap[x, y].gem.destination.Value.Y * Data.tileSize)
                                {
                                    return;
                                }
                            }
                        }
                    }
                }
            }

            //for (int i = Data.gameObjects.Count - 1; i >= 0; i--)
            //{
            //    if (Data.gameObjects[i] is Gem g)
            //    {
            //        if ((int)(g.position.Y / Data.tileSize) == Data.tileMap.GetLength(1) - 1 || Data.tileMap[(int)(g.position.X / Data.tileSize), (int)(g.position.Y / Data.tileSize) + 1].gem != null)
            //        {
            //            // Make position more accurate on the tile, and add check if gem can keep moving

            //            int tempYPos = Data.tileMap[(int)(g.position.X / Data.tileSize), (int)(g.position.Y / Data.tileSize)].position.Y * Data.tileSize;
            //            Gem temp = new(new(g.position.X, tempYPos), g.gemType);

            //            Data.tileMap[(int)(g.position.X / Data.tileSize), (int)(g.position.Y / Data.tileSize)].gem = temp;
            //            Data.gameObjects.RemoveAt(i);
            //            ClearMatches();
            //        }
            //    }
            //}

            //for (int i = 0; i < Data.gameObjects.Count; i++)
            //{
            //    if (Data.gameObjects[i] is Gem g)
            //    {
            //        if (g.position.ToPoint().Y >= g.destination.Value.Y)
            //        {
            //            return;
            //        }
            //    }
            //}
        }

        public static void ChangeGem(int x, int y)
        {
            Gem temp = Data.tileMap[x, y].gem;
            temp.position.Y = Data.tileMap[x, (int)(Data.tileMap[x, y].gem.position.Y / 64)].position.Y * Data.tileSize;
            Data.tileMap[x, (int)(Data.tileMap[x, y].gem.position.Y / 64)].isFilled = true;
            Data.tileMap[x, (int)(Data.tileMap[x, y].gem.position.Y / 64)].gem = temp;
            Data.tileMap[x, y].gem.Direction(Direction.none);
            Data.tileMap[x, y].gem = null;
            ClearMatches();
        }

        public static void MarkDestination()
        {
            bool gemMissing = false;
            int? currentYLevel = null;
            List<Point> des = new();

            for (int x = 0; x < Data.tileMap.GetLength(0); x++)
            {
                for (int y = Data.tileMap.GetLength(1) - 1; y >= 0; y--)
                {
                    if (Data.tileMap[x, y].canHaveGem && Data.tileMap[x, y].gem == null)
                    {
                        gemMissing = true;
                        currentYLevel = y;
                        break;
                    }
                }

                if (gemMissing && currentYLevel != null)
                {
                    for (int y = (int)currentYLevel; y >= 0; y--)
                    {
                        des.Add(new Point(x, y));
                    }
                    gemMissing = false;
                }
            }

            for (int i = 0; i < des.Count; i++)
            {
                if (Data.tileMap[des[i].X, des[i].Y].gem != null)
                {
                    Gem temp = new Gem(Data.tileMap[des[i].X, des[i].Y].position.ToVector2() * 64, Data.tileMap[des[i].X, des[i].Y].gem.gemType);
                    temp.destination = new Point(des[i].X, des[0].Y);
                    temp.Direction(Direction.down);

                    Data.gameObjects.Add(temp);
                    i++;
                    des.RemoveAt(0);
                }
            }
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

                        //Gem temp = (new Gem(new Vector2(canMove[i].X, y) * Data.tileSize, Data.tileMap[canMove[i].X, y].gem.gemType));
                        //temp.Direction(Direction.down);
                        //Data.gameObjects.Add(temp);
                        //Data.tileMap[canMove[i].X, y].gem = null;
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
            MoveGemsDown();
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

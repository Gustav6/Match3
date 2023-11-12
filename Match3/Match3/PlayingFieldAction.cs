﻿using Microsoft.VisualBasic;
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
        private static int gemsLeft = 50;
        private static bool gemIsMissing;
        private static int amountOfGemsAdded;

        // Try
        // Changing the way i move gems: Do this by switching the refrence in the tile map but not the gems position.
        // Explenation
        // Doing this allows me to have the tile as not filled until the gem arrives to it's destination.
        // Advantage 
        // Gem never Leaves the tile map this allows me to change destination on the fly.

        public static void DeclearDestination()
        {
            for (int x = 0; x < Data.tileMap.GetLength(0); x++)
            {
                Point? des = null;
                bool hasFound = false;

                for (int y = Data.tileMap.GetLength(1) - 1; y >= 0; y--)
                {
                    if (!hasFound)
                    {
                        if (des == null && !Data.tileMap[x, y].isFilled && Data.tileMap[x, y].canHaveGem)
                        {
                            if (Data.tileMap[x, y].gem == null)
                            {
                                des = Data.tileMap[x, y].position;
                            }
                        }
                    }

                    if (Data.tileMap[x, y].gem != null && des != null)
                    {
                        Data.tileMap[x, y].gem.destination = des;
                        Data.tileMap[des.Value.X, des.Value.Y].gem = Data.tileMap[x, y].gem;
                        Data.tileMap[x, y].gem = null;
                        hasFound = true;
                        des = null;
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

        public static void CheckIfGemCanMove()
        {
            for (int x = 0; x < Data.tileMap.GetLength(0); x++)
            {
                for (int y = 0; y < Data.tileMap.GetLength(1); y++)
                {
                    if (Data.tileMap[x, y].gem != null && Data.tileMap[x, y].gem.destination != null)
                    {
                        if (Data.tileMap[x, y].gem.position.Y >= Data.tileMap[x, y].gem.destination.Value.Y * Data.tileSize)
                        {
                            Data.tileMap[x, y].gem.Direction(Direction.none);
                            Data.tileMap[x, y].gem.position = Data.tileMap[x, y].gem.destination.Value.ToVector2() * Data.tileSize;
                            Data.tileMap[x, y].isFilled = true;
                            Data.tileMap[x, y].gem.destination = null;
                            ClearMatches();
                        }
                    }
                }
            }

            MoveGemsDown();
            SpawnNewGems();
        }

        public static void MoveGemsDown()
        {
            CanGemMove();

            if (gemIsMissing)
            {
                DeclearDestination();

                for (int x = 0; x < Data.tileMap.GetLength(0); x++)
                {
                    for (int y = Data.tileMap.GetLength(1) - 1; y >= 0; y--)
                    {
                        if (Data.tileMap[x, y].gem != null)
                        {
                            if (Data.tileMap[x, y].gem.destination != null && Data.tileMap[x, y].gem.position.Y < Data.tileMap[x, y].gem.destination.Value.Y * Data.tileSize)
                            {
                                Data.tileMap[x, y].gem.Direction(Direction.down);
                            }
                        }
                    }
                }
            }
        }

        private static void CanGemMove()
        {
            List<Point> rowThatCanMove = new();

            for (int x = 0; x < Data.tileMap.GetLength(0); x++)
            {
                for (int y = Data.tileMap.GetLength(1) - 1; y >= 0; y--)
                {
                    if (Data.tileMap[x, y].canHaveGem && Data.tileMap[x, y].gem == null && !Data.tileMap[x, y].isFilled)
                    {
                        gemIsMissing = true;
                        rowThatCanMove.Add(new Point(x, y));
                        break;
                    }
                }
            }

            for (int x = 0; x < Data.tileMap.GetLength(0); x++)
            {
                for (int y = Data.tileMap.GetLength(1) - 1; y >= 0; y--)
                {
                    for (int i = 0; i < rowThatCanMove.Count; i++)
                    {
                        if (Data.tileMap[x, y].gem != null)
                        {
                            if (Data.tileMap[x, y].position.Y < rowThatCanMove[i].Y && Data.tileMap[x, y].position.X == rowThatCanMove[i].X)
                            {
                                Data.tileMap[x, y].isFilled = false;
                            }
                        }
                    }
                }
            }

            if (rowThatCanMove.Count == 0)
            {
                gemIsMissing = false;
            }
        }

        public static void SpawnNewGems()
        {
            // Spawn gems at rows where there was a clear

            if (gemsLeft > 0 && gemIsMissing)
            {
                List<Point> emptySlot = new();

                for (int x = 0; x < Data.tileMap.GetLength(0); x++)
                {
                    amountOfGemsAdded = 1;

                    for (int y = Data.tileMap.GetLength(1) - 1; y >= 0; y--)
                    {
                        if (Data.tileMap[x, y].canHaveGem && Data.tileMap[x, y].gem == null)
                        {
                            emptySlot.Add(new Point(x, y));
                        }
                    }

                    if (emptySlot.Count != 0)
                    {
                        for (int i = emptySlot.Count - 1; i >= 0; i--)
                        {
                            Gem temp = new(new(x * Data.tileSize, -amountOfGemsAdded * Data.tileSize), Data.Random(0, TextureManager.textures.Length));
                            temp.destination = new Point(x, emptySlot[i].Y);
                            temp.Direction(Direction.down);
                            Data.tileMap[x, emptySlot[i].Y].gem = temp;
                            emptySlot.RemoveAt(i);
                            amountOfGemsAdded++;
                            gemsLeft--;
                        }
                    }

                    // reset amount of gems added
                }
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
                        if (Data.tileMap[x, y].gem != null && Data.tileMap[x, y].isFilled)
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
                        if (Data.tileMap[x, y].gem != null && Data.tileMap[x, y].isFilled)
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

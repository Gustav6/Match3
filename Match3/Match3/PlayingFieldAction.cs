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
    public static class PlayingFieldAction
    {
        public static int gemsLeft = 500;
        private static bool anyGemCanMove;
        private static int amountOfGemsAddedPerRow;

        private static float timer = 0.1f;
        private static float clearDelay = 0.1f;
        public static bool canClear;

        public static void GiveCurrentGemsADestination()
        {
            for (int x = 0; x < Data.tileMap.GetLength(0); x++)
            {
                Point? newDestination = null;
                bool foundGem = false;

                for (int y = Data.tileMap.GetLength(1) - 1; y >= 0; y--)
                {
                    if (newDestination == null && !Data.tileMap[x, y].isFilled && Data.tileMap[x, y].canHaveGem && Data.tileMap[x, y].gem == null)
                    {
                        newDestination = Data.tileMap[x, y].position;
                    }
                    else if (Data.tileMap[x, y].gem != null && newDestination != null && !Data.tileMap[x, y].isFilled)
                    {
                        Data.tileMap[x, y].gem.destination = newDestination;
                        Data.tileMap[newDestination.Value.X, newDestination.Value.Y].gem = Data.tileMap[x, y].gem;
                        Data.tileMap[newDestination.Value.X, newDestination.Value.Y].gem.Direction(Direction.down);
                        Data.tileMap[x, y].gem = null;
                        foundGem = true;
                    }

                    if (foundGem)
                    {
                        y = Data.tileMap.GetLength(1) - 1;
                        newDestination = null;
                        foundGem = false;
                    }
                }
            }
        }

        public static void HasGemReachedDestination(GameTime gameTime)
        {
            for (int x = 0; x < Data.tileMap.GetLength(0); x++)
            {
                for (int y = 0; y < Data.tileMap.GetLength(1); y++)
                {
                    if (Data.tileMap[x, y].gem != null && Data.tileMap[x, y].gem.destination != null)
                    {
                        if (Data.tileMap[x, y].gem.position.Y >= Data.tileMap[x, y].gem.destination.Value.Y * Data.tileLocation + Data.tileMapOffset.Y + Data.gemOrigin.Y)
                        {
                            ChangeGem(x, y);
                            canClear = true;
                        }
                    }
                }
            }

            if (canClear)
            {
                ClearMatches(gameTime);
                canClear = true;
            }
        }

        public static void ChangeGem(int x, int y)
        {
            Data.tileMap[x, y].gem.Direction(Direction.none);
            Data.tileMap[x, y].gem.position = Data.tileMap[x, y].gem.destination.Value.ToVector2() * Data.tileLocation + Data.tileMapOffset + Data.gemOrigin;
            Data.tileMap[x, y].gem.destination = null;
            Data.tileMap[x, y].isFilled = true;
        }

        public static void ChecksForGem()
        {
            CheckIfGemsCanMove();

            if (anyGemCanMove)
            {
                GiveCurrentGemsADestination();
            }

            if (gemsLeft > 0)
            {
                SpawnNewGems();
            }
        }

        private static void CheckIfGemsCanMove()
        {
            bool gemCanMove;

            for (int x = 0; x < Data.tileMap.GetLength(0); x++)
            {
                gemCanMove = false;

                for (int y = Data.tileMap.GetLength(1) - 1; y >= 0; y--)
                {
                    if (!gemCanMove && Data.tileMap[x, y].canHaveGem && Data.tileMap[x, y].gem == null)
                    {
                        gemCanMove = true;
                        anyGemCanMove = true;
                    }
                    else if (Data.tileMap[x, y].gem != null && gemCanMove)
                    {
                        Data.tileMap[x, y].isFilled = false;
                    }
                }
            }
        }

        public static void SpawnNewGems()
        {
            List<Point> emptySlot = new();

            for (int x = 0; x < Data.tileMap.GetLength(0); x++)
            {
                for (int y = Data.tileMap.GetLength(1) - 1; y >= 0; y--)
                {
                    if (Data.tileMap[x, y].canHaveGem && Data.tileMap[x, y].gem == null)
                    {
                        emptySlot.Add(new Point(x, y));
                    }
                }

                for (int i = 0; i < emptySlot.Count; i++)
                {
                    if (gemsLeft > 0)
                    {
                        int randomGem = Data.Random(0, TextureManager.textures.Length);
                        Gem temp = new(new(x * Data.tileLocation + Data.tileMapOffset.X + Data.gemOrigin.X, (-Data.tileSize) - amountOfGemsAddedPerRow * Data.tileLocation), randomGem);
                        temp.destination = emptySlot[i];
                        temp.Direction(Direction.down);
                        Data.tileMap[x, emptySlot[i].Y].gem = temp;
                        amountOfGemsAddedPerRow++;
                        gemsLeft--;
                    }
                }

                emptySlot.Clear();
                amountOfGemsAddedPerRow = 0;
            }
        }

        public static void ClearMatches(GameTime gameTime)
        {
            if (timer <= 0)
            {
                List<Point[]> verticalMatches = CheckVertical(), horizontalMatches = CheckHorizontal();

                foreach (Point[] points in horizontalMatches)
                {
                    foreach (Point point in points)
                    {
                        Data.tileMap[point.X, point.Y].gem = null;
                        Data.tileMap[point.X, point.Y].isFilled = false;
                        Data.gamePoints += 100;
                    }
                }
                foreach (Point[] points in verticalMatches)
                {
                    foreach (Point point in points)
                    {
                        Data.tileMap[point.X, point.Y].gem = null;
                        Data.tileMap[point.X, point.Y].isFilled = false;
                        Data.gamePoints += 100;
                    }
                }

                timer = clearDelay;
            }
            else
            {
                timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public static List<Point[]> CheckVertical()
        {
            Gem currentGem = null;
            List<Point[]> totalMatches = new();
            List<Point> tempMatches = new();

            for (int x = 0; x < Data.tileMap.GetLength(0); x++)
            {
                for (int y = 0; y < Data.tileMap.GetLength(0); y++)
                {
                    if (currentGem != null && Data.tileMap[x, y].gem != null && currentGem.gemType == Data.tileMap[x, y].gem.gemType
                        && Data.tileMap[x, y].gem.velociy == Vector2.Zero && currentGem.position.X == Data.tileMap[x, y].gem.position.X
                        && Data.tileMap[x, y].isFilled)
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
                        else if (!Data.tileMap[x, y].canHaveGem || !Data.tileMap[x, y].isFilled)
                        {
                            currentGem = null;
                        }
                        tempMatches.Add(new Point(x, y));
                    }
                }

                if (tempMatches.Count >= 3)
                {
                    totalMatches.Add(tempMatches.ToArray());
                }

                tempMatches.Clear();
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
                for (int x = 0; x < Data.tileMap.GetLength(0); x++)
                {

                    if (currentGem != null && Data.tileMap[x, y].gem != null && currentGem.gemType == Data.tileMap[x, y].gem.gemType 
                        && Data.tileMap[x, y].gem.velociy == Vector2.Zero && currentGem.position.Y == Data.tileMap[x, y].gem.position.Y 
                        && Data.tileMap[x, y].isFilled)
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
                        if (Data.tileMap[x, y].gem != null && Data.tileMap[x, y].isFilled && Data.tileMap[x, y].gem.velociy == Vector2.Zero)
                        {
                            currentGem = Data.tileMap[x, y].gem;
                        }
                        else if (!Data.tileMap[x, y].canHaveGem || !Data.tileMap[x, y].isFilled)
                        {
                            currentGem = null;
                        }
                        tempMatches.Add(new Point(x, y));
                    }
                }

                if (tempMatches.Count >= 3)
                {
                    totalMatches.Add(tempMatches.ToArray());
                }

                tempMatches.Clear();
            }

            return totalMatches;
        }
    }
}

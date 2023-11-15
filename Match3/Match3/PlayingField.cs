using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3
{
    public class PlayingField
    {
        public int gemsLeft = 500;
        private bool anyGemCanMove;
        private int amountOfGemsAddedPerRow;

        private float timer = 0.1f;
        private float clearDelay = 0.1f;
        public bool canClear;

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

                if (canSwitch && !canClear)
                {
                    //GemChange(currentGem.Value, prevGem.Value);

                    int prevGemType = Data.tileMap[prevGem.Value.X, prevGem.Value.Y].gem.gemType;
                    int currentGemType = Data.tileMap[currentGem.Value.X, currentGem.Value.Y].gem.gemType;

                    Data.tileMap[prevGem.Value.X, prevGem.Value.Y].gem.TypeAndTexture(currentGemType);
                    Data.tileMap[currentGem.Value.X, currentGem.Value.Y].gem.TypeAndTexture(prevGemType);

                    Data.tileMap[currentGem.Value.X, currentGem.Value.Y].gem.scale = new Vector2(1, 1);
                    Data.tileMap[prevGem.Value.X, prevGem.Value.Y].gem.scale = new Vector2(1, 1);

                    List<Point[]> verticalMatches = CheckVertical(), horizontalMatches = CheckHorizontal();

                    if (verticalMatches.Count == 0 && horizontalMatches.Count == 0)
                    {
                        Data.tileMap[prevGem.Value.X, prevGem.Value.Y].gem.TypeAndTexture(prevGemType);
                        Data.tileMap[currentGem.Value.X, currentGem.Value.Y].gem.TypeAndTexture(currentGemType);
                    }

                    canClear = true;
                    prevGem = null;
                    currentGem = null;
                }
                else if (!canSwitch)
                {
                    Data.tileMap[currentGem.Value.X, currentGem.Value.Y].gem.scale = new Vector2(1, 1);
                    Data.tileMap[prevGem.Value.X, prevGem.Value.Y].gem.scale = new Vector2(1, 1);
                    prevGem = null;
                    currentGem = null;
                }
            }
            else if (currentGem != null)
            {
                if (prevGem == null && Data.tileMap[currentGem.Value.X, currentGem.Value.Y].isFilled)
                {
                    prevGem = currentGem;
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

        public void GemChange(Point? prev, Point? current)
        {
            //int currentGemType = Data.tileMap[current.Value.X, current.Value.Y].gem.gemType;
            //int prevGemType = Data.tileMap[prev.Value.X, prev.Value.Y].gem.gemType;

            //Data.tileMap[prev.Value.X, prev.Value.Y].gem.gemType = currentGemType;
            //Data.tileMap[current.Value.X, current.Value.Y].gem.gemType = prevGemType;

            //Data.tileMap[prev.Value.X, prev.Value.Y].isFilled = false;
            //Data.tileMap[current.Value.X, current.Value.Y].isFilled = false;

            //Data.tileMap[current.Value.X, current.Value.Y].gem.destination = Data.tileMap[prev.Value.X, prev.Value.Y].position;
            //Data.tileMap[prev.Value.X, prev.Value.Y].gem.destination = Data.tileMap[current.Value.X, current.Value.Y].position;

            if (prev.Value.Y > current.Value.Y)
            {
                Data.tileMap[current.Value.X, current.Value.Y].gem.Direction(Direction.down);
                Data.tileMap[prev.Value.X, prev.Value.Y].gem.Direction(Direction.up);
            }
            else if (prev.Value.Y < current.Value.Y)
            {
                Data.tileMap[current.Value.X, current.Value.Y].gem.Direction(Direction.up);
                Data.tileMap[prev.Value.X, prev.Value.Y].gem.Direction(Direction.down);
            }
            else if (prev.Value.X > current.Value.X)
            {
                Data.tileMap[current.Value.X, current.Value.Y].gem.Direction(Direction.right);
                Data.tileMap[prev.Value.X, prev.Value.Y].gem.Direction(Direction.left);
            }
            else if (prev.Value.X < current.Value.X)
            {
                Data.tileMap[current.Value.X, current.Value.Y].gem.Direction(Direction.left);
                Data.tileMap[prev.Value.X, prev.Value.Y].gem.Direction(Direction.right);
            }
        }

        public void GiveGemsDestination()
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

        public void HasGemReachedDestination(GameTime gameTime)
        {
            for (int x = 0; x < Data.tileMap.GetLength(0); x++)
            {
                for (int y = Data.tileMap.GetLength(1) - 1; y >= 0; y--)
                {
                    if (Data.tileMap[x, y].gem != null && Data.tileMap[x, y].gem.destination != null)
                    {
                        float yDestination = Data.tileMap[x, y].gem.destination.Value.Y * Data.tileLocation + Data.tileMapOffset.Y;
                        float xDestination = Data.tileMap[x, y].gem.destination.Value.X * Data.tileLocation + Data.tileMapOffset.X;

                        if (Data.tileMap[x, y].gem.velociy.X == 0)
                        {
                            if (Data.tileMap[x, y].gem.position.Y >= yDestination)
                            {
                                ChangeGem(x, y);
                            }
                        }

                        //if (Data.tileMap[x, y].gem.velociy.Y == 0 && Data.tileMap[x, y].gem.destination != null)
                        //{
                        //    if (Data.tileMap[x, y].gem.destination.Value.X > Data.tileMap[x, y].position.X && Data.tileMap[x, y].gem.position.X >= xDestination ||
                        //        Data.tileMap[x, y].gem.destination.Value.X < Data.tileMap[x, y].position.X && Data.tileMap[x, y].gem.position.X <= xDestination)
                        //    {
                        //        ChangeGem(x, y);
                        //    }
                        //}
                    }
                }
            }

            if (canClear)
            {
                ClearMatches(gameTime);
            }
        }

        public void ChangeGem(int x, int y)
        {
            Data.tileMap[x, y].gem.Direction(Direction.none);
            Data.tileMap[x, y].gem.position = Data.tileMap[x, y].gem.destination.Value.ToVector2() * Data.tileLocation + Data.tileMapOffset;
            Data.tileMap[x, y].gem.destination = null;
            Data.tileMap[x, y].isFilled = true;
            canClear = true;
        }

        public void CheckPlayingField()
        {
            CheckIfGemsCanMove();

            if (anyGemCanMove)
            {
                GiveGemsDestination();
            }

            if (gemsLeft > 0)
            {
                SpawnNewGems();
            }
        }

        private void CheckIfGemsCanMove()
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

        public void SpawnNewGems()
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
                        Gem temp = new(new(x * Data.tileLocation + Data.tileMapOffset.X, (-Data.tileSize) - amountOfGemsAddedPerRow * Data.tileLocation), randomGem)
                        {
                            destination = emptySlot[i]
                        };

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

        public void ClearMatches(GameTime gameTime)
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
                canClear = false;
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

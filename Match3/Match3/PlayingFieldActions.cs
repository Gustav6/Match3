using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3
{
    public class PlayingFieldActions
    {
        private static int gemsLeft = 50;

        public static void CheckUnderGem()
        {
            for (int i = Data.gameObjects.Count - 1; i >= 0; i--)
            {
                if (Data.gameObjects[i] is Gem g)
                {
                    if ((int)(g.position.Y / Data.tileSize) == Data.tileMap.GetLength(1) - 1 || Data.tileMap[(int)(g.position.X / Data.tileSize), (int)(g.position.Y / Data.tileSize) + 1].gem != null)
                    {
                        // Make position more accurate on the tile, and add check if gem can keep moving
                        Gem temp = new(new(g.position.X, Data.tileMap[(int)(g.position.X / Data.tileSize), (int)(g.position.Y / Data.tileSize)].position.Y * Data.tileSize), g.gemType);

                        Data.tileMap[(int)(g.position.X / Data.tileSize), (int)(g.position.Y / Data.tileSize)].gem = temp;
                        Data.gameObjects.RemoveAt(i);
                        PlayingField.ClearMatches();
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
                        }
                    }
                }
            }

            for (int i = 0; i < Data.gameObjects.Count; i++)
            {
                if (Data.gameObjects[i] is Gem g)
                {
                    if (g.velociy == Vector2.Zero)
                    {
                        g.Direction(Direction.down);
                    }
                }
            }

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

        private static List<Point> RowWithMissingGem()
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
    }
}

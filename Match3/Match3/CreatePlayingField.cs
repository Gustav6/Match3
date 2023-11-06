using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3
{
    public static class CreatePlayingField
    {
        public static void FinalizedPlayingField()
        {
            CreateTempMap();
            SpawnGems();
        }

        private static void SpawnGems()
        {
            for (int x = 0; x < Data.tileMap.GetLength(0); x++)
            {
                for (int y = 0; y < Data.tileMap.GetLength(1); y++)
                {
                    if (Data.tileMap[x, y].canHaveGem)
                    {
                        Data.tileMap[x, y] = new Tile() { gem = new Gem(new Vector2(x * Data.tileSize, y * Data.tileSize), Data.Random(0, TextureManager.textures.Length)), canHaveGem = true, position = new Point(x, y) };

                        if (Data.InBounds(x, y - 2) && Data.tileMap[x, y - 2].gem != null && Data.tileMap[x, y].gem != null)
                        {
                            if (Data.tileMap[x, y].gem.texutre == Data.tileMap[x, y - 2].gem.texutre)
                            {
                                Data.tileMap[x, y].gem = null;
                                y--;
                            }
                        }

                        if (Data.InBounds(x - 2, y) && Data.tileMap[x - 2, y].gem != null && Data.tileMap[x, y].gem != null)
                        {
                            if (Data.tileMap[x, y].gem.texutre == Data.tileMap[x - 2, y].gem.texutre)
                            {
                                Data.tileMap[x, y].gem = null;
                                y--;
                            }
                        }   
                    }
                }
            }
        }

        private static void CreateTempMap()
        {
            // Create a map that shows what tiles can hold gems (0 = cant, 1 = can)
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
            {
                for (int x = 0; x < Data.tileMap.GetLength(0); x++)
                {
                    int number = tempMap[y, x];

                    if (number == 1)
                    {
                        Data.tileMap[x, y].canHaveGem = true;
                    }
                }
            }
        }
    }
}

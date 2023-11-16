using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3
{
    public class CreatePlayingField
    {
        public static void CreateNewPlayingField(int[,] map)
        {
            CreateTileMap(map);
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
                        Vector2 temp = new(x * Data.tileLocation + Data.tileMapOffset.X, y * Data.tileLocation + Data.tileMapOffset.Y);
                        Data.tileMap[x, y] = new Tile() { gem = new Gem(temp, Data.Random(0, TextureManager.textures.Length)), canHaveGem = true, position = new Point(x, y), isFilled = true };

                        if (Data.InBounds(x, y - 2) && Data.tileMap[x, y - 2].gem != null && Data.tileMap[x, y].gem != null)
                        {
                            if (Data.tileMap[x, y].gem.texture == Data.tileMap[x, y - 2].gem.texture)
                            {
                                Data.tileMap[x, y].gem = null;
                                y--;
                            }
                        }

                        if (Data.InBounds(x - 2, y) && Data.tileMap[x - 2, y].gem != null && Data.tileMap[x, y].gem != null)
                        {
                            if (Data.tileMap[x, y].gem.texture == Data.tileMap[x - 2, y].gem.texture)
                            {
                                Data.tileMap[x, y].gem = null;
                                y--;
                            }
                        }   
                    }
                }
            }
        }

        private static void CreateTileMap(int[,] tempMap)
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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3
{
    public static class PlayingField
    {

        public static void Update()
        {
            ClearMatch();

            for (int x = 0; x < Data.tileMap.GetLength(0); x++)
            {
                for (int y = 0; y < Data.tileMap.GetLength(1); y++)
                {
                    //if (Data.tileMap[x, y].gem != null)
                    //{

                    //}
                }
            }
        }

        private static void ClearMatch()
        {
            if (Matches())
            {
                // Remove objects that matches
            }
        }

        private static bool Matches()
        {
            for (int x = 0; x < Data.tileMap.GetLength(0); x++)
            {
                for (int y = 0; y < Data.tileMap.GetLength(1); y++)
                {
                    // Check tilemap if 3 or more neighbours makes a match
                }
            }

            return false;
        }

        public static void CreatePlayingField()
        {
            int[,] tempMap = new int[,]
            {
                {1, 1, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 1},
                {0, 1, 1, 1, 1, 1, 0},
                {0, 0, 1, 1, 1, 0, 0},
                {0, 1, 1, 0, 1, 1, 0},
                {0, 0, 1, 1, 1, 0, 0},
                {0, 1, 1, 1, 1, 1, 0},
                {1, 1, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 1}
            };
            Data.tileMap = new Tile[tempMap.GetLength(1), tempMap.GetLength(0)];

            CreateTileMap(tempMap);
        }

        public static void CreateTileMap(int[,] tempMap)
        {
            for (int x = 0; x < Data.tileMap.GetLength(0); x++)
            {
                for (int y = 0; y < Data.tileMap.GetLength(1); y++)
                {
                    int number = tempMap[y, x];

                    if (number == 1)
                    {
                        Data.tileMap[x, y].isFilled = true;
                    }
                }
            }
        }
    }
}

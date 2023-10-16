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
                    if (Data.tileMap[x, y].gem != null)
                    {

                    }
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
            Data.tileMap = new Tile[Data.gameWidth, Data.gameHeight];

            //SpawnRandomGems();
        }

        public static void SpawnRandomGems()
        {
            for (int x = 0; x < Data.tileMap.GetLength(0); x++)
            {
                for (int y = 0; y < Data.tileMap.GetLength(1); y++)
                {

                }
            }
        }
    }
}

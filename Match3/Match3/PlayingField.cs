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
            MoveGem();
            ClearMatch();
        }

        public static void MoveGem()
        {
            for (int i = 0; i < Data.gameObjects.Count; i++)
            {
                if (Data.gameObjects[i] is Gem g)
                    if (InputManager.GetMouseBounds(true).Intersects(g.boundingBox) && InputManager.MouseHasBeenPressed(InputManager.currentMS.LeftButton, InputManager.prevMS.LeftButton))
                    {
                        // Move gem with mouse and move back if it does not make a match
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

        public static void FinalPlayingField()
        {
            CreatePlayingField();
            SpawnGems();
        }

        public static void SpawnGems()
        {
            for (int x = 0; x < Data.tileMap.GetLength(0); x++)
                for (int y = 0; y < Data.tileMap.GetLength(1); y++)
                    if (Data.tileMap[x, y].canHaveGem)
                    {
                        // Controll before spawning if it will make a match
                        Data.gameObjects.Add(new Gem(new Vector2(x * Data.tileSize, y * Data.tileSize), Data.Random(0, TextureManager.textures.Length)));
                    }
        }

        public static void CreatePlayingField()
        {
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
                {0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0},
            };
            Data.tileMap = new Tile[tempMap.GetLength(1), tempMap.GetLength(0)];

            CreateTileMap(tempMap);
        }

        public static void CreateTileMap(int[,] tempMap)
        {
            for (int x = 0; x < Data.tileMap.GetLength(0); x++)
                for (int y = 0; y < Data.tileMap.GetLength(1); y++)
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

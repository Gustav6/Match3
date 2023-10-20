using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Match3
{
    public static class PlayingField
    {
        //private static Gem pastGem;
        //private static Gem currentGem;
        private static int[,] pastGem;
        private static int[,] currentGem;
        private static Gem temp;
        static int mouseY;
        static int mouseX;
        //static Vector2 temp;

        public static void Update()
        {
            //CheckForGem();
            MoveGem();
            //ClearMatch();
        }

        public static void MoveGem()
        {
            mouseX = InputManager.currentMS.X / Data.tileSize;
            mouseY = InputManager.currentMS.Y / Data.tileSize;

            SelectedGem();

            if (currentGem != null && pastGem != null && pastGem != currentGem)
            {
                temp = Data.tileMap[pastGem.GetLength(0), pastGem.GetLength(1)].gem;

                Data.tileMap[pastGem.GetLength(0), pastGem.GetLength(1)].gem = Data.tileMap[currentGem.GetLength(0), currentGem.GetLength(1)].gem;
                Data.tileMap[currentGem.GetLength(0), currentGem.GetLength(1)].gem = temp;

                currentGem = null;
                pastGem = null;
            }

            if (currentGem != null)
            {
                pastGem = currentGem;
            }
        }

        public static void SelectedGem()
        {
            if (0 <= mouseX && mouseX < Data.tileMap.GetLength(0))
                if (0 <= mouseY && mouseY < Data.tileMap.GetLength(1))
                    if (InputManager.MouseHasBeenPressed(InputManager.currentMS.LeftButton, InputManager.prevMS.LeftButton))
                        currentGem = new int[mouseX, mouseY];
        }

        private static void Matches()
        {
            for (int x = 0; x < Data.tileMap.GetLength(0); x++)
            {
                for (int y = 0; y < Data.tileMap.GetLength(1); y++)
                {
                    bool matchFound = true;

                    // Check tilemap if 3 or more neighbours makes a match

                    //if (Data.tileMap[x, y])
                    //{

                    //}

                }
            }

            for (int x = 0; x < Data.tileMap.GetLength(0); x++)
            {
                for (int y = 0; y < Data.tileMap.GetLength(1); y++)
                {
                    // Check tilemap if 3 or more neighbours makes a match


                }
            }
        }

        private static bool inBounds(int x, int y)
        {
            return 0 <= y && y < Data.tileMap.GetLength(1) && 0 <= x && x < Data.tileMap.GetLength(0);
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
                        Data.tileMap[x, y] = new Tile() { gem = new Gem(new Vector2(x * Data.tileSize, y * Data.tileSize), Data.Random(0, TextureManager.textures.Length)) };
                        // Controll before spawning if it will make a match
                        //Data.gameObjects.Add(new Gem(new Vector2(x * Data.tileSize, y * Data.tileSize), Data.Random(0, TextureManager.textures.Length)));
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

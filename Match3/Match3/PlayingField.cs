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
        private static int[,] pastGem;
        private static int[,] currentGem;

        public static void Update()
        {
            //CheckForGem();
            MoveGem();
            //ClearMatch();
        }

        public static void MoveGem()
        {
            // Inputs
            int mouseX = InputManager.currentMS.X / Data.tileSize;
            int mouseY = InputManager.currentMS.Y / Data.tileSize;

            if (InputManager.MouseHasBeenPressed(InputManager.currentMS.LeftButton, InputManager.prevMS.LeftButton))
                SelectedGem(mouseX, mouseY);

            if (currentGem != null && pastGem != null && pastGem != currentGem)
            {
                // Check if object can or cant switch
                if (pastGem.GetLength(0) + 1 == currentGem.GetLength(0) && pastGem.GetLength(1) == currentGem.GetLength(1) || 
                    pastGem.GetLength(0) - 1 == currentGem.GetLength(0) && pastGem.GetLength(1) == currentGem.GetLength(1) ||
                    pastGem.GetLength(0) == currentGem.GetLength(0) && pastGem.GetLength(1) + 1 == currentGem.GetLength(1) || 
                    pastGem.GetLength(0) == currentGem.GetLength(0) && pastGem.GetLength(1) - 1 == currentGem.GetLength(1))
                {
                    Gem temp = Data.tileMap[pastGem.GetLength(0), pastGem.GetLength(1)].gem;

                    Data.tileMap[pastGem.GetLength(0), pastGem.GetLength(1)].gem = Data.tileMap[currentGem.GetLength(0), currentGem.GetLength(1)].gem;
                    Data.tileMap[currentGem.GetLength(0), currentGem.GetLength(1)].gem = temp;

                    Data.tileMap[currentGem.GetLength(0), currentGem.GetLength(1)].gem.color = Color.White;
                    Data.tileMap[pastGem.GetLength(0), pastGem.GetLength(1)].gem.color = Color.White;
                }
                else if (pastGem.GetLength(0) + 1 == currentGem.GetLength(0) && pastGem.GetLength(1) != currentGem.GetLength(1) ||
                         pastGem.GetLength(0) - 1 == currentGem.GetLength(0) && pastGem.GetLength(1) != currentGem.GetLength(1) ||
                         pastGem.GetLength(0) != currentGem.GetLength(0) && pastGem.GetLength(1) + 1 == currentGem.GetLength(1) ||
                         pastGem.GetLength(0) != currentGem.GetLength(0) && pastGem.GetLength(1) - 1 == currentGem.GetLength(1))
                {
                    Data.tileMap[currentGem.GetLength(0), currentGem.GetLength(1)].gem.color = Color.White;
                    Data.tileMap[pastGem.GetLength(0), pastGem.GetLength(1)].gem.color = Color.White;
                }

                // Resets variables

                currentGem = null;
                pastGem = null;
            }

            if (currentGem != null)
                pastGem = currentGem;
        }

        public static void SelectedGem(int _mouseX, int _mouseY)
        {
            if (0 <= _mouseX && _mouseX < Data.tileMap.GetLength(0))
                if (0 <= _mouseY && _mouseY < Data.tileMap.GetLength(1))
                        if (Data.tileMap[_mouseX, _mouseY].canHaveGem)
                        {
                            currentGem = new int[_mouseX, _mouseY];

                            // Make a visual change when selecting object
                            Data.tileMap[currentGem.GetLength(0), currentGem.GetLength(1)].gem.color *= 0.8f;

                            // Revert change if you press same object and or invalid target
                            if (pastGem != null && Data.tileMap[currentGem.GetLength(0), currentGem.GetLength(1)].gem == Data.tileMap[pastGem.GetLength(0), pastGem.GetLength(1)].gem)
                                Data.tileMap[pastGem.GetLength(0), pastGem.GetLength(1)].gem.color = Color.White;
                        }
        }

        private static void Matches()
        {
            // Check tilemap if 3 or more horizontal objects make a match
            for (int x = 0; x < Data.tileMap.GetLength(0); x++)
            {
                for (int y = 0; y < Data.tileMap.GetLength(1); y++)
                {

                }
            }

            // Check tilemap if 3 or more veritcal objects make a match
            for (int y = 0; y < Data.tileMap.GetLength(1); y++)
            {
                for (int x = 0; x < Data.tileMap.GetLength(0); x++)
                {

                }
            }
        }

        private static bool inBounds(int x, int y)
        {
            return 0 <= y && y < Data.tileMap.GetLength(1) && 0 <= x && x < Data.tileMap.GetLength(0);
        }

        public static void CompletedPlayingField()
        {
            CreatePlayingField();
            SpawnGems();
        }

        public static void SpawnGems()
        {
            for (int x = 0; x < Data.tileMap.GetLength(0); x++)
                for (int y = 0; y < Data.tileMap.GetLength(1); y++)
                    if (Data.tileMap[x, y].canHaveGem)
                        Data.tileMap[x, y] = new Tile() { gem = new Gem(new Vector2(x * Data.tileSize, y * Data.tileSize), Data.Random(0, TextureManager.textures.Length)), canHaveGem = true, removed = false };
        }

        public static void CreatePlayingField()
        {
            // Create a map that shows where gems can be (0 = cant, 1 = can)
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
            // Set project wide tilemap to temp with the inverse of the temp map
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
                        Data.tileMap[x, y].canHaveGem = true;
                }
        }
    }
}

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3
{
    public class GameManger
    {
        public static void Update(GameTime gameTime)
        {
            InputManager.GetInput();
            UpdateLoop(gameTime);
            RemoveLoop();
        }

        private static void UpdateLoop(GameTime gameTime)
        {
            for (int i = 0; i < Data.gameObjects.Count; i++)
            {
                Data.gameObjects[i].Update(gameTime);
            }
        }

        private static void RemoveLoop()
        {
            for (int i = Data.gameObjects.Count - 1; i >= 0; i--)
            {
                if (Data.gameObjects[i].isRemoved)
                {
                    Data.gameObjects.RemoveAt(i);
                }
            }
        }

        public static bool CanMove()
        {
            for (int x = 0; x < Data.tileMap.GetLength(0); x++)
            {
                for (int y = 0; y < Data.tileMap.GetLength(1); y++)
                {
                    // Check if one move can get match with 3 or more

                }
            }

            return false;
        }

        public static bool GameOver()
        {
            if (!CanMove())
            {
                return true;
            }

            return false;
        }
    }
}

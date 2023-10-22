using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3
{
    public static class GameManger
    {
        public static void Update(GameTime gameTime)
        {
            InputManager.GetInput();
            PlayingField.Update();
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
    }
}

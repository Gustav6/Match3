using Microsoft.VisualBasic;
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
        readonly PlayingFieldInteraction interaction = new();

        public void Update(GameTime gameTime)
        {
            PlayingField();
            UpdateLoop(gameTime);
            RemoveLoop();
        }

        private static void UpdateLoop(GameTime gameTime)
        {
            // run updatefor every game object in game objects list
            for (int i = 0; i < Data.gameObjects.Count; i++)
            {
                Data.gameObjects[i].Update(gameTime);
            }

            // run update for every gem in the tilemap
            for (int x = 0; x < Data.tileMap.GetLength(0); x++)
            {
                for (int y = 0; y < Data.tileMap.GetLength(1); y++)
                {
                    Data.tileMap[x, y].gem?.Update(gameTime);
                }
            }
        }

        public void PlayingField()
        {
            PlayingFieldAction.CheckIfGemCanMove();
            interaction.MoveSelectedGem();
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

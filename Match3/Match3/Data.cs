using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3
{
    public static class Data
    {
        public static int gamePoints;

        public static List<GameObject> gameObjects = new();

        public static Tile[,] tileMap;
        public static int tileSize = 64;

        public static int bufferWidth = 1920;
        public static int bufferHeight = 1080;

        public static int Random(int minimumValue, int maximumValue)
        {
            Random rng = new();
            return rng.Next(minimumValue, maximumValue);
        }

        public static bool InBounds(int x, int y)
        {
            return 0 <= y && y < Data.tileMap.GetLength(1) && 0 <= x && x < Data.tileMap.GetLength(0);
        }

        public static void Gravity(Gem gem)
        {

        }
    }

    public enum GemType
    {
        blueGemTexture,
        redGemTexture,
        greenGemTexture,
        purpleGemTexture,
        yellowgemTexture
    }

    public enum Direction
    {
        up,
        down,
        left,
        right,
        none
    }
}

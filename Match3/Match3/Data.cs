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
        public static List<GameObject> gameObjects = new List<GameObject>();

        public static Tile[,] tileMap;
        public static int gameHeight = 10;
        public static int gameWidth = 8;
        public static int tileSize = 64;

        public static int bufferWidth = 1920;
        public static int bufferHeight = 1080;

        public static int Random(int minimumValue, int maximumValue)
        {
            Random rng = new Random();
            return rng.Next(minimumValue, maximumValue);
        }

        public enum gemType
        {
            blueGemTexture,
            redGemTexture,
            greenGemTexture,
            purpleGemTexture,
            yellowgemTexture
        }
    }
}

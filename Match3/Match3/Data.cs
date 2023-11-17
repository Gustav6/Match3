using Microsoft.Xna.Framework;
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
        public static int gemsCollected;

        public static List<GameObject> gameObjects = new();
        public static List<Explosion> explosions = new();
        public static readonly PlayingField playingField = new();

        public static int bufferWidth = 1920;
        public static int bufferHeight = 1080;

        public static Tile[,] tileMap;

        public static int gemSize = 64;
        public static int gemMaxMoveSpeed = 600;
        public static int gemStartSpeed = 75;

        public static int tileSize = 72;
        public static float tileSpacing = 1.2f;
        public static Vector2 gemOrigin = new (32, 32);
        public static int tileLocation = (int)(gemSize * tileSpacing);
        public static Vector2 tileMapOffset = new (300 + gemOrigin.X, 110 + gemOrigin.Y);
        public static GraphicsDevice graphicsDevice;

        public static float volume = 1;

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
        clearGemTexture
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

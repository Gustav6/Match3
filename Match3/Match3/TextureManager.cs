using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3
{
    public static class TextureManager
    {
        public static Texture2D tileTexture;
        public static Texture2D boundingBoxTexture;
        public static Texture2D[] textures;
        public static Texture2D explosionTexture;
        public static SpriteFont font;
        public static Texture2D background;

        public static void LoadTextures(ContentManager content, GraphicsDevice graphicsDevice)
        {
            font = content.Load<SpriteFont>("spritefont");
            textures = new Texture2D[Enum.GetNames<GemType>().Length];

            tileTexture = GenerateTexture(graphicsDevice, Data.tileSize, Data.tileSize);

            boundingBoxTexture = new Texture2D(graphicsDevice, 1, 1);
            boundingBoxTexture.SetData<Color>(new Color[] { Color.Green * 0.8f });

            explosionTexture = content.Load<Texture2D>("visualEffect");
            //textures[(int)GemType.blueGemTexture] = (content.Load<Texture2D>("yellowGem"));
            textures[(int)GemType.greenGemTexture] = content.Load<Texture2D>("gem1");
            textures[(int)GemType.purpleGemTexture] = content.Load<Texture2D>("gem2");
            textures[(int)GemType.redGemTexture] = content.Load<Texture2D>("gem3");
            textures[(int)GemType.blueGemTexture] = content.Load<Texture2D>("gem4");
            textures[(int)GemType.clearGemTexture] = content.Load<Texture2D>("gem5");

            background = content.Load<Texture2D>("background");
        }

        public static Texture2D GenerateTexture(GraphicsDevice graphicsDevice, int width, int height)
        {
            Texture2D temp;

            temp = new Texture2D(graphicsDevice, width, height);
            Color[] colorArray = new Color[width * height];
            for (int i = 0; i < colorArray.Length; i++)
            {
                colorArray[i] = Color.White;
            }
            temp.SetData<Color>(colorArray);

            return temp;
        }
    }
}

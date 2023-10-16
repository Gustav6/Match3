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
        public static Texture2D[] arrayOfTextures = new Texture2D[10];

        public static void LoadTextures(ContentManager content, GraphicsDevice graphicsDevice)
        {
            tileTexture = new Texture2D(graphicsDevice, Data.tileSize, Data.tileSize);
            Color[] colorArray = new Color[Data.tileSize * Data.tileSize];
            for (int i = 0; i < colorArray.Length; i++)
            {
                colorArray[i] = Color.White;
            }
            tileTexture.SetData<Color>(colorArray);

            arrayOfTextures[(int)Data.gemType.blueGemTexture] = (content.Load<Texture2D>("gemBlue"));
            arrayOfTextures[(int)Data.gemType.greenGemTexture] = content.Load<Texture2D>("gemGreen");
            arrayOfTextures[(int)Data.gemType.purpleGemTexture] = content.Load<Texture2D>("gemPurple");
            arrayOfTextures[(int)Data.gemType.redGemTexture] = content.Load<Texture2D>("gemRed");
            arrayOfTextures[(int)Data.gemType.yellowgemTexture] = content.Load<Texture2D>("gemYellow");
        }
    }
}

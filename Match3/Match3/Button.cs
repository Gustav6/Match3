using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Match3
{
    public class Button : InteractableGui
    {
        public string stringOnButton;
        public Color stringColor;
        public Color buttonColor;
        public Vector2 stringLocation;
        public int width;
        public int height;
        public float alpha;

        public Button(Vector2 _position, int _width, int _height, Color _color, string _stringOnButton, Color _stringColor)
        {
            position = _position;
            stringOnButton = _stringOnButton;
            stringLocation = new Vector2(_position.X - (TextureManager.font.MeasureString(_stringOnButton).X / 2), _position.Y - (TextureManager.font.MeasureString(_stringOnButton).Y / 2));
            stringColor = _stringColor;
            alpha = 100;
            buttonColor = _color;
            width = _width;
            height = _height;

            SetButtonTexture(width, height);
            SetScale(1, 1);
            SetOrigin(texture);
            SetSourceRectangle(texture);
            SetBoundingBoxSizeAndLocation(width, height);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void SetButtonTexture(int width, int height)
        {
            texture = new Texture2D(Data.graphicsDevice, width, height);
            SetButtonColor(buttonColor);
        }

        public void SetButtonColor(Color _color)
        {
            Color[] colorArray = new Color[width * height];
            for (int i = 0; i < colorArray.Length; i++)
            {
                colorArray[i] = color;
            }
            texture.SetData<Color>(colorArray);
            color = _color;
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.DrawString(TextureManager.font, stringOnButton, stringLocation, stringColor);
            //DrawBoundingBox(spriteBatch);
        }
    }
}

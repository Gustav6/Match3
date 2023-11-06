using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3
{
    public abstract class GameObject
    {
        public Texture2D texutre;
        public Vector2 position;
        public Color color;
        public bool isRemoved;
        public Vector2 scale;
        public Rectangle soureceRectangle;
        public float rotation;
        public Vector2 origin;

        public virtual void Update(GameTime gameTime)
        {

        }

        public void SetSource(Texture2D _texture)
        {
            soureceRectangle = new Rectangle(0, 0, _texture.Width, _texture.Height);
        }

        public static void Destroy(GameObject gameObject)
        {
            gameObject.isRemoved = true;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texutre, position, soureceRectangle, color, rotation, origin, scale, SpriteEffects.None, 0);
        }
    }
}

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
        public Texture2D texture;
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

        public void SetSourceRectangle(Texture2D _texture)
        {
            soureceRectangle = new Rectangle(0, 0, _texture.Width, _texture.Height);
        }
        public void SetOrigin(Texture2D _texture)
        {
            origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
        }

        public static void Destroy(GameObject gameObject)
        {
            gameObject.isRemoved = true;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, soureceRectangle, color, rotation, origin, scale, SpriteEffects.None, 0);
        }
    }
}

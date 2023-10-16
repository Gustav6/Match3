using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3
{
    public abstract class Moveable : GameObject
    {
        public Vector2 velociy;
        public float moveSpeed;

        public override void Update(GameTime gameTime)
        {
            Move(gameTime);
            base.Update(gameTime);
        }

        public void Move(GameTime gameTime)
        {
            if (velociy != Vector2.Zero)
                velociy.Normalize();

            position += velociy * moveSpeed * (float)gameTime.ElapsedGameTime.Seconds;
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Match3
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferHeight = Data.bufferHeight;
            _graphics.PreferredBackBufferWidth = Data.bufferWidth;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            PlayingField.CreatePlayingField();

            Data.gameObjects.Add(new Gem(new Vector2(200, 200), TextureManager.arrayOfTextures[(int)Data.gemType.greenGemTexture]));
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            TextureManager.LoadTextures(Content, GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            GameManger.Update(gameTime);
            PlayingField.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            for (int x = 0; x < Data.tileMap.GetLength(0); x++)
            {
                for (int y = 0; y < Data.tileMap.GetLength(1); y++)
                {
                    _spriteBatch.Draw(Data.tileMap[x, y].isFilled ? Data.tileMap[x, y].gem.texutre : TextureManager.tileTexture, new Vector2(x * Data.tileSize, y * Data.tileSize), Color.White);
                }
            }

            for (int i = 0; i < Data.gameObjects.Count; i++)
            {
                Data.gameObjects[i].Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
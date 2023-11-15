using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Match3
{
    public class Game1 : Game
    {
        private Explosion explosion;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private bool debugF;
        private bool debugT;

        private readonly GameManger gameManager = new();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferHeight = Data.bufferHeight,
                PreferredBackBufferWidth = Data.bufferWidth
            };
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            explosion = new(new (1000, 200));

            CreatePlayingField.CreateNewPlayingField(new int[,]
            {
                {0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0},
                {0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                {0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0},
                {0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0}
            });
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

            InputManager.GetInput();

            explosion.Update(gameTime);

            if (InputManager.HasBeenPressed(Keys.Q))
            {
                debugF = !debugF;
            }
            if (InputManager.HasBeenPressed(Keys.W))
            {
                debugT = !debugT;
            }

            gameManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            _spriteBatch.DrawString(TextureManager.font, "Gems Left: " + Data.playingField.gemsLeft.ToString(), new Vector2(1000, 75), Color.White);
            _spriteBatch.DrawString(TextureManager.font, "Points: " + Data.gamePoints.ToString(), new Vector2(1000, 125), Color.Green);

            for (int x = 0; x < Data.tileMap.GetLength(0); x++)
            {
                for (int y = 0; y < Data.tileMap.GetLength(1); y++)
                {
                    Vector2 temp = new((int)((x * Data.tileLocation + Data.tileMapOffset.X) - Data.gemOrigin.X), (int)((y * Data.tileLocation + Data.tileMapOffset.Y) - Data.gemOrigin.Y));
                    //_spriteBatch.Draw(TextureManager.tileTexture, temp, Data.tileMap[x, y].canHaveGem ? Color.White * 0.2f : Color.Blue * 0.2f);

                    Data.tileMap[x, y].gem?.Draw(_spriteBatch);

                    if (debugF)
                    {
                        if (Data.tileMap[x, y].isFilled)
                        {
                            _spriteBatch.Draw(TextureManager.tileTexture, temp, Color.White);
                            _spriteBatch.DrawString(TextureManager.font, "IsFilled Debug", new Vector2(100, Data.bufferHeight - 100), Color.White);
                        }
                    }
                    else if (debugT)
                    {
                        if (Data.tileMap[x, y].gem != null)
                        {
                            _spriteBatch.Draw(TextureManager.tileTexture, temp, Color.Purple);
                            _spriteBatch.DrawString(TextureManager.font, "Gem Null Debug", new Vector2(100, Data.bufferHeight - 100), Color.White);
                        }
                    }
                }
            }
            for (int i = 0; i < Data.gameObjects.Count; i++)
            {
                Data.gameObjects[i].Draw(_spriteBatch);
            }

            explosion.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
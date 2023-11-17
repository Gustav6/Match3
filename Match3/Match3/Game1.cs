using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Match3
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private bool debugF;
        private bool debugG;
        private bool debugS;

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

            MenuManager.Initialize();

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

            MediaPlayer.Play(SoundManager.backgroundMusic);
        }

        protected override void LoadContent()
        {
            Data.graphicsDevice = GraphicsDevice;
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            SoundManager.LoadSounds(Content);
            TextureManager.LoadTextures(Content, GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            InputManager.GetInput();

            for (int i = 0; i < Data.explosions.Count; i++)
            {
                Data.explosions[i].Update(gameTime);
            }

            if (InputManager.HasBeenPressed(Keys.Q))
            {
                debugF = !debugF;
            }
            else if (InputManager.HasBeenPressed(Keys.W))
            {
                debugG = !debugG;
            }
            else if (InputManager.HasBeenPressed(Keys.E))
            {
                debugS = !debugS;
            }

            gameManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            _spriteBatch.Draw(TextureManager.background, new Vector2(0, 0), Color.White);

            _spriteBatch.DrawString(TextureManager.font, "Gems Left In Mine: \n" + Data.playingField.gemsLeft.ToString(), new Vector2(1500, 100), Color.White);
            _spriteBatch.DrawString(TextureManager.font, "Total Gems Mined: \n" + Data.gemsCollected.ToString(), new Vector2(1500, 200), Color.White);

            for (int x = 0; x < Data.tileMap.GetLength(0); x++)
            {
                for (int y = 0; y < Data.tileMap.GetLength(1); y++)
                {
                    Vector2 temp = new((int)((x * Data.tileLocation + Data.tileMapOffset.X) - Data.gemOrigin.X), (int)((y * Data.tileLocation + Data.tileMapOffset.Y) - Data.gemOrigin.Y));
                    temp.Y -= (Data.tileSize - Data.gemSize) / 2;
                    temp.X -= (Data.tileSize - Data.gemSize) / 2;

                    if (Data.tileMap[x, y].canHaveGem)
                    {
                        _spriteBatch.Draw(TextureManager.tileTexture, temp,  Color.MediumPurple * 0.5f);
                    }

                    Data.tileMap[x, y].gem?.Draw(_spriteBatch);

                    if (debugF)
                    {
                        if (Data.tileMap[x, y].isFilled)
                        {
                            _spriteBatch.Draw(TextureManager.tileTexture, temp, Color.Red);
                            _spriteBatch.DrawString(TextureManager.font, "IsFilled Debug", new Vector2(100, Data.bufferHeight - 100), Color.White);
                        }
                    }
                    else if (debugG)
                    {
                        if (Data.tileMap[x, y].gem != null)
                        {
                            _spriteBatch.Draw(TextureManager.tileTexture, temp, Color.Green);
                            _spriteBatch.DrawString(TextureManager.font, "Gem Null Debug", new Vector2(100, Data.bufferHeight - 100), Color.White);
                        }
                    }
                    else if (debugS)
                    {
                        if (Data.tileMap[x, y].gem != null && Data.tileMap[x, y].canHaveGem)
                        {
                            _spriteBatch.Draw(TextureManager.tileTexture, temp, Color.White);
                            _spriteBatch.DrawString(TextureManager.font, Data.tileMap[x, y].gem.gemType.ToString(), new Vector2(temp.X + Data.gemSize / 2.8f, temp.Y + Data.gemSize / 5), Color.Black);
                            _spriteBatch.DrawString(TextureManager.font, "Gem Type Debug", new Vector2(100, Data.bufferHeight - 100), Color.White);
                        }
                    }
                }
            }
            for (int i = 0; i < Data.gameObjects.Count; i++)
            {
                Data.gameObjects[i].Draw(_spriteBatch);
            }

            for (int i = 0; i < Data.explosions.Count; i++)
            {
                Data.explosions[i].Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
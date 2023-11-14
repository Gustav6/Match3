﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Match3
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private bool debugF;
        private bool debugT;

        private GameManger gameManager = new();

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

            _spriteBatch.DrawString(TextureManager.font, "Gems Left: " + PlayingFieldAction.gemsLeft.ToString(), new Vector2(1000, 75), Color.White);
            _spriteBatch.DrawString(TextureManager.font, "Points: " + Data.gamePoints.ToString(), new Vector2(1000, 125), Color.Green);

            for (int x = 0; x < Data.tileMap.GetLength(0); x++)
            {
                for (int y = 0; y < Data.tileMap.GetLength(1); y++)
                {
                    Vector2 temp = new((int)((x * Data.tileLocation + Data.tileMapOffset.X)), (int)((y * Data.tileLocation + Data.tileMapOffset.Y)));
                    _spriteBatch.Draw(TextureManager.tileTexture, temp, Data.tileMap[x, y].canHaveGem ? Color.White * 0.2f : Color.Blue * 0.2f);
                }
            }

            for (int x = 0; x < Data.tileMap.GetLength(0); x++)
            {
                for (int y = 0; y < Data.tileMap.GetLength(1); y++)
                {
                    Data.tileMap[x, y].gem?.Draw(_spriteBatch);
                }
            }

            for (int x = 0; x < Data.tileMap.GetLength(0); x++)
            {
                for (int y = 0; y < Data.tileMap.GetLength(1); y++)
                {
                    if (debugF)
                    {
                        if (Data.tileMap[x, y].isFilled)
                        {
                            Vector2 temp = new((int)((x * Data.tileLocation + Data.tileMapOffset.X)), (int)((y * Data.tileLocation + Data.tileMapOffset.Y)));
                            _spriteBatch.Draw(TextureManager.tileTexture, temp, Color.White);
                            _spriteBatch.DrawString(TextureManager.font, "IsFilled Debug", new Vector2(100, Data.bufferHeight - 100), Color.White);
                        }
                    }
                    else if (debugT)
                    {
                        if (Data.tileMap[x, y].gem != null)
                        {
                            Vector2 temp = new((int)((x * Data.tileLocation + Data.tileMapOffset.X)), (int)((y * Data.tileLocation + Data.tileMapOffset.Y)));
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

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
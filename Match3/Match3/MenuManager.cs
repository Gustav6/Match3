using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3
{
    public static class MenuManager
    {
        public static Button lastButton;

        public static void Update()
        {
            ControlCurrentButton();
        }

        public static void CheckIfInteracted()
        {
            bool hasPressed = InputManager.MouseHasBeenPressed(InputManager.currentMS.LeftButton, InputManager.prevMS.LeftButton);

            if (hasPressed)
            {
                if (CurrentButton().stringOnButton == "Restart")
                {
                    Restart();
                }
                else if (CurrentButton().stringOnButton == "Mute Sounds")
                {
                    MuteSounds();
                }
            }
        }

        public static void Restart()
        {
            for (int x = 0; x < Data.tileMap.GetLength(0); x++)
            {
                for (int y = 0; y < Data.tileMap.GetLength(1); y++)
                {
                    if (Data.tileMap[x, y].gem != null)
                    {
                        Vector2 temp = new(x * Data.tileLocation + Data.tileMapOffset.X - 64, y * Data.tileLocation + Data.tileMapOffset.Y - 64);
                        Data.explosions.Add(new Explosion(temp, Color.White));
                    }
                }
            }

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

            if (Data.playingField.gemsLeft != Data.playingField.maxGemsThatCanBeAdded)
            {
                Data.playingField.gemsLeft = Data.playingField.maxGemsThatCanBeAdded;
            }

            Data.gemsCollected = 0;
            SoundManager.collectSound.Play(Data.volume, 0.5f, 1);
        }

        public static void MuteSounds()
        {
            if (Data.volume != 0)
            {
                Data.volume = 0;
                CurrentButton().stringColor = Color.Red;
            }
            else
            {
                Data.volume = 1;
                CurrentButton().stringColor = Color.Green;
            }

            MediaPlayer.Volume = Data.volume;
        }

        public static Button CurrentButton()
        {
            for (int i = 0; i < Data.gameObjects.Count; i++)
            {
                if (Data.gameObjects[i] is Button b)
                {
                    if (InputManager.GetMouseBounds(true).Intersects(b.boundingBox))
                    {
                        return b;
                    }
                }
            }

            return null;
        }

        public static void ControlCurrentButton()
        {
            Button temp = CurrentButton();

            if (CurrentButton() != null)
            {
                CheckIfInteracted();
                temp.SetButtonColor(Color.Purple * 0.75f);
                if (temp.scale.X <= 1.2f && temp.scale.Y <= 1.1f)
                {
                    temp.scale.X = 1.0175f * MathF.Pow(temp.scale.X, 1.005f);
                    temp.scale.Y = 1.0145f * MathF.Pow(temp.scale.Y, 1.005f);
                }
                lastButton = temp;
            }
            else if (CurrentButton() == null && lastButton != null)
            {
                lastButton.SetScale(1, 1);
                lastButton.SetButtonColor(Color.White * 0);
            }
        }
        public static void Initialize()
        {
            Data.gameObjects.Add(new Button(new(1700, 900), 250, 75, Color.White * 0, "Restart", Color.White));
            Data.gameObjects.Add(new Button(new(1700, 1005), 345, 75, Color.White * 0, "Mute Sounds", Color.Green));
        }
    }
}

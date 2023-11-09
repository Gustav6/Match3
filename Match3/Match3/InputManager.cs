using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3
{
    public static class InputManager
    {
        static KeyboardState currentKS;
        static KeyboardState prevKS;
        public static MouseState currentMS;
        public static MouseState prevMS;

        public static void GetInput()
        {
            GetKeyboardState();
            GetMouseState();
        }

        private static KeyboardState GetKeyboardState()
        {
            prevKS = currentKS;
            currentKS = Keyboard.GetState();
            return currentKS;
        }

        private static MouseState GetMouseState()
        {
            prevMS = currentMS;
            currentMS = Mouse.GetState();
            return currentMS;
        }

        public static Rectangle GetMouseBounds(bool currentState)
        {
            if (currentState)
                return new Rectangle(currentMS.X, currentMS.Y, 1, 1);
            else
                return new Rectangle(prevMS.X, prevMS.Y, 1, 1);
        }

        public static bool IsPressed(Keys key)
        {
            return currentKS.IsKeyDown(key);
        }

        public static bool HasBeenPressed(Keys key)
        {
            return currentKS.IsKeyDown(key) && !prevKS.IsKeyDown(key);
        }

        public static bool MouseIsPressed(ButtonState buttonState)
        {
            if (buttonState == ButtonState.Pressed)
            {
                return true;
            }
            return false;
        }

        public static bool MouseHasBeenPressed(ButtonState buttonState, ButtonState previousButtonState)
        {
            if (buttonState == ButtonState.Pressed && previousButtonState == ButtonState.Released)
            {
                return true;
            }
            return false;
        }
    }
}

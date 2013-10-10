using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SolarFusion.Input
{
    public class InputHelper
    {
        Dictionary<Keys, bool> mKeyboard = new Dictionary<Keys, bool>();
        Dictionary<Buttons, bool> mGamepad = new Dictionary<Buttons, bool>();

        static public Dictionary<PlayerIndex?, GamePadState> CurrentGamePadState = new Dictionary<PlayerIndex?, GamePadState>();
        static public Dictionary<PlayerIndex?, GamePadState> PreviousGamePadState = new Dictionary<PlayerIndex?, GamePadState>();
        static public KeyboardState CurrentKeyboardState;
        static public KeyboardState PreviousKeyboardState;

        public InputHelper()
        {
            if (CurrentGamePadState.Count == 0)
            {
                CurrentGamePadState.Add(PlayerIndex.One, GamePad.GetState(PlayerIndex.One));
                CurrentGamePadState.Add(PlayerIndex.Two, GamePad.GetState(PlayerIndex.Two));
                CurrentGamePadState.Add(PlayerIndex.Three, GamePad.GetState(PlayerIndex.Three));
                CurrentGamePadState.Add(PlayerIndex.Four, GamePad.GetState(PlayerIndex.Four));

                PreviousGamePadState.Add(PlayerIndex.One, GamePad.GetState(PlayerIndex.One));
                PreviousGamePadState.Add(PlayerIndex.Two, GamePad.GetState(PlayerIndex.Two));
                PreviousGamePadState.Add(PlayerIndex.Three, GamePad.GetState(PlayerIndex.Three));
                PreviousGamePadState.Add(PlayerIndex.Four, GamePad.GetState(PlayerIndex.Four));
            }
        }

        static public void startUpdate()
        {
            CurrentGamePadState[PlayerIndex.One] = GamePad.GetState(PlayerIndex.One);
            CurrentGamePadState[PlayerIndex.Two] = GamePad.GetState(PlayerIndex.Two);
            CurrentGamePadState[PlayerIndex.Three] = GamePad.GetState(PlayerIndex.Three);
            CurrentGamePadState[PlayerIndex.Four] = GamePad.GetState(PlayerIndex.Four);

            CurrentKeyboardState = Keyboard.GetState(PlayerIndex.One);
        }

        static public void endUpdate()
        {
            PreviousGamePadState[PlayerIndex.One] = CurrentGamePadState[PlayerIndex.One];
            PreviousGamePadState[PlayerIndex.Two] = CurrentGamePadState[PlayerIndex.Two];
            PreviousGamePadState[PlayerIndex.Three] = CurrentGamePadState[PlayerIndex.Three];
            PreviousGamePadState[PlayerIndex.Four] = CurrentGamePadState[PlayerIndex.Four];

            PreviousKeyboardState = CurrentKeyboardState;
        }

        public void AddKeyboardInput(Keys keyPressed, bool isReleased)
        {
            if (mKeyboard.ContainsKey(keyPressed))
            {
                return;
            }
            mKeyboard.Add(keyPressed, isReleased);
        }

        public void AddGamepadInput(Buttons buttonPressed, bool isReleased)
        {
            if (mGamepad.ContainsKey(buttonPressed))
            {
                return;
            }
            mGamepad.Add(buttonPressed, isReleased);
        }

        public bool IsPressed(PlayerIndex? playerIndex)
        {
            foreach (Keys aKey in mKeyboard.Keys)
            {
                if (mKeyboard[aKey] == true)
                {
                    if (CurrentKeyboardState.IsKeyDown(aKey) == true && PreviousKeyboardState.IsKeyDown(aKey) == false)
                    {
                        return true;
                    }
                }
                else
                {
                    if (CurrentKeyboardState.IsKeyDown(aKey))
                    {
                        return true;
                    }
                }
            }

            foreach (Buttons aButton in mGamepad.Keys)
            {
                if (mGamepad[aButton] == true)
                {
                    if (CurrentGamePadState[playerIndex].IsButtonDown(aButton) == true && PreviousGamePadState[playerIndex].IsButtonDown(aButton) == false)
                    {
                        return true;
                    }
                }
                else
                {
                    if (CurrentGamePadState[playerIndex].IsButtonDown(aButton))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}

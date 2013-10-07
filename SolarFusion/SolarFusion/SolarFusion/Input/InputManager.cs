using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SolarFusion.Input
{
    public class InputManager
    {
        Dictionary<string, InputHelper> mInputs = new Dictionary<string, InputHelper>();

        public InputManager()
        {

        }

        public InputHelper NewInput(string action)
        {
            if (mInputs.ContainsKey(action) == false)
            {
                mInputs.Add(action, new InputHelper());
            }

            return mInputs[action];
        }

        public void startUpdate()
        {
            InputHelper.startUpdate();
        }

        public void endUpdate()
        {
            InputHelper.endUpdate();
        }

        public void resetAllInput()
        {
            mInputs.Clear();
        }

        public bool IsPressed(string action, PlayerIndex player)
        {
            if (mInputs.ContainsKey(action) == false)
            {
                return false;
            }

            return mInputs[action].IsPressed(player);
        }

        public void AddGamePadInput(string action, Buttons buttonPressed, bool isReleased)
        {
            NewInput(action).AddGamepadInput(buttonPressed, isReleased);
        }

        public void AddKeyboardInput(string action, Keys keyPressed, bool isReleased)
        {
            NewInput(action).AddKeyboardInput(keyPressed, isReleased);
        }
    }
}

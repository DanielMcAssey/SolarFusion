using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using System.Diagnostics;

using SolarFusion.Core;

namespace SolarFusion.Input
{
    public class InputManager
    {
        Dictionary<string, InputHelper> mInputs = new Dictionary<string, InputHelper>();

        public InputManager()
        {
            //Add GamePad Input
            this.AddGamePadInput("NAV_UP", SysConfig.INPUT_GAMEPAD_UP_DPAD, true);
            this.AddGamePadInput("NAV_UP", SysConfig.INPUT_GAMEPAD_UP_STICK, true);
            this.AddGamePadInput("NAV_DOWN", SysConfig.INPUT_GAMEPAD_DOWN_DPAD, true);
            this.AddGamePadInput("NAV_DOWN", SysConfig.INPUT_GAMEPAD_DOWN_STICK, true);
            this.AddGamePadInput("NAV_LEFT", SysConfig.INPUT_GAMEPAD_LEFT_DPAD, true);
            this.AddGamePadInput("NAV_LEFT", SysConfig.INPUT_GAMEPAD_LEFT_STICK, true);
            this.AddGamePadInput("NAV_RIGHT", SysConfig.INPUT_GAMEPAD_RIGHT_DPAD, true);
            this.AddGamePadInput("NAV_RIGHT", SysConfig.INPUT_GAMEPAD_RIGHT_STICK, true);

            this.AddGamePadInput("NAV_SELECT", SysConfig.INPUT_GAMEPAD_SELECT, true);
            this.AddGamePadInput("NAV_CANCEL", SysConfig.INPUT_GAMEPAD_CANCEL, true);

            this.AddGamePadInput("GLOBAL_START", SysConfig.INPUT_GAMEPAD_START, true);
            this.AddGamePadInput("GLOBAL_DEBUG", SysConfig.INPUT_GAMEPAD_DEBUG, true);
            this.AddGamePadInput("GAME_PAUSE", SysConfig.INPUT_GAMEPAD_START, true);

            this.AddGamePadInput("PLAY_MOVE_LEFT", SysConfig.INPUT_GAMEPAD_LEFT_DPAD, false);
            this.AddGamePadInput("PLAY_MOVE_LEFT", SysConfig.INPUT_GAMEPAD_LEFT_STICK, false);
            this.AddGamePadInput("PLAY_MOVE_RIGHT", SysConfig.INPUT_GAMEPAD_RIGHT_DPAD, false);
            this.AddGamePadInput("PLAY_MOVE_RIGHT", SysConfig.INPUT_GAMEPAD_RIGHT_STICK, false);
            this.AddGamePadInput("PLAY_MOVE_JUMP", SysConfig.INPUT_GAMEPAD_JUMP, true);
            this.AddGamePadInput("PLAY_WEAPON_FIRE", SysConfig.INPUT_GAMEPAD_FIRE, true);

            //Add Keyboard Input
            this.AddKeyboardInput("NAV_UP", SysConfig.INPUT_KEYBOARD_UP, true);
            this.AddKeyboardInput("NAV_DOWN", SysConfig.INPUT_KEYBOARD_DOWN, true);
            this.AddKeyboardInput("NAV_LEFT", SysConfig.INPUT_KEYBOARD_LEFT, true);
            this.AddKeyboardInput("NAV_RIGHT", SysConfig.INPUT_KEYBOARD_RIGHT, true);

            this.AddKeyboardInput("NAV_SELECT", SysConfig.INPUT_KEYBOARD_SELECT, true);
            this.AddKeyboardInput("NAV_CANCEL", SysConfig.INPUT_KEYBOARD_CANCEL, true);

            this.AddKeyboardInput("GLOBAL_START", SysConfig.INPUT_KEYBOARD_START, true);
            this.AddKeyboardInput("GLOBAL_DEBUG", SysConfig.INPUT_KEYBOARD_DEBUG, true);
            this.AddKeyboardInput("GAME_PAUSE", SysConfig.INPUT_KEYBOARD_CANCEL, true);

            this.AddKeyboardInput("PLAY_MOVE_LEFT", SysConfig.INPUT_KEYBOARD_LEFT, false);
            this.AddKeyboardInput("PLAY_MOVE_RIGHT", SysConfig.INPUT_KEYBOARD_RIGHT, false);
            this.AddKeyboardInput("PLAY_MOVE_JUMP", SysConfig.INPUT_KEYBOARD_JUMP, true);
            this.AddKeyboardInput("PLAY_WEAPON_FIRE", SysConfig.INPUT_KEYBOARD_FIRE, true);
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

        public bool IsPressed(string action, PlayerIndex? player)
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace SolarFusion.Core.Config
{
    public enum ControlType
    {
        _KEYBOARD,
        _GAMEPAD
    }

    public class SystemSettings
    {
        //Video Settings
        public int VIDEO_RES_WIDTH = 1024;
        public int VIDEO_RES_HEIGHT = 768;
        public bool VIDEO_FULLSCREEN = false;
        public float VIDEO_BRIGHTNESS = 0.5f;

        //Audio Settings
        public float AUDIO_MAIN_VOLUME = 1.0f;
        public float AUDIO_MUSIC_VOLUME = 1.0f;
        public float AUDIO_SFX_VOLUME = 1.0f;

        //Current Control Scheme
        public int CURRENT_CONTROL_TYPE = (int)ControlType._KEYBOARD;
        public int CURRENT_ACTION_MENU = (int)Keys.Escape;
        public int CURRENT_ACTION_JUMP = (int)Keys.Space;

        //Default GamePad Scheme
        public int GAMEPAD_CONTROL_TYPE = (int)ControlType._GAMEPAD;
        public int GAMEPAD_ACTION_MENU = (int)Buttons.Start;
        public int GAMEPAD_ACTION_JUMP = (int)Buttons.A;

        //Default Keyboard Scheme
        public int KEYBOARD_CONTROL_TYPE = (int)ControlType._KEYBOARD;
        public int KEYBOARD_ACTION_MENU = (int)Keys.Escape;
        public int KEYBOARD_ACTION_JUMP = (int)Keys.Space;
    }
}

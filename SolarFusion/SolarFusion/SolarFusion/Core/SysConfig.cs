using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.IsolatedStorage;
using Microsoft.Xna.Framework.Input;

namespace SolarFusion.Core
{
    public class SysConfig
    {
        //General Configuration
        public const string CONFIG_GAME_BUILD = "1.0-beta-branch";
        public const string CONFIG_GAME_BUILD_TYPE = "BETA";
        public const string CONFIG_GAME_NAME_CLEAN = "Jumpista";
        public const string CONFIG_GAME_NAME = CONFIG_GAME_NAME_CLEAN + " - " + CONFIG_GAME_BUILD_TYPE + " Build (" + CONFIG_GAME_BUILD + ")";

        //Debug Settings
        public const string CONFIG_DEBUG_TEAMCITY_BUILD_CONFIG = "xna_4-[Win32|X360]";
        public const string CONFIG_DEBUG_TEAMCITY_BUILD_AGENT_SERVER = "glokon-s03";
        public const string CONFIG_DEBUG_TEAMCITY_KEY = "HwF0r58n8VmVPWoR8G3c9S9qWWB117NL";
        public const string CONFIG_DEBUG_JIRA = "jira.blackholedev.net";
        public const string CONFIG_DEBUG_JIRA_METHOD = "POST";
        public const string CONFIG_DEBUG_JIRA_KEY = "pAG7zeZ16AY1Ht3924QsC1rup8T3x8pC";

        //Directory Mappings
        public const string CONFIG_CONTENT_ROOT = "Content";
        public const string CONFIG_DATA_DIR_SHADERS = "Core/Shaders/";
        public const string CONFIG_DATA_DIR_GUI = "System/UI/";
        public const string CONFIG_DATA_DIR_SETTINGS = "System/Config/";
        public const string CONFIG_DATA_DIR_GUI_FONTS = CONFIG_DATA_DIR_GUI + "Fonts/";
        public const string CONFIG_DATA_DIR_GUI_BG = CONFIG_DATA_DIR_GUI + "Backgrounds/";

        //Default Assets
        public const string ASSET_CONFIG_GUI_FONT = CONFIG_DATA_DIR_GUI_FONTS + "font_GUI";
        public const string ASSET_CONFIG_HUD_FONT = CONFIG_DATA_DIR_GUI_FONTS + "font_HUD";
        public const string ASSET_CONFIG_DEBUG_FONT = CONFIG_DATA_DIR_GUI_FONTS + "font_DEBUG";
        public const string ASSET_CONFIG_BLANK_BG = CONFIG_DATA_DIR_GUI_BG + "texture_BLANK";
        public const string ASSET_CONFIG_BLACK_BG = CONFIG_DATA_DIR_GUI_BG + "texture_BLACK";
        public const string ASSET_CONFIG_DEFAULT_BG = CONFIG_DATA_DIR_GUI_BG + "bg";
        public const string ASSET_CONFIG_MSGBOX_BG = CONFIG_DATA_DIR_GUI_BG + "msgbox";

        //Settings
        public const string ASSET_CONFIG_SETTINGS_FILE = "base.config";

        //Default Strings
        public const string ASSET_CONFIG_MSG_LOADING = "LOADING";
        public const string ASSET_CONFIG_MSG_BOX_USAGE = "\n'OK' - Yes\n'CANCEL' - No";

        //Default GamePad Controls
        public const Buttons INPUT_GAMEPAD_UP_DPAD = Buttons.DPadUp;
        public const Buttons INPUT_GAMEPAD_UP_STICK = Buttons.LeftThumbstickUp;
        public const Buttons INPUT_GAMEPAD_DOWN_DPAD = Buttons.DPadDown;
        public const Buttons INPUT_GAMEPAD_DOWN_STICK = Buttons.LeftThumbstickDown;
        public const Buttons INPUT_GAMEPAD_LEFT_DPAD = Buttons.DPadLeft;
        public const Buttons INPUT_GAMEPAD_LEFT_STICK = Buttons.LeftThumbstickLeft;
        public const Buttons INPUT_GAMEPAD_RIGHT_DPAD = Buttons.DPadRight;
        public const Buttons INPUT_GAMEPAD_RIGHT_STICK = Buttons.LeftThumbstickRight;

        public const Buttons INPUT_GAMEPAD_START = Buttons.Start;
        public const Buttons INPUT_GAMEPAD_SELECT = Buttons.A;
        public const Buttons INPUT_GAMEPAD_CANCEL = Buttons.B;

        public const Buttons INPUT_GAMEPAD_JUMP = Buttons.A;
        public const Buttons INPUT_GAMEPAD_FIRE = Buttons.RightShoulder;
        public const Buttons INPUT_GAMEPAD_DEBUG = Buttons.RightStick;

        //Default Keyboard Controls
        public const Keys INPUT_KEYBOARD_UP = Keys.Up;
        public const Keys INPUT_KEYBOARD_DOWN = Keys.Down;
        public const Keys INPUT_KEYBOARD_LEFT = Keys.Left;
        public const Keys INPUT_KEYBOARD_RIGHT = Keys.Right;

        public const Keys INPUT_KEYBOARD_START = Keys.Enter;
        public const Keys INPUT_KEYBOARD_SELECT = Keys.Enter;
        public const Keys INPUT_KEYBOARD_CANCEL = Keys.Escape;

        public const Keys INPUT_KEYBOARD_JUMP = Keys.Space;
        public const Keys INPUT_KEYBOARD_FIRE = Keys.X;
        public const Keys INPUT_KEYBOARD_DEBUG = Keys.F2;
    }
}

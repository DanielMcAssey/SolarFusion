using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.IsolatedStorage;

namespace SolarFusion.Core
{
    public class SysConfig
    {
        //General Configuration
        public const string CONFIG_GAME_BUILD = "66843e9";
        public const string CONFIG_GAME_BUILD_TYPE = "Development";
        public const string CONFIG_GAME_NAME = "SolarFusion - Development Build (" + CONFIG_GAME_BUILD + ")";
        public const string CONFIG_GAME_NAME_CLEAN = "SolarFusion";

        //Debug Settings
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

        //Settings
        public const string ASSET_CONFIG_SETTINGS_FILE = "base.config";

        //Default Strings
        public const string ASSET_ENGINE_STRING_MSG_BOX_USAGE = "\n Pad A button, Space, Enter = ok \nB button, Esc = cancel";
        public const string ASSET_ENGINE_STRING_LOADING = "Loading...";
    }
}

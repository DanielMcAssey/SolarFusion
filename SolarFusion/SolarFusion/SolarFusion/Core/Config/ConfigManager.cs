using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using SolarFusion.Core;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;

using System.Diagnostics;

namespace SolarFusion.Core.Config
{
    public class ConfigManager
    {
        public bool _LOADED = false;
        private SystemSettings _obj_settings;
#if WINDOWS
        private string WIN32_data_system_dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), SysConfig.CONFIG_GAME_NAME_CLEAN);
#elif XBOX
        private IsolatedStorageFile X360_data_file = IsolatedStorageFile.GetUserStoreForApplication();
#endif

        public ConfigManager()
        {
            _obj_settings = new SystemSettings();
#if WINDOWS
            WIN32_ReadFile();
#elif XBOX
            X360_ReadFile();
#endif
        }

        public SystemSettings Settings
        {
            get { return this._obj_settings; }
        }

#if WINDOWS
        private void WIN32_ReadFile()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SystemSettings));
            Stream _stream = null;

            try
            {
                if (!Directory.Exists(WIN32_data_system_dir))
                {
                    Directory.CreateDirectory(WIN32_data_system_dir);
                    WIN32_CreateNewFile();
                }
                else
                {
                    if (File.Exists(WIN32_data_system_dir + "/" + SysConfig.ASSET_CONFIG_SETTINGS_FILE))
                    {
                        _stream = File.Open(WIN32_data_system_dir + "/" + SysConfig.ASSET_CONFIG_SETTINGS_FILE, FileMode.Open);
                        serializer = new XmlSerializer(typeof(SystemSettings));
                        _obj_settings = (SystemSettings)serializer.Deserialize(_stream);
                        _stream.Close();
                    }
                    else
                    {
                        WIN32_CreateNewFile();
                    }
                }
            }
            catch (InvalidOperationException ex01)
            {
                if (_stream != null)
                    _stream.Close();

                WIN32_CreateNewFile();
            }
            catch (Exception ex)
            {
                this._LOADED = false;
                return;
            }
        }

        private void WIN32_CreateNewFile()
        {
            try
            {
                if (File.Exists(WIN32_data_system_dir + "/" + SysConfig.ASSET_CONFIG_SETTINGS_FILE))
                {
                    File.Delete(WIN32_data_system_dir + "/" + SysConfig.ASSET_CONFIG_SETTINGS_FILE);
                }

                FileStream _stream = File.Create(WIN32_data_system_dir + "/" + SysConfig.ASSET_CONFIG_SETTINGS_FILE);
                XmlSerializer serializer = new XmlSerializer(typeof(SystemSettings));

                serializer.Serialize(_stream, _obj_settings);
                _stream.Close();

                this._LOADED = true;
            }
            catch (Exception ex)
            {
                this._LOADED = false;
                return;
            }
        }
#endif

#if XBOX
        private void X360_ReadFile()
        {

        }

        private void X360_CreateNewFile()
        {
            using (X360_data_file)
            {

            }
        }
#endif
    }
}

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

namespace SolarFusion.Core
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
                        WIN32_CreateNewFile(); // Creates a new file.
                    }
                }
            }
            catch (InvalidOperationException ex01) // Error reading and deserializing the file, so creates a new one.
            {
                if (_stream != null)
                    _stream.Close(); // Closes the stream if it exists.

                WIN32_CreateNewFile(); // Creates a new file.
            }
            catch (Exception ex) // Catches any exception.
            {
                // Toggles the _LOADED variable.
                this._LOADED = false;
                return;
            }
        }

        public void WIN32_CreateNewFile()
        {
            try
            {
                // Since this function create's a new file, it checks if it already exists, and delete's it.
                if (File.Exists(WIN32_data_system_dir + "/" + SysConfig.ASSET_CONFIG_SETTINGS_FILE))
                {
                    File.Delete(WIN32_data_system_dir + "/" + SysConfig.ASSET_CONFIG_SETTINGS_FILE);
                }

                // Opens a file stream, when the file is created.
                FileStream _stream = File.Create(WIN32_data_system_dir + "/" + SysConfig.ASSET_CONFIG_SETTINGS_FILE);

                // Creates a new XML serializer object.
                XmlSerializer serializer = new XmlSerializer(typeof(SystemSettings));

                // Serializes the class to the file and closes the stream.
                serializer.Serialize(_stream, _obj_settings);
                _stream.Close();

                // Toggles the _LOADED variable.
                this._LOADED = true;
            }
            catch (Exception ex) // Catches any type of exception.
            {
                // Toggles the _LOADED variable.
                this._LOADED = false;
                return;
            }
        }
#endif

#if XBOX
        private void X360_ReadFile()
        {
            using (X360_data_file)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SystemSettings));
                IsolatedStorageFileStream _stream = null;

                try
                {
                    if (X360_data_file.FileExists(SysConfig.ASSET_CONFIG_SETTINGS_FILE))
                    {
                        _stream = X360_data_file.OpenFile(SysConfig.ASSET_CONFIG_SETTINGS_FILE, FileMode.Open);
                        serializer = new XmlSerializer(typeof(SystemSettings));
                        _obj_settings = (SystemSettings)serializer.Deserialize(_stream);
                        _stream.Close();
                    }
                    else
                    {
                        X360_CreateNewFile(); // Creates a new file.
                    }
                }
                catch (InvalidOperationException ex01) // Error reading and deserializing the file, so creates a new one.
                {
                    if (_stream != null)
                        _stream.Close(); // Closes the stream if it exists.

                    X360_CreateNewFile(); // Creates a new file.
                }
                catch (Exception ex) // Catches any exception.
                {
                    // Toggles the _LOADED variable.
                    this._LOADED = false;
                    return;
                }
            }
        }

        public void X360_CreateNewFile()
        {
            using (X360_data_file)
            {
                try
                {
                    // Since this function create's a new file, it checks if it already exists, and delete's it.
                    if (X360_data_file.FileExists(SysConfig.ASSET_CONFIG_SETTINGS_FILE))
                    {
                        X360_data_file.DeleteFile(SysConfig.ASSET_CONFIG_SETTINGS_FILE);
                    }

                    // Opens a file stream, when the file is created.
                    IsolatedStorageFileStream _stream = X360_data_file.CreateFile(SysConfig.ASSET_CONFIG_SETTINGS_FILE);

                    // Creates a new XML serializer object.
                    XmlSerializer serializer = new XmlSerializer(typeof(SystemSettings));

                    // Serializes the class to the file and closes the stream.
                    serializer.Serialize(_stream, _obj_settings);
                    _stream.Close();

                    // Toggles the _LOADED variable.
                    this._LOADED = true;
                }
                catch (Exception ex) // Catches any type of exception.
                {
                    // Toggles the _LOADED variable.
                    this._LOADED = false;
                    return;
                }
            }
        }
#endif
    }
}

/***********************************************
 *
 *   PangServ - Pangya Server Emulator       
 *   Copyright © 2016 - 2017 PangServ-Team
 *
 ***********************************************
 * Common configuration files handling.
 ***********************************************/
using System;
using System.IO;
using System.Collections.Generic;
using showmsg;
using inilib;

namespace configs
{
    class config
    {
        private string _file;
        private IniHandle ini;

        // Holding the current config values.
        public Dictionary<string,object> values;

        public enum e_c_type
        {
            C_TYPE_COMMON,
        }

        public config(string filename, e_c_type c_type = e_c_type.C_TYPE_COMMON)
        {
            _file = filename;
            filename = string.Format("{0}conf\\{1}", AppDomain.CurrentDomain.BaseDirectory, filename);

            try
            {
                using (StreamReader stream = new StreamReader(filename))
                {
                    ini = new IniHandle(_file, stream.ReadToEnd());
                    values = new Dictionary<string, object>();
                }
            }
            catch (Exception e)
            {
                console.error("The file could not be read:");
                console.error(e.Message);
                return;
            }

            switch (c_type)
            {
                case e_c_type.C_TYPE_COMMON:
                    _ReadCommonConfig();
                    return;
                default:
                    return;
            }
        }

        private void _ReadCommonConfig()
        {
            int[] t_pos = null;
            // read settings.
            // Login Server configs
            t_pos = ini.GroupPos("login");
            values.Add("login.port", ini.ReadIniField(t_pos, "port", "8888", 0, 65535));
            values.Add("login.host", ini.ReadIniField(t_pos, "host", "127.0.0.1"));
            values.Add("login.name", ini.ReadIniField(t_pos, "name", "Login Server"));
            // Game Server configs
            t_pos = ini.GroupPos("game");
            values.Add("game.port", ini.ReadIniField(t_pos, "port", "7997", 0, 65535));
            values.Add("game.host", ini.ReadIniField(t_pos, "host", "127.0.0.1"));
            values.Add("game.name", ini.ReadIniField(t_pos, "name", "Pangya Server"));
            values.Add("game.icon", ini.ReadIniField(t_pos, "icon", "1")); // Todo: min / max values?
            // Sync Server configs
            t_pos = ini.GroupPos("sync");
            values.Add("sync.port", ini.ReadIniField(t_pos, "port", "7998", 0, 65535));
            values.Add("sync.host", ini.ReadIniField(t_pos, "host", "127.0.0.1"));

            console.status("Common configuration file '"+_file+"' read.");
        }
    }
}
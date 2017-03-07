/***********************************************
 *
 *   PangServ - Pangya Server Emulator       
 *   Copyright © 2016 - 2017 PangServ-Team
 *
 ***********************************************
 * A simple but complete ini-read-library.
 * Copyright © 15peaces 2012 - 2017
 ***********************************************/
using System;
using showmsg;
namespace inilib
{
    class IniHandle
    {
        private string fn;
        private string[] lines;

        // Constructor
        public IniHandle(string file, string content)
        {
            fn = file;
            lines = content.Split(new[] { "\n", "\r\n" }, StringSplitOptions.None);
        }

        // Get group-range
        public int[] GroupPos(string group = "")
        {
            if (group == "")
            {
                return new[] { 0, 0 };// No Group.
            }

            string lowerline;
            int[] ret = new[] { -1, -1 };
            for (int i = 0; i < lines.Length; i++)
            {
                console.debug(lines[i]);
                lowerline = lines[i].ToLower();

                if (ret[0] < 0)
                {
                    if (lowerline.Contains("[" + group.ToLower() + "]"))
                    {
                        ret[0] = i; // Group found.
                    }

                }
                else
                {
                    if (lowerline.StartsWith("[") || i == lines.Length-1) // next group or end of file.
                    {
                        ret[1] = --i; // End of group found.
                        return(ret);
                    }
                }
            }
            console.error("Unable to find Group '"+group+"' in configuration file '"+fn+"'.");
            return ret; // Group not found.
        }

        public string ReadIniField(int[] group_index, string key, string _default = "", int min = int.MinValue, int max = int.MaxValue)
        {
            if (group_index[0] < 0 || group_index[1] > lines.Length)
            {
                console.error("Invalid group index (start: '" + group_index[0].ToString() + "' ; end: '" + group_index[1].ToString() + "') in configuration file '" + fn + "'. Defaulting values...");
                return _default;
            }

            string[] tarr = null;
            for (int i = group_index[0]; i < group_index[1]; i++)
            {
                if (lines[i].StartsWith(key))
                {
                    tarr = lines[i].Split(new[] { "=" }, StringSplitOptions.None);
                    break;
                }
            }

            string ret = "";
            if (tarr == null)
            {
                ret = _default;
            }
            else
            {
                ret = tarr[1];
            }

            // Assuming integer value and checking min / max values.
            if (min != int.MinValue || max != int.MaxValue)
            {
                int iret = Convert.ToInt32(ret);
                if (iret < min || iret > max)
                {
                    console.warning("Invalid value '" + iret.ToString() + "' (Min: " + min.ToString() + " Max: " + max.ToString() + ") for '" + key + "' in configuration file '" + fn + "'. Defaulting value...");
                    ret = _default;
                }
            }

            return ret;
        }
    }
}
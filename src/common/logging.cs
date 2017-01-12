/***********************************************
 *
 *   PangServ - Pangya Server Emulator       
 *   Copyright © 2016 PangServ-Team
 *
 ***********************************************
 * Logging.
 ***********************************************/
using System;

namespace Logging
{
    class cLogging
    {
        private string _file;

        public enum e_LogType
        {
            LMSG_NORMAL,
            LMSG_STATUS,
            LMSG_SQL,
            LMSG_INFORMATION,
            LMSG_NOTICE,
            LMSG_WARNING,
            LMSG_DEBUG,
            LMSG_ERROR,
            LMSG_FATALERROR
        }

        public cLogging(string filename)
        {
            _file = string.Format("{0}log\\{1}", AppDomain.CurrentDomain.BaseDirectory, filename);
        }

        public void write(string msg, e_LogType logType = e_LogType.LMSG_NORMAL)
        {
            using(System.IO.StreamWriter w = System.IO.File.AppendText(_file))
            {
                w.WriteLine("{0} {1} [{2}] : {3}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString(), logType.ToString(), msg);
            }
        }
    }
}
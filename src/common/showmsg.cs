/***********************************************
 *
 *   PangServ - Pangya Server Emulator       
 *   Copyright © 2016 PangServ-Team
 *
 ***********************************************
 * Complete Message-Handling using win-api.
 * Copyright © 15peaces 2012 - 2017
 ***********************************************/
using System; // Console

namespace showmsg
{
    public class console
    {
        // Specifies how silent the console is.
        static short msg_silent = 0;

        // enum with console color codes (FG and BG colors are the same)
        enum e_color
        {
            CL_BLACK,
            CL_BLUE,
            CL_GREEN,
            CL_CYAN,
            CL_RED,
            CL_MAGENTA,
            CL_YELLOW,
            CL_GREY = 7,
            CL_WHITE = 15
        }

        enum e_msg_type
        {
            MSG_NONE,
            MSG_STATUS,
            MSG_SQL,
            MSG_INFORMATION,
            MSG_NOTICE,
            MSG_WARNING,
            MSG_DEBUG,
            MSG_ERROR,
            MSG_FATALERROR
        }

        static bool _vShowMessage(e_msg_type flag, string message = "")
        {
            if (message == "")
            {
                console.error("Empty string passed to _vShowMessage()");
                return false;
            }

            if ((flag == e_msg_type.MSG_INFORMATION && msg_silent == 1)
                || (flag == e_msg_type.MSG_STATUS && msg_silent == 2)
                || (flag == e_msg_type.MSG_NOTICE && msg_silent == 4)
                || (flag == e_msg_type.MSG_WARNING && msg_silent == 8)
                || (flag == e_msg_type.MSG_ERROR && msg_silent == 16)
                || (flag == e_msg_type.MSG_SQL && msg_silent == 16)
                || (flag == e_msg_type.MSG_DEBUG && msg_silent == 32))
                return false; // Do Not Print it.

            string prefix = "";
            e_color color = e_color.CL_GREY;

            switch (flag)
            {
                case e_msg_type.MSG_NONE: // direct WriteLine replacement.
                    break;
                case e_msg_type.MSG_STATUS: // Bright Green (To inform about good things)
                    prefix = "[Status]";
                    color = e_color.CL_GREEN;
                    break;
                case e_msg_type.MSG_SQL: // Bright Violet (For dumping out anything related with SQL)
                    prefix = "[SQL]";
                    color = e_color.CL_MAGENTA;
                    break;
                case e_msg_type.MSG_INFORMATION: // Bright White (Variable information)
                    prefix = "[Info]";
                    color = e_color.CL_WHITE;
                    break;
                case e_msg_type.MSG_NOTICE: // Bright White (Less than a warning)
                    prefix = "[Notice]";
                    color = e_color.CL_WHITE;
                    break;
                case e_msg_type.MSG_WARNING: // Bright Yellow
                    prefix = "[Warning]";
                    color = e_color.CL_YELLOW;
                    break;
                case e_msg_type.MSG_DEBUG: // Bright Cyan, important stuff!
                    prefix = "[Debug]";
                    color = e_color.CL_CYAN;
                    break;
                case e_msg_type.MSG_ERROR: // Bright Red  (Regular errors)
                    prefix = "[Error]";
                    color = e_color.CL_RED;
                    break;
                case e_msg_type.MSG_FATALERROR: // Bright Red (Fatal errors, abort(); If possible)
                    prefix = "[Fatal Error]";
                    color = e_color.CL_RED;
                    break;
                default:
                    error(String.Format("In function _vShowMessage() -> Invalid flag ({0}) passed.", flag));
                    return false;
            }

            Console.ForegroundColor = (ConsoleColor)e_color.CL_GREY;
            char[] letters = prefix.ToCharArray();
            foreach (char c in letters)
            {
                Console.ForegroundColor = (ConsoleColor)color;
                Console.Write(c);
            }
            Console.ForegroundColor = (ConsoleColor)e_color.CL_GREY;
            if (prefix != "")
                Console.Write(" ");

            letters = message.ToCharArray();
            foreach (char c in letters)
                Console.Write(c);

            Console.WriteLine();
 
            return true;
        }
 
        public static bool unformated(byte type, string message)
        {
            return _vShowMessage((e_msg_type)type, message);
        }

        public static bool message(string message)
        {
            return _vShowMessage(e_msg_type.MSG_NONE, message);
        }

        public static bool status(string message)
        {
            return _vShowMessage(e_msg_type.MSG_STATUS, message);
        }

        public static bool sql(string message)
        {
            return _vShowMessage(e_msg_type.MSG_SQL, message);
        }

        public static bool information(string message)
        {
            return _vShowMessage(e_msg_type.MSG_INFORMATION, message);
        }

        public static bool notice(string message)
        {
            return _vShowMessage(e_msg_type.MSG_NOTICE, message);
        }

        public static bool warning(string message)
        {
            return _vShowMessage(e_msg_type.MSG_WARNING, message);
        }

        public static bool debug(string message)
        {
            return _vShowMessage(e_msg_type.MSG_DEBUG, message);
        }

        public static bool error(string message)
        {
            return _vShowMessage(e_msg_type.MSG_ERROR, message);
        }

        public static bool fatalerror(string message)
        {
            return _vShowMessage(e_msg_type.MSG_FATALERROR, message);
        }
    }
}

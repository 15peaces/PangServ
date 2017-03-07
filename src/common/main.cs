/***********************************************
 *
 *   PangServ - Pangya Server Emulator       
 *   Copyright © 2016 - 2017 PangServ-Team
 *
 ***********************************************
 * Common code.
 ***********************************************/
using showmsg; // class console.
using Logging;
using sync;
using configs;

namespace CommonMain
{
    class CMain
    {
        static void Main()
        {
            cLogging commonlog = new cLogging("common.log");
            commonlog.write("Common Service started.");
            console.message("================================================");
            console.message("||                                            ||");
            console.message("||   PangServ - Pangya Server Emulator        ||");
            console.message("||   Copyright <c> 2016 - 2017 PangServ-Team  ||");
            console.message("||                                            ||");
            console.message("================================================");

            config com_config = new config("server.ini");
            cSyncClient Syncserver = new cSyncClient(commonlog);
            Syncserver.StartListening();
            commonlog.write("Common Service stopped.");
        }  
    }
}

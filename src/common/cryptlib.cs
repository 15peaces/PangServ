/***********************************************
 *
 *   PangServ - Pangya Server Emulator       
 *   Copyright © 2016 PangServ-Team
 *
 ***********************************************
 * Packet decryption.
 ***********************************************/
using System;

namespace CryptLib
{
    class cCryptLib
    {
        // Decrypt pangya client packets
        // uses _client_decrypt function.
        public string ClientDecrypt(string data, byte key)
        {
            int buffoutSize = 0;
            char[] buffout = null;            

            _client_decrypt(data.ToCharArray(), data.Length, ref buffout, ref buffoutSize, (char)key);

            return (buffout.ToString());
        }
        
        // This function accepts the full packet send by the client as data and returns the decrypted packet starting with the Id of the packet.
        private static bool _client_decrypt(char[] buffin, int size, ref char[] buffout, ref int buffoutSize, char key)
        {
            // buffin is the Data received by the Pangya Client.
            // That mean, when you have a packet from the client, you must give it to this function.
            // This function should only know how to decrypt a single packet

            // We should allocate and decrypt buffin into buffout here
            // For now we'll only copy it and return it...
            buffout = new char[size];
            Array.Copy(buffin, buffout, size);
            buffoutSize = size;

            return true;
        }

        // Encrypt pangya client packets
        // uses _client_encrypt function.
        public string ClientEncrypt(string data, byte key, byte packetId)
        {
            int buffoutSize = 0;
            char[] buffout = null;

            _client_encrypt(data.ToCharArray(), data.Length, ref buffout, ref buffoutSize, (char)key, (char)packetId);

            return (buffout.ToString());
        }

        // This function accepts the decrypted packet as data starting with the Id of the packet.
        private static bool _client_encrypt(char[] buffin, int size, ref char[] buffout, ref int buffoutSize, char key, char packetid)
        {
            // buffin start with the PacketId as a WORD + the data in the packet

            // We should allocate and encrypt buffin into buffout here
            // For now we'll only copy it and return it...
            buffout = new char[size];
            Array.Copy(buffin, buffout, size);
            buffoutSize = size;

            return true;
        }

        // Encrypt Pangya server packets
        // uses _server_encrypt function.
        public string ServerEncrypt(string data, byte key)
        {
            int buffoutSize = 0;
            char[] buffout = null;

            _server_encrypt(data.ToCharArray(), data.Length, ref buffout, ref buffoutSize, (char)key);

            return (buffout.ToString());
        }

        // This function accepts the decrypted packet as data starting with the Id of the packet.
        private static bool _server_encrypt(char[] buffin, int size, ref char[] buffout, ref int buffoutSize, char key)
        {
            // buffin start with the PacketId as a WORD + the data in the packet

            // We should allocate and encrypt buffin into buffout here
            // For now we'll only copy it and return it...
            buffout = new char[size];
            Array.Copy(buffin, buffout, size);
            buffoutSize = size;

            return true;
        }
    }
}

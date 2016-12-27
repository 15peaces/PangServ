/***********************************************
 *
 *   PangServ - Pangya Server Emulator       
 *   Copyright © 2016 PangServ-Team
 *
 ***********************************************
 * Client Packets and Interface.
 ***********************************************/
using System;
using System.Net.Sockets;

using buffering;
using CryptLib;
using defs;
using showmsg;

namespace client
{
    class ClientPacket:membuffer // Client Packets
    {
        public string ToStr()
        {
            long previousOffset;
            long size;
            string result;

            previousOffset = Seek(0, System.IO.SeekOrigin.Current);
            Seek(0, System.IO.SeekOrigin.Begin);
            size = GetSize();

            result = ReadStr(size);
            Seek((uint)previousOffset, System.IO.SeekOrigin.Begin);

            return result;
        }

        public string GetRemainingData()
        {
            long previousOffset;
            long size;
            string result;

            previousOffset = Seek(0, System.IO.SeekOrigin.Current);
            size = GetSize() - previousOffset;

            result = ReadStr(size);
            Seek((uint)previousOffset, System.IO.SeekOrigin.Begin);

            return result;
        }
    }

    class clif<ClientType> // CLientInterFace
    {
        // Protected
        protected byte m_key;
        protected membuffer m_buffout;
        protected Socket m_socket;
        protected cCryptLib m_cryptLib;

        // Public
        public int ID;
        public string Host;
        public PlayerUID UID;
        public ClientType Data;

        // Protected
        protected string FGetHost()
        {
            console.debug(String.Format("Remote-Host: {0}", m_socket.RemoteEndPoint.ToString()));
            return (m_socket.RemoteEndPoint.ToString());
        }

        // Public
        // Constructor
        public clif(Socket socket_, cCryptLib cryptlib)
        {
            Host = FGetHost();

            m_buffout = new membuffer();

            m_cryptLib = cryptlib;
            m_socket = socket_;
        }

        public byte GetKey()
        {
            return m_key;
        }

        void send(membuffer data, bool encrypt = true)
        {
            long oldPos, size;
            string buf;

            oldPos = data.Seek(0, System.IO.SeekOrigin.Current);
            data.Seek(0, System.IO.SeekOrigin.Begin);
            size = data.GetSize();
            buf = data.ReadStr(size);
            send(buf, encrypt);
            data.Seek((uint)oldPos, System.IO.SeekOrigin.Begin);
        }

        void send(string data)
        {
            send(data, true);
        }

        void send(string data, bool encrypt)
        {
            string encrypted;

            if (encrypt)
            {
                if(UID.login == "Sync")
                    encrypted = m_cryptLib.ClientEncrypt(data, m_key, 0);
                else
                    encrypted = m_cryptLib.ServerEncrypt(data, m_key);

                m_buffout.WriteStr(encrypted);
            }
            else
                m_buffout.WriteStr(data);
        }

        bool HasUID(PlayerUID pl_uid)
        {
            if (UID.id == 0)
                return (pl_uid.login == UID.login);

            return (pl_uid.id == UID.id);
        }

        void Disconnect()
        {
            m_socket.Close();
        }
    }
}

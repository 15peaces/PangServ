/***********************************************
 *
 *   PangServ - Pangya Server Emulator       
 *   Copyright © 2016 - 2017 PangServ-Team
 *
 ***********************************************
 * Syncroning functions.
 ***********************************************/
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using showmsg;
using buffering;
using CryptLib;
using Logging;
using Client;

namespace sync
{
    class cSyncClient
    {
        // Private
        private byte m_key;
        private bool m_haveKey;

        private System.Timers.Timer m_timer;
        private TcpClient m_client;

        private static ManualResetEvent allDone = new ManualResetEvent(false);

        private membuffer m_buffin;
        private membuffer m_buffout;
        private cCryptLib m_cryptlib;
        private cLogging m_log;

        // Public
        public uint port = 10103;
        public string host = "127.0.0.1";
        public Socket socket_;

        private void TriggerOnRead(ClientPacket packet)
        {

        }

        private void TriggerOnConnect()
        {

        }

        private void OnTimer(object sender)
        {
            int y;

            if (m_buffout.GetSize() > 0)
            {
                y = socket_.Send(m_buffout.Read((int)m_buffout.GetSize()));
                m_buffout.Delete(0, y);
            }
        }

        /* Dummy functions for now... [15peaces] */
        private void OnClientLookup()
        {
            m_log.write("cSyncClient.OnClientLookup", cLogging.e_LogType.LMSG_NOTICE);
        }

        private void OnClientConnecting()
        {
            m_log.write("cSyncClient.OnClientConnecting", cLogging.e_LogType.LMSG_NOTICE);
        }

        private void OnClientConnect()
        {
            m_log.write("cSyncClient.OnClientConnect", cLogging.e_LogType.LMSG_NOTICE);
        }

        private void OnClientDisconnect(object sender, Socket socket_)
        {
            m_log.write("cSyncClient.OnClientDisconnect", cLogging.e_LogType.LMSG_NOTICE);
        }
        /* End Dummy functions */

        public void OnClientRead(Socket socket_)
        {
            int size = 0;
            uint realPacketSize;
            byte[] rec_buf = new byte[5012];
            string buffer;
            ClientPacket clientPacket_;

            allDone.Set();
            m_log.write("cSyncClient.OnClientRead", cLogging.e_LogType.LMSG_NOTICE);

            socket_.Receive(rec_buf);
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            buffer = enc.GetString(rec_buf);

            m_buffin.WriteStr(buffer);

            if (m_buffin.GetSize() > 2)
            {
                m_buffin.Seek(2, System.IO.SeekOrigin.Begin);
                size = BitConverter.ToInt32(m_buffin.Read(2), 0);
            }
            else
                return;

            realPacketSize = (uint)size + 4;

            do
            {
                m_buffin.Seek(0, System.IO.SeekOrigin.Begin);
                buffer = m_buffin.ReadStr(realPacketSize);
                m_buffin.Delete(0, (int)realPacketSize);

                if (!m_haveKey)
                {
                    clientPacket_ = new ClientPacket(buffer);
                    HandleReadKey(clientPacket_);
                    TriggerOnConnect();
                }
                else
                {
                    buffer = m_cryptlib.ClientDecrypt(buffer, m_key);
                    clientPacket_ = new ClientPacket(buffer);
                    TriggerOnRead(clientPacket_);
                }

                // Clear data.
                clientPacket_ = null;

                if (m_buffin.GetSize() > 2)
                {
                    m_buffin.Seek(2, System.IO.SeekOrigin.Begin);
                    size = BitConverter.ToInt32(m_buffin.Read(2), 0);
                    realPacketSize = (uint)size + 4;
                }
                else
                    return;
            }
            while (m_buffin.GetSize() >= realPacketSize);
        }

        /* Dummy functions for now... [15peaces] */
        private void OnClientWrite(object sender, Socket socket_)
        {
            m_log.write("cSyncClient.OnClientWrite", cLogging.e_LogType.LMSG_NOTICE);
        }

        private void OnClientError(object sender, Socket socket_, int ErrorCode = 0)
        {
            m_log.write("cSyncClient.OnClientError", cLogging.e_LogType.LMSG_NOTICE);
        }
        /* End Dummy functions */

        private void HandleReadKey(ClientPacket packet)
        {
            packet.buf.Skip(4);
            m_key = packet.buf.ReadByte();

            if (m_key <= 0)
            {
                m_log.write("cSyncClient.HandleReadKey: Failed to get Key.", cLogging.e_LogType.LMSG_ERROR, false);
                return;
            }

            m_haveKey = true;
            return;
        }

        // Public
        // Constructor
        public cSyncClient(cLogging log)
        {
            m_haveKey = false;
            m_key = 3;
            m_client = new TcpClient();
            socket_ = m_client.Client;
            m_cryptlib = new cCryptLib();
            m_log = log;

            m_timer = new System.Timers.Timer();
            m_timer.Interval = 30;
            m_buffin = new membuffer();
            m_buffout = new membuffer();
        }

        public void StartListening()
        {
            // Data buffer for incoming data.
            byte[] bytes = new byte[5012];

            IPAddress ipAdress = IPAddress.Parse(host);
            TcpListener myList = new TcpListener(ipAdress, (int)port);

            myList.Start();
            // Bind the socket to the local endpoint and listen for incoming connections.
            m_log.write("The server is running at  " + ipAdress + " port " + port, cLogging.e_LogType.LMSG_STATUS, false);

            int counter = 0;
            while (true)
            {
                counter++;
                // Set the event to nonsignaled state.
                allDone.Reset();
                console.information("Waiting for a connection...");
                socket_ = myList.AcceptSocket();
                OnClientConnecting();
                OnClientRead(socket_);
                allDone.WaitOne();
            }

            console.message("\nPress ENTER to Exit...");
            Console.Read();
            socket_.Close();
            myList.Stop();
        }

        /* Dummy functions for now... [15peaces] */
        public void StopListening()
        {

        }
        /* End Dummy functions */

        public void Send(string data, bool encrypted = true)
        {
            m_log.write("cSyncClient.Send", cLogging.e_LogType.LMSG_STATUS);

            if (encrypted)
                m_buffout.WriteStr(m_cryptlib.ClientEncrypt(data, m_key, 0));
            else
                m_buffout.WriteStr(data);

            return;
        }
    }
}
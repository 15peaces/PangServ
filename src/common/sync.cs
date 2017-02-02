/***********************************************
 *
 *   PangServ - Pangya Server Emulator       
 *   Copyright © 2016 - 2017 PangServ-Team
 *
 ***********************************************
 * Syncroning functions.
 ***********************************************/
using System.Timers;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using buffering;
using CryptLib;
using Logging;
using Client;
using System;
using System.Text;

namespace sync
{
    class cSyncClient
    {
        private byte m_key;
        private bool m_haveKey;

        private System.Timers.Timer m_timer;
        private Socket m_clientSocket;

        private membuffer m_buffin;
        private membuffer m_buffout;
        private cCryptLib m_cryptlib;
        private cLogging m_log;
        public Socket socket_;

        private static ManualResetEvent allDone = new ManualResetEvent(false);

        public void StartListening()
        {
            // Data buffer for incoming data.
            byte[] bytes = new byte[5012];

            IPAddress ipAdress = IPAddress.Parse("127.0.0.1");
            int port = 10103;
            TcpListener myList = new TcpListener(ipAdress, port);

            myList.Start();
            // Bind the socket to the local endpoint and listen for incoming connections.
            Console.WriteLine("The server is running at  " + ipAdress + " port " + port);
  
            int counter = 0;
            while (true)
             {
                counter += 1;
                // Set the event to nonsignaled state.
                allDone.Reset();
                    Console.WriteLine("Waiting for a connection...");
                    socket_ = myList.AcceptSocket();
                    OnClientConnecting();
                    OnClientRead( socket_);
                    allDone.WaitOne();
             }



            Console.WriteLine("\nPress ENTER to Exit...");
            Console.Read();
            socket_.Close();
            myList.Stop();

        }

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
                y = m_clientSocket.Send(m_buffout.Read((int)m_buffout.GetSize()));
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
                size = System.BitConverter.ToInt32(m_buffin.Read(2), 0);
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
                    size = System.BitConverter.ToInt32(m_buffin.Read(2), 0);
                    realPacketSize = (uint)size + 4;
                }
                else
                    return;
            }
            while (m_buffin.GetSize() >= realPacketSize);
        }

        private void HandleReadKey(ClientPacket packet)
        {

        }

        public cSyncClient(cLogging log)
        {
            m_log = log;
        }
    }
}
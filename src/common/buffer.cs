/***********************************************
 *
 *   PangServ - Pangya Server Emulator       
 *   Copyright © 2016 - 2017 PangServ-Team
 *
 ***********************************************
 * Buffer
 ***********************************************/
using System;
using System.IO; // MemoryStream

namespace buffering
{
    class stringbuffer // This will be removed later, please use membuffer if possible... [15peaces]
    {
        private string m_data;

        public stringbuffer(string data = "")
        {
            m_data = data;
            return;
        }

        public void Write(string data)
        {
            m_data = m_data + data;
            return;
        }

        public string Read(uint offset, uint length)
        {
            return (m_data.Substring((int)offset, (int)length));
        }

        public uint GetLength()
        {
            return ((uint)m_data.Length);
        }

        public string GetData()
        {
            return (m_data);
        }
    }

    class membuffer
    {
        private MemoryStream m_data;
        private bool locked = false;

        public membuffer()
        {
            m_data = new MemoryStream();
            m_data.Seek(0, 0);
        }

        public void Lock()
        {
            locked = true;
        }

        public void Unlock()
        {
            locked = false;
        }

        public bool WriteByte(byte src)
        {
            if (locked || !m_data.CanWrite)
                return false;

            m_data.WriteByte(src);

            return true;
        }

        public byte ReadByte()
        {
            if (locked || !m_data.CanRead)
                return 0;

            int i = m_data.ReadByte();

            if (i <= 0)
                return 0;

            return ((byte)i);
        }

        public bool WriteUInt16(UInt16 src)
        {
            if (locked || !m_data.CanWrite)
                return false;

            byte[] i = BitConverter.GetBytes(src);
            m_data.Write(i, (int)m_data.Position, 2);

            return true;
        }

        public UInt16 ReadUInt16()
        {
            if (locked || !m_data.CanRead)
                return 0;

            byte[] data = new byte[2];
            int i = m_data.Read(data, (int)m_data.Position, 2);

            return (BitConverter.ToUInt16(data, 0));
        }

        public bool WriteUInt32(UInt32 src)
        {
            if (locked || !m_data.CanWrite)
                return false;

            byte[] i = BitConverter.GetBytes(src);
            m_data.Write(i, (int)m_data.Position, 4);

            return true;
        }

        public UInt32 ReadUInt32()
        {
            if (locked || !m_data.CanRead)
                return 0;

            byte[] data = new byte[4];
            int i = m_data.Read(data, (int)m_data.Position, 4);

            return (BitConverter.ToUInt32(data, 0));
        }

        public bool WriteInt(int src)
        {
            if (locked || !m_data.CanWrite)
                return false;

            byte[] i = BitConverter.GetBytes(src);
            m_data.Write(i, (int)m_data.Position, 8);

            return true;
        }

        public int ReadInt()
        {
            if (locked || !m_data.CanRead)
                return 0;

            byte[] data = new byte[8];
            int i = m_data.Read(data, (int)m_data.Position, 8);

            return (BitConverter.ToInt32(data, 0));
        }

        public bool WriteDouble(double src)
        {
            if (locked || !m_data.CanWrite)
                return false;

            byte[] i = BitConverter.GetBytes(src);
            m_data.Write(i, (int)m_data.Position, 4);

            return true;
        }

        public double ReadDouble()
        {
            if (locked || !m_data.CanRead)
                return 0;

            byte[] data = new byte[4];
            int i = m_data.Read(data, (int)m_data.Position, 4);

            return (BitConverter.ToDouble(data, 0));
        }

        public bool WriteStr(string src)
        {
            if (locked || !m_data.CanWrite)
                return false;

            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();

            byte[] i = enc.GetBytes(src);
            m_data.Write(i, (int)m_data.Position, src.Length);

            return true;
        }

        public string ReadStr(long count = 0)
        {
            if (locked || !m_data.CanRead)
                return "";

            long size = m_data.Length - m_data.Position;

            if (count < 1)
                count = size;

            if (size <= 0)
                return "";

            byte[] data = new byte[size];
            int i = m_data.Read(data, 0, (int)count);

            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            return (enc.GetString(data));
        }

        public bool Write(byte[] src, int count)
        {
            if (locked || !m_data.CanWrite)
                return false;

            m_data.Write(src, (int)m_data.Position, count);

            return true;
        }

        public byte[] Read(int count)
        {
            if (locked || !m_data.CanRead)
                return new byte[0];

            byte[] data = new byte[count];
            int i = m_data.Read(data, (int)m_data.Position, count);

            return (data);
        }

        public void Skip(uint count)
        {
            if (!locked)
                m_data.Seek(count, SeekOrigin.Current);

            return;
        }

        public long Seek(uint offset, SeekOrigin origin)
        {
            if (!locked && m_data.CanSeek)
                return m_data.Seek(offset, origin);

            return -1;
        }

        public long GetSize()
        {
            if (!locked)
                return m_data.Length;
            return -1;
        }

        public void Delete(int offset, int length)
        {
            if (locked || !m_data.CanRead || !m_data.CanWrite)
                return;

            if (offset > m_data.Length)
                return;

            if (offset + length > m_data.Length)
                length = (int)m_data.Length - offset;

            int rest = (int)m_data.Length - offset - length;

            byte[] buf = m_data.GetBuffer();
            byte[] buf2 = new byte[m_data.Length-length];

            Buffer.BlockCopy(buf, 0, buf2, 0, offset);

            Buffer.BlockCopy(buf, offset + length, buf2, offset, rest);
            m_data.SetLength(m_data.Length - length);

            m_data.Dispose();
            m_data = new MemoryStream();
            m_data.Write(buf2, 0, buf2.Length);

            return;
        }

        public Stream ToStream()
        {
            if (locked || !m_data.CanRead)
                return Stream.Null;

            byte[] buf = m_data.GetBuffer();

            Stream t = new MemoryStream();

            t.Write(buf, 0, buf.Length);

            return(t);
        }
    }
}

using System;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;


namespace GeissBarcodeTest
{
    // SIMENS Ethernet / Binary 통신
    // ※ PLC DB영역이 word 단위 통신이므로 byte 단위 처리시 마지막 짝수 byte가 reset 되는 현상 주의
    // ※ 1회 Read, Write 가능 Byte 수가 메뉴얼 상 Maximum 2048 Byte 이나 그 이상 가능(400 기종에서 16384 byte 까지 TEST 完)
    public class SimensEthernet
    {
        Socket readSocket;              // PLC통신 READ Socket
        Socket writeSocket;             // PLC통신 WRITE Socket

        private object m_Label = null;

        const int BUF_MAX = 8192;       // 송수신 버퍼 크기 
        const int FRAME_SIZE = 16;

        private bool m_IsConnected = false;

        public bool IsConnected
        {
            get { return m_IsConnected; }
            set 
            { 
                m_IsConnected = value;

                if (m_Label != null)
                {
                    switch (m_Label.GetType().Name)
                    {
                        case "ToolStripStatusLabel":
                            ((ToolStripStatusLabel)m_Label).BackColor = m_IsConnected == true ? System.Drawing.Color.Green : System.Drawing.Color.Red;
                            break;
                        case "Label":
                            ((Label)m_Label).BackColor = m_IsConnected == true ? System.Drawing.Color.Green : System.Drawing.Color.Red;
                            break;
                    }
                }
            }
        }

        enum OPCode : int
        {
            WRITE_REQ = 0x03,           // Write Request
            WRITE_ACK = 0x04,           // Write Acknowledgment
            READ_REQ = 0x05,            // Read Request
            READ_ACK = 0x06             // Read Acknowledgment
        }

        enum AddrArea : int
        {
            DB = 0x01,                  // Data block in main memory ≪use words/ DBNR : 1~255/ Address : 0~2047≫
            M = 0x02,                   // Flag area ≪use bytes : 0~255≫
            I = 0x03,                   // Process image of the inputs(PII) ≪use bytes : 0~127≫
            Q = 0x04,                   // Process image of the outputs(PIQ) ≪use bytes : 0~127≫
            PI = 0x05,                  // In I/O modules (with source data input modules) ≪use bytes : 0~255 (digital I/Os : 0~127 | analog I/Os : 128~255)≫
            PQ = 0x05,                  // In I/O modules (with dest data output modules) ≪use bytes : 0~255 (digital I/Os : 0~127 | analog I/Os : 128~255)≫
            C = 0x06,                   // Counter cells ≪use words : 0~255 (1 word)≫
            T = 0x07                    // Timer cells ≪use words : 0~255 (1 word)≫
        }

        enum Unit : int
        {
            BYTE = 1,
            WORD = 2
        }

        public SimensEthernet()
		{
		}

        //++++++++++++++++++++++++++++++++++++++++++++++++++++
        // READ/WRITE PORT 두 개에 동시에 Connect
        //++++++++++++++++++++++++++++++++++++++++++++++++++++
        public bool Connect2Port(string ip, int readPort, int writePort)
        {
            try
            {
                if (readSocket == null || !readSocket.Connected)
                {
                    readSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    // 송수신 무응답 처리(5초 이내 Send/Receive 이루어져야...)
                    readSocket.Blocking = true;
                    readSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 5000);
                    readSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 5000);

                    readSocket.Connect(ip, readPort);
                }

                if (writeSocket == null || !writeSocket.Connected)
                {
                    writeSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    // 송수신 무응답 처리(5초 이내 Send/Receive 이루어져야...)
                    writeSocket.Blocking = true;
                    writeSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 5000);
                    writeSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 5000);

                    writeSocket.Connect(ip, writePort);
                    
                }

                IsConnected = true;
            }
            catch (Exception ex)
            {
                //DeltaControl.Log.getInstance().Add(ex);
                IsConnected = false;
                return false;
            }
            finally
            {
                
            }

            return true;

        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++
        // READ/WRITE 두 개의 포트 연결 해제
        //++++++++++++++++++++++++++++++++++++++++++++++++++++
        public void Disconnect2Port()
        {
            try
            {
                if (readSocket != null)
                {
                    readSocket.Shutdown(SocketShutdown.Both);
                    readSocket.Close();
                }

                if (writeSocket != null)
                {
                    writeSocket.Shutdown(SocketShutdown.Both);
                    writeSocket.Close();
                }
            }
            catch
            {
            }
            finally
            {
                IsConnected = false;
            }

        }

        #region SetEventLabel
        public void SetEventLabel(object pLabel)
        {
            m_Label = pLabel;
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++
        // PLC Data Read(BYTE 기준)
        //++++++++++++++++++++++++++++++++++++++++++++++++++++
        public bool GetPlcValues(string startAddress, int count, byte[] bytes, out string errorMsg)
        {
            int sndSize;
            int rcvSize = 0;
            int bytesRec;
            byte[] sndBuf = new byte[FRAME_SIZE];
            byte[] rcvBuf = new byte[BUF_MAX + FRAME_SIZE];

            int DBNo;
            int addrNo;

            Array.Clear(bytes, 0, bytes.Length);
            errorMsg = "";

            if (!SelectAddrSimens(startAddress, out DBNo, out addrNo))
                return false;

            try
            {
                sndSize = MakeReadFrame(DBNo, addrNo, Unit.BYTE, count, sndBuf);
                readSocket.Send(sndBuf, sndSize, SocketFlags.None);

                while (true)
                {
                    bytesRec = readSocket.Receive(rcvBuf, rcvSize, rcvBuf.Length - rcvSize, SocketFlags.None);

                    if (bytesRec == 0) break;
                    rcvSize += bytesRec;
                    if (rcvSize >= ((count + 1) / 2) * 2 + 16) break;
                }
                
                if (rcvSize > 0)
                {
                    // rcvBuf[8] : Error No.
                    //if ( rcvBuf[5] == (byte)OPCode.READ_ACK && rcvBuf[8] == 0x00)
                    if (rcvSize == ((count + 1) / 2) * 2 + 16 && rcvBuf[5] == (byte)OPCode.READ_ACK && rcvBuf[8] == 0x00)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            bytes[i] = rcvBuf[i + 16];
                        }
                    }
                    else if (rcvSize > ((count + 1) / 2) * 2 + 16)
                    {
                        errorMsg = "Overflow Data (" + rcvSize + " Bytes)";
                        return false;
                    }
                    else if (rcvSize < ((count + 1) / 2) * 2 + 16)
                    {
                        errorMsg = "Incomplete Data (" + rcvSize + " Bytes)";
                        return false;
                    }
                }
                else
                {
                    errorMsg = "No Received";
                    return false;
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(e.ToString());
                errorMsg = ex.ToString();
                IsConnected = false;

                return false;
            }

            return true;

        }


        //++++++++++++++++++++++++++++++++++++++++++++++++++++
        // PLC Data Read(WORD 기준)
        //++++++++++++++++++++++++++++++++++++++++++++++++++++
        public bool GetPlcValues(string startAddress, int count, ushort[] words, out string errorMsg)
        {
            int sndSize;
            int rcvSize = 0;
            int bytesRec;
            byte[] sndBuf = new byte[FRAME_SIZE];
            byte[] rcvBuf = new byte[BUF_MAX + FRAME_SIZE];

            int DBNo;
            int addrNo;

            Array.Clear(words, 0, words.Length);
            errorMsg = "";

            if (!SelectAddrSimens(startAddress, out DBNo, out addrNo))
                return false;

            try
            {
                sndSize = MakeReadFrame(DBNo, addrNo, Unit.WORD, count, sndBuf);
                readSocket.Send(sndBuf, sndSize, SocketFlags.None);

                while (true)
                {
                    bytesRec = readSocket.Receive(rcvBuf, rcvSize, rcvBuf.Length - rcvSize, SocketFlags.None);

                    if (bytesRec == 0) break;
                    rcvSize += bytesRec;
                    if (rcvSize >= count * 2 + 16) break;
                }

                if (rcvSize > 0)
                {
                    // rcvBuf[8] : Error No.
                    if (rcvSize == count * 2 + 16 && rcvBuf[5] == (byte)OPCode.READ_ACK && rcvBuf[8] == 0x00)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            words[i] = (ushort)(rcvBuf[i * 2 + 16] << 8 | rcvBuf[i * 2 + 1 + 16]);
                        }
                    }
                    else if (rcvSize > count * 2 + 16)
                    {
                        errorMsg = "Overflow Data (" + rcvSize + " Bytes)";
                        return false;
                    }
                    else if (rcvSize < count * 2 + 16)
                    {
                        errorMsg = "Incomplete Data (" + rcvSize + " Bytes)";
                        return false;
                    }
                }
                else
                {
                    errorMsg = "No Received";
                    return false;
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(e.ToString());
                errorMsg = ex.ToString();
                IsConnected = false;
                return false;
            }

            return true;

        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 임의 Address부터 N BYTE Write (User 조작)
        //++++++++++++++++++++++++++++++++++++++++++++++++++++
        public bool SetPlcValues(string startAddress, int count, byte[] bytes, out string errorMsg)
        {
            int sndSize;
            int rcvSize = 0;
            int bytesRec;
            byte[] sndBuf = new byte[BUF_MAX + FRAME_SIZE];
            byte[] rcvBuf = new byte[FRAME_SIZE];

            int DBNo;
            int addrNo;

            errorMsg = "";

            if (!SelectAddrSimens(startAddress, out DBNo, out addrNo))
                return false;

            try
            {
                sndSize = MakeWriteFrame(DBNo, addrNo, count, bytes, sndBuf);
                writeSocket.Send(sndBuf, sndSize, SocketFlags.None);

                while (true)
                {
                    bytesRec = writeSocket.Receive(rcvBuf, rcvSize, rcvBuf.Length - rcvSize, SocketFlags.None);

                    if (bytesRec == 0) break;
                    rcvSize += bytesRec;
                    if (rcvSize >= 16) break;
                }

                if (rcvSize > 0)
                {
                    // rcvBuf[8] : Error No.
                    if (rcvSize == 16 && rcvBuf[5] == (byte)OPCode.WRITE_ACK && rcvBuf[8] == 0x00)
                    {
                        return true;
                    }
                    else if (rcvSize > 16)
                    {
                        errorMsg = "Overflow Data (" + rcvSize + " Bytes)";
                        return false;
                    }
                    else if (rcvSize < 16)
                    {
                        errorMsg = "Incomplete Data (" + rcvSize + " Bytes)";
                        return false;
                    }
                }
                else
                {
                    errorMsg = "No Received";
                    return false;
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(e.ToString());
                errorMsg = ex.ToString();
                IsConnected = false;
                return false;
            }

            return true;

        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 임의 Address부터 N WORD Write (User 조작)
        //++++++++++++++++++++++++++++++++++++++++++++++++++++
        public bool SetPlcValues(string startAddress, int count, ushort[] words, out string errorMsg)
        {
            int sndSize;
            int rcvSize = 0;
            int bytesRec;
            byte[] sndBuf = new byte[BUF_MAX + FRAME_SIZE];
            byte[] rcvBuf = new byte[FRAME_SIZE];

            int DBNo;
            int addrNo;

            errorMsg = "";

            if (!SelectAddrSimens(startAddress, out DBNo, out addrNo))
                return false;

            try
            {
                sndSize = MakeWriteFrame(DBNo, addrNo, count, words, sndBuf);
                writeSocket.Send(sndBuf, sndSize, SocketFlags.None);

                while (true)
                {
                    bytesRec = writeSocket.Receive(rcvBuf, rcvSize, rcvBuf.Length - rcvSize, SocketFlags.None);

                    if (bytesRec == 0) break;
                    rcvSize += bytesRec;
                    if (rcvSize >= 16) break;
                }

                if (rcvSize > 0)
                {
                    // rcvBuf[8] : Error No.
                    if (rcvSize == 16 && rcvBuf[5] == (byte)OPCode.WRITE_ACK && rcvBuf[8] == 0x00)
                    {
                        return true;
                    }
                    else if (rcvSize > 16)
                    {
                        errorMsg = "Overflow Data (" + rcvSize + " Bytes)";
                        return false;
                    }
                    else if (rcvSize < 16)
                    {
                        errorMsg = "Incomplete Data (" + rcvSize + " Bytes)";
                        return false;
                    }
                }
                else
                {
                    errorMsg = "No Received";
                    return false;
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(e.ToString());
                errorMsg = ex.ToString();
                IsConnected = false;
                return false;
            }

            return true;

        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++
        // FETCH Request Frame [READ]
        //++++++++++++++++++++++++++++++++++++++++++++++++++++
        int MakeReadFrame(int DBNo, int startAddress, Unit readUnit, int count, byte[] sendBuffer)
        {
            int idx = 0;
            byte[] buf = new byte[2];

            Array.Clear(sendBuffer, 0, sendBuffer.Length);

            sendBuffer[idx++] = (byte)'S';                  // System ID [1/2]
            sendBuffer[idx++] = (byte)'5';                  // System ID [2/2]
            sendBuffer[idx++] = (byte)16;                   // Length of header
            sendBuffer[idx++] = (byte)1;                    // ID OP code
            sendBuffer[idx++] = (byte)3;                    // Legnth OP code
            sendBuffer[idx++] = (byte)OPCode.READ_REQ;      // OP Code
            sendBuffer[idx++] = (byte)3;                    // ORG field
            sendBuffer[idx++] = (byte)8;                    // Length ORG field
            sendBuffer[idx++] = (byte)AddrArea.DB;   	    // ORG ID (Use DB = 1)
            sendBuffer[idx++] = (byte)DBNo;			        // DBNR ≪if use DB -> 1~255≫
            buf = BitConverter.GetBytes(startAddress);
            sendBuffer[idx++] = buf[1];                     // Start address : H Byte
            sendBuffer[idx++] = buf[0];                     // Start address : L Byte
                                                            // ≪if use DB -> 0~2047≫

            buf = (readUnit == Unit.BYTE) ? BitConverter.GetBytes((count+1) / 2) : BitConverter.GetBytes(count);
            sendBuffer[idx++] = buf[1];                     // Length : H Byte
            sendBuffer[idx++] = buf[0];                     // Length : L Byte
            // ≪if use DB -> 1~2048≫

            sendBuffer[idx++] = (byte)0xff;                 // Empty field
            sendBuffer[idx++] = (byte)2;                    // Length empty field

            return idx;
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++
        // FETCH Request Frame [WRITE BYTES]
        //++++++++++++++++++++++++++++++++++++++++++++++++++++
        int MakeWriteFrame(int DBNo, int startAddress, int count, byte[] bytes, byte[] sendBuffer)
        {
            int idx = 0;
            byte[] buf = new byte[2];

            Array.Clear(sendBuffer, 0, sendBuffer.Length);

            sendBuffer[idx++] = (byte)'S';                  // System ID [1/2]
            sendBuffer[idx++] = (byte)'5';                  // System ID [2/2]
            sendBuffer[idx++] = (byte)16;                   // Length of header
            sendBuffer[idx++] = (byte)1;                    // ID OP code
            sendBuffer[idx++] = (byte)3;                    // Legnth OP code
            sendBuffer[idx++] = (byte)OPCode.WRITE_REQ;     // OP Code
            sendBuffer[idx++] = (byte)3;                    // ORG field
            sendBuffer[idx++] = (byte)8;                    // Length ORG field
            sendBuffer[idx++] = (byte)AddrArea.DB;   	    // ORG ID (Use DB = 1)
            sendBuffer[idx++] = (byte)DBNo;			        // DBNR ≪if use DB -> 1~255≫
            buf = BitConverter.GetBytes(startAddress);
            sendBuffer[idx++] = buf[1];                     // Start address : H Byte
            sendBuffer[idx++] = buf[0];                     // Start address : L Byte
                                                            // ≪if use DB -> 0~2047≫
            buf = BitConverter.GetBytes((count+1) / 2);
            sendBuffer[idx++] = buf[1];                     // Length : H Byte
            sendBuffer[idx++] = buf[0];                     // Length : L Byte
                                                            // ≪if use DB -> 1~2048≫

            sendBuffer[idx++] = (byte)0xff;                 // Empty field
            sendBuffer[idx++] = (byte)2;                    // Length empty field

            // DATA
            for (int i = 0; i < count; i++)
            {
                sendBuffer[idx++] = bytes[i];
            }

            return ((idx+1)/2)*2;
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++
        // FETCH Request Frame [WRITE WORDS]
        //++++++++++++++++++++++++++++++++++++++++++++++++++++
        int MakeWriteFrame(int DBNo, int startAddress, int count, ushort[] words, byte[] sendBuffer)
        {
            int idx = 0;
            byte[] buf = new byte[2];

            Array.Clear(sendBuffer, 0, sendBuffer.Length);

            sendBuffer[idx++] = (byte)'S';                  // System ID [1/2]
            sendBuffer[idx++] = (byte)'5';                  // System ID [2/2]
            sendBuffer[idx++] = (byte)16;                   // Length of header
            sendBuffer[idx++] = (byte)1;                    // ID OP code
            sendBuffer[idx++] = (byte)3;                    // Legnth OP code
            sendBuffer[idx++] = (byte)OPCode.WRITE_REQ;     // OP Code
            sendBuffer[idx++] = (byte)3;                    // ORG field
            sendBuffer[idx++] = (byte)8;                    // Length ORG field
            sendBuffer[idx++] = (byte)AddrArea.DB;   	    // ORG ID (Use DB = 1)
            sendBuffer[idx++] = (byte)DBNo;			        // DBNR ≪if use DB -> 1~255≫
            buf = BitConverter.GetBytes(startAddress);
            sendBuffer[idx++] = buf[1];                     // Start address : H Byte
            sendBuffer[idx++] = buf[0];                     // Start address : L Byte
            // ≪if use DB -> 0~2047≫
            buf = BitConverter.GetBytes(count);
            sendBuffer[idx++] = buf[1];                     // Length : H Byte
            sendBuffer[idx++] = buf[0];                     // Length : L Byte
            // ≪if use DB -> 1~2048≫

            sendBuffer[idx++] = (byte)0xff;                 // Empty field
            sendBuffer[idx++] = (byte)2;                    // Length empty field

            // DATA
            for (int i = 0; i < count; i++)
            {
                buf = BitConverter.GetBytes(words[i]);
                sendBuffer[idx++] = buf[1];  // H
                sendBuffer[idx++] = buf[0];  // L
            }

            return idx;
        }

        // PLC Add. 숫자부분 추출 [Simens]
        bool SelectAddrSimens(string plcAddr, out int DBNo, out int AddrNo)
        {
            int i;
            int idx;
            int splitIdx;
            byte[] buff = new byte[80];
            string str;

            DBNo = -1;
            AddrNo = -1;

            plcAddr = plcAddr.Trim();
            splitIdx = plcAddr.IndexOf('.');

            if (!Char.IsLetter(plcAddr[0]) || splitIdx < 0)
                return false;

            idx = 0;
            if (plcAddr[0].Equals('D') && plcAddr[1].Equals('B'))
            {
                for (i = 2; i < splitIdx; i++)
                {
                    if (plcAddr[i] < 0x20) break;
                    if ((plcAddr[i] >= '0') && (plcAddr[i] <= '9'))
                        buff[idx++] = Convert.ToByte(plcAddr[i]);
                }

                if (idx == 0)
                    return false;

                str = Encoding.Default.GetString(buff, 0, idx);
                DBNo = Convert.ToInt32(str);
            }
            else
            {
                DBNo = -1;
            }

            idx = 0;
            Array.Clear(buff, 0, buff.Length);
            for (i = splitIdx + 1; i < plcAddr.Length; i++)
            {
                if (plcAddr[i] < 0x20) break;
                if ((plcAddr[i] >= '0') && (plcAddr[i] <= '9'))
                    buff[idx++] = Convert.ToByte(plcAddr[i]);
            }

            if (idx == 0)
                return false;

            str = Encoding.Default.GetString(buff, 0, idx);
            AddrNo = Convert.ToInt32(str);

            return true;
        }
    }
}
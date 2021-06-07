using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace GeissBarcodeTest
{
    #region class BYTECollection
    public class BYTECollection : CollectionBase
    {
        #region 생성자
        public BYTECollection()
        {
        }
        #endregion
        public static implicit operator BYTECollection(BYTE[] pAddrs)
        {
            BYTECollection aCollection = new BYTECollection();
            foreach (BYTE aAddr in pAddrs)
            {
                aCollection.Add(aAddr);
            }
            return aCollection;
        }

        public static implicit operator BYTECollection(byte[] pAddrs)
        {
            BYTECollection aCollection = new BYTECollection();
            foreach (byte aAddr in pAddrs)
            {
                aCollection.Add(aAddr);
            }
            return aCollection;
        }

        public byte this[int i]
        {
            get { return ((BYTE)this.List[i]).Byte; }
            set { ((BYTE)this.List[i]).Byte = value; }
        }

        public bool this[int i, int j]
        {
            get { return ((BYTE)this.List[i])[j]; }
            set { ((BYTE)this.List[i])[j] = value; }
        }

        public int Add(BYTE pAddr)
        {
            return List.Add(pAddr);
        }

        public int Add(BYTE[] pAddrs)
        {
            int iResult = 0;
            foreach (BYTE pAddr in pAddrs)
            {
                iResult += Add(pAddr);
            }
            return iResult;
        }

        public byte[] ReadTotalByte()
        {
            byte[] tempByte = ReadByte(0, this.List.Count);

            return tempByte;
        }
        public byte[] ReadByte(int iStart, int iCount)
        {
            byte[] tempByte = new byte[iCount];
            for (int i = iStart; i < iCount; i++)
            {
                tempByte[i] = ((BYTE)this.List[i]).Byte;
            }

            return tempByte;
        }


        #region NoUse
        #region Contains
        public bool Contains(BYTE pAddr)
        {
            return List.Contains(pAddr);
        }
        #endregion
        #region CopyTo
        public void CopyTo(BYTE[] arraypAddr, int index)
        {
            List.CopyTo(arraypAddr, index);
        }
        #endregion
        #region IndexOf
        public int IndexOf(BYTE pAddr)
        {
            return List.IndexOf(pAddr);
        }
        #endregion
        #region Insert
        public void Insert(int index, BYTE pAddr)
        {
            List.Insert(index, pAddr);
        }
        #endregion
        #region Remove
        public void Remove(BYTE pAddr)
        {
            List.Remove(pAddr);
        }
        #endregion
        #endregion

        
    }
    #endregion

    public class BYTE
    {
        private byte m_Byte;
        private bool[] m_Bits;

        public static implicit operator BYTE(byte pByte)
        {
            BYTE aNewAddr = new BYTE();
            aNewAddr.Byte = pByte;

            return aNewAddr;
        }

        public BYTE()
        {
            m_Byte = new byte();
            m_Bits = new bool[8];

            Array.Clear(m_Bits, 0, m_Bits.Length);
        }

        public byte Byte
        {
            get {return m_Byte;}
            set
            {
                m_Byte = value;
                ByteToBit();
            }
        }

        public bool this[int idx]
        {
            get { return m_Bits[idx]; }
            set
            {
                m_Bits[idx] = value;
                BitToByte();
            }
        }

        private void ByteToBit()
        {
            Array.Clear(m_Bits, 0, 8);

            int pInt = (int)m_Byte;
            int cnt = 0;

            while (pInt != 0)
            {
                int uiTemp = pInt % 2;
                pInt = pInt / 2;

                m_Bits[cnt] = Convert.ToBoolean(uiTemp);

                cnt++;
            }
        }

        private void BitToByte()
        {
            int iResult = 0;

            for (int i = 0; i < 8; i++)
            {
                iResult += m_Bits[i] == true ? 1 : 0 * Square(2, i);
            }

            m_Byte = (byte)iResult;
        }

        private int Square(int value1, int value2)
        {
            int iResult = 1;

            for (int i=0 ; i<value2 ; i++)
            {
                iResult =  iResult * value1;
            }

            return iResult;
        }
    }
}

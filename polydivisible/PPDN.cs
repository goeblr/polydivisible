using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace polydivisible
{
    // potential polidivisible number
    class PPDN
    {
        public PPDN(byte _base)
        {
            m_base = _base;
            m_nextBase = 0;
            m_nextBaseRemainder = 0;
            m_digits = new byte[_base - 1];
            m_digitsLeft = new List<byte>();

            m_lastBases = new Stack<BigInteger>(_base);
            m_lastBaseRemainders = new Stack<int>(_base);

            m_digitsUpToNow = 0;

            for (byte d = 1; d < _base; d++)
            {
                m_digitsLeft.Add(d);
            }
        }

        public PPDN(PPDN other)
        {
            m_base = other.m_base;
            m_nextBase = other.m_nextBase;
            m_nextBaseRemainder = other.m_nextBaseRemainder;

            m_lastBases = new Stack<BigInteger>(other.m_lastBases);
            m_lastBaseRemainders = new Stack<int>(other.m_lastBaseRemainders);

            m_digits = (byte[])other.m_digits.Clone();
            m_digitsUpToNow = other.m_digitsUpToNow;
        }

        public IList<byte> digitsLeft()
        {
            return m_digitsLeft.AsReadOnly();
        }

        public bool addDigit(byte digit)
        {
            if (m_digitsUpToNow == m_base - 1)
                return false;

            if (m_digitsUpToNow == 0)
            {
                m_digits[0] = digit;
                m_nextBase = digit * m_base;
                m_digitsUpToNow = 1;

                m_lastBases.Push(new BigInteger(0));
                m_lastBaseRemainders.Push(0);

                m_nextBaseRemainder = (int)(m_nextBase % (m_digitsUpToNow + 1));

                m_digitsLeft.Remove(digit);
                return true;
            }
            else
            {
                int divisor = m_digitsUpToNow + 1;
                bool divisible = ((m_nextBaseRemainder + digit) % divisor == 0);

                if (divisible)
                {
                    m_digits[m_digitsUpToNow] = digit;
                    m_digitsUpToNow++;
                    
                    m_lastBases.Push(m_nextBase);
                    m_lastBaseRemainders.Push(m_nextBaseRemainder);
                    
                    m_nextBase = (m_nextBase + digit) * m_base;
                    m_nextBaseRemainder = (int)(m_nextBase % (m_digitsUpToNow + 1));

                    m_digitsLeft.Remove(digit);
                }
                return divisible;
            }
        }

        public void removeLastDigit()
        {
            if (m_digitsUpToNow >= 1)
            {
                byte digitToRemove = m_digits[m_digitsUpToNow-1];
                m_digitsUpToNow--;
                //m_nextBase = (m_nextBase  / m_base - digitToRemove);
                //m_nextBaseRemainder = (int)(m_nextBase % (m_digitsUpToNow + 1));
                m_nextBase = m_lastBases.Pop();
                m_nextBaseRemainder = m_lastBaseRemainders.Pop();

                m_digitsLeft.Add(digitToRemove);
            }
        }

        public override string ToString()
        {
            string desc = "<";
            for (int position = 0; position < m_digitsUpToNow; position++)
            {
                desc += m_digits[position].ToString() + ", ";
            }
            desc += ">";
            return desc;
        }

        private byte m_base;
        private byte[] m_digits;
        private byte m_digitsUpToNow;
        private List<byte> m_digitsLeft;
        private BigInteger m_nextBase;
        private int m_nextBaseRemainder;
        private Stack<BigInteger> m_lastBases;
        private Stack<int> m_lastBaseRemainders;
    }
}

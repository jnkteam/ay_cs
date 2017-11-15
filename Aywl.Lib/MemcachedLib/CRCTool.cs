namespace MemcachedLib
{
    using System;

    public class CRCTool
    {
        private ulong crchighbit;
        private ulong crcinit = 0xffffL;
        private ulong crcinit_direct;
        private ulong crcinit_nondirect;
        private ulong crcmask;
        private ulong[] crctab = new ulong[0x100];
        private ulong crcxor = 0L;
        private int direct = 1;
        private int order = 0x10;
        private ulong polynom = 0x1021L;
        private int refin = 0;
        private int refout = 0;

        public ushort CalcCRCITT(byte[] p)
        {
            uint num = 0xffff;
            for (int i = 0; i < p.Length; i++)
            {
                uint num2 = (uint) (p[i] << 8);
                for (int j = 0; j < 8; j++)
                {
                    if (((num ^ num2) & 0x8000) != 0)
                    {
                        num = (num << 1) ^ 0x1021;
                    }
                    else
                    {
                        num = num << 1;
                    }
                    num2 = num2 << 1;
                }
            }
            return (ushort) num;
        }

        public ulong crcbitbybit(byte[] p)
        {
            int num;
            ulong num4;
            ulong crc = this.crcinit_nondirect;
            for (num = 0; num < p.Length; num++)
            {
                ulong num3 = p[num];
                if (this.refin != 0)
                {
                    num3 = this.reflect(num3, 8);
                }
                for (ulong i = 0x80L; i != 0L; i = i >> 1)
                {
                    num4 = crc & this.crchighbit;
                    crc = crc << 1;
                    if ((num3 & i) != 0L)
                    {
                        crc |= (ulong) 1L;
                    }
                    if (num4 != 0L)
                    {
                        crc ^= this.polynom;
                    }
                }
            }
            for (num = 0; num < this.order; num++)
            {
                num4 = crc & this.crchighbit;
                crc = crc << 1;
                if (num4 != 0L)
                {
                    crc ^= this.polynom;
                }
            }
            if (this.refout != 0)
            {
                crc = this.reflect(crc, this.order);
            }
            crc ^= this.crcxor;
            return (crc & this.crcmask);
        }

        public ulong crcbitbybitfast(byte[] p)
        {
            ulong crc = this.crcinit_direct;
            for (int i = 0; i < p.Length; i++)
            {
                ulong num3 = p[i];
                if (this.refin != 0)
                {
                    num3 = this.reflect(num3, 8);
                }
                for (ulong j = 0x80L; j > 0L; j = j >> 1)
                {
                    ulong num4 = crc & this.crchighbit;
                    crc = crc << 1;
                    if ((num3 & j) > 0L)
                    {
                        num4 ^= this.crchighbit;
                    }
                    if (num4 > 0L)
                    {
                        crc ^= this.polynom;
                    }
                }
            }
            if (this.refout > 0)
            {
                crc = this.reflect(crc, this.order);
            }
            crc ^= this.crcxor;
            return (crc & this.crcmask);
        }

        public ulong crctable(byte[] p)
        {
            int num2;
            ulong crc = this.crcinit_nondirect;
            if (this.refin != 0)
            {
                crc = this.reflect(crc, this.order);
            }
            if (this.refin == 0)
            {
                for (num2 = 0; num2 < p.Length; num2++)
                {
                    crc = ((crc << 8) | p[num2]) ^ this.crctab[(int) ((IntPtr) ((crc >> (this.order - 8)) & ((ulong) 0xffL)))];
                }
            }
            else
            {
                num2 = 0;
                while (num2 < p.Length)
                {
                    crc = (ulong) ((((int) (crc >> 8)) | (p[num2] << (this.order - 8))) ^ ((int) this.crctab[(int) ((IntPtr) (crc & ((ulong) 0xffL)))]));
                    num2++;
                }
            }
            if (this.refin == 0)
            {
                for (num2 = 0; num2 < (this.order / 8); num2++)
                {
                    crc = (crc << 8) ^ this.crctab[(int) ((IntPtr) ((crc >> (this.order - 8)) & ((ulong) 0xffL)))];
                }
            }
            else
            {
                for (num2 = 0; num2 < (this.order / 8); num2++)
                {
                    crc = (crc >> 8) ^ this.crctab[(int) ((IntPtr) (crc & ((ulong) 0xffL)))];
                }
            }
            if ((this.refout ^ this.refin) != 0)
            {
                crc = this.reflect(crc, this.order);
            }
            crc ^= this.crcxor;
            return (crc & this.crcmask);
        }

        public ulong crctablefast(byte[] p)
        {
            int num2;
            ulong crc = this.crcinit_direct;
            if (this.refin != 0)
            {
                crc = this.reflect(crc, this.order);
            }
            if (this.refin == 0)
            {
                for (num2 = 0; num2 < p.Length; num2++)
                {
                    crc = (crc << 8) ^ this.crctab[(int) ((IntPtr) (((crc >> (this.order - 8)) & ((ulong) 0xffL)) ^ p[num2]))];
                }
            }
            else
            {
                for (num2 = 0; num2 < p.Length; num2++)
                {
                    crc = (crc >> 8) ^ this.crctab[(int) ((IntPtr) ((crc & ((ulong) 0xffL)) ^ p[num2]))];
                }
            }
            if ((this.refout ^ this.refin) != 0)
            {
                crc = this.reflect(crc, this.order);
            }
            crc ^= this.crcxor;
            return (crc & this.crcmask);
        }

        private void generate_crc_table()
        {
            for (int i = 0; i < 0x100; i++)
            {
                ulong crc = (ulong) i;
                if (this.refin != 0)
                {
                    crc = this.reflect(crc, 8);
                }
                crc = crc << (this.order - 8);
                for (int j = 0; j < 8; j++)
                {
                    ulong num3 = crc & this.crchighbit;
                    crc = crc << 1;
                    if (num3 != 0L)
                    {
                        crc ^= this.polynom;
                    }
                }
                if (this.refin != 0)
                {
                    crc = this.reflect(crc, this.order);
                }
                this.crctab[i] = crc & this.crcmask;
            }
        }

        public void Init(CRCCode CodingType)
        {
            ulong num;
            ulong crcinit;
            int num3;
            switch (CodingType)
            {
                case CRCCode.CRC_CCITT:
                    this.order = 0x10;
                    this.direct = 1;
                    this.polynom = 0x1021L;
                    this.crcinit = 0xffffL;
                    this.crcxor = 0L;
                    this.refin = 0;
                    this.refout = 0;
                    break;

                case CRCCode.CRC16:
                    this.order = 0x10;
                    this.direct = 1;
                    this.polynom = 0x8005L;
                    this.crcinit = 0L;
                    this.crcxor = 0L;
                    this.refin = 1;
                    this.refout = 1;
                    break;

                case CRCCode.CRC32:
                    this.order = 0x20;
                    this.direct = 1;
                    this.polynom = 0x4c11db7L;
                    this.crcinit = 0xffffffffL;
                    this.crcxor = 0xffffffffL;
                    this.refin = 1;
                    this.refout = 1;
                    break;
            }
            this.crcmask = (ulong) ((((1L << ((this.order - 1) & 0x3f)) - 1L) << 1) | 1L);
            this.crchighbit = ((ulong) 1L) << (this.order - 1);
            this.generate_crc_table();
            if (this.direct == 0)
            {
                this.crcinit_nondirect = this.crcinit;
                crcinit = this.crcinit;
                for (num3 = 0; num3 < this.order; num3++)
                {
                    num = crcinit & this.crchighbit;
                    crcinit = crcinit << 1;
                    if (num != 0L)
                    {
                        crcinit ^= this.polynom;
                    }
                }
                crcinit &= this.crcmask;
                this.crcinit_direct = crcinit;
            }
            else
            {
                this.crcinit_direct = this.crcinit;
                crcinit = this.crcinit;
                for (num3 = 0; num3 < this.order; num3++)
                {
                    num = crcinit & ((ulong) 1L);
                    if (num != 0L)
                    {
                        crcinit ^= this.polynom;
                    }
                    crcinit = crcinit >> 1;
                    if (num != 0L)
                    {
                        crcinit |= this.crchighbit;
                    }
                }
                this.crcinit_nondirect = crcinit;
            }
        }

        private ulong reflect(ulong crc, int bitnum)
        {
            ulong num2 = 1L;
            ulong num3 = 0L;
            for (ulong i = ((ulong) 1L) << (bitnum - 1); i != 0L; i = i >> 1)
            {
                if ((crc & i) != 0L)
                {
                    num3 |= num2;
                }
                num2 = num2 << 1;
            }
            return num3;
        }

        public enum CRCCode
        {
            CRC_CCITT,
            CRC16,
            CRC32
        }
    }
}


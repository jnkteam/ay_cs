namespace MemcachedLib
{
    using System;
    using System.Text;

    public class HashingAlgorithmHelper
    {
        public static int NewHashingAlgorithm(string key)
        {
            CRCTool tool = new CRCTool();
            tool.Init(CRCTool.CRCCode.CRC32);
            int num = (int) tool.crctablefast(Encoding.UTF8.GetBytes(key));
            return ((num >> 0x10) & 0x7fff);
        }

        public static int OriginalHashingAlgorithm(string key)
        {
            int num = 0;
            char[] chArray = key.ToCharArray();
            for (int i = 0; i < chArray.Length; i++)
            {
                num = (num * 0x21) + chArray[i];
            }
            return num;
        }
    }
}


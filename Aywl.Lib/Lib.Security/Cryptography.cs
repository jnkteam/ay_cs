using OriginalStudio.Lib.ExceptionHandling;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace OriginalStudio.Lib.Security
{
    /// <summary>
    /// 加密解密算法操作。
    /// </summary>
	public sealed class Cryptography
	{
		private Cryptography()
		{
		}

        #region MD5加密

        public static string MD5(string strToEncrypt)
		{
			byte[] hash = new MD5CryptoServiceProvider().ComputeHash(Encoding.GetEncoding("GB2312").GetBytes(strToEncrypt));
			string str = "";
			for (int index = 0; index < hash.Length; index++)
			{
				str += hash[index].ToString("x").PadLeft(2, '0');
			}
			return str;
		}

		public static string MD5(string strToEncrypt, string encodeing)
		{
			byte[] hash = new MD5CryptoServiceProvider().ComputeHash(Encoding.GetEncoding(encodeing).GetBytes(strToEncrypt));
			string str = "";
			for (int index = 0; index < hash.Length; index++)
			{
				str += hash[index].ToString("x").PadLeft(2, '0');
			}
			return str;
		}

        #endregion

        #region AES加密解密

		private static byte[] RijndaelIV
		{
			get
			{
                return Cryptography.MD5StrToByte("创意工场 OriginalStudio Provider");
			}
		}

		private static byte[] RijndaelKey
		{
			get
			{
				byte[] numArray = new byte[32];
                Array.Copy(Cryptography.MD5StrToByte("创意工场 OriginalStudio Provider"), 0, numArray, 0, 16);
                Array.Copy(Cryptography.MD5ByteToByte(Cryptography.MD5StrToByte("OriginalStudio Provider")), 0, numArray, 16, 16);
				return numArray;
			}
		}

        public static string RijndaelDecrypt(string strToDecrypt)
        {
            if (String.IsNullOrWhiteSpace(strToDecrypt)) return "";
            byte[] rijndaelKey = Cryptography.RijndaelKey;
            byte[] rijndaelIv = Cryptography.RijndaelIV;
            byte[] buffer = Convert.FromBase64String(strToDecrypt);
            byte[] numArray = new byte[buffer.Length];
            MemoryStream memoryStream = new MemoryStream(buffer);
            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateDecryptor(rijndaelKey, rijndaelIv), CryptoStreamMode.Read);
            string result;
            try
            {
                cryptoStream.Read(numArray, 0, numArray.Length);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
                memoryStream.Close();
                cryptoStream.Close();
                result = string.Empty;
                return result;
            }
            result = Encoding.UTF8.GetString(numArray);
            return result;
        }

        public static string RijndaelEncrypt(string strToEncrypt)
        {
            if (String.IsNullOrWhiteSpace(strToEncrypt)) return "";

            byte[] rijndaelKey = Cryptography.RijndaelKey;
            byte[] rijndaelIv = Cryptography.RijndaelIV;
            byte[] bytes = Encoding.UTF8.GetBytes(strToEncrypt);
            byte[] numArray = new byte[0];
            MemoryStream memoryStream = new MemoryStream();
            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateEncryptor(rijndaelKey, rijndaelIv), CryptoStreamMode.Write);
            byte[] inArray;
            string result;
            try
            {
                cryptoStream.Write(bytes, 0, bytes.Length);
                cryptoStream.FlushFinalBlock();
                inArray = memoryStream.ToArray();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
                memoryStream.Close();
                cryptoStream.Close();
                result = string.Empty;
                return result;
            }
            result = Convert.ToBase64String(inArray);
            return result;
        }
        
        #endregion

        #region DES加密解密

        private const string DESkey = "创意工场 OriginalStudio Provider";

		private static byte[] DESIV;

		private static byte[] DESKey;

        public static string DESDecryptString(string inputStr, string keyStr = "")
        {
            string result;
            if (inputStr == null || inputStr.Length == 0)
            {
                result = string.Empty;
            }
            else
            {
                DESCryptoServiceProvider cryptoServiceProvider = new DESCryptoServiceProvider();
                if (keyStr == null || keyStr.Length == 0)
                {
                    keyStr = DESkey;
                }
                byte[] buffer = new byte[inputStr.Length / 2];
                for (int index = 0; index < inputStr.Length / 2; index++)
                {
                    int num = Convert.ToInt32(inputStr.Substring(index * 2, 2), 16);
                    buffer[index] = (byte)num;
                }
                byte[] hash = new SHA1Managed().ComputeHash(Encoding.Default.GetBytes(keyStr));
                Cryptography.DESKey = new byte[8];
                Cryptography.DESIV = new byte[8];
                for (int index = 0; index < 8; index++)
                {
                    Cryptography.DESKey[index] = hash[index];
                }
                for (int index = 8; index < 16; index++)
                {
                    Cryptography.DESIV[index - 8] = hash[index];
                }
                cryptoServiceProvider.Key = Cryptography.DESKey;
                cryptoServiceProvider.IV = Cryptography.DESIV;
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoServiceProvider.CreateDecryptor(), CryptoStreamMode.Write);
                cryptoStream.Write(buffer, 0, buffer.Length);
                cryptoStream.FlushFinalBlock();
                result = Encoding.Default.GetString(memoryStream.ToArray());
            }
            return result;
        }

        public static string DESEncryptString(string inputStr, string keyStr = "")
		{
			string result;
			if (inputStr == null || inputStr.Length == 0)
			{
				result = string.Empty;
			}
			else
			{
				DESCryptoServiceProvider cryptoServiceProvider = new DESCryptoServiceProvider();
				if (keyStr == null || keyStr.Length == 0)
				{
                    keyStr = DESkey;
				}
				byte[] bytes = Encoding.Default.GetBytes(inputStr);
				byte[] hash = new SHA1Managed().ComputeHash(Encoding.Default.GetBytes(keyStr));
				Cryptography.DESKey = new byte[8];
				Cryptography.DESIV = new byte[8];
				for (int index = 0; index < 8; index++)
				{
					Cryptography.DESKey[index] = hash[index];
				}
				for (int index = 8; index < 16; index++)
				{
					Cryptography.DESIV[index - 8] = hash[index];
				}
				cryptoServiceProvider.Key = Cryptography.DESKey;
				cryptoServiceProvider.IV = Cryptography.DESIV;
				MemoryStream memoryStream = new MemoryStream();
				CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoServiceProvider.CreateEncryptor(), CryptoStreamMode.Write);
				cryptoStream.Write(bytes, 0, bytes.Length);
				cryptoStream.FlushFinalBlock();
				StringBuilder stringBuilder = new StringBuilder();
				byte[] array = memoryStream.ToArray();
				for (int i = 0; i < array.Length; i++)
				{
					byte num = array[i];
					stringBuilder.AppendFormat("{0:X2}", num);
				}
				cryptoStream.Close();
				memoryStream.Close();
				result = stringBuilder.ToString();
			}
			return result;
		}

        #endregion

        #region  连接字符串加密解密

        public static string DecryptConnString(string connString)
		{
			return Cryptography.DESDecryptString(connString, string.Empty);
		}

		public static string EncryptConnString(string connString)
		{
			return Cryptography.DESEncryptString(connString, string.Empty);
		}

		public static string EncryptPassword(string password)
		{
			return Cryptography.MD5(password);
		}

        #endregion

        #region 字节数组MD5转换

        public static byte[] MD5ByteToByte(byte[] bytesToEncrypt)
		{
			return ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(bytesToEncrypt);
		}

		public static string MD5ByteToStr(byte[] bytesToEncrypt)
		{
			return Convert.ToBase64String(Cryptography.MD5ByteToByte(bytesToEncrypt));
		}

		public static byte[] MD5StrToByte(string strToEncrypt)
		{
			return Cryptography.MD5ByteToByte(Encoding.UTF8.GetBytes(strToEncrypt));
		}

        #endregion

        #region SHA加密

        public static string SHA1(string strToEncrypt)
		{
			return Convert.ToBase64String(((HashAlgorithm)CryptoConfig.CreateFromName("SHA1")).ComputeHash(Encoding.UTF8.GetBytes(strToEncrypt)));
        }

        #endregion
    }
}

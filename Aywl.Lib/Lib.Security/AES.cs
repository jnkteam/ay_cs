using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace OriginalStudio.Lib.Security
{
	public class AES
	{
		private static byte[] Keys = new byte[]
		{
			65,
			114,
			101,
			121,
			111,
			117,
			109,
			121,
			83,
			110,
			111,
			119,
			109,
			97,
			110,
			63
		};

		public static string Decode(string decryptString, string decryptKey)
		{
			string result;
			try
			{
				decryptKey = Utility.GetSubString(decryptKey, 32, "");
				decryptKey = decryptKey.PadRight(32, ' ');
				ICryptoTransform decryptor = new RijndaelManaged
				{
					Key = Encoding.UTF8.GetBytes(decryptKey),
					IV = AES.Keys
				}.CreateDecryptor();
				byte[] inputBuffer = Convert.FromBase64String(decryptString);
				result = Encoding.UTF8.GetString(decryptor.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length));
			}
			catch
			{
				result = "";
			}
			return result;
		}

		public static string DESDecrypt(string pToDecrypt, string sKey)
		{
			byte[] buffer = Convert.FromBase64String(pToDecrypt);
			string result;
			using (DESCryptoServiceProvider cryptoServiceProvider = new DESCryptoServiceProvider())
			{
				cryptoServiceProvider.Key = Encoding.ASCII.GetBytes(sKey);
				cryptoServiceProvider.IV = Encoding.ASCII.GetBytes(sKey);
				MemoryStream memoryStream = new MemoryStream();
				using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoServiceProvider.CreateDecryptor(), CryptoStreamMode.Write))
				{
					cryptoStream.Write(buffer, 0, buffer.Length);
					cryptoStream.FlushFinalBlock();
					cryptoStream.Close();
				}
				string @string = Encoding.UTF8.GetString(memoryStream.ToArray());
				memoryStream.Close();
				result = @string;
			}
			return result;
		}

		public static string DoDecrypt(string pToDecrypt, string sKey)
		{
			DESCryptoServiceProvider cryptoServiceProvider = new DESCryptoServiceProvider();
			byte[] buffer = new byte[pToDecrypt.Length / 2];
			for (int index = 0; index < pToDecrypt.Length / 2; index++)
			{
				int num = Convert.ToInt32(pToDecrypt.Substring(index * 2, 2), 16);
				buffer[index] = (byte)num;
			}
			cryptoServiceProvider.Key = Encoding.ASCII.GetBytes(sKey);
			cryptoServiceProvider.IV = Encoding.ASCII.GetBytes(sKey);
			MemoryStream memoryStream = new MemoryStream();
			CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoServiceProvider.CreateDecryptor(), CryptoStreamMode.Write);
			cryptoStream.Write(buffer, 0, buffer.Length);
			cryptoStream.FlushFinalBlock();
			StringBuilder stringBuilder = new StringBuilder();
			return Encoding.Default.GetString(memoryStream.ToArray());
		}

		public static string DoDecrypt1(string pToDecrypt, string sKey)
		{
			return AES.DESDecrypt(pToDecrypt, sKey);
		}

		public static string DESEncrypt(string pToEncrypt, string sKey)
		{
			string result;
			using (DESCryptoServiceProvider cryptoServiceProvider = new DESCryptoServiceProvider())
			{
				byte[] bytes = Encoding.UTF8.GetBytes(pToEncrypt);
				cryptoServiceProvider.Key = Encoding.ASCII.GetBytes(sKey);
				cryptoServiceProvider.IV = Encoding.ASCII.GetBytes(sKey);
				MemoryStream memoryStream = new MemoryStream();
				using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoServiceProvider.CreateEncryptor(), CryptoStreamMode.Write))
				{
					cryptoStream.Write(bytes, 0, bytes.Length);
					cryptoStream.FlushFinalBlock();
					cryptoStream.Close();
				}
				string str = Convert.ToBase64String(memoryStream.ToArray());
				memoryStream.Close();
				result = str;
			}
			return result;
		}

		public static string Encode(string encryptString, string encryptKey)
		{
			encryptKey = Utility.GetSubString(encryptKey, 32, "");
			encryptKey = encryptKey.PadRight(32, ' ');
			ICryptoTransform encryptor = new RijndaelManaged
			{
				Key = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 32)),
				IV = AES.Keys
			}.CreateEncryptor();
			byte[] bytes = Encoding.UTF8.GetBytes(encryptString);
			return Convert.ToBase64String(encryptor.TransformFinalBlock(bytes, 0, bytes.Length));
		}
	}
}

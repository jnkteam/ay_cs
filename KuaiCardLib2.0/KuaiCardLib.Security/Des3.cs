using KuaiCardLib.ExceptionHandling;
using System;
using System.IO;
using System.Security.Cryptography;

namespace KuaiCardLib.Security
{
	public class Des3
	{
		public static byte[] Des3EncodeCBC(byte[] key, byte[] iv, byte[] data)
		{
			byte[] result;
			try
			{
				MemoryStream memoryStream = new MemoryStream();
				CryptoStream cryptoStream = new CryptoStream(memoryStream, new TripleDESCryptoServiceProvider
				{
					Mode = CipherMode.CBC,
					Padding = PaddingMode.PKCS7
				}.CreateEncryptor(key, iv), CryptoStreamMode.Write);
				cryptoStream.Write(data, 0, data.Length);
				cryptoStream.FlushFinalBlock();
				byte[] numArray = memoryStream.ToArray();
				cryptoStream.Close();
				memoryStream.Close();
				result = numArray;
			}
			catch (CryptographicException ex)
			{
				Console.WriteLine("A Cryptographic error occurred: {0}", ex.Message);
				result = null;
			}
			return result;
		}

		public static byte[] Des3DecodeCBC(byte[] key, byte[] iv, byte[] data)
		{
			byte[] result;
			try
			{
				MemoryStream memoryStream = new MemoryStream(data);
				CryptoStream cryptoStream = new CryptoStream(memoryStream, new TripleDESCryptoServiceProvider
				{
					Mode = CipherMode.CBC,
					Padding = PaddingMode.PKCS7
				}.CreateDecryptor(key, iv), CryptoStreamMode.Read);
				byte[] buffer = new byte[data.Length];
				cryptoStream.Read(buffer, 0, buffer.Length);
				result = buffer;
			}
			catch (CryptographicException ex)
			{
				Console.WriteLine("A Cryptographic error occurred: {0}", ex.Message);
				result = null;
			}
			return result;
		}

		public static byte[] Des3EncodeECB(byte[] key, byte[] iv, byte[] data)
		{
			byte[] result;
			try
			{
				MemoryStream memoryStream = new MemoryStream();
				CryptoStream cryptoStream = new CryptoStream(memoryStream, new TripleDESCryptoServiceProvider
				{
					Mode = CipherMode.ECB,
					Padding = PaddingMode.PKCS7
				}.CreateEncryptor(key, iv), CryptoStreamMode.Write);
				cryptoStream.Write(data, 0, data.Length);
				cryptoStream.FlushFinalBlock();
				byte[] numArray = memoryStream.ToArray();
				cryptoStream.Close();
				memoryStream.Close();
				result = numArray;
			}
			catch (CryptographicException ex)
			{
				ExceptionHandler.HandleException(ex);
				result = null;
			}
			return result;
		}

		public static byte[] Des3DecodeECB(byte[] key, byte[] iv, byte[] data)
		{
			byte[] result;
			try
			{
				MemoryStream memoryStream = new MemoryStream(data);
				CryptoStream cryptoStream = new CryptoStream(memoryStream, new TripleDESCryptoServiceProvider
				{
					Mode = CipherMode.ECB,
					Padding = PaddingMode.PKCS7
				}.CreateDecryptor(key, iv), CryptoStreamMode.Read);
				byte[] buffer = new byte[data.Length];
				cryptoStream.Read(buffer, 0, buffer.Length);
				result = buffer;
			}
			catch (CryptographicException ex)
			{
				Console.WriteLine("A Cryptographic error occurred: {0}", ex.Message);
				result = null;
			}
			return result;
		}
	}
}

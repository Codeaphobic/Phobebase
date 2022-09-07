using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Phobebase.Hash
{
    public class SHA265
    {
        public static byte[] Hash(string data)
		{
			byte[] byteData = Encoding.UTF8.GetBytes(data);
			SHA256Managed hasher = new SHA256Managed();

			return hasher.ComputeHash(byteData);
		}

		public static byte[] Hash(byte[] data)
		{
			SHA256Managed hasher = new SHA256Managed();

			return hasher.ComputeHash(data);
		}

		public static string GetHexStringFromHash(byte[] hash)
		{
			string hexString = "";

			foreach (byte b in hash) hexString += b.ToString("x2");

			return hexString;
		}
    }
}
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
	// Mainly just for the Intergrated Save System so if your not using it proll dont need this

    public class SHA265
    {
		// Hashes string with SHA256
        public static byte[] Hash(string data)
		{
			byte[] byteData = Encoding.UTF8.GetBytes(data);
			SHA256Managed hasher = new SHA256Managed();

			return hasher.ComputeHash(byteData);
		}

		// Hashes byte array with SHA256
		public static byte[] Hash(byte[] data)
		{
			SHA256Managed hasher = new SHA256Managed();

			return hasher.ComputeHash(data);
		}

		// Makes bytes into a hex string - prolly could be in a different file 🤷‍♀️
		public static string GetHexStringFromHash(byte[] hash)
		{
			string hexString = "";

			foreach (byte b in hash) hexString += b.ToString("x2");

			return hexString;
		}
    }
}
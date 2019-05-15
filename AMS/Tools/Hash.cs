//Code from Professor Holmes' PeerVal Project for his Hasher

using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Tool is for hashing passwords and anything else that needs it
/// </summary>
namespace AMS.Tools
{
	public class Hash
	{

		/// <summary>
		/// 
		/// </summary>
		/// <param name="passToHash">password we need hashed</param>
		/// <param name="salt">salt to apply to hash</param>
		/// <param name="pepper">pepper to apply to hash</param>
		/// <param name="stretches">how many times we should iterate the hash. 12000 - 24000</param>
		/// <returns></returns>
		public static string Get(string passToHash, string salt, string pepper, int stretches, int numberOfChars = 64)
		{
			// https://lockmedown.com/hash-right-implementing-pbkdf2-net/
			// https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.cryptography.keyderivation.keyderivation.pbkdf2?view=aspnetcore-2.2

			// 64 characters every 3 bits makes a new 
			byte[] myBytes = KeyDerivation.Pbkdf2(passToHash,
				Encoding.UTF8.GetBytes(salt + pepper),
				KeyDerivationPrf.HMACSHA256,
				stretches,
				GetBase64NumberLength(numberOfChars)
				);

			return Convert.ToBase64String(myBytes).Substring(0, numberOfChars);

		}

		/// <summary>
		/// Gets better number for converting numbers from Base64 string conversion.
		/// </summary>
		/// <param name="numberOfChars"></param>
		/// <returns></returns>
		private static int GetBase64NumberLength(int numberOfChars)
		{
			int newNumb = numberOfChars / 4 * 3;
			newNumb += numberOfChars % 4 > 0 ? 1 : 0;
			return newNumb;
		}

		/// <summary>
		/// What does this do?
		/// </summary>
		/// <param name="passToHash">password we need hashed</param>
		/// <param name="salt">salt to apply to hash</param>
		/// <param name="pepper">pepper to apply to hash</param>
		/// <param name="stretches">how many times we should iterate the hash. 12000 - 24000</param>
		/// <param name="hashToCompare">hash to comare against</param>
		/// <returns></returns>
		public static bool IsValid(string passToCheck, string salt, string pepper, int stretches, string hashToCompare)
		{
			return Get(passToCheck, salt, pepper, stretches, hashToCompare.Length) == hashToCompare;
		}
		/// <summary>
		/// Generate a new salt
		/// </summary>
		/// <returns></returns>
		public static string GenerateSalt(int numberOfChars = 50)
		{

			byte[] newSalter = new byte[numberOfChars]; //new byte[GetBase64NumberLength(numberOfChars)];
			RandomNumberGenerator rGen = RandomNumberGenerator.Create();
			rGen.GetBytes(newSalter);
			return Convert.ToBase64String(newSalter).Substring(0, numberOfChars);
			//return Convert.(newSalter);
		}
	}
}

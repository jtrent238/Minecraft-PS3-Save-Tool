using System;

namespace Minecraft.Utility
{
	public static class BigEndianUtility
	{
		/// <summary>
		/// Changes the endianness from host to big endian and back
		/// by reversing the array if needed. Returns the array
		/// for conveniece.
		/// </summary>
		public static byte[] EnsureEndian(byte[] array)
		{
			if (BitConverter.IsLittleEndian) Array.Reverse(array);
			return array;
		}
	}
}

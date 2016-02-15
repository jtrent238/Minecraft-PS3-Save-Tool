using System;
using System.IO;

namespace Minecraft.Utility
{
	public class BigEndianReader : BinaryReader
	{
		public BigEndianReader(Stream input)
			: base(input) {  }
		
		private byte[] ReadEnsured(int length)
		{
			return BigEndianUtility.EnsureEndian(ReadBytes(length));
		}
		public override short ReadInt16()
		{
			return BitConverter.ToInt16(ReadEnsured(2), 0);
		}
		public override int ReadInt32()
		{
			return BitConverter.ToInt32(ReadEnsured(4), 0);
		}
		public override long ReadInt64()
		{
			return BitConverter.ToInt64(ReadEnsured(8), 0);
		}
		public override float ReadSingle()
		{
			return BitConverter.ToSingle(ReadEnsured(4), 0);
		}
		public override double ReadDouble()
		{
			return BitConverter.ToDouble(ReadEnsured(8), 0);
		}
	}
}

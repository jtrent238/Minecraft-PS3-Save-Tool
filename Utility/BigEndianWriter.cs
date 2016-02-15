using System;
using System.IO;

namespace Minecraft.Utility
{
	public class BigEndianWriter : BinaryWriter
	{
		public BigEndianWriter(Stream output)
			: base(output) {  }
		
		private void WriteEnsured(byte[] value)
		{
			Write(BigEndianUtility.EnsureEndian(value));
		}
		public override void Write(short value)
		{
			WriteEnsured(BitConverter.GetBytes(value));
		}
		public override void Write(int value)
		{
			WriteEnsured(BitConverter.GetBytes(value));
		}
		public override void Write(long value)
		{
			WriteEnsured(BitConverter.GetBytes(value));
		}
		public override void Write(float value)
		{
			WriteEnsured(BitConverter.GetBytes(value));
		}
		public override void Write(double value)
		{
			WriteEnsured(BitConverter.GetBytes(value));
		}
	}
}

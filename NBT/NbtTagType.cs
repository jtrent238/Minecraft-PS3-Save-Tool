using System;

namespace Minecraft.NBT
{
	public enum NbtTagType : byte
	{
		End = 0,
		Byte,
		Short,
		Int,
		Long,
		Float,
		Double,
		ByteArray,
		String,
		List,
		Compound,
		IntArray
	}
}
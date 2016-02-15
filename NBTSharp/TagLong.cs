using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NBTSharp
{
    public class TagLong : Tag
    {
        public const byte TagType = 4;
        public long number;

        public TagLong()
        {
            this.name = "";
            this.tagType = TagLong.TagType;
            this.number = 0;
        }

        public TagLong(String name, long number)
        {
            this.name = name;
            this.tagType = TagLong.TagType;
            this.number = number;
        }

        public override void read(BinaryReader reader)
        {
            try
            {
                this.name = Util.readString(reader);
                this.number = TagLong.readData(reader);
            }
            catch (IOException exception)
            {
                throw new FailedReadException(exception.Message, exception);
            }
        }

        public static long readData(BinaryReader reader)
        {
            if (BitConverter.IsLittleEndian)
            {
                return Util.readLongBigEndian(reader);
            }
            else
            {
                return reader.ReadInt64();
            }
        }

        public override void writeData(BinaryWriter writer)
        {
            Util.writeData(writer, BitConverter.GetBytes(number));
        }

        public override string ToString()
        {
            return number.ToString();
        }

        public static bool isValidString(String name)
        {
            long number = 0;
            return long.TryParse(name, out number);
        }
    }
}

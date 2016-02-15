using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NBTSharp
{
    public class TagShort : Tag
    {
        public const byte TagType = 2;
        public short number;

        public TagShort()
        {
            this.name = "";
            this.tagType = TagShort.TagType;
            this.number = 0;
        }

        public TagShort(String name, short number)
        {
            this.name = name;
            this.tagType = TagShort.TagType;
            this.number = number;
        }

        public override void read(BinaryReader reader)
        {
            try
            {
                this.name = Util.readString(reader);
                this.number = TagShort.readData(reader);
            }
            catch (IOException exception)
            {
                throw new FailedReadException(exception.Message, exception);
            }
        }

        public static short readData(BinaryReader reader)
        {
            if (BitConverter.IsLittleEndian)
            {
                return Util.readShortBigEndian(reader);
            }
            else
            {
                return reader.ReadInt16();
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
            short number = 0;
            return short.TryParse(name, out number);
        }
    }
}

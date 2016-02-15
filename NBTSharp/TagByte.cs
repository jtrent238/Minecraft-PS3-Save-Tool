using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NBTSharp
{
    public class TagByte : Tag
    {
        public const byte TagType = 1;
        public sbyte number;

        public TagByte()
        {
            this.name = "";
            this.tagType = TagByte.TagType;
        }

        public TagByte(String name, sbyte number)
        {
            this.name = name;
            this.tagType = TagByte.TagType;
            this.number = number;
        }

        public override void read(BinaryReader reader)
        {
            try
            {
                this.name = Util.readString(reader);
                this.number = TagByte.readData(reader);
            }
            catch (IOException exception)
            {
                throw new FailedReadException(exception.Message, exception);
            }
        }

        public static sbyte readData(BinaryReader reader)
        {
            return reader.ReadSByte();
        }

        public override void writeData(BinaryWriter writer)
        {
            writer.Write(number);
        }

        public override string ToString()
        {
            return number.ToString();
        }

        public static bool isValidString(String name)
        {
            sbyte number = 0;
            return sbyte.TryParse(name, out number);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NBTSharp
{
    public class TagInt : Tag
    {
        public const byte TagType = 3;
        public int number;

        public TagInt()
        {
            this.name = "";
            this.tagType = TagInt.TagType;
            this.number = 0;
        }

        public TagInt(String name, int number)
        {
            this.name = name;
            this.tagType = TagInt.TagType;
            this.number = number;
        }

        public override void read(BinaryReader reader)
        {
            try
            {
                this.name = Util.readString(reader);
                this.number = TagInt.readData(reader);
            }
            catch (IOException exception)
            {
                throw new FailedReadException(exception.Message, exception);
            }
        }

        public static int readData(BinaryReader reader)
        {
            if (BitConverter.IsLittleEndian)
            {
                return Util.readIntegerBigEndian(reader);
            }
            else
            {
                return reader.ReadInt32();
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
            int number = 0;
            return int.TryParse(name, out number);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NBTSharp
{
    public class TagDouble : Tag
    {
        public const byte TagType = 6;
        public double number;

        public TagDouble()
        {
            this.name = "";
            this.tagType = TagDouble.TagType;
            this.number = 0;
        }

        public TagDouble(String name, double number)
        {
            this.name = name;
            this.tagType = TagDouble.TagType;
            this.number = number;
        }

        public override void read(BinaryReader reader)
        {
            try
            {
                this.name = Util.readString(reader);
                this.number = TagDouble.readData(reader);
            }
            catch (IOException exception)
            {
                throw new FailedReadException(exception.Message, exception);
            }
        }

        public static double readData(BinaryReader reader)
        {
            if (BitConverter.IsLittleEndian)
            {
                return Util.readDoubleBigEndian(reader);
            }
            else
            {
                return reader.ReadDouble();
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
            double number = 0;
            return double.TryParse(name, out number);
        }
    }
}

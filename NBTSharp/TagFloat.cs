using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NBTSharp
{
    public class TagFloat : Tag
    {
        public const byte TagType = 5;
        public float number;

        public TagFloat()
        {
            this.name = "";
            this.tagType = TagFloat.TagType;
            this.number = 0;
        }

        public TagFloat(String name, float number)
        {
            this.name = name;
            this.tagType = TagFloat.TagType;
            this.number = number;
        }

        public override void read(BinaryReader reader)
        {
            try
            {
                this.name = Util.readString(reader);
                this.number = TagFloat.readData(reader);
            }
            catch (IOException exception)
            {
                throw new FailedReadException(exception.Message, exception);
            }
        }

        public static float readData(BinaryReader reader)
        {
            if (BitConverter.IsLittleEndian)
            {
                return Util.readFloatBigEndian(reader);
            }
            else
            {
                return reader.ReadSingle();
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
            float number = 0;
            return float.TryParse(name, out number);
        }
    }
}

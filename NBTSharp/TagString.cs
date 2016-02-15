using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NBTSharp
{
    public class TagString : Tag
    {
        public const byte TagType = 8;
        public String text;

        public TagString()
        {
            this.name = "";
            this.tagType = TagString.TagType;
            this.text = "";
        }

        public TagString(String name, String text)
        {
            this.name = name;
            this.tagType = TagString.TagType;
            this.text = text;
        }

        public override void read(BinaryReader reader)
        {
            try
            {
                this.name = Util.readString(reader);
                this.text = Util.readUTF8String(reader);
            }
            catch (IOException exception)
            {
                throw new FailedReadException(exception.Message, exception);
            }
        }

        public static String readData(BinaryReader reader)
        {
            return Util.readString(reader);
        }

        public override void writeData(BinaryWriter writer)
        {
            Util.writeUTF8String(writer, this.text);
        }

        public override string ToString()
        {
            return text.ToString();
        }

        public static bool isValidString(String name)
        {
            return true;
        }
    }
}

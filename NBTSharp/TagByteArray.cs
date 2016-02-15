using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NBTSharp
{
    public class TagByteArray : Tag
    {
        public const byte TagType = 7;
        public byte[] array;

        public TagByteArray()
        {
            this.name = "";
            this.tagType = TagByteArray.TagType;
            this.array = null;
        }

        public TagByteArray(String name, int length, byte[] bytes)
        {
            this.name = name;
            this.tagType = TagByteArray.TagType;

            this.array = new byte[length];

            int i = 0;
            foreach (byte number in bytes)
            {
                array[i] = number;
                i++;
            }
        }

        //tag bytes are signed, so unsigned bytes can't be returned in a tag
        //to return a tag, the byte array itself is returned
        public override Tag findTagByName(String name)
        {
            return this;
        }

        public override void read(BinaryReader reader)
        {
            try
            {
                this.name = Util.readString(reader);
                this.array = readData(reader);
            }
            catch (IOException exception)
            {
                throw new FailedReadException(exception.Message, exception);
            }
        }

        public static byte[] readData(BinaryReader reader)
        {
            int length = 0;

            if (BitConverter.IsLittleEndian)
            {
                length = Util.readIntegerBigEndian(reader);
            }
            else
            {
                length = reader.ReadInt32();
            }

            byte[] bytes = new byte[length];

            for (int a = 0; a < length; a++)
            {
                bytes[a] = reader.ReadByte();
            }

            return bytes;
        }

        public byte this[int i]
        {
            get
            {
                return array[i];
            }

            set
            {
                array[i] = value;
            }
        }

        public IEnumerator<byte> GetEnumerator()
        {
            for (int i = 0; i < array.Length; i++)
            {
                yield return array[i];
            }
        }

        public override void writeData(BinaryWriter writer)
        {
            Util.writeData(writer, BitConverter.GetBytes(array.Length));

            for (int a = 0; a < array.Length; a++)
            {
                writer.Write(array[a]);
            }
        }

        public override string ToString()
        {
            return name.ToString();
        }

        public static bool isValidString(String name)
        {
            return true;
        }
    }
}

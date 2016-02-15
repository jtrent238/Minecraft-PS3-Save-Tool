using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NBTSharp
{
    public class Util
    {
        //checks the endianess and writes the data using the appropriate method
        public static void writeData(BinaryWriter writer, byte[] array)
        {
            if (BitConverter.IsLittleEndian)
            {
                Util.writeBytesReverse(writer, array);
            }
            else
            {
                writer.Write(array);
            }
        }

        public static void writeBytesReverse(BinaryWriter writer, byte[] array)
        {
            for (int a = array.Length - 1; a >= 0; a--)
            {
                writer.Write(array[a]);
            }
        }

        //NBT strings are not null terminated, but c# strings are, so the string is written one char at a time
        public static void writeString(BinaryWriter writer, String text)
        {
            short length = (short)text.Length;
            Util.writeData(writer, BitConverter.GetBytes(length));

            foreach (char c in text)
            {
                writer.Write(c);
            }
        }

        //this method reads a short in big endian and returns a short in little endian
        public static short readShortBigEndian(BinaryReader reader)
        {
            short result = 0;
            for (int a = 1; a >= 0; a--)
            {
                result += (short)(reader.ReadByte() << a * 8);
            }

            return result;
        }

        //read a big endian integer and return it in little endian
        public static int readIntegerBigEndian(BinaryReader reader)
        {
            int result = 0;
            for (int a = 3; a >= 0; a--)
            {
                result += (reader.ReadByte() << a * 8);
            }

            return result;
        }

        //read a big endian long and return it in little endian
        public static long readLongBigEndian(BinaryReader reader)
        {
            long result = 0;
            for (int a = 7; a >= 0; a--)
            {
                result += ((long)reader.ReadByte() << a * 8);
            }

            return result;
        }

        //read a big endian float and return it in little endian
        public static float readFloatBigEndian(BinaryReader reader)
        {
            byte[] bytes = new byte[4];

            for (int a = 3; a >= 0; a--)
            {
                bytes[a] = reader.ReadByte();
            }

            return BitConverter.ToSingle(bytes, 0);
        }

        //read a big endian double and return it in little endian
        public static double readDoubleBigEndian(BinaryReader reader)
        {
            byte[] bytes = new byte[8];

            for (int a = 7; a >= 0; a--)
            {
                bytes[a] = reader.ReadByte();
            }

            return BitConverter.ToDouble(bytes, 0);
        }

        public static String readString(BinaryReader reader)
        {
            short length = 0;

            if (BitConverter.IsLittleEndian)
            {
                length = readShortBigEndian(reader);
            }
            else
            {
                length = reader.ReadInt16();
            }

            String name = "";

            try
            {
                for (int a = 0; a < length; a++)
                {
                    name += reader.ReadChar();
                }
            }
            catch (IOException exception)
            {
                throw new InvalidStringException(exception.Message);
            }

            return name;
        }

        public static String readUTF8String(BinaryReader reader)
        {
            short length = 0;

            if (BitConverter.IsLittleEndian)
            {
                length = readShortBigEndian(reader);
            }
            else
            {
                length = reader.ReadInt16();
            }

            Encoding encoder = new UTF8Encoding();
            byte[] bytes = new byte[length];
            String name = "";
            
            try
            {
                for (int a = 0; a < length; a++)
                {
                    bytes[a] = reader.ReadByte();
                    name += (char)bytes[a];
                }
            }
            catch (IOException exception)
            {
                throw new InvalidStringException(exception.Message);
            }

            return encoder.GetString(bytes);
        }

        public static void writeUTF8String(BinaryWriter writer, string text)
        {
            byte[] utf8String = Encoding.UTF8.GetBytes(text);
            short length = (short)utf8String.Length;

            //write the length
            writeData(writer, BitConverter.GetBytes(length));

            for (int a = 0; a < length; a++)
            {
                writer.Write(utf8String[a]);
            }
        }

        public static String getTypeName(int tagType)
        {
            switch (tagType)
            {
                case 0:
                    return "Tag_End";
                case 1:
                    return "Tag_Byte";
                case 2:
                    return "Tag_Short";
                case 3:
                    return "Tag_Int";
                case 4:
                    return "Tag_Long";
                case 5:
                    return "Tag_Float";
                case 6:
                    return "Tag_Double";
                case 7:
                    return "Tag_Byte_Array";
                case 8:
                    return "Tag_String";
                case 9:
                    return "Tag_List";
                case 10:
                    return "Tag_Compound";
                default:
                    throw new InvalidTagException(tagType + " is not a valid tag type");
            }
        }
    }
}

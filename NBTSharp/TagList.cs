using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NBTSharp
{
    public class TagList : Tag
    {
        public const byte TagType = 9;
        public sbyte tagId; //the type of the items in the list
        public List<object> list;

        public TagList()
        {
            this.name = "";
            this.tagType = TagList.TagType;
            this.tagId = 0;
        }

        public void addTag(Tag tag)
        {
            list.Add(tag);
        }

        public override Tag findTagByName(String name)
        {
            //access the tag compound, and return it, or just return the tag list itself
            int index = 0;
            if (int.TryParse(name, out index))
            {
                if (this.tagId == TagCompound.TagType)
                {
                    //interface text is 1 indexed
                    return (TagCompound)list[index - 1];
                }
            }
            else
            {
                throw new InvalidPathException(name + " is not a valid list index");
            }

            return this;
        }

        public override void read(BinaryReader reader)
        {
            try
            {
                this.list = new List<object>();
                this.name = Util.readString(reader);
                this.tagId = reader.ReadSByte();
                int length = 0;

                if (BitConverter.IsLittleEndian)
                {
                    length = Util.readIntegerBigEndian(reader);
                }
                else
                {
                    length = reader.ReadInt32();
                }

                for (int a = 0; a < length; a++)
                {
                    switch (tagId)
                    {
                        case 1:
                            list.Add(TagByte.readData(reader));
                            break;
                        case 2:
                            list.Add(TagShort.readData(reader));
                            break;
                        case 3:
                            list.Add(TagInt.readData(reader));
                            break;
                        case 4:
                            list.Add(TagLong.readData(reader));
                            break;
                        case 5:
                            list.Add(TagFloat.readData(reader));
                            break;
                        case 6:
                            list.Add(TagDouble.readData(reader));
                            break;
                        case 7:
                            list.Add(TagByteArray.readData(reader));
                            break;
                        case 8:
                            list.Add(TagString.readData(reader));
                            break;
                        case 9:
                            TagList newList = new TagList();
                            newList.read(reader);
                            list.Add(newList);
                            break;
                        case 10:
                            List<Tag> newTagList = TagCompound.readData(reader);
                            TagCompound compound = new TagCompound("", newTagList);
                            list.Add(compound);
                            break;
                    }
                }
            }
            catch (IOException exception)
            {
                throw new FailedReadException(exception.Message, exception);
            }
        }

        public object this[int i]
        {
            get
            {
                return list[i];
            }

            set
            {
                list[i] = value;
            }
        }

        public IEnumerator<object> GetEnumerator()
        {
            for (int i = 0; i < list.Count; i++)
            {
                yield return list[i];
            }
        }

        public override void writeData(BinaryWriter writer)
        {
            writer.Write(tagId);

            Util.writeData(writer, BitConverter.GetBytes(list.Count));

            foreach (object obj in list)
            {
                switch (tagId)
                {
                    case 1:
                        writer.Write((sbyte)obj);
                        break;
                    case 2:
                        short shortnum = (short)obj;
                        Util.writeData(writer, BitConverter.GetBytes(shortnum));
                        break;
                    case 3:
                        int intnum = (int)obj;
                        Util.writeData(writer, BitConverter.GetBytes(intnum));
                        break;
                    case 4:
                        long longnum = (long)obj;
                        Util.writeData(writer, BitConverter.GetBytes(longnum));
                        break;
                    case 5:
                        float floatnum = (float)obj;
                        Util.writeData(writer, BitConverter.GetBytes(floatnum));
                        break;
                    case 6:
                        double doublenum = (double)obj;
                        Util.writeData(writer, BitConverter.GetBytes(doublenum));
                        break;
                    case 7:
                        byte[] array = (byte[])obj;
                        
                        for (int a = 0; a < array.Length; a++)
                        {
                            writer.Write(array[a]);
                        }

                        break;
                    case 8:
                        Util.writeUTF8String(writer, (string)obj);
                        break;
                    case 9:
                        TagList writeList = (TagList)obj;
                        writeList.write(writer);
                        break;
                    case 10:
                        TagCompound compound = (TagCompound)obj;
                        List<Tag> tagList = compound.tags;

                        foreach (Tag tag in tagList)
                        {
                            tag.write(writer);
                        }

                        //tag compound ends with a zero byte
                        writer.Write((byte)0);

                        break;
                }
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NBTSharp
{
    public class TagCompound : Tag
    {
        public const byte TagType = 10;
        public List<Tag> tags;

        public TagCompound()
        {
            this.name = "";
            this.tagType = TagCompound.TagType;
            this.tags = new List<Tag>();
        }

        public TagCompound(String name)
        {
            this.name = name;
            this.tagType = TagCompound.TagType;
            this.tags = new List<Tag>();
        }

        public TagCompound(String name, List<Tag> tags)
        {
            this.name = name;
            this.tagType = TagCompound.TagType;
            this.tags = tags;
        }

        public override Tag findTagByName(String name)
        {
            for (int i = 0; i < tags.Count; i++)
            {
                if (tags[i].name == name)
                {
                    return tags[i];
                }
            }

            return null;
        }

        public void addTag(Tag tag)
        {
            tags.Add(tag);
        }

        public void removeTag(Tag tag)
        {
            tags.Remove(tag);
        }

        public override void read(BinaryReader reader)
        {
            try
            {
                this.name = Util.readString(reader);
                this.tags = readData(reader);                
            }
            catch (IOException exception)
            {
                throw new FailedReadException(exception.Message, exception);
            }
        }

        public static List<Tag> readData(BinaryReader reader)
        {            
            bool completed = false;
            List<Tag> compound = new List<Tag>();

            while (!completed)
            {
                byte tagType = reader.ReadByte();

                //Console.WriteLine("TagType: " + Util.getTypeName(tagType));

                switch (tagType)
                {
                    case 0:
                        completed = true;
                        break;
                    case 1:
                        TagByte tagByte = new TagByte();
                        tagByte.read(reader);
                        compound.Add(tagByte);
                        break;
                    case 2:
                        TagShort tagShort = new TagShort();
                        tagShort.read(reader);
                        compound.Add(tagShort);
                        break;
                    case 3:
                        TagInt tagInt = new TagInt();
                        tagInt.read(reader);
                        compound.Add(tagInt);
                        break;
                    case 4:
                        TagLong tagLong = new TagLong();
                        tagLong.read(reader);
                        compound.Add(tagLong);
                        break;
                    case 5:
                        TagFloat tagFloat = new TagFloat();
                        tagFloat.read(reader);
                        compound.Add(tagFloat);
                        break;
                    case 6:
                        TagDouble tagDouble = new TagDouble();
                        tagDouble.read(reader);
                        compound.Add(tagDouble);
                        break;
                    case 7:
                        TagByteArray tagByteArray = new TagByteArray();
                        tagByteArray.read(reader);
                        compound.Add(tagByteArray);
                        break;
                    case 8:
                        TagString tagString = new TagString();
                        tagString.read(reader);
                        compound.Add(tagString);
                        break;
                    case 9:
                        TagList tagList = new TagList();
                        tagList.read(reader);
                        compound.Add(tagList);
                        break;
                    case 10:
                        TagCompound tagCompound = new TagCompound();
                        tagCompound.read(reader);
                        compound.Add(tagCompound);
                        break;
                    default:
                        throw new InvalidTagException("An invalid tag type: " + tagType + ", was found while parsing a TAG_Compound");
                }
            }

            return compound;
        }

        public override void writeData(BinaryWriter writer)
        {
            foreach (Tag tag in tags)
            {
                tag.write(writer);
            }

            //a tag compound ends with a Tag_End, which is a single byte of zero
            writer.Write((byte)0);
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
